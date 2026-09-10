using System.Diagnostics;
using System.Net;
using System.Text;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;
using Microsoft.Extensions.Options;

// Mints an EVE SSO refresh token for the live auth probe.
//
//   1. reads Client ID / Secret Key (env ESI_CLIENT_ID / ESI_SECRET_KEY, else prompts)
//   2. opens the browser to the SSO consent screen
//   3. catches the redirect on http://localhost:<port>/callback with an HttpListener
//   4. exchanges the code, calls Verify() to confirm the character, prints the refresh token
//
// Assumes a confidential client (Client ID + Secret Key). Register the callback URL
// EXACTLY as printed on your app at https://developers.eveonline.com/.

string Env(string key) => Environment.GetEnvironmentVariable(key)?.Trim() ?? "";

var clientId = Env("ESI_CLIENT_ID");
var secretKey = Env("ESI_SECRET_KEY");
if (clientId.Length == 0) clientId = Prompt("ESI Client ID", secret: false);
if (secretKey.Length == 0) secretKey = Prompt("ESI Secret Key", secret: true);

if (clientId.Length == 0 || secretKey.Length == 0)
{
    Console.Error.WriteLine("Client ID and Secret Key are both required.");
    return 1;
}

var port = int.TryParse(Env("ESI_CALLBACK_PORT"), out var parsedPort) ? parsedPort : 8080;
var callbackUrl = $"http://localhost:{port}/callback";

