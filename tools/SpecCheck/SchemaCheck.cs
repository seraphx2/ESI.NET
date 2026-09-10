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

    public static SchemaResult Run(Spec spec, Wrapper wrapper, bool strict)
    {
        var specByKey = spec.Operations
            .GroupBy(o => o.Key)
            .ToDictionary(g => g.Key, g => g.First());

        var findings = new List<Finding>();
        int checkedCount = 0, noSchema = 0, noModel = 0;

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

            var model = FromClr(clrType, 0, new HashSet<Type>());
            var schema = FromSchema(schemaElement, spec, 0, new HashSet<string>());
            Compare(model, schema, "Data", endpoint.Key, strict, findings);
            checkedCount++;
        }

        return new SchemaResult
        {
            Findings = findings,
            Checked = checkedCount,
            SkippedNoSchema = noSchema,
            SkippedNoModel = noModel,
        };
    }

    // ---- CLR type -> Node ---------------------------------------------------

    public static Node FromClr(Type type, int depth, HashSet<Type> seen)
    {
        type = Nullable.GetUnderlyingType(type) is { } underlying ? underlying : type;

        if (TryScalar(type, out var scalar)) // also handles enums
            return scalar;

        if (depth >= MaxDepth)
            return Node.Unknown("max depth");

        if (typeof(IDictionary).IsAssignableFrom(type)
            || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
            return Node.Unknown("dictionary / additionalProperties");

        if (ElementType(type) is { } element)
            return Node.Array(FromClr(element, depth + 1, seen));

        if (type.IsClass && type != typeof(object))
        {
            if (!seen.Add(type))
                return Node.Unknown("recursive type " + type.Name);

            var node = Node.Object();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetIndexParameters().Length > 0)
                    continue;
                var name = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? property.Name;
                node.Members[name] = new NodeMember(FromClr(property.PropertyType, depth + 1, seen), Required: false);
            }
            seen.Remove(type);
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

    public static Node FromSchema(JsonElement element, Spec spec, int depth, HashSet<string> seenRefs)
    {
        if (depth >= MaxDepth)
            return Node.Unknown("max depth");

        string? refName = element.ValueKind == JsonValueKind.Object
                          && element.TryGetProperty("$ref", out var r)
            ? r.GetString()
            : null;

        if (refName is not null && !seenRefs.Add(refName))
            return Node.Unknown("recursive $ref");

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
                    var partNode = FromSchema(part, spec, depth + 1, seenRefs);
                    if (partNode.Kind == NodeKind.Object)
                        foreach (var (k, v) in partNode.Members)
                            merged.Members[k] = v;
                }
                return merged.Members.Count > 0 ? merged : Node.Unknown("allOf");
            }

            if (element.TryGetProperty("oneOf", out _) || element.TryGetProperty("anyOf", out _))
                return Node.Unknown("oneOf / anyOf union");

            var (jsonType, nullable) = ReadType(element);

            if (jsonType == "array" || element.TryGetProperty("items", out _))
            {
                var items = element.TryGetProperty("items", out var it)
                    ? FromSchema(it, spec, depth + 1, seenRefs)
                    : Node.Unknown("array without items");
                return Node.Array(items);
            }

            if (jsonType == "object" || element.TryGetProperty("properties", out _))
            {
                var node = Node.Object();
                var required = element.TryGetProperty("required", out var req) && req.ValueKind == JsonValueKind.Array
                    ? req.EnumerateArray().Select(x => x.GetString()).Where(x => x is not null).ToHashSet()!
                    : new HashSet<string>();

                if (element.TryGetProperty("properties", out var props))
                    foreach (var p in props.EnumerateObject())
                        node.Members[p.Name] = new NodeMember(
                            FromSchema(p.Value, spec, depth + 1, seenRefs),
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
                seenRefs.Remove(refName);
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

    public static void Compare(Node model, Node spec, string crumb, string endpoint, bool strict, List<Finding> findings)
    {
        void Add(Severity sev, string category, string message) =>
            findings.Add(new Finding(sev, category, endpoint, $"{crumb}: {message}"));

        var warn = strict ? Severity.Error : Severity.Warning;

        if (model.Kind == NodeKind.Unknown || spec.Kind == NodeKind.Unknown)
        {
            Add(Severity.Info, "schema-unverified",
                $"not compared ({model.Note ?? spec.Note})");
            return;
        }

        if (model.Kind != spec.Kind)
        {
            Add(Severity.Error, "schema-shape",
                $"model is {model.Kind.ToString().ToLowerInvariant()}, spec is {spec.Kind.ToString().ToLowerInvariant()}");
            return;
        }

        switch (model.Kind)
        {
            case NodeKind.Object:
                foreach (var (name, member) in spec.Members)
                {
                    if (!model.Members.ContainsKey(name))
                        Add(warn, "schema-missing-property",
                            $"spec has \"{name}\"{(member.Required ? " (required)" : "")}, model does not");
                }
                foreach (var name in model.Members.Keys)
                {
                    if (!spec.Members.ContainsKey(name))
                        Add(warn, "schema-extra-property", $"model has \"{name}\", spec does not");
                }
                foreach (var (name, member) in spec.Members)
                {
                    if (model.Members.TryGetValue(name, out var modelMember))
                        Compare(modelMember.Node, member.Node, $"{crumb}.{name}", endpoint, strict, findings);
                }
                break;

            case NodeKind.Array:
                if (model.Items is not null && spec.Items is not null)
                    Compare(model.Items, spec.Items, $"{crumb}[]", endpoint, strict, findings);
                break;

            case NodeKind.Scalar:
                CompareScalar(model, spec, Add, warn);
                break;
        }
    }

    private static void CompareScalar(Node model, Node spec, Action<Severity, string, string> add, Severity warn)
    {
        var m = model.JsonType;
        var s = spec.JsonType;
        if (s is null || m is null)
            return;

        if (m == s)
        {
            // ESI types every id as int64; ESI.NET overwhelmingly uses int. A real latent
            // overflow (structure / item / journal ids already exceed int32) but systemic
            // and long-shipping, so it is a warning unless --strict.
            if (m == "integer" && model.Format == "int32" && spec.Format == "int64")
                add(warn, "schema-int-width", "model is int (int32), spec is int64 - overflow risk");

            if (m == "string")
            {
                if (spec.EnumValues is { Count: > 0 } specEnum)
                {
                    if (model.EnumValues is null)
                        add(Severity.Info, "schema-enum-unmodelled", $"spec is an enum ({specEnum.Count} values), model is a plain string");
                    else
                    {
                        var extra = model.EnumValues.Except(specEnum).ToList();
                        var gone = specEnum.Except(model.EnumValues).ToList();
                        if (extra.Count > 0 || gone.Count > 0)
                            add(warn, "schema-enum-drift",
                                $"enum differs - model-only [{string.Join(", ", extra)}], spec-only [{string.Join(", ", gone)}]");
                    }
                }
                else if (spec.Format == "date-time" && model.Format != "date-time")
                    add(Severity.Info, "schema-date-as-string", "spec is date-time, model is a plain string");
            }
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
