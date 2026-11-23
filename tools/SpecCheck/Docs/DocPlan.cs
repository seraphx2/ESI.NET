using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;

namespace ESI.NET.Tools.SpecCheck.Docs;

/// <summary>What one spec operation says, ready to be written onto the Logic method that calls it.</summary>
public sealed class OperationDoc
{
    /// <summary>The wrapper's own key (<c>"GET /characters/{character_id}/wallet"</c>), as <c>Execute</c> sees it.</summary>
    public required string Key { get; init; }
    public required SpecOperation Op { get; init; }
    public required bool Authenticated { get; init; }

    /// <summary>XML doc text.</summary>
    public required string Summary { get; init; }
    public required IReadOnlyList<string> Remarks { get; init; }
    public required string? ReturnsOverride { get; init; }

    public required string? Scope { get; init; }
    public required IReadOnlyList<string> Roles { get; init; }
    public required bool PagePaginated { get; init; }
    public required bool NoContent { get; init; }

    /// <summary>Spec parameters and request-body properties by JSON name -&gt; description.</summary>
    public required IReadOnlyDictionary<string, string?> SpecParams { get; init; }

    /// <summary>Description of the request body as a whole (for a body that is a bare array).</summary>
    public required string? BodyDescription { get; init; }
    public required bool BodyIsArray { get; init; }

    public required OperationOverride? Override { get; init; }
}

/// <summary>
/// Joins the spec to the wrapper: every implemented endpoint gets an <see cref="OperationDoc"/>,
/// every model property reachable from a response gets its spec description(s).
/// </summary>
public sealed class DocPlan
{
    public IReadOnlyDictionary<(string Class, string Method), OperationDoc> Methods { get; }

    /// <summary>Every implemented endpoint, keyed by the wrapper's normalized route.</summary>
    public IReadOnlyList<OperationDoc> Endpoints { get; }

    /// <summary>Model property -&gt; the descriptions the spec gives it (several when the model is shared).</summary>
    public IReadOnlyDictionary<PropertyInfo, PropertySpec> Properties { get; }

    public DocOverrides Overrides { get; }

    private DocPlan(Dictionary<(string, string), OperationDoc> methods, List<OperationDoc> endpoints,
        Dictionary<PropertyInfo, PropertySpec> properties, DocOverrides overrides)
    {
        Methods = methods;
        Endpoints = endpoints;
        Properties = properties;
        Overrides = overrides;
    }

    public static DocPlan Build(Spec spec, Wrapper wrapper, DocOverrides overrides)
    {
        var byKey = spec.Operations.ToDictionary(o => o.Key);
        var byLoose = spec.Operations.GroupBy(o => o.LooseKey).ToDictionary(g => g.Key, g => g.First());

        var methods = new Dictionary<(string, string), OperationDoc>();
        var endpoints = new List<OperationDoc>();
        var properties = new Dictionary<PropertyInfo, PropertySpec>();

        foreach (var endpoint in wrapper.Endpoints)
        {
            if (!byKey.TryGetValue(endpoint.Key, out var op) && !byLoose.TryGetValue(endpoint.LooseKey, out op))
                continue; // not in the spec - coverage check reports it

            var doc = Describe(spec, endpoint, op, overrides);
            endpoints.Add(doc);
            methods.TryAdd((endpoint.Class, endpoint.Method), doc);

            if (endpoint.ModelType is not null && op.ResponseSchema is { } schema)
                Walk(endpoint.ModelType, schema, spec, properties, new HashSet<Type>(), 0);
        }

        return new DocPlan(methods, endpoints, properties, overrides);
    }

