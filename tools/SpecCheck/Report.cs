using System.Text;

namespace ESI.NET.Tools.SpecCheck;

/// <summary>
/// Renders the check results to stdout, appends a markdown summary to
/// <c>$GITHUB_STEP_SUMMARY</c> when running in Actions, and returns the process
/// exit code (non-zero if any <see cref="Severity.Error"/>, or any
/// <see cref="Severity.Warning"/> when <paramref name="strict"/>).
/// </summary>
public static class Report
{
    public static int Render(Spec spec, Wrapper wrapper, CoverageResult coverage, bool strict)
    {
        var findings = coverage.Findings;
        var text = BuildText(spec, wrapper, coverage);
        Console.WriteLine(text);

        var summaryPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
        if (!string.IsNullOrEmpty(summaryPath))
        {
            try { File.AppendAllText(summaryPath, BuildMarkdown(spec, coverage)); }
            catch (Exception ex) { Console.Error.WriteLine($"(could not write GITHUB_STEP_SUMMARY: {ex.Message})"); }
        }

        var errors = findings.Count(f => f.Severity == Severity.Error);
        var warnings = findings.Count(f => f.Severity == Severity.Warning);

        Console.WriteLine();
        Console.WriteLine($"{errors} error(s), {warnings} warning(s). "
                          + $"Coverage {coverage.TotalCovered}/{coverage.TotalOperations} "
                          + $"({Percent(coverage.TotalCovered, coverage.TotalOperations)}).");

        var failed = errors > 0 || (strict && warnings > 0);
        Console.WriteLine(failed ? "RESULT: drift detected." : "RESULT: clean.");
        return failed ? 1 : 0;
    }

    private static string BuildText(Spec spec, Wrapper wrapper, CoverageResult coverage)
    {
        var sb = new StringBuilder();
        sb.AppendLine("ESI.NET spec-check");
        sb.AppendLine($"  openapi {spec.OpenApiVersion}, info.version {spec.InfoVersion}");
        sb.AppendLine($"  spec operations : {spec.Operations.Count}");
        sb.AppendLine($"  wrapper endpoints: {wrapper.Endpoints.Count}");
        sb.AppendLine();

        sb.AppendLine("COVERAGE BY TAG");
        foreach (var row in coverage.Rows)
        {
            sb.AppendLine($"  {row.Tag,-26} {row.Covered,3}/{row.Total,-3}  {Percent(row.Covered, row.Total)}");
            foreach (var op in row.Missing)
                sb.AppendLine($"      - missing  {op.Key}");
        }
        sb.AppendLine($"  {"TOTAL",-26} {coverage.TotalCovered,3}/{coverage.TotalOperations,-3}");
        sb.AppendLine();

        sb.AppendLine($"ORPHANED (implemented, not in spec)  [{coverage.Orphaned.Count}]");
        foreach (var e in coverage.Orphaned)
            sb.AppendLine($"  {e.Key,-55} {e.Class}.{e.Method}");
        sb.AppendLine();

        sb.AppendLine($"PARAMETER-NAME DRIFT  [{coverage.ParamNameMismatch.Count}]");
        foreach (var (w, s) in coverage.ParamNameMismatch)
            sb.AppendLine($"  {w.Key}  ~  {s.Key}   ({w.Class}.{w.Method})");
        sb.AppendLine();

        var scanWarnings = coverage.Findings.Where(f => f.Category == "scan").ToList();
        if (scanWarnings.Count > 0)
        {
            sb.AppendLine($"SCANNER WARNINGS  [{scanWarnings.Count}]");
            foreach (var f in scanWarnings)
                sb.AppendLine($"  {f.Message}");
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    private static string BuildMarkdown(Spec spec, CoverageResult coverage)
    {
        var sb = new StringBuilder();
        sb.AppendLine("## ESI.NET spec-check");
        sb.AppendLine();
        sb.AppendLine($"`openapi {spec.OpenApiVersion}` · `info.version {spec.InfoVersion}` · "
                      + $"coverage **{coverage.TotalCovered}/{coverage.TotalOperations}** "
                      + $"({Percent(coverage.TotalCovered, coverage.TotalOperations)})");
        sb.AppendLine();
        sb.AppendLine("| Tag | Covered | |");
        sb.AppendLine("| --- | --- | --- |");
        foreach (var row in coverage.Rows)
            sb.AppendLine($"| {row.Tag} | {row.Covered}/{row.Total} | {(row.Covered == row.Total ? "✅" : "⚠️")} |");
        sb.AppendLine();

        if (coverage.Orphaned.Count > 0)
        {
            sb.AppendLine($"### ❌ Orphaned — implemented, not in spec ({coverage.Orphaned.Count})");
            foreach (var e in coverage.Orphaned)
                sb.AppendLine($"- `{e.Key}` — {e.Class}.{e.Method}");
            sb.AppendLine();
        }

        var missing = coverage.Rows.SelectMany(r => r.Missing).ToList();
        if (missing.Count > 0)
        {
            sb.AppendLine($"<details><summary>⚠️ Missing — in spec, not implemented ({missing.Count})</summary>");
            sb.AppendLine();
            foreach (var op in missing)
                sb.AppendLine($"- `{op.Key}` — {op.Tag}");
            sb.AppendLine();
            sb.AppendLine("</details>");
            sb.AppendLine();
        }

        if (coverage.ParamNameMismatch.Count > 0)
        {
            sb.AppendLine($"### ⚠️ Parameter-name drift ({coverage.ParamNameMismatch.Count})");
            foreach (var (w, s) in coverage.ParamNameMismatch)
                sb.AppendLine($"- `{w.Key}` ~ `{s.Key}` — {w.Class}.{w.Method}");
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string Percent(int n, int d) => d == 0 ? "n/a" : $"{100.0 * n / d:0.#}%";
}
