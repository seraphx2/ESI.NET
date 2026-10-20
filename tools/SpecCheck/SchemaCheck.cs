using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace ESI.NET.Tools.SpecCheck;

public sealed class SchemaResult
{
    public required IReadOnlyList<Finding> Findings { get; init; }
    public required int Checked { get; init; }
    public required int SkippedNoSchema { get; init; }
    public required int SkippedNoModel { get; init; }
}

/// <summary>
/// Tier 2. For every covered endpoint, flattens the <c>EsiResponse&lt;T&gt;</c> model and
/// the resolved 200 schema into <see cref="Node"/> trees and walks them together.
/// </summary>
public static class SchemaCheck
{
    private const int MaxDepth = 12;

    public static SchemaResult Run(Spec spec, Wrapper wrapper)
    {
        var specByKey = spec.Operations
            .GroupBy(o => o.Key)
            .ToDictionary(g => g.Key, g => g.First());

        var findings = new List<Finding>();
        int noSchema = 0, noModel = 0;

        // Pass 1: build the (model, schema) node trees for every checkable endpoint.
        var pairs = new List<(string Key, Node Model, Node Schema)>();
        foreach (var endpoint in wrapper.Endpoints)
        {
            if (!specByKey.TryGetValue(endpoint.Key, out var op))
                continue; // orphaned - Tier 1's problem

            if (op.ResponseSchema is not { } schemaElement)
            {
                noSchema++;
                continue;
            }
            if (endpoint.ModelType is not { } clrType)
            {
                noModel++;
                continue;
            }

            pairs.Add((endpoint.Key,
                FromClr(clrType, 0, new Dictionary<Type, Node>()),
                FromSchema(schemaElement, spec, 0, new Dictionary<string, Node?>(StringComparer.Ordinal))));
        }

        // For each CLR object type, the union of every schema property name lined up
        // against it across all endpoints. A model class serving several endpoints is a
        // superset of any one of them; a field only counts as "extra" if it appears in
        // NONE of that type's schemas (genuinely dropped upstream).
        var knownByType = new Dictionary<Type, HashSet<string>>();
        foreach (var (_, model, schema) in pairs)
            CollectKnownProps(model, schema, knownByType, new HashSet<(Node, Node)>());

        // Pass 2: the actual comparison.
        foreach (var (key, model, schema) in pairs)
            Compare(model, schema, "Data", key, knownByType, findings);

        return new SchemaResult
        {
            Findings = findings,
            Checked = pairs.Count,
            SkippedNoSchema = noSchema,
            SkippedNoModel = noModel,
        };
    }