    private static OperationDoc Describe(Spec spec, ImplementedEndpoint endpoint, SpecOperation op, DocOverrides overrides)
    {
        var raw = op.Raw;
        overrides.Operations.TryGetValue(op.Key, out var over);

        string summary;
        if (over?.Summary is { } own)
        {
            summary = DocText.Sentence(own, isXml: true);
        }
        else
        {
            var title = Str(raw, "summary");
            var description = Str(raw, "description");
            if (title is null || (description is not null && SameText(title, description)))
                summary = DocText.Sentence(description ?? title ?? op.OperationId);
            else
                summary = DocText.Sentence(title) + (description is null ? "" : " " + DocText.Sentence(description));
        }

        string? scope = null;
        if (raw.TryGetProperty("security", out var security) && security.ValueKind == JsonValueKind.Array)
        {
            // OpenAPI: the array lists alternatives (any one will do), and each alternative lists
            // scopes that are all required. The runtime table holds one required scope per route,
            // which is what every ESI operation declares today. Anything else would make the
            // pre-flight check refuse calls ESI accepts, so stop rather than guess.
            var alternatives = security.EnumerateArray()
                .Select(req => req.EnumerateObject()
                    .SelectMany(p => p.Value.EnumerateArray())
                    .Select(s => s.GetString())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList())
                .Where(a => a.Count > 0)
                .ToList();
            if (alternatives.Count > 1 || alternatives.Any(a => a.Count > 1))
                throw new InvalidOperationException(
                    $"{op.Key} declares security {security.GetRawText()}: more than one scope or alternative. "
                    + "EsiEndpointRequirement holds a single required scope; extend it (and EsiRequest's check) before regenerating.");
            scope = alternatives.FirstOrDefault()?[0];
        }

        var roles = raw.TryGetProperty("x-required-roles", out var r) && r.ValueKind == JsonValueKind.Array
            ? r.EnumerateArray().Select(x => x.GetString()!).Where(x => x is not null).ToList()
            : new List<string>();

        var specParams = new Dictionary<string, string?>(StringComparer.Ordinal);
        var pagePaginated = false;
        if (raw.TryGetProperty("parameters", out var parameters))
        {
            foreach (var p in parameters.EnumerateArray())
            {
                var param = spec.Resolve(p);
                var where = Str(param, "in");
                var name = Str(param, "name");
                if (name is null || where is not ("path" or "query"))
                    continue;
                if (name == "page")
                    pagePaginated = true;
                specParams[name] = Str(param, "description")
                                   ?? (param.TryGetProperty("schema", out var s) ? Str(spec.Resolve(s), "description") : null);
            }
        }

        string? bodyDescription = null;
        var bodyIsArray = false;
        if (raw.TryGetProperty("requestBody", out var requestBody))
        {
            requestBody = spec.Resolve(requestBody);
            bodyDescription = Str(requestBody, "description");
            if (requestBody.TryGetProperty("content", out var content)
                && content.TryGetProperty("application/json", out var json)
                && json.TryGetProperty("schema", out var bodySchema))
            {
                var declared = bodySchema;
                bodySchema = spec.Resolve(bodySchema);
                bodyDescription ??= Str(declared, "description") ?? Str(bodySchema, "description");
                bodyIsArray = Str(bodySchema, "type") == "array" || bodySchema.TryGetProperty("items", out _);
                if (bodySchema.TryGetProperty("properties", out var props))
                    foreach (var prop in props.EnumerateObject())
                        specParams.TryAdd(prop.Name, Str(prop.Value, "description") ?? Str(spec.Resolve(prop.Value), "description"));
            }
        }

        var remarks = new List<string>
        {
            $"Calls <c>{op.Method} {op.Path}</c>.",
        };
        if (scope is not null)
            remarks.Add($"Requires the <c>{scope}</c> scope.");
        if (roles.Count == 1)
            remarks.Add($"The character must also hold the <c>{roles[0]}</c> corporation role.");
        else if (roles.Count > 1)
            remarks.Add("The character must also hold one of these corporation roles: "
                        + string.Join(", ", roles.Select(x => $"<c>{x}</c>")) + ".");
        if (pagePaginated)
            remarks.Add("Paginated: <see cref=\"EsiCallOptions.Page\"/> picks the page and "
                        + "<see cref=\"EsiResponse{T}.Pages\"/> gives the page count.");
        if (Str(raw, "x-pagination") == "cursor")
            remarks.Add("Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.");
        if (Str(raw, "x-client-cache-ttl") is { } ttl && int.TryParse(ttl, out var seconds) && seconds > 0)
            remarks.Add($"ESI caches the response for {DocText.Duration(seconds)}.");
        if (over?.Remarks is { } extra)
            remarks.Add(DocText.Sentence(extra, isXml: true));

        var noContent = raw.TryGetProperty("responses", out var responses)
                        && responses.TryGetProperty("204", out _)
                        && !responses.TryGetProperty("200", out _)
                        && !responses.TryGetProperty("201", out _);

        return new OperationDoc
        {
            Key = endpoint.Key,
            Op = op,
            Authenticated = endpoint.Authenticated,
            Summary = summary,
            Remarks = remarks,
            ReturnsOverride = over?.Returns,
            Scope = scope,
            Roles = roles,
            PagePaginated = pagePaginated,
            NoContent = noContent,
            SpecParams = specParams,
            BodyDescription = bodyDescription,
            BodyIsArray = bodyIsArray,
            Override = over,
        };
    }

    private static readonly HashSet<string> Filler = new(StringComparer.Ordinal)
    {
        "a", "an", "the", "of", "for", "to", "get", "gets", "return", "returns", "retrieve", "list", "lists", "s", "your", "given",
    };

