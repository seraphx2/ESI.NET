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
    public static int Render(Spec spec, Wrapper wrapper, CoverageResult coverage, SchemaResult? schema, bool strict)
    {
        var findings = coverage.Findings.Concat(schema?.Findings ?? Enumerable.Empty<Finding>()).ToList();

        Console.WriteLine(BuildText(spec, wrapper, coverage));
        if (schema is not null)
            Console.WriteLine(BuildSchemaText(schema));

        var summaryPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
        if (!string.IsNullOrEmpty(summaryPath))
        {
            try
            {
                File.AppendAllText(summaryPath, BuildMarkdown(spec, coverage));
                if (schema is not null)
                    File.AppendAllText(summaryPath, BuildSchemaMarkdown(schema));
            }
            catch (Exception ex) { Console.Error.WriteLine($"(could not write GITHUB_STEP_SUMMARY: {ex.Message})"); }
        }

        var errors = findings.Count(f => f.Severity == Severity.Error);
        var warnings = findings.Count(f => f.Severity == Severity.Warning);
        var infos = findings.Count(f => f.Severity == Severity.Info);

        Console.WriteLine();
        Console.WriteLine($"{errors} error(s), {warnings} warning(s), {infos} info. "
                          + $"Coverage {coverage.TotalCovered}/{coverage.TotalOperations} "
                          + $"({Percent(coverage.TotalCovered, coverage.TotalOperations)})"
                          + (schema is not null ? $"; schema-checked {schema.Checked} endpoint(s)." : "."));

        var failed = errors > 0 || (strict && warnings > 0);
        Console.WriteLine(failed ? "RESULT: drift detected." : "RESULT: clean.");
        return failed ? 1 : 0;
    }

    private static string BuildSchemaText(SchemaResult schema)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("SCHEMA DRIFT (Tier 2)");
        sb.AppendLine($"  checked {schema.Checked} endpoint(s); "
                      + $"skipped {schema.SkippedNoSchema} (no JSON schema), {schema.SkippedNoModel} (model type unresolved)");

        var byCategory = schema.Findings
            .GroupBy(f => (f.Severity, f.Category))
            .OrderByDescending(g => g.Key.Severity)
            .ThenByDescending(g => g.Count());
        foreach (var group in byCategory)
            sb.AppendLine($"    {group.Key.Severity,-7} {group.Key.Category,-26} {group.Count()}");

        foreach (var endpoint in schema.Findings.GroupBy(f => f.Where).OrderBy(g => g.Key))
        {
            sb.AppendLine();
            sb.AppendLine($"  {endpoint.Key}");
            foreach (var f in endpoint.OrderByDescending(x => x.Severity))
                sb.AppendLine($"    {f.Severity.ToString().ToUpperInvariant(),-7} {f.Message}");
        }

        return sb.ToString().TrimEnd();
    }

    private static string BuildSchemaMarkdown(SchemaResult schema)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("## Schema drift (Tier 2)");
        sb.AppendLine();
        sb.AppendLine($"Checked **{schema.Checked}** endpoints. "
                      + $"Skipped {schema.SkippedNoSchema} (no schema) + {schema.SkippedNoModel} (unresolved model).");
        sb.AppendLine();

        if (schema.Findings.Count == 0)
        {
            sb.AppendLine("No schema drift. ✅");
            return sb.ToString();
        }

        sb.AppendLine("| Severity | Category | Count |");
        sb.AppendLine("| --- | --- | --- |");
        foreach (var group in schema.Findings
                     .GroupBy(f => (f.Severity, f.Category))
                     .OrderByDescending(g => g.Key.Severity).ThenByDescending(g => g.Count()))
            sb.AppendLine($"| {group.Key.Severity} | {group.Key.Category} | {group.Count()} |");
        sb.AppendLine();

        sb.AppendLine($"<details><summary>{schema.Findings.Count} finding(s) by endpoint</summary>");
        sb.AppendLine();
        foreach (var endpoint in schema.Findings.GroupBy(f => f.Where).OrderBy(g => g.Key))
        {
            sb.AppendLine($"**`{endpoint.Key}`**");
            foreach (var f in endpoint.OrderByDescending(x => x.Severity))
                sb.AppendLine($"- {f.Severity}: {f.Message}");
            sb.AppendLine();
        }
        sb.AppendLine("</details>");
        return sb.ToString();
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