    /// <summary>
    /// Records, per CLR object type, every schema property name that ever lines up with it.
    /// </summary>
    private static void CollectKnownProps(
        Node model, Node schema, Dictionary<Type, HashSet<string>> acc, HashSet<(Node, Node)> visited)
    {
        if (!visited.Add((model, schema)))
            return;

        // A dictionary model lined up against a fixed-property schema: every member
        // lines up with the dictionary's value type (mirrors Walk).
        if (model.Kind == NodeKind.Map && schema.Kind == NodeKind.Object)
        {
            foreach (var member in schema.Members.Values)
                CollectKnownProps(model.Values!, member.Node, acc, visited);
            return;
        }

        if (model.Kind != schema.Kind)
            return;

        switch (model.Kind)
        {
            case NodeKind.Object:
                if (model.ClrType is { } type)
                {
                    if (!acc.TryGetValue(type, out var names))
                        acc[type] = names = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var name in schema.Members.Keys)
                        names.Add(name);
                }
                foreach (var (name, schemaMember) in schema.Members)
                    if (model.Members.TryGetValue(name, out var modelMember))
                        CollectKnownProps(modelMember.Node, schemaMember.Node, acc, visited);
                break;

            case NodeKind.Array:
                if (model.Items is not null && schema.Items is not null)
                    CollectKnownProps(model.Items, schema.Items, acc, visited);
                break;

            case NodeKind.Map:
                CollectKnownProps(model.Values!, schema.Values!, acc, visited);
                break;
        }
    }

    // ---- CLR type -> Node ---------------------------------------------------

    /// <param name="inProgress">Class types currently being built, so a recursive
    /// reference closes a cycle onto the outer node instead of recursing forever.</param>
    public static Node FromClr(Type type, int depth, Dictionary<Type, Node> inProgress)
    {
        // A property is null-tolerant if it is Nullable<T> or a reference type
        // (string, a class). A bare value type (long, bool, DateTime, an enum) is not.
        var nullTolerant = !type.IsValueType || Nullable.GetUnderlyingType(type) is not null;
        type = Nullable.GetUnderlyingType(type) is { } underlying ? underlying : type;

        if (TryScalar(type, out var scalar)) // also handles enums
        {
            scalar.Nullable = nullTolerant;
            return scalar;
        }

        if (type == typeof(object))
            return Node.Any();

        if (depth >= MaxDepth)
            return Node.Unknown("max depth");

        // Before ElementType: a dictionary is also an IEnumerable<KeyValuePair<,>>.
        if (DictionaryValueType(type) is { } valueType)
            return Node.Map(FromClr(valueType, depth + 1, inProgress));
        if (typeof(IDictionary).IsAssignableFrom(type))
            return Node.Map(Node.Any());

        if (ElementType(type) is { } element)
            return Node.Array(FromClr(element, depth + 1, inProgress));

        if (type.IsClass)
        {
            if (inProgress.TryGetValue(type, out var outer))
                return outer;

            var node = Node.Object();
            node.ClrType = type;
            inProgress[type] = node;
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetIndexParameters().Length > 0)
                    continue;
                var name = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? property.Name;
                node.Members[name] = new NodeMember(FromClr(property.PropertyType, depth + 1, inProgress), Required: false);
            }
            inProgress.Remove(type);
            return node;
        }

        return Node.Unknown(type.Name);
    }

    private static bool TryScalar(Type type, out Node node)
    {
        if (type.IsEnum)
        {
            node = Node.Scalar("string");
            node.EnumValues = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? f.Name)
                .ToArray();
            return true;
        }

        node = Type.GetTypeCode(type) switch
        {
            TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16
                or TypeCode.Int32 or TypeCode.UInt32 => Node.Scalar("integer", "int32"),
            TypeCode.Int64 or TypeCode.UInt64 => Node.Scalar("integer", "int64"),
            TypeCode.Single => Node.Scalar("number", "float"),
            TypeCode.Double => Node.Scalar("number", "double"),
            TypeCode.Decimal => Node.Scalar("number"),
            TypeCode.Boolean => Node.Scalar("boolean"),
            TypeCode.String or TypeCode.Char => Node.Scalar("string"),
            TypeCode.DateTime => Node.Scalar("string", "date-time"),
            _ when type == typeof(DateTimeOffset) => Node.Scalar("string", "date-time"),
            _ when type == typeof(Guid) || type == typeof(TimeSpan) => Node.Scalar("string"),
            _ => null!,
        };
        return node is not null;
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
        if (type == typeof(string))
            return null;
        if (type.IsArray)
            return type.GetElementType();
        var enumerable = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        return enumerable?.GetGenericArguments()[0];
    }

    // ---- OpenAPI schema -> Node ------------------------------------------

    /// <param name="inProgress">$refs currently being built, mapped to their object node
    /// once created, so a recursive $ref closes a cycle onto it. A ref that recurses
    /// before reaching an object (null entry) cannot be represented and is Unknown.</param>
    public static Node FromSchema(JsonElement element, Spec spec, int depth, Dictionary<string, Node?> inProgress)
    {
        if (depth >= MaxDepth)
            return Node.Unknown("max depth");

        string? refName = element.ValueKind == JsonValueKind.Object
                          && element.TryGetProperty("$ref", out var r)
            ? r.GetString()
            : null;

        if (refName is not null)
        {
            if (inProgress.TryGetValue(refName, out var outer))
                return outer ?? Node.Unknown("recursive $ref");
            inProgress[refName] = null;
        }

        try
        {
            element = spec.Resolve(element);
            if (element.ValueKind != JsonValueKind.Object)
                return Node.Unknown("non-object schema");

            // allOf: shallow-merge object members
            if (element.TryGetProperty("allOf", out var allOf) && allOf.ValueKind == JsonValueKind.Array)
            {
                var merged = Node.Object();
                foreach (var part in allOf.EnumerateArray())
                {
                    var partNode = FromSchema(part, spec, depth + 1, inProgress);
                    if (partNode.Kind == NodeKind.Object)
                        foreach (var (k, v) in partNode.Members)
                            merged.Members[k] = v;
                }
                return merged.Members.Count > 0 ? merged : Node.Unknown("allOf");
            }

            // ESI writes a tagged union as a oneOf of single-key objects, exactly one key
            // present. Merged into one object of optional members, it is checked as a typed
            // union class binds it (SovereigntyClaim) and, per key, against a dictionary's
            // value type (Dictionary<string, object>). A null variant just means nullable.
            if (element.TryGetProperty("oneOf", out var union) || element.TryGetProperty("anyOf", out union))
            {
                var merged = Node.Object();
                foreach (var variant in union.EnumerateArray())
                {
                    var variantNode = FromSchema(variant, spec, depth + 1, inProgress);
                    if (variantNode is { Kind: NodeKind.Scalar, JsonType: "null" })
                        continue;
                    if (variantNode.Kind != NodeKind.Object)
                        return Node.Unknown("oneOf / anyOf with a non-object variant");
                    foreach (var (k, v) in variantNode.Members)
                        merged.Members.TryAdd(k, v with { Required = false });
                }
                return merged;
            }

            var (jsonType, nullable) = ReadType(element);

            if (jsonType == "array" || element.TryGetProperty("items", out _))
            {
                var items = element.TryGetProperty("items", out var it)
                    ? FromSchema(it, spec, depth + 1, inProgress)
                    : Node.Unknown("array without items");
                return Node.Array(items);
            }

            // A free-form object: arbitrary keys, each value matching additionalProperties.
            if (!element.TryGetProperty("properties", out _)
                && element.TryGetProperty("additionalProperties", out var additional)
                && additional.ValueKind is JsonValueKind.Object or JsonValueKind.True)
            {
                var values = additional.ValueKind == JsonValueKind.True || !additional.EnumerateObject().Any()
                    ? Node.Any()
                    : FromSchema(additional, spec, depth + 1, inProgress);
                return Node.Map(values);
            }

            if (jsonType == "object" || element.TryGetProperty("properties", out _))
            {
                var node = Node.Object();
                if (refName is not null)
                    inProgress[refName] = node;
                var required = element.TryGetProperty("required", out var req) && req.ValueKind == JsonValueKind.Array
                    ? req.EnumerateArray().Select(x => x.GetString()).Where(x => x is not null).ToHashSet()!
                    : new HashSet<string>();

                if (element.TryGetProperty("properties", out var props))
                    foreach (var p in props.EnumerateObject())
                        node.Members[p.Name] = new NodeMember(
                            FromSchema(p.Value, spec, depth + 1, inProgress),
                            required.Contains(p.Name));
                return node;
            }

            if (jsonType is null)
                return Node.Unknown("no type");

            var scalar = Node.Scalar(jsonType, element.TryGetProperty("format", out var fmt) ? fmt.GetString() : null);
            scalar.Nullable = nullable;
            if (element.TryGetProperty("enum", out var en) && en.ValueKind == JsonValueKind.Array)
                scalar.EnumValues = en.EnumerateArray().Select(x => x.ToString()).ToArray();
            return scalar;
        }
        finally
        {
            if (refName is not null)
                inProgress.Remove(refName);
        }
    }

    /// <summary>OpenAPI 3.1 <c>type</c> is a string or an array that may include <c>"null"</c>.</summary>
    private static (string? Type, bool Nullable) ReadType(JsonElement element)
    {
        if (!element.TryGetProperty("type", out var t))
            return (null, false);

        if (t.ValueKind == JsonValueKind.String)
            return (t.GetString(), false);

        if (t.ValueKind == JsonValueKind.Array)
        {
            var values = t.EnumerateArray().Select(x => x.GetString()).ToList();
            var nullable = values.Remove("null");
            return (values.FirstOrDefault(), nullable);
        }
        return (null, false);
    }

    // ---- compare ----------------------------------------------------------

    public static void Compare(
        Node model, Node spec, string crumb, string endpoint,
        IReadOnlyDictionary<Type, HashSet<string>> knownByType, List<Finding> findings) =>
        Walk(model, spec, crumb, endpoint, knownByType, findings, new HashSet<(Node, Node)>());

    private static void Walk(
        Node model, Node spec, string crumb, string endpoint,
        IReadOnlyDictionary<Type, HashSet<string>> knownByType, List<Finding> findings,
        HashSet<(Node, Node)> visited)
    {
        void Add(Severity sev, string category, string message) =>
            findings.Add(new Finding(sev, category, endpoint, $"{crumb}: {message}"));

        // An object-typed model binds any JSON value. A pair already walked is a
        // recursive type coming round again - its first occurrence covered it.
        if (model.Kind == NodeKind.Any || !visited.Add((model, spec)))
            return;

        if (model.Kind == NodeKind.Unknown || spec.Kind is NodeKind.Unknown or NodeKind.Any)
        {
            Add(Severity.Info, "schema-unverified",
                $"not compared ({model.Note ?? spec.Note ?? "spec allows any value"})");
            return;
        }

        // A dictionary model binds any object; each spec property must fit its value type.
        if (model.Kind == NodeKind.Map && spec.Kind == NodeKind.Object)
        {
            foreach (var (name, member) in spec.Members)
                Walk(model.Values!, member.Node, $"{crumb}.{name}", endpoint, knownByType, findings, visited);
            return;
        }

        if (model.Kind != spec.Kind)
        {
            Add(Severity.Error, "schema-shape", $"model is {KindName(model.Kind)}, spec is {KindName(spec.Kind)}");
            return;
        }

        switch (model.Kind)
        {
            case NodeKind.Object:
                foreach (var (name, member) in spec.Members)
                {
                    if (!model.Members.ContainsKey(name))
                        Add(Severity.Warning, "schema-missing-property",
                            $"spec has \"{name}\"{(member.Required ? " (required)" : "")}, model does not");
                }
                foreach (var name in model.Members.Keys)
                {
                    if (spec.Members.ContainsKey(name))
                        continue;
                    // Present in another endpoint's schema for the same model class -> shared, not drift.
                    if (model.ClrType is { } ct
                        && knownByType.TryGetValue(ct, out var known)
                        && known.Contains(name))
                        continue;
                    Add(Severity.Warning, "schema-extra-property",
                        $"model has \"{name}\", not in the spec for any endpoint using this model");
                }
                foreach (var (name, member) in spec.Members)
                {
                    if (model.Members.TryGetValue(name, out var modelMember))
                        Walk(modelMember.Node, member.Node, $"{crumb}.{name}", endpoint, knownByType, findings, visited);
                }
                break;

            case NodeKind.Array:
                if (model.Items is not null && spec.Items is not null)
                    Walk(model.Items, spec.Items, $"{crumb}[]", endpoint, knownByType, findings, visited);
                break;

            case NodeKind.Map:
                Walk(model.Values!, spec.Values!, $"{crumb}{{*}}", endpoint, knownByType, findings, visited);
                break;

            case NodeKind.Scalar:
                CompareScalar(model, spec, Add);
                break;
        }
    }

    private static string KindName(NodeKind kind) =>
        kind == NodeKind.Map ? "dictionary" : kind.ToString().ToLowerInvariant();

    private static void CompareScalar(Node model, Node spec, Action<Severity, string, string> add)
    {
        var m = model.JsonType;
        var s = spec.JsonType;
        if (s is null || m is null)
            return;

        // The spec explicitly allows null but the model is a bare value type -> throws.
        // (ESI's spec rarely declares this; the runtime `--probe` catches the rest.)
        if (spec.Nullable && !model.Nullable)
            add(Severity.Error, "schema-nullable",
                "spec allows null here; the model is a non-nullable value type and will throw on null");

        if (m == s)
        {
            // Enum values that drifted (a typo, or ESI added/removed a value).
            if (m == "string" && spec.EnumValues is { Count: > 0 } specEnum && model.EnumValues is not null)
            {
                var extra = model.EnumValues.Except(specEnum).ToList();
                var gone = specEnum.Except(model.EnumValues).ToList();
                if (extra.Count > 0 || gone.Count > 0)
                    add(Severity.Warning, "schema-enum-drift",
                        $"enum differs - model-only [{string.Join(", ", extra)}], spec-only [{string.Join(", ", gone)}]");
            }

            // StringEnumConverter throws on a value the C# enum does not declare.
            if (m == "string" && model.EnumValues is not null && spec.EnumValues is not { Count: > 0 })
                add(Severity.Error, "schema-enum-open",
                    "model is an enum, spec allows any string - an undeclared value will throw");

            // Same JSON type, narrower CLR type: deserialization throws past the narrow range.
            if (m == "integer" && model.Format == "int32" && spec.Format == "int64")
                add(Severity.Error, "schema-int-width",
                    "model is 32-bit, spec is int64 - values beyond 2,147,483,647 will throw");
            if (m == "string" && model.Format == "date-time" && spec.Format is not ("date-time" or "date"))
                add(Severity.Error, "schema-type",
                    $"model is a date, spec is a {(spec.Format is null ? "plain" : spec.Format)} string - a non-date value will throw");
            return;
        }

        // categories differ
        if (m == "number" && s == "integer")
            add(Severity.Info, "schema-number-widening", "model is floating point, spec is integer");
        else if (m == "integer" && s == "number")
            add(Severity.Error, "schema-type", "model is integer, spec is number - non-integral values will throw");
        else
            add(Severity.Error, "schema-type", $"model is {m}, spec is {s}");
    }
}