    /// <summary>
    /// ESI's summary and description often say the same thing ("Get a character's wallet
    /// balance" / "Returns a character's wallet balance"); then only one is kept.
    /// </summary>
    private static bool SameText(string? a, string? b)
    {
        static HashSet<string> Words(string? s) => System.Text.RegularExpressions.Regex
            .Split((s ?? "").ToLowerInvariant(), "[^a-z0-9]+")
            .Where(w => w.Length > 0 && !Filler.Contains(w))
            .Select(w => w.EndsWith('s') && w.Length > 3 ? w[..^1] : w)
            .ToHashSet(StringComparer.Ordinal);

        var x = Words(a);
        var y = Words(b);
        if (x.Count == 0 || y.Count == 0)
            return x.Count == y.Count;
        var shared = x.Intersect(y).Count();
        return shared >= 0.75 * x.Count && y.Count - shared <= 2;
    }

    private static string? Str(JsonElement element, string name)
        => element.ValueKind == JsonValueKind.Object && element.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String
            ? v.GetString()
            : null;

    // ---- model properties ------------------------------------------------

    private const int MaxDepth = 12;

    /// <summary>Walks a CLR model and its spec schema side by side, recording each property's description.</summary>
    private static void Walk(Type clr, JsonElement schema, Spec spec, Dictionary<PropertyInfo, PropertySpec> into, HashSet<Type> path, int depth)
    {
        if (depth > MaxDepth)
            return;

        clr = Nullable.GetUnderlyingType(clr) ?? clr;
        schema = spec.Resolve(schema);
        if (schema.ValueKind != JsonValueKind.Object || clr == typeof(string) || clr.IsPrimitive || clr.IsEnum
            || clr == typeof(decimal) || clr == typeof(DateTime) || clr == typeof(object))
            return;

        if (DictionaryValueType(clr) is { } valueType)
        {
            if (schema.TryGetProperty("additionalProperties", out var additional) && additional.ValueKind == JsonValueKind.Object)
                Walk(valueType, additional, spec, into, path, depth + 1);
            return;
        }

        if (ElementType(clr) is { } elementType)
        {
            if (schema.TryGetProperty("items", out var items))
                Walk(elementType, items, spec, into, path, depth + 1);
            return;
        }

        if (!clr.IsClass || !path.Add(clr))
            return;

        try
        {
            var members = Members(schema, spec);
            foreach (var property in clr.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetIndexParameters().Length > 0)
                    continue;
                var json = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? property.Name;
                if (!members.TryGetValue(json, out var member))
                    continue;

                var declared = property.DeclaringType == property.ReflectedType
                    ? property
                    : property.DeclaringType!.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance)!;
                if (!into.TryGetValue(declared, out var entry))
                    into[declared] = entry = new PropertySpec(json);

                var resolved = spec.Resolve(member);
                var description = Str(member, "description") ?? Str(resolved, "description") ?? Str(member, "title");
                if (!string.IsNullOrWhiteSpace(description))
                    entry.Descriptions.Add(description);

                if (resolved.TryGetProperty("enum", out var values) && values.ValueKind == JsonValueKind.Array)
                    entry.EnumValues = values.EnumerateArray().Select(v => v.ToString()).ToList();
                else if (resolved.TryGetProperty("items", out var itemSchema)
                         && spec.Resolve(itemSchema).TryGetProperty("enum", out var itemValues))
                    entry.EnumValues = itemValues.EnumerateArray().Select(v => v.ToString()).ToList();

                Walk(property.PropertyType, member, spec, into, path, depth + 1);
            }
        }
        finally
        {
            path.Remove(clr);
        }
    }

    /// <summary>An object schema's members, with <c>allOf</c> / <c>oneOf</c> / <c>anyOf</c> merged.</summary>
    private static Dictionary<string, JsonElement> Members(JsonElement schema, Spec spec)
    {
        var members = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        schema = spec.Resolve(schema);
        if (schema.TryGetProperty("properties", out var props))
            foreach (var p in props.EnumerateObject())
                members.TryAdd(p.Name, p.Value);
        foreach (var combinator in new[] { "allOf", "oneOf", "anyOf" })
            if (schema.TryGetProperty(combinator, out var parts) && parts.ValueKind == JsonValueKind.Array)
                foreach (var part in parts.EnumerateArray())
                    foreach (var (k, v) in Members(part, spec))
                        members.TryAdd(k, v);
        return members;
    }

    private static Type? DictionaryValueType(Type type)
    {
        var interfaces = type.IsInterface ? type.GetInterfaces().Append(type) : type.GetInterfaces();
        var dictionary = interfaces.FirstOrDefault(i => i.IsGenericType
            && (i.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                || i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)));
        return dictionary?.GetGenericArguments()[1];
    }

    private static Type? ElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();
        var enumerable = type.GetInterfaces().Append(type)
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        return enumerable?.GetGenericArguments()[0];
    }
}

/// <summary>What the spec says about one model property, across every endpoint that returns the model.</summary>
public sealed class PropertySpec(string jsonName)
{
    public string JsonName { get; } = jsonName;
    public List<string> Descriptions { get; } = new();
    public List<string>? EnumValues { get; set; }
}
