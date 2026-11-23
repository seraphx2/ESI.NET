using ESI.NET;
using ESI.NET.Tools.SpecCheck;
using ESI.NET.Tools.SpecCheck.Docs;

// spec-check [--spec <url|path>] [--source <dir>] [--no-schema] [--probe] [--issue-body <path>]
//
//   --spec        OpenAPI document. Default: the live ESI meta spec.
//   --source      ESI.NET/Logic directory. Default: auto-detected from the repo root.
//   --no-schema   Tier 1 (coverage) only; skip Tier 2 (schema drift).
//   --issue-body  On drift, write a GitHub issue body (markdown + drift fingerprint) here.
//   --write-docs  Write the XML doc comments and EsiEndpointRequirements.g.cs from the spec
//                 at the PINNED compatibility date, plus tools/SpecCheck/docs-report.txt.
//   --check-docs  Fail if --write-docs would change anything (docs / scope table drift).
//
// Any finding fails the run. Deliberate exceptions go in tools/SpecCheck/allowlist.txt.

var specSource = "https://esi.evetech.net/meta/openapi.json";
string? sourceDir = null;
string? issueBodyPath = null;
var runSchema = true;
var runProbe = false;
var docsMode = (string?)null;

for (var i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--spec" when i + 1 < args.Length: specSource = args[++i]; break;
        case "--source" when i + 1 < args.Length: sourceDir = args[++i]; break;
        case "--no-schema": runSchema = false; break;
        case "--probe": runProbe = true; break;
        case "--issue-body" when i + 1 < args.Length: issueBodyPath = args[++i]; break;
        case "--write-docs": docsMode = "write"; break;
        case "--check-docs": docsMode = "check"; break;
        case "-h" or "--help":
            Console.WriteLine("usage: spec-check [--spec <url|path>] [--source <dir>] [--no-schema] [--probe] [--issue-body <path>] [--write-docs | --check-docs]");
            return 0;
        default:
            Console.Error.WriteLine($"unknown argument: {args[i]}");
            return 2;
    }
}

sourceDir ??= LocateLogicDirectory();
if (sourceDir is null || !Directory.Exists(sourceDir))
{
    Console.Error.WriteLine("could not locate ESI.NET/Logic; pass --source <dir>");
    return 2;
}

using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
http.DefaultRequestHeaders.UserAgent.ParseAdd("ESI.NET-spec-check/1.0");

// SpecCheck targets the LATEST published ESI snapshot, not the date the wrapper
// currently pins. A newly published compatibility date then shows up as drift to
// act on (wrap the new endpoints, fix the changed shapes, bump
// EsiVersion.CompatibilityDate, cut a release) instead of a silent gap.
var pinnedDate = EsiVersion.CompatibilityDate;
if (specSource.StartsWith("http", StringComparison.OrdinalIgnoreCase))
{
    var targetDate = pinnedDate;
    // Docs describe the API the wrapper actually calls, so they follow the pinned date.
    if (docsMode is null)
    try
    {
        using var datesResponse = await http.GetAsync("https://esi.evetech.net/meta/compatibility-dates");
        datesResponse.EnsureSuccessStatusCode();
        using var doc = System.Text.Json.JsonDocument.Parse(await datesResponse.Content.ReadAsStringAsync());
        var latest = doc.RootElement.GetProperty("compatibility_dates").EnumerateArray()
            .Select(e => e.GetString())
            .Where(d => !string.IsNullOrEmpty(d))
            .OrderByDescending(d => d, StringComparer.Ordinal)
            .FirstOrDefault();
        if (!string.IsNullOrEmpty(latest))
            targetDate = latest!;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"(could not read /meta/compatibility-dates: {ex.Message} - falling back to the pinned date)");
    }

    http.DefaultRequestHeaders.Add("X-Compatibility-Date", targetDate);
    Console.WriteLine(targetDate == pinnedDate
        ? $"ESI compatibility date {targetDate} (EsiVersion.CompatibilityDate is current)"
        : $"checking against latest ESI compatibility date {targetDate}; the wrapper pins {pinnedDate} - a catch-up release is due");
}

Spec spec;
try
{
    spec = await Spec.LoadAsync(specSource, http);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"could not load the spec from {specSource}: {ex.Message}");
    return 2;
}

var wrapper = Wrapper.Scan(sourceDir, typeof(EsiClient).Assembly);

