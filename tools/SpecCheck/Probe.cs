using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace ESI.NET.Tools.SpecCheck;

/// <summary>
/// Live check: fetch real responses and deserialize them into the wrapper's
/// models. Catches the case SchemaCheck cannot - a field the spec marks non-null
/// that ESI actually returns as <c>null</c>, which throws for every caller.
///
/// Public GETs are always probed (path params from a fixture set). When
/// ESI_CLIENT_ID / ESI_SECRET_KEY / ESI_REFRESH_TOKEN are present, authenticated
/// GETs are probed too - identity comes from the token, sub-ids
/// (contract_id, mail_id, ...) are pulled from the matching list endpoint.
/// </summary>
public static class Probe
{
    // Well-known ids that exist on Tranquility.
    private static readonly Dictionary<string, string> Fixtures = new(StringComparer.Ordinal)
    {
        ["type_id"] = "34",                 // Tritanium
        ["region_id"] = "10000002",         // The Forge
        ["constellation_id"] = "20000020",  // Kimotoro
        ["system_id"] = "30000142",         // Jita
        ["star_id"] = "40009076",
        ["planet_id"] = "40009077",
        ["asteroid_belt_id"] = "40000005",
        ["moon_id"] = "40009081",
        ["stargate_id"] = "50001248",
        ["station_id"] = "60003760",        // Jita 4-4 CNAP
        ["graphic_id"] = "10",
        ["category_id"] = "6",              // Ship
        ["group_id"] = "25",                // Frigate
        ["market_group_id"] = "2",
        ["attribute_id"] = "4",             // Mass
        ["effect_id"] = "10",
        ["schematic_id"] = "65",
        ["alliance_id"] = "99005338",
        ["corporation_id"] = "98356193",
        ["character_id"] = "2112625428",
        ["faction_id"] = "500001",
        ["war_id"] = "1",
        ["division"] = "1",
    };

    private static readonly Dictionary<string, string> QueryFor = new(StringComparer.Ordinal)
    {
        ["GET /markets/{region_id}/history"] = "?type_id=34",
    };

    public static async Task<int> RunAsync(Spec spec, Wrapper wrapper, string baseUrl, string compatDate)
    {
        var specKeys = spec.Operations.Select(o => o.Key).ToHashSet();

        using var http = NewClient(compatDate);
        var publicExit = await ProbeSetAsync(
            "public", http, baseUrl,
            wrapper.Endpoints.Where(e => e.HttpMethod == "GET" && !e.Authenticated && e.ModelType is not null && specKeys.Contains(e.Key)),
            new Dictionary<string, string>(Fixtures, StringComparer.Ordinal),
            resolvers: null);

        var clientId = Environment.GetEnvironmentVariable("ESI_CLIENT_ID");
        var secretKey = Environment.GetEnvironmentVariable("ESI_SECRET_KEY");
        var refreshToken = Environment.GetEnvironmentVariable("ESI_REFRESH_TOKEN");
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(refreshToken))
        {
            Console.WriteLine();
            Console.WriteLine("PROBE - no ESI_CLIENT_ID / ESI_SECRET_KEY / ESI_REFRESH_TOKEN; skipping authenticated endpoints");
            return publicExit;
        }