var scopesRaw = Env("ESI_SCOPES") is { Length: > 0 } raw ? raw : "esi-wallet.read_character_wallet.v1";
List<string> scopes;
if (string.Equals(scopesRaw.Trim(), "all", StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("  scopes      : fetching every scope from the ESI spec ...");
    scopes = await FetchAllScopesAsync();
}
else
{
    scopes = scopesRaw
        .Split(new[] { ' ', ',', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
        .Distinct()
        .ToList();
}

static async Task<List<string>> FetchAllScopesAsync()
{
    using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    http.DefaultRequestHeaders.UserAgent.ParseAdd("ESI.NET-mint-token/1.0");
    http.DefaultRequestHeaders.Add("X-Compatibility-Date", ESI.NET.EsiVersion.CompatibilityDate);
    using var doc = System.Text.Json.JsonDocument.Parse(
        await http.GetStringAsync("https://esi.evetech.net/meta/openapi.json"));
    var flows = doc.RootElement
        .GetProperty("components").GetProperty("securitySchemes")
        .GetProperty("OAuth2").GetProperty("flows").GetProperty("authorizationCode")
        .GetProperty("scopes");
    return flows.EnumerateObject().Select(p => p.Name).OrderBy(s => s, StringComparer.Ordinal).ToList();
}

var dataSource = Enum.TryParse<DataSource>(Env("ESI_DATASOURCE"), ignoreCase: true, out var ds)
    ? ds
    : DataSource.Tranquility;

Console.WriteLine();
Console.WriteLine($"  data source : {dataSource}");
Console.WriteLine($"  callback    : {callbackUrl}   <- must be registered on your app, character for character");
Console.WriteLine($"  scopes      : {string.Join(" ", scopes)}");
Console.WriteLine();

var config = Options.Create(new EsiConfig
{
    EsiUrl = "https://esi.evetech.net/",
    DataSource = dataSource,
    ClientId = clientId,
    SecretKey = secretKey,
    CallbackUrl = callbackUrl,
    UserAgent = "ESI.NET MintToken (local token minter)",
});

var client = new EsiClient(config);
var state = Guid.NewGuid().ToString("N");
var authUrl = client.SSO.CreateAuthenticationUrl(scopes, state);

using var listener = new HttpListener();
listener.Prefixes.Add($"http://localhost:{port}/callback/");
try
{
    listener.Start();
}
catch (HttpListenerException ex)
{
    Console.Error.WriteLine($"Could not bind {callbackUrl}: {ex.Message}");
    Console.Error.WriteLine("Set ESI_CALLBACK_PORT to a free port and register that callback on your app.");
    return 1;
}

Console.WriteLine("Opening your browser to log in. If it does not open, paste this URL:");
Console.WriteLine();
Console.WriteLine("  " + authUrl);
Console.WriteLine();
try
{
    Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
}
catch
{
    // headless / no default browser: the printed URL above is the fallback
}

var timeout = TimeSpan.FromMinutes(5);
var contextTask = listener.GetContextAsync();
if (await Task.WhenAny(contextTask, Task.Delay(timeout)) != contextTask)
{
    Console.Error.WriteLine($"Timed out after {timeout.TotalMinutes:0} minutes waiting for the SSO redirect.");
    return 1;
}

var ctx = await contextTask;
var query = ctx.Request.QueryString;

if (query["error"] is { Length: > 0 } ssoError)
{
    Reply($"<h2>SSO returned an error</h2><p>{WebUtility.HtmlEncode(ssoError)}: {WebUtility.HtmlEncode(query["error_description"])}</p>", 400);
    Console.Error.WriteLine($"SSO error: {ssoError} - {query["error_description"]}");
    return 1;
}

if (query["state"] != state)
{
    Reply("<h2>State mismatch</h2><p>Ignoring this response. Re-run the tool.</p>", 400);
    Console.Error.WriteLine("The state parameter did not match - stale or forged redirect. Aborting.");
    return 1;
}

var code = query["code"];
if (string.IsNullOrEmpty(code))
{
    Reply("<h2>No authorization code in the redirect.</h2>", 400);
    Console.Error.WriteLine("The redirect carried no ?code= value.");
    return 1;
}

SsoToken token;
try
{
    token = await client.SSO.GetToken(GrantType.AuthorizationCode, code);
}
catch (Exception ex)
{
    Reply($"<h2>Token exchange failed</h2><p>{WebUtility.HtmlEncode(ex.Message)}</p>", 400);
    Console.Error.WriteLine("GetToken failed: " + ex.Message);
    return 1;
}

// Verify() both confirms the character and exercises the JWKS validation path
// (the risky part of the Microsoft.IdentityModel.Tokens 6 -> 8 upgrade).
string who;
try
{
    var authChar = await client.SSO.Verify(token);
    who = $"{authChar.CharacterName} (id {authChar.CharacterID})  scopes: {authChar.Scopes}";
}
catch (Exception ex)
{
    who = "Verify FAILED: " + ex.Message;
}

Reply($"<h2>Done.</h2><p>Refresh token minted for <b>{WebUtility.HtmlEncode(who)}</b>.<br>You can close this tab and return to the terminal.</p>");

var secretName = Env("ESI_SECRET_NAME") is { Length: > 0 } customName ? customName : "ESI_REFRESH_TOKEN";
// gh refuses to pick when a repo has several GitHub remotes (forks pulled in as
// remotes). Default to origin; ESI_SECRET_REPO overrides.
var secretRepo = Env("ESI_SECRET_REPO") is { Length: > 0 } explicitRepo ? explicitRepo : ResolveOriginRepo();

Console.WriteLine();
Console.WriteLine("======================================================================");
Console.WriteLine("  Character    : " + who);
Console.WriteLine("  Access token : expires in " + token.ExpiresIn + "s (the refresh token below is long-lived)");
Console.WriteLine();
Console.WriteLine("  REFRESH TOKEN:");
Console.WriteLine();
Console.WriteLine("    " + token.RefreshToken);
Console.WriteLine("======================================================================");
Console.WriteLine();

var setSecret = Env("ESI_SET_SECRET") is "1" or "true" or "TRUE" or "yes";
if (!setSecret && !Console.IsInputRedirected)
{
    Console.Write($"Set the {secretName} GitHub secret now with gh? [y/N]: ");
    setSecret = (Console.ReadLine() ?? "").Trim().ToLowerInvariant() is "y" or "yes";
}

var ghError = "";
if (setSecret && TrySetSecret(secretName, secretRepo, token.RefreshToken, out ghError))
{
    Console.WriteLine($"  {secretName} set via gh{(secretRepo.Length > 0 ? $" ({secretRepo})" : "")}.");
}
else
{
    if (setSecret)
        Console.WriteLine("  gh could not set the secret: " + ghError);

    var repoArg = secretRepo.Length > 0 ? $" --repo {secretRepo}" : "";
    Console.WriteLine();
    Console.WriteLine("  Store it yourself with:");
    Console.WriteLine();
    Console.WriteLine($"    gh secret set {secretName}{repoArg} --body \"{token.RefreshToken}\"");
}

return 0;

static string ResolveOriginRepo()
{
    try
    {
        var psi = new ProcessStartInfo("git")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        psi.ArgumentList.Add("remote");
        psi.ArgumentList.Add("get-url");
        psi.ArgumentList.Add("origin");

        using var proc = Process.Start(psi);
        if (proc is null) return "";
        var url = proc.StandardOutput.ReadToEnd().Trim();
        proc.WaitForExit();
        if (proc.ExitCode != 0) return "";

        // git@github.com:owner/repo.git  |  https://github.com/owner/repo(.git)
        var match = System.Text.RegularExpressions.Regex.Match(url, @"github\.com[/:]([^/]+)/(.+?)(?:\.git)?/?$");
        return match.Success ? $"{match.Groups[1].Value}/{match.Groups[2].Value}" : "";
    }
    catch
    {
        return "";
    }
}

static bool TrySetSecret(string name, string repo, string value, out string error)
{
    error = "";
    try
    {
        var psi = new ProcessStartInfo("gh")
        {
            RedirectStandardInput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        psi.ArgumentList.Add("secret");
        psi.ArgumentList.Add("set");
        psi.ArgumentList.Add(name);
        if (repo.Length > 0)
        {
            psi.ArgumentList.Add("--repo");
            psi.ArgumentList.Add(repo);
        }

        using var proc = Process.Start(psi);
        if (proc is null)
        {
            error = "could not start gh (is it installed and on PATH?)";
            return false;
        }

        proc.StandardInput.Write(value); // value goes over stdin, never argv / shell history
        proc.StandardInput.Close();
        var stderr = proc.StandardError.ReadToEnd();
        proc.WaitForExit();

        if (proc.ExitCode == 0)
            return true;

        error = string.IsNullOrWhiteSpace(stderr) ? $"gh exited {proc.ExitCode}" : stderr.Trim();
        return false;
    }
    catch (Exception ex)
    {
        error = ex.Message;
        return false;
    }
}

string Reply(string html, int status = 200)
{
    ctx.Response.StatusCode = status;
    ctx.Response.ContentType = "text/html; charset=utf-8";
    var bytes = Encoding.UTF8.GetBytes(
        $"<!doctype html><meta charset=utf-8><body style=\"font:14px system-ui;margin:3rem;max-width:40rem\">{html}</body>");
    ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
    ctx.Response.Close();
    return html;
}

static string Prompt(string label, bool secret)
{
    Console.Write(label + ": ");

    if (!secret || Console.IsInputRedirected)
        return (Console.ReadLine() ?? "").Trim();

    var sb = new StringBuilder();
    while (true)
    {
        var key = Console.ReadKey(intercept: true);
        if (key.Key == ConsoleKey.Enter) break;
        if (key.Key == ConsoleKey.Backspace)
        {
            if (sb.Length > 0) { sb.Length--; Console.Write("\b \b"); }
        }
        else if (!char.IsControl(key.KeyChar))
        {
            sb.Append(key.KeyChar);
            Console.Write('*');
        }
    }
    Console.WriteLine();
    return sb.ToString().Trim();
}
