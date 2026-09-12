using System.Text.Json;
using System.Text.RegularExpressions;

namespace ESI.NET.Tools.SpecCheck;

/// <summary>One (method, path) operation lifted from the OpenAPI document.</summary>
public sealed record SpecOperation(
    string Method,               // GET, POST, PUT, DELETE
    string Path,                 // "/alliances/{alliance_id}" - normalized, no trailing slash
    string OperationId,
    string Tag,
    JsonElement? ResponseSchema) // resolved 200/201 application/json schema (Tier 2); null if none
{
    public string Key => $"{Method} {Path}";
    public string LooseKey => $"{Method} {Spec.ParamAgnostic(Path)}";
}

/// <summary>
/// The parsed ESI OpenAPI 3.1 document. Enumerates operations and resolves local
/// <c>$ref</c> pointers into <c>#/components/schemas</c>.
/// </summary>
public sealed class Spec
{
    private readonly JsonDocument _doc;
    private JsonElement Root => _doc.RootElement;

    public string OpenApiVersion { get; }
    public string InfoVersion { get; }
    public IReadOnlyList<SpecOperation> Operations { get; }

    private Spec(JsonDocument doc)
    {
        _doc = doc;
        OpenApiVersion = Root.TryGetProperty("openapi", out var v) ? v.GetString() ?? "" : "";
        InfoVersion = Root.TryGetProperty("info", out var info) && info.TryGetProperty("version", out var iv)
            ? iv.GetString() ?? "" : "";
        Operations = EnumerateOperations().ToList();
    }

    public static async Task<Spec> LoadAsync(string source, HttpClient http)
    {
        var json = source.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            ? await http.GetStringAsync(source)
            : await File.ReadAllTextAsync(source);
        return new Spec(JsonDocument.Parse(json));
    }

    private static readonly string[] HttpMethods = { "get", "post", "put", "delete", "patch" };

    private IEnumerable<SpecOperation> EnumerateOperations()
    {
        if (!Root.TryGetProperty("paths", out var paths))
            yield break;

        foreach (var path in paths.EnumerateObject())
        {
            var normalized = NormalizePath(path.Name);
            foreach (var method in HttpMethods)
            {
                if (!path.Value.TryGetProperty(method, out var op))
                    continue;

                var operationId = op.TryGetProperty("operationId", out var oid) ? oid.GetString() ?? "" : "";
                var tag = op.TryGetProperty("tags", out var tags)
                          && tags.ValueKind == JsonValueKind.Array
                          && tags.GetArrayLength() > 0
                    ? tags[0].GetString() ?? "(untagged)"
                    : "(untagged)";

                yield return new SpecOperation(
                    method.ToUpperInvariant(),
                    normalized,
                    operationId,
                    tag,
                    ResolveResponseSchema(op));
            }
        }
    }

    private JsonElement? ResolveResponseSchema(JsonElement op)
    {
        if (!op.TryGetProperty("responses", out var responses))
            return null;

        foreach (var code in new[] { "200", "201" })
        {
            if (responses.TryGetProperty(code, out var response)
                && response.TryGetProperty("content", out var content)
                && content.TryGetProperty("application/json", out var json)
                && json.TryGetProperty("schema", out var schema))
                return Resolve(schema);
        }
        return null;
    }

    /// <summary>Follows a local <c>$ref</c> chain to the concrete schema. Inline schemas pass through.</summary>
    public JsonElement Resolve(JsonElement schema) => Resolve(schema, new HashSet<string>());

    private JsonElement Resolve(JsonElement schema, HashSet<string> seen)
    {
        while (schema.ValueKind == JsonValueKind.Object
               && schema.TryGetProperty("$ref", out var refElement))
        {
            var pointer = refElement.GetString() ?? "";
            if (!seen.Add(pointer))
                break; // cycle
            var target = ResolvePointer(pointer);
            if (target is null)
                break;
            schema = target.Value;
        }
        return schema;
    }

    private JsonElement? ResolvePointer(string pointer)
    {
        if (!pointer.StartsWith("#/"))
            return null; // external refs unsupported; ESI's spec is self-contained

        var current = Root;
        foreach (var raw in pointer[2..].Split('/'))
        {
            var segment = raw.Replace("~1", "/").Replace("~0", "~");
            if (current.ValueKind != JsonValueKind.Object || !current.TryGetProperty(segment, out var next))
                return null;
            current = next;
        }
        return current;
    }

    /// <summary>Leading slash, no trailing slash. <c>"/alliances/{id}/"</c> -&gt; <c>"/alliances/{id}"</c>.</summary>
    public static string NormalizePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return "/";
        if (!path.StartsWith('/'))
            path = "/" + path;
        return path.Length > 1 ? path.TrimEnd('/') : path;
    }

    /// <summary>Replaces every <c>{param}</c> with <c>{}</c> so paths match regardless of parameter names.</summary>
    public static string ParamAgnostic(string path) => Regex.Replace(path, "{[^}]+}", "{}");
}
