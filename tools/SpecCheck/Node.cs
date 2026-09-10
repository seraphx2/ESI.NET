namespace ESI.NET.Tools.SpecCheck;

public enum NodeKind { Object, Array, Scalar, Unknown }

/// <summary>
/// A structural view of a type, built from either an OpenAPI schema
/// (<see cref="SchemaCheck.FromSchema"/>) or a CLR type
/// (<see cref="SchemaCheck.FromClr"/>). The two are then walked together by
/// <see cref="SchemaCheck.Compare"/>.
/// </summary>
public sealed class Node
{
    public NodeKind Kind { get; private init; }

    // Object
    public Dictionary<string, NodeMember> Members { get; } = new(StringComparer.Ordinal);

    /// <summary>
    /// The CLR type this object node was built from (<see cref="SchemaCheck.FromClr"/> only).
    /// Lets the schema check tell that a model class is shared across endpoints, so a
    /// property absent from one endpoint's schema but present in a sibling's is not "extra".
    /// </summary>
    public Type? ClrType { get; set; }

    // Array
    public Node? Items { get; private set; }

    // Scalar
    public string? JsonType { get; private set; }        // integer | number | string | boolean
    public string? Format { get; private set; }          // int32 | int64 | date-time | float | double
    public IReadOnlyList<string>? EnumValues { get; set; }
    public bool Nullable { get; set; }

    // Unknown
    public string? Note { get; private set; }

    public static Node Object() => new() { Kind = NodeKind.Object };
    public static Node Array(Node items) => new() { Kind = NodeKind.Array, Items = items };
    public static Node Scalar(string jsonType, string? format = null) =>
        new() { Kind = NodeKind.Scalar, JsonType = jsonType, Format = format };
    public static Node Unknown(string note) => new() { Kind = NodeKind.Unknown, Note = note };
}

public sealed record NodeMember(Node Node, bool Required);