if (docsMode is not null)
{
    try
    {
        return RunDocs(spec, wrapper, sourceDir, docsMode == "write", pinnedDate);
    }
    catch (InvalidOperationException ex)
    {
        // The spec says something the generator cannot represent (e.g. a route accepting
        // several scopes): exit 2 = "did not complete", not 1 = "out of date".
        Console.Error.WriteLine($"cannot generate docs: {ex.Message}");
        return 2;
    }
}
var notWrapped = LoadAllowlist(sourceDir);
var coverage = CoverageCheck.Run(spec, wrapper, notWrapped);
var schema = runSchema ? SchemaCheck.Run(spec, wrapper) : null;
var exit = Report.Render(spec, wrapper, coverage, schema, issueBodyPath);

if (runProbe)
    exit |= await Probe.RunAsync(spec, wrapper, "https://esi.evetech.net", pinnedDate);

return exit;

static int RunDocs(Spec spec, Wrapper wrapper, string logicDir, bool write, string compatibilityDate)
{
    var root = Directory.GetParent(logicDir)!.Parent!.FullName;
    var overrides = DocOverrides.Load(Path.Combine(root, "tools", "SpecCheck", "docs.json"));
    var plan = DocPlan.Build(spec, wrapper, overrides);
    var writer = new DocWriter(plan, typeof(EsiClient).Assembly, root, compatibilityDate);
    var changes = writer.Render();

    var report = new List<string> { $"# docs report - compatibility date {compatibilityDate}", "" };
    report.Add($"## missing ({writer.Missing.Count}) - add text to tools/SpecCheck/docs.json");
    report.AddRange(writer.Missing.Distinct().OrderBy(x => x, StringComparer.Ordinal));
    report.Add("");
    report.Add($"## weak - the spec only restates the name; add text to docs.json ({writer.Weak.Distinct().Count()})");
    report.AddRange(writer.Weak.Distinct().OrderBy(x => x, StringComparer.Ordinal));
    report.Add("");
    report.Add($"## replaced hand-written docs ({writer.Replaced.Count})");
    report.AddRange(writer.Replaced.OrderBy(x => x, StringComparer.Ordinal));

    if (write)
    {
        foreach (var (path, text) in changes)
            File.WriteAllText(path, text, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: HasBom(path)));
        File.WriteAllLines(Path.Combine(root, "tools", "SpecCheck", "docs-report.txt"), report);
        Console.WriteLine($"wrote {changes.Count} file(s); {writer.Missing.Distinct().Count()} member(s) still undocumented (see tools/SpecCheck/docs-report.txt)");
        return 0;
    }

    if (changes.Count == 0)
    {
        Console.WriteLine("docs and scope table are current");
        return 0;
    }
    Console.WriteLine($"{changes.Count} file(s) are out of date with the spec - run spec-check --write-docs:");
    foreach (var path in changes.Keys.OrderBy(x => x, StringComparer.Ordinal))
        Console.WriteLine("  " + Path.GetRelativePath(root, path));
    return 1;
}

static bool HasBom(string path)
{
    if (!File.Exists(path))
        return true;
    using var stream = File.OpenRead(path);
    Span<byte> head = stackalloc byte[3];
    return stream.Read(head) == 3 && head[0] == 0xEF && head[1] == 0xBB && head[2] == 0xBF;
}

// tools/SpecCheck/allowlist.txt sits beside this project; sourceDir is <root>/ESI.NET/Logic.
static IReadOnlySet<string> LoadAllowlist(string logicDir)
{
    var root = Directory.GetParent(logicDir)?.Parent?.FullName;
    var path = root is null ? null : Path.Combine(root, "tools", "SpecCheck", "allowlist.txt");
    if (path is null || !File.Exists(path))
        return new HashSet<string>();

    return File.ReadAllLines(path)
        .Select(l => l.Trim())
        .Where(l => l.Length > 0 && !l.StartsWith('#'))
        .ToHashSet(StringComparer.Ordinal);
}

// Walk up from the working directory (and from the binary) until a folder holds
// ESI.NET.sln, then return its ESI.NET/Logic.
static string? LocateLogicDirectory()
{
    foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
    {
        for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
        {
            if (File.Exists(Path.Combine(dir.FullName, "ESI.NET.sln")))
            {
                var logic = Path.Combine(dir.FullName, "ESI.NET", "Logic");
                if (Directory.Exists(logic))
                    return logic;
            }
        }
    }
    return null;
}
