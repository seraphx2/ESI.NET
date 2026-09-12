namespace ESI.NET.Tools.SpecCheck;

public sealed record CoverageRow(string Tag, int Covered, int Total)
{
    public IReadOnlyList<SpecOperation> Missing { get; init; } = Array.Empty<SpecOperation>();
}

public sealed class CoverageResult
{
    public required IReadOnlyList<CoverageRow> Rows { get; init; }

    /// <summary>In the wrapper, but the spec has no such (method, path). Latent 404s.</summary>
    public required IReadOnlyList<ImplementedEndpoint> Orphaned { get; init; }

    /// <summary>Same route shape, different <c>{parameter}</c> name - matches loosely, not exactly.</summary>
    public required IReadOnlyList<(ImplementedEndpoint Wrapper, SpecOperation Spec)> ParamNameMismatch { get; init; }

    public required int TotalCovered { get; init; }
    public required int TotalOperations { get; init; }
    public required IReadOnlyList<Finding> Findings { get; init; }
}

/// <summary>
/// Tier 1. Set-difference between the spec's (method, path) operations and the
/// wrapper's. Orphaned endpoints, missing endpoints, and parameter-name drift all
/// fail the run; endpoints listed in the allowlist are exempt from the missing check.
/// </summary>
public static class CoverageCheck
{
    public static CoverageResult Run(Spec spec, Wrapper wrapper, IReadOnlySet<string> notWrapped)
    {
        var specByKey = spec.Operations
            .GroupBy(o => o.Key)
            .ToDictionary(g => g.Key, g => g.First());

        var specByLooseKey = spec.Operations.ToLookup(o => o.LooseKey);
        var wrapperKeys = wrapper.Endpoints.Select(e => e.Key).ToHashSet();

        var findings = new List<Finding>();

        // ---- coverage per tag -------------------------------------------------
        var rows = spec.Operations
            .GroupBy(o => o.Tag)
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
            .Select(g =>
            {
                var missing = g.Where(o => !wrapperKeys.Contains(o.Key)).OrderBy(o => o.Key).ToList();
                return new CoverageRow(g.Key, g.Count() - missing.Count, g.Count()) { Missing = missing };
            })
            .ToList();

        foreach (var op in rows.SelectMany(r => r.Missing))
        {
            var looselyCovered = wrapper.Endpoints.Any(e => e.LooseKey == op.LooseKey);
            if (looselyCovered)
                continue; // handled below as a parameter-name mismatch
            if (notWrapped.Contains(op.Key))
                continue; // deliberately not wrapped - see tools/SpecCheck/allowlist.txt
            findings.Add(new Finding(
                Severity.Warning,
                "missing-endpoint", op.Key,
                $"in the spec (tag {op.Tag}, {op.OperationId}) but not implemented"));
        }

        // ---- orphaned + parameter-name drift --------------------------------
        var orphaned = new List<ImplementedEndpoint>();
        var paramMismatch = new List<(ImplementedEndpoint, SpecOperation)>();

        foreach (var endpoint in wrapper.Endpoints.Where(e => !specByKey.ContainsKey(e.Key)))
        {
            var loose = specByLooseKey[endpoint.LooseKey].ToList();
            if (loose.Count > 0)
            {
                paramMismatch.Add((endpoint, loose[0]));
                findings.Add(new Finding(
                    Severity.Warning,
                    "parameter-name", endpoint.Key,
                    $"{endpoint.Class}.{endpoint.Method} - route matches {loose[0].Key} but the parameter name(s) differ"));
            }
            else
            {
                orphaned.Add(endpoint);
                findings.Add(new Finding(
                    Severity.Error,
                    "orphaned-endpoint", endpoint.Key,
                    $"{endpoint.Class}.{endpoint.Method} - implemented but absent from the spec (renamed or removed upstream)"));
            }
        }

        // How SpecCheck read a route, not an API change - informational only.
        foreach (var warning in wrapper.Warnings)
            findings.Add(new Finding(Severity.Info, "scan", "-", warning));

        var covered = spec.Operations.Count(o => wrapperKeys.Contains(o.Key));

        return new CoverageResult
        {
            Rows = rows,
            Orphaned = orphaned,
            ParamNameMismatch = paramMismatch,
            TotalCovered = covered,
            TotalOperations = spec.Operations.Count,
            Findings = findings,
        };
    }
}
