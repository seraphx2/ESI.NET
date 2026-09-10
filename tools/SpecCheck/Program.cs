using ESI.NET;
using ESI.NET.Tools.SpecCheck;

// spec-check [--spec <url|path>] [--source <dir>] [--strict] [--no-schema]
//
//   --spec       OpenAPI document. Default: the live ESI meta spec.
//   --source     ESI.NET/Logic directory. Default: auto-detected from the repo root.
//   --strict     Warnings fail the build too.
//   --no-schema  Tier 1 (coverage) only; skip Tier 2 (schema drift).

var specSource = "https://esi.evetech.net/meta/openapi.json";
string? sourceDir = null;
var strict = false;
var runSchema = true;

for (var i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--spec" when i + 1 < args.Length: specSource = args[++i]; break;
        case "--source" when i + 1 < args.Length: sourceDir = args[++i]; break;
        case "--strict": strict = true; break;
        case "--no-schema": runSchema = false; break;
        case "-h" or "--help":
            Console.WriteLine("usage: spec-check [--spec <url|path>] [--source <dir>] [--strict] [--no-schema]");
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
var coverage = CoverageCheck.Run(spec, wrapper, strict);
var schema = runSchema ? SchemaCheck.Run(spec, wrapper, strict) : null;
return Report.Render(spec, wrapper, coverage, schema, strict);

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