        var authExit = await ProbeAuthAsync(spec, wrapper, baseUrl, compatDate, clientId!, secretKey!, refreshToken!);
        return publicExit | authExit;
    }

    private static async Task<int> ProbeAuthAsync(
        Spec spec, Wrapper wrapper, string baseUrl, string compatDate,
        string clientId, string secretKey, string refreshToken)
    {
        using var http = NewClient(compatDate);

        // --- refresh-token exchange ---
        string accessToken;
        try
        {
            using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://login.eveonline.com/v2/oauth/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "refresh_token",
                    ["refresh_token"] = refreshToken,
                }),
            };
            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secretKey}")));
            using var tokenResponse = await http.SendAsync(tokenRequest);
            var tokenBody = await tokenResponse.Content.ReadAsStringAsync();
            if (!tokenResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"PROBE (auth) - token exchange failed ({(int)tokenResponse.StatusCode}): {tokenBody}");
                return 1;
            }
            using var tokenJson = JsonDocument.Parse(tokenBody);
            accessToken = tokenJson.RootElement.GetProperty("access_token").GetString()!;
            if (tokenJson.RootElement.TryGetProperty("refresh_token", out var rotated)
                && rotated.GetString() is { } rt && rt != refreshToken)
                Console.WriteLine("::warning::ESI_REFRESH_TOKEN was rotated by this run - update the secret");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PROBE (auth) - token exchange error: {ex.Message}");
            return 1;
        }
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // --- identity ---
        var characterId = JwtSubject(accessToken);
        string corporationId, allianceId = null!;
        try
        {
            using var doc = JsonDocument.Parse(await http.GetStringAsync($"{baseUrl}/characters/{characterId}/"));
            corporationId = doc.RootElement.GetProperty("corporation_id").GetRawText();
            using var corp = JsonDocument.Parse(await http.GetStringAsync($"{baseUrl}/corporations/{corporationId}/"));
            if (corp.RootElement.TryGetProperty("alliance_id", out var a))
                allianceId = a.GetRawText();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PROBE (auth) - could not resolve identity: {ex.Message}");
            return 1;
        }

        Console.WriteLine();
        Console.WriteLine($"PROBE (auth) - character {characterId}, corporation {corporationId}, alliance {allianceId ?? "-"}");

        var fixtures = new Dictionary<string, string>(Fixtures, StringComparer.Ordinal)
        {
            ["character_id"] = characterId,
            ["corporation_id"] = corporationId,
        };
        if (allianceId is not null)
            fixtures["alliance_id"] = allianceId;

        // param -> (list endpoint, dotted path to an id; "[]" = first array element)
        var resolvers = new (string Param, string ListPath, string[] IdPath)[]
        {
            ("contract_id", "/characters/{character_id}/contracts/", new[] { "[]", "contract_id" }),
            ("mail_id", "/characters/{character_id}/mail/", new[] { "[]", "mail_id" }),
            ("planet_id", "/characters/{character_id}/planets/", new[] { "[]", "planet_id" }),
            ("event_id", "/characters/{character_id}/calendar/", new[] { "[]", "event_id" }),
            ("fleet_id", "/characters/{character_id}/fleet/", new[] { "fleet_id" }),
            ("project_id", "/corporations/{corporation_id}/projects/", new[] { "projects", "[]", "id" }),
            ("access_list_id", "/characters/{character_id}/access-lists/", new[] { "access_lists", "[]", "id" }),
            ("operation_id", "/characters/{character_id}/mercenary-tactical-operations/", new[] { "operations", "[]", "id" }),
            ("mercenary_den_id", "/characters/{character_id}/structures/mercenary-dens/", new[] { "mercenary_dens", "[]", "id" }),
            ("skyhook_id", "/corporations/{corporation_id}/structures/skyhooks/", new[] { "skyhooks", "[]", "id" }),
            ("sovereignty_hub_id", "/corporations/{corporation_id}/structures/sovereignty-hubs/", new[] { "sovereignty_hubs", "[]", "id" }),
            ("job_id", "/characters/{character_id}/freelance-jobs/", new[] { "freelance_jobs", "[]", "id" }),
        };

        var specKeys = spec.Operations.Select(o => o.Key).ToHashSet();
        return await ProbeSetAsync(
            "auth", http, baseUrl,
            wrapper.Endpoints.Where(e => e.HttpMethod == "GET" && e.Authenticated && e.ModelType is not null && specKeys.Contains(e.Key)),
            fixtures,
            resolvers);
    }

    private static async Task<int> ProbeSetAsync(
        string label, HttpClient http, string baseUrl,
        IEnumerable<ImplementedEndpoint> candidates,
        Dictionary<string, string> fixtures,
        (string Param, string ListPath, string[] IdPath)[]? resolvers)
    {
        var resolverByParam = (resolvers ?? Array.Empty<(string, string, string[])>())
            .ToDictionary(r => r.Item1, r => (r.Item2, r.Item3), StringComparer.Ordinal);
        var resolved = new Dictionary<string, string?>(StringComparer.Ordinal);

        async Task<string?> ResolveAsync(string param)
        {
            if (fixtures.TryGetValue(param, out var f)) return f;
            if (resolved.TryGetValue(param, out var cached)) return cached;
            if (!resolverByParam.TryGetValue(param, out var r)) return resolved[param] = null;

            try
            {
                var listUrl = $"{baseUrl.TrimEnd('/')}{Substitute(r.Item1, fixtures)}";
                using var response = await http.GetAsync(listUrl);
                if (response.StatusCode != HttpStatusCode.OK)
                    return resolved[param] = null;
                using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var node = doc.RootElement;
                foreach (var seg in r.Item2)
                {
                    if (seg == "[]")
                    {
                        if (node.ValueKind != JsonValueKind.Array || node.GetArrayLength() == 0)
                            return resolved[param] = null;
                        node = node[0];
                    }
                    else if (!node.TryGetProperty(seg, out node))
                        return resolved[param] = null;
                }
                return resolved[param] = node.ValueKind == JsonValueKind.String ? node.GetString() : node.GetRawText();
            }
            catch
            {
                return resolved[param] = null;
            }
        }

        var targets = candidates.GroupBy(e => e.Key).Select(g => g.First()).OrderBy(e => e.Path).ToList();
        Console.WriteLine();
        Console.WriteLine($"PROBE ({label}) - {targets.Count} GET endpoint(s)");

        var failures = new List<string>();
        int ok = 0, skipped = 0;

        foreach (var endpoint in targets)
        {
            var replacements = new Dictionary<string, string>(StringComparer.Ordinal);
            string? unresolved = null;
            foreach (var token in Tokens(endpoint.Path))
            {
                var value = await ResolveAsync(token);
                if (value is null) { unresolved = token; break; }
                replacements[token] = value;
            }
            if (unresolved is not null)
            {
                skipped++;
                Console.WriteLine($"  skip  {endpoint.Key}  (no {{{unresolved}}})");
                continue;
            }

            var path = endpoint.Path;
            foreach (var (k, v) in replacements) path = path.Replace($"{{{k}}}", v);
            var query = QueryFor.TryGetValue(endpoint.Key, out var q) ? q : "";
            var url = $"{baseUrl.TrimEnd('/')}{path}{query}";

            try
            {
                using var response = await http.GetAsync(url);
                var body = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    skipped++;
                    Console.WriteLine($"  skip  {endpoint.Key}  ({(int)response.StatusCode})");
                    continue;
                }
                try
                {
                    JsonConvert.DeserializeObject(body.Trim(), endpoint.ModelType!);
                    ok++;
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    var msg = ex.Message.Split('\n')[0].Trim();
                    failures.Add($"{endpoint.Key}  ->  {endpoint.ModelTypeText}\n      {msg}");
                    Console.WriteLine($"  FAIL  {endpoint.Key}");
                    Console.WriteLine($"        {msg}");
                }
            }
            catch (Exception ex)
            {
                skipped++;
                Console.WriteLine($"  skip  {endpoint.Key}  (transport: {ex.Message.Split('\n')[0]})");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"PROBE ({label}) RESULT: {ok} ok, {failures.Count} failure(s), {skipped} skipped.");
        foreach (var f in failures)
            Console.WriteLine($"  - {f}");
        return failures.Count > 0 ? 1 : 0;
    }

    private static HttpClient NewClient(string compatDate)
    {
        var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        http.DefaultRequestHeaders.UserAgent.ParseAdd("ESI.NET-spec-check-probe/1.0");
        http.DefaultRequestHeaders.Add("X-Compatibility-Date", compatDate);
        http.DefaultRequestHeaders.Add("X-Tenant", "tranquility");
        return http;
    }

    private static string JwtSubject(string jwt)
    {
        var payload = jwt.Split('.')[1].Replace('-', '+').Replace('_', '/');
        payload += (payload.Length % 4) switch { 2 => "==", 3 => "=", _ => "" };
        using var doc = JsonDocument.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));
        var sub = doc.RootElement.GetProperty("sub").GetString()!; // "CHARACTER:EVE:2112625428"
        return sub.Split(':').Last();
    }

    private static string Substitute(string template, Dictionary<string, string> values)
    {
        foreach (var (k, v) in values)
            template = template.Replace($"{{{k}}}", v);
        return template;
    }

    private static IEnumerable<string> Tokens(string path)
    {
        var i = 0;
        while ((i = path.IndexOf('{', i)) >= 0)
        {
            var j = path.IndexOf('}', i);
            if (j < 0) yield break;
            yield return path.Substring(i + 1, j - i - 1);
            i = j + 1;
        }
    }
}
