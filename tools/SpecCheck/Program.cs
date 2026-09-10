using ESI.NET;
using ESI.NET.Tools.SpecCheck;

// spec-check [--spec <url|path>] [--source <dir>] [--no-schema]
//
//   --spec       OpenAPI document. Default: the live ESI meta spec.
//   --source     ESI.NET/Logic directory. Default: auto-detected from the repo root.
//   --no-schema  Tier 1 (coverage) only; skip Tier 2 (schema drift).
//
// Any finding fails the run. Deliberate exceptions go in tools/SpecCheck/allowlist.txt.

var specSource = "https://esi.evetech.net/meta/openapi.json";
string? sourceDir = null;
var runSchema = true;

for (var i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--spec" when i + 1 < args.Length: specSource = args[++i]; break;
        case "--source" when i + 1 < args.Length: sourceDir = args[++i]; break;
        case "--no-schema": runSchema = false; break;
        case "-h" or "--help":
            Console.WriteLine("usage: spec-check [--spec <url|path>] [--source <dir>] [--no-schema]");
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
var notWrapped = LoadAllowlist(sourceDir);
var coverage = CoverageCheck.Run(spec, wrapper, notWrapped);
var schema = runSchema ? SchemaCheck.Run(spec, wrapper) : null;
return Report.Render(spec, wrapper, coverage, schema);

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
