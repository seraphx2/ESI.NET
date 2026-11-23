using System.Text.Json;

namespace ESI.NET.Tools.SpecCheck.Docs;

/// <summary>
/// The hand-written half of the docs, from <c>tools/SpecCheck/docs.json</c>. The generator
/// owns every comment it writes, so text a person wrote must live here, not in the source
/// file - otherwise the next <c>--write-docs</c> would overwrite it.
/// </summary>
/// <remarks>
/// Lookup order for a model property: <see cref="Properties"/> (one property of one model),
/// then a real spec description, then <see cref="Fields"/> (by JSON name, shared by every
/// model), then a weak spec description. For a method parameter: the operation's own
/// <c>params</c>, then the spec, then <see cref="Parameters"/> (by C# name).
/// </remarks>
public sealed class DocOverrides
{
    /// <summary>Per operation (<c>"GET /characters/{character_id}/wallet"</c>).</summary>
    public Dictionary<string, OperationOverride> Operations { get; init; } = new();

    /// <summary>Method parameter text by C# parameter name, shared across methods.</summary>
    public Dictionary<string, string> Parameters { get; init; } = new();

    /// <summary>Model property text by JSON field name, shared across models.</summary>
    public Dictionary<string, string> Fields { get; init; } = new();

    /// <summary>Model property text by <c>Namespace.Type.Property</c> (nested types: <c>Outer.Inner</c>).</summary>
    public Dictionary<string, string> Properties { get; init; } = new();

    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    public static DocOverrides Load(string path)
        => File.Exists(path)
            ? JsonSerializer.Deserialize<DocOverrides>(File.ReadAllText(path), Json) ?? new DocOverrides()
            : new DocOverrides();
}

public sealed class OperationOverride
{
    /// <summary>Replaces the spec's summary + description.</summary>
    public string? Summary { get; init; }

    /// <summary>Extra remarks, after the generated ones.</summary>
    public string? Remarks { get; init; }

    /// <summary>Replaces the generated <c>&lt;returns&gt;</c>.</summary>
    public string? Returns { get; init; }

    /// <summary>Parameter text by C# parameter name, for this operation only.</summary>
    public Dictionary<string, string> Params { get; init; } = new();
}
