using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace ESI.NET.Tools.SpecCheck.Docs;

/// <summary>
/// Writes the XML doc comments the spec can provide - on every Logic method that calls an ESI
/// endpoint and on every model type and property - plus <c>EsiEndpointRequirements.g.cs</c>,
/// the route -&gt; scope / roles table the runtime scope check reads. Only comments are
/// touched: each edit replaces a member's <c>///</c> block and nothing else.
/// </summary>
/// <remarks>
/// The generator owns what it writes; hand-written text goes in <c>docs.json</c>
/// (<see cref="DocOverrides"/>). A member the generator has no text for keeps whatever doc it
/// already has, and is reported if it has none.
/// </remarks>
public sealed class DocWriter
{
    private const int Width = 100;

    private readonly DocPlan _plan;
    private readonly Assembly _assembly;
    private readonly string _root;
    private readonly string _compatibilityDate;

    /// <summary>Things the generator could not document, for a person to add to docs.json.</summary>
    public List<string> Missing { get; } = new();

    /// <summary>Members documented only by a spec description that restates the name.</summary>
    public List<string> Weak { get; } = new();

    /// <summary>Hand-written docs the generator replaced - review once, then they live in docs.json.</summary>
    public List<string> Replaced { get; } = new();

    public DocWriter(DocPlan plan, Assembly assembly, string repoRoot, string compatibilityDate)
    {
        _plan = plan;
        _assembly = assembly;
        _root = repoRoot;
        _compatibilityDate = compatibilityDate;
    }

    /// <summary>Path -&gt; new content for every file whose content would change.</summary>
    public Dictionary<string, string> Render()
    {
        var changes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in Directory.EnumerateFiles(Path.Combine(_root, "ESI.NET", "Logic"), "*.cs").OrderBy(f => f, StringComparer.Ordinal))
            Collect(changes, file, RenderLogic);

        Collect(changes, Path.Combine(_root, "ESI.NET", "EsiClient.cs"), RenderClient);

        var types = ModelTypes();
        foreach (var file in Directory.EnumerateFiles(Path.Combine(_root, "ESI.NET", "Models"), "*.cs", SearchOption.AllDirectories).OrderBy(f => f, StringComparer.Ordinal))
            Collect(changes, file, (path, text) => RenderModels(path, text, types));

        var table = Path.Combine(_root, "ESI.NET", "EsiEndpointRequirements.g.cs");
        // Keep the newline style of the copy on disk: git stores it with LF and checks it out with
        // CRLF on Windows, LF on Linux, so a fixed style would read as "out of date" on one of them.
        var existing = File.Exists(table) ? File.ReadAllText(table) : null;
        var tableText = RenderTable(existing is null ? Environment.NewLine : NewlineOf(existing));
        if (!string.Equals(existing, tableText, StringComparison.Ordinal))
            changes[table] = tableText;

        return changes;
    }

    private static void Collect(Dictionary<string, string> changes, string file, Func<string, string, string> render)
    {
        var text = File.ReadAllText(file);
        var updated = render(file, text);
        if (!string.Equals(updated, text, StringComparison.Ordinal))
            changes[file] = updated;
    }

    // ---- Logic methods ------------------------------------------------------

    private string RenderLogic(string path, string text)
    {
        var root = CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
        var edits = new List<Edit>();
        var newline = text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";

        foreach (var type in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
        {
            if (Tag(type.Identifier.Text) is { } tag && Accessor(type.Identifier.Text) is { } accessor)
            {
                edits.Add(DocEdit(text, type, Summary($"ESI's {tag} endpoints. Reach them through <see cref=\"EsiClient.{accessor}\"/>."),
                    newline, type.Identifier.Text, keepHandWritten: true));

                foreach (var ctor in type.Members.OfType<ConstructorDeclarationSyntax>().Where(c => c.Modifiers.Any(SyntaxKind.PublicKeyword)))
                {
                    var lines = Summary($"Creates the {tag} endpoints. <see cref=\"EsiClient\"/> does this; there is no need to call it.");
                    Inline(lines, "<param name=\"client\">", "The <see cref=\"System.Net.Http.HttpClient\"/> requests are sent with.", "</param>");
                    Inline(lines, "<param name=\"config\">", "The ESI.NET configuration.", "</param>");
                    edits.Add(DocEdit(text, ctor, lines, newline, type.Identifier.Text + "..ctor", keepHandWritten: true));
                }
            }

            var overloaded = type.Members.OfType<MethodDeclarationSyntax>()
                .GroupBy(m => m.Identifier.Text).Where(g => g.Count() > 1).Select(g => g.Key).ToHashSet();

            foreach (var method in type.Members.OfType<MethodDeclarationSyntax>())
            {
                if (!method.Modifiers.Any(SyntaxKind.PublicKeyword))
                    continue;
                if (!_plan.Methods.TryGetValue((type.Identifier.Text, method.Identifier.Text), out var doc))
                    continue;

                var lines = MethodDoc(type.Identifier.Text, method, doc);
                edits.Add(DocEdit(text, method, lines, newline, $"{type.Identifier.Text}.{method.Identifier.Text}"));
            }
        }

        return Apply(text, edits);
    }

    private List<string> MethodDoc(string className, MethodDeclarationSyntax method, OperationDoc doc)
    {
        var lines = new List<string>();
        Block(lines, "summary", doc.Summary);

        lines.Add("<remarks>");
        foreach (var remark in doc.Remarks)
            Para(lines, remark);
        lines.Add("</remarks>");

        var body = method.ToString();
        foreach (var parameter in method.ParameterList.Parameters)
        {
            var name = parameter.Identifier.Text;
            var text = ParamText(doc, name, parameter.Type?.ToString() ?? "", body);
            if (text is null)
            {
                Missing.Add($"param  {doc.Op.Key}  {className}.{method.Identifier.Text}({name})");
                continue;
            }
            Inline(lines, $"<param name=\"{name}\">", text, "</param>");
        }

        Inline(lines, "<returns>", Returns(doc, method), "</returns>");
        return lines;
    }

    private string? ParamText(OperationDoc doc, string name, string type, string body)
    {
        if (doc.Override?.Params.TryGetValue(name, out var own) == true)
            return DocText.Sentence(own, isXml: true);

        if (name == "options")
            return OptionsText(doc, body);

        var snake = DocText.Snake(name);
        string? weak = null;
        foreach (var candidate in new[] { snake, snake.TrimEnd('s'), snake + "s" })
        {
            if (doc.SpecParams.TryGetValue(candidate, out var description) && !string.IsNullOrWhiteSpace(description))
            {
                if (!DocText.IsWeak(description, candidate))
                    return DocText.Sentence(description);
                weak ??= description;
            }
        }

        if (doc.BodyIsArray && !string.IsNullOrWhiteSpace(doc.BodyDescription) && (type.Contains('[') || type.Contains("List<")))
            return DocText.Sentence(doc.BodyDescription!);

        if (_plan.Overrides.Parameters.TryGetValue(name, out var shared))
            return DocText.Sentence(shared, isXml: true);

        if (weak is null)
            return null;
        Weak.Add($"param  {doc.Op.Key}  ({name})  \"{weak}\"");
        return DocText.Sentence(weak);
    }

    private static string OptionsText(OperationDoc doc, string body)
    {
        if (!doc.Authenticated)
            return "Per-call options: cancellation, <see cref=\"EsiCallOptions.IfNoneMatch\"/>, "
                   + "<see cref=\"EsiCallOptions.BypassCache\"/>"
                   + (doc.PagePaginated ? ", and <see cref=\"EsiCallOptions.Page\"/>." : ".");

        var fills = new List<string>();
        if (body.Contains("Character.CharacterID", StringComparison.Ordinal))
            fills.Add("<c>{character_id}</c>");
        if (body.Contains("Character.CorporationID", StringComparison.Ordinal))
            fills.Add("<c>{corporation_id}</c>");
        if (body.Contains("Character.AllianceID", StringComparison.Ordinal))
            fills.Add("<c>{alliance_id}</c>");

        var text = "Per-call options. <see cref=\"EsiCallOptions.Character\"/> is required: its access token "
                   + "authorizes the call"
                   + (fills.Count > 0 ? " and its IDs fill " + string.Join(" and ", fills) : "")
                   + ".";
        if (doc.PagePaginated)
            text += " <see cref=\"EsiCallOptions.Page\"/> picks the page.";
        return text;
    }

    private static string Returns(OperationDoc doc, MethodDeclarationSyntax method)
    {
        if (doc.ReturnsOverride is { } own)
            return DocText.Sentence(own, isXml: true);
        if (doc.NoContent)
            return "The response. Success is <c>204 No Content</c>, with no <see cref=\"EsiResponse{T}.Data\"/>.";

        // Task<EsiResponse<X>> -> X
        var model = method.ReturnType is GenericNameSyntax { TypeArgumentList.Arguments: [GenericNameSyntax { TypeArgumentList.Arguments: [var inner] }] }
            ? inner
            : null;
        return "The response. On success, <see cref=\"EsiResponse{T}.Data\"/> holds " + Describe(model) + ".";
    }

    private static string Describe(TypeSyntax? type) => type switch
    {
        null => "the result",
        ArrayTypeSyntax array => "a list of " + Describe(array.ElementType, plural: true),
        GenericNameSyntax { Identifier.Text: "List" or "IReadOnlyList" or "IList" or "IEnumerable", TypeArgumentList.Arguments: [var element] }
            => "a list of " + Describe(element, plural: true),
        GenericNameSyntax { Identifier.Text: "Dictionary" or "IReadOnlyDictionary" } => "a map of <c>" + Escape(type.ToString()) + "</c>",
        PredefinedTypeSyntax predefined => "the <c>" + predefined + "</c> value",
        _ => "the <see cref=\"" + Cref(type) + "\"/>",
    };

    /// <summary>A type as a cref: <c>Leaderboards&lt;FactionTotal&gt;</c> -&gt; <c>Leaderboards{FactionTotal}</c>.</summary>
    private static string Cref(TypeSyntax type) => type.ToString().Replace('<', '{').Replace('>', '}');

    private static string Describe(TypeSyntax type, bool plural) => type switch
    {
        PredefinedTypeSyntax predefined => "<c>" + predefined + "</c> values",
        GenericNameSyntax or ArrayTypeSyntax => "<c>" + Escape(type.ToString()) + "</c>",
        _ => "<see cref=\"" + type + "\"/>",
    };

    private static string Escape(string s) => s.Replace("<", "&lt;").Replace(">", "&gt;");

    /// <summary>The spec tag of a Logic class's endpoints ("Wallet", "Faction Warfare").</summary>
    private string? Tag(string className)
        => _plan.Methods.Where(m => m.Key.Class == className)
            .GroupBy(m => m.Value.Op.Tag)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

    /// <summary>The <see cref="EsiClient"/> property that exposes a Logic class.</summary>
    private string? Accessor(string className)
        => typeof(EsiClient).GetProperties().FirstOrDefault(p => p.PropertyType.Name == className)?.Name;

    private static string NewlineOf(string text) => text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";

    /// <summary>EsiClient / IEsiClient: one property per Logic class.</summary>
    private string RenderClient(string path, string text)
    {
        var root = CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
        var edits = new List<Edit>();
        var newline = NewlineOf(text);

        foreach (var property in root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
        {
            var typeName = property.Type.ToString();
            if (Tag(typeName) is not { } tag)
                continue;
            edits.Add(DocEdit(text, property, Summary($"ESI's {tag} endpoints."), newline, "EsiClient." + property.Identifier.Text, keepHandWritten: true));
        }

        return Apply(text, edits);
    }

    // ---- models --------------------------------------------------------------

    private sealed record ModelType(Type Clr, string File, int Position);

    /// <summary>Every public model type, by full name with nested types dotted (<c>Ns.Outer.Inner</c>).</summary>
    private Dictionary<string, Type> ModelTypes()
    {
        Type?[] all;
        try { all = _assembly.GetTypes(); }
        catch (ReflectionTypeLoadException ex) { all = ex.Types; }

        return all.Where(t => t is not null && t.Namespace?.StartsWith("ESI.NET.Models", StringComparison.Ordinal) == true && IsPublic(t))
            .ToDictionary(t => DottedName(t!), t => t!);
    }

    private static bool IsPublic(Type t) => t.IsPublic || (t.IsNestedPublic && t.DeclaringType is { } d && IsPublic(d));

    /// <summary><c>Ns.Outer+Inner`1</c> -&gt; <c>Ns.Outer.Inner</c>: the name as written in source.</summary>
    private static string DottedName(Type t)
        => System.Text.RegularExpressions.Regex.Replace(
            (t.IsGenericType ? t.GetGenericTypeDefinition() : t).FullName ?? t.Name, @"`\d+", "").Replace('+', '.');

    private string RenderModels(string path, string text, Dictionary<string, Type> types)
    {
        var root = CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
        var edits = new List<Edit>();
        var newline = text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";

        foreach (var declaration in root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            if (!types.TryGetValue(SyntaxName(declaration), out var clr))
                continue;

            // Obsolete types are on their way out and documented by hand, once, at the type.
            if (clr.IsDefined(typeof(ObsoleteAttribute), inherit: false))
                continue;

            if (TypeText(clr) is { } typeText)
                edits.Add(DocEdit(text, declaration, Summary(typeText), newline, DottedName(clr), keepHandWritten: true));
            else if (!HasDoc(declaration))
                Missing.Add($"type   {DottedName(clr)}");

            foreach (var property in declaration.Members.OfType<PropertyDeclarationSyntax>())
            {
                if (!property.Modifiers.Any(SyntaxKind.PublicKeyword))
                    continue;
                var info = clr.GetProperty(property.Identifier.Text, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (info is null)
                    continue;

                var key = $"{DottedName(clr)}.{info.Name}";
                var propertyText = PropertyText(info, key);
                if (propertyText is not null)
                    edits.Add(DocEdit(text, property, Summary(propertyText), newline, key));
                else if (!HasDoc(property))
                    Missing.Add($"field  {key}  ({JsonName(info)})");
            }
        }

        return Apply(text, edits);
    }

    private static string SyntaxName(TypeDeclarationSyntax declaration)
    {
        var names = declaration.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().Select(t => t.Identifier.Text).Reverse();
        var ns = declaration.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().Select(n => n.Name.ToString()).Reverse();
        return string.Join(".", ns.Concat(names));
    }

    private static string JsonName(PropertyInfo info) => info.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? info.Name;

    private string? PropertyText(PropertyInfo info, string key)
    {
        if (_plan.Overrides.Properties.TryGetValue(key, out var own))
            return DocText.Sentence(own, isXml: true);

        _plan.Properties.TryGetValue(info, out var spec);
        var json = spec?.JsonName ?? JsonName(info);

        string? text = null;
        var real = spec?.Descriptions.FirstOrDefault(d => !DocText.IsWeak(d, json));
        if (real is not null)
            text = DocText.Sentence(real);
        else if (_plan.Overrides.Fields.TryGetValue(json, out var shared))
            text = DocText.Sentence(shared, isXml: true);
        else if (spec?.Descriptions.FirstOrDefault() is { } weak)
        {
            text = DocText.Sentence(weak);
            Weak.Add($"field  {key}  ({json})  \"{weak}\"");
        }

        if (text is null)
            return null;

        var underlying = Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType;
        var isStringish = underlying == typeof(string) || underlying == typeof(string[]) || underlying == typeof(List<string>);
        if (isStringish && spec?.EnumValues is { Count: > 0 and <= 40 } values && !text.Contains("<c>", StringComparison.Ordinal))
            text += " One of " + string.Join(", ", values.Select(v => $"<c>{v}</c>")) + ".";

        return text;
    }

    private string? TypeText(Type clr)
    {
        var returnedBy = _plan.Methods
            .Where(m => m.Value.Op.ResponseSchema is not null && ReturnsType(m.Key.Class, m.Key.Method, clr))
            .Select(m => Cref(m.Key.Class, m.Key.Method))
            .Distinct()
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToList();

        if (returnedBy.Count > 0)
            return "Returned by " + Join(returnedBy) + ".";

        var parents = ModelParents(clr);
        if (parents.Count > 0)
            return "Part of " + Join(parents) + ".";

        return null;
    }

    private static string Join(List<string> items) => items.Count switch
    {
        1 => items[0],
        2 => items[0] + " and " + items[1],
        _ => string.Join(", ", items.Take(items.Count - 1)) + ", and " + items[^1],
    };

    private bool ReturnsType(string className, string method, Type clr)
    {
        var logic = _assembly.GetType("ESI.NET.Logic." + className);
        var info = logic?.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).FirstOrDefault(m => m.Name == method);
        if (info is null)
            return false;
        var model = info.ReturnType.GetGenericArguments()[0].GetGenericArguments()[0];
        return Unwrap(model) == clr;
    }

    private static Type Unwrap(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        if (t.IsArray)
            return Unwrap(t.GetElementType()!);
        if (t.IsGenericType && t != typeof(string))
            return Unwrap(t.GetGenericArguments()[^1]);
        return t;
    }

    private List<string> ModelParents(Type clr)
    {
        Type?[] all;
        try { all = _assembly.GetTypes(); }
        catch (ReflectionTypeLoadException ex) { all = ex.Types; }

        return all.Where(t => t is not null && t.Namespace?.StartsWith("ESI.NET.Models", StringComparison.Ordinal) == true && t != clr
                              && !t.IsDefined(typeof(ObsoleteAttribute), inherit: false))
            .SelectMany(t => t!.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(p => Unwrap(p.PropertyType) == clr)
                .Select(p => $"<see cref=\"{DottedName(t)}.{p.Name}\"/>"))
            .Distinct()
            .OrderBy(x => x, StringComparer.Ordinal)
            .Take(6)
            .ToList();
    }

    private string Cref(string className, string method)
    {
        var logic = _assembly.GetType("ESI.NET.Logic." + className);
        var overloads = logic?.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Count(m => m.Name == method) ?? 0;
        return overloads == 1
            ? $"<see cref=\"ESI.NET.Logic.{className}.{method}\"/>"
            : $"<c>{className}.{method}</c>";
    }

    // ---- requirements table ------------------------------------------------

    private string RenderTable(string newline)
    {
        var sb = new StringBuilder();
        sb.Append("// <auto-generated>\n");
        sb.Append("// Generated by tools/SpecCheck (--write-docs) from the ESI OpenAPI spec,\n");
        sb.Append($"// compatibility date {_compatibilityDate}. Do not edit: rerun the generator.\n");
        sb.Append("// </auto-generated>\n");
        sb.Append("using System;\n");
        sb.Append("using System.Collections.Generic;\n\n");
        sb.Append("namespace ESI.NET\n{\n");
        sb.Append("    internal static partial class EsiEndpointRequirements\n    {\n");
        sb.Append("        /// <summary>Authenticated route (<c>\"GET /characters/{character_id}/wallet\"</c>) -&gt; scope and corporation roles.</summary>\n");
        sb.Append("        private static readonly Dictionary<string, EsiEndpointRequirement> Table = new Dictionary<string, EsiEndpointRequirement>(StringComparer.Ordinal)\n");
        sb.Append("        {\n");
        foreach (var doc in _plan.Endpoints.Where(e => e.Scope is not null).DistinctBy(e => e.Key).OrderBy(e => e.Key, StringComparer.Ordinal))
        {
            var roles = doc.Roles.Count == 0
                ? "NoRoles"
                : "new[] { " + string.Join(", ", doc.Roles.Select(r => $"\"{r}\"")) + " }";
            sb.Append($"            [\"{doc.Key}\"] = new EsiEndpointRequirement(\"{doc.Scope}\", {roles}),\n");
        }
        sb.Append("        };\n    }\n}\n");
        return newline == "\n" ? sb.ToString() : sb.ToString().Replace("\n", newline);
    }

    // ---- comment splicing ---------------------------------------------------

    private readonly record struct Edit(int Start, int End, string Text);

    private static bool HasDoc(SyntaxNode node)
        => node.GetLeadingTrivia().Any(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));

    /// <summary>Replaces (or inserts) the <c>///</c> block above <paramref name="node"/>.</summary>
    private Edit DocEdit(string text, SyntaxNode node, List<string> lines, string newline, string label, bool keepHandWritten = false)
    {
        var lineStart = LineStart(text, node.SpanStart);
        var indent = text[lineStart..node.SpanStart];
        if (indent.Any(c => !char.IsWhiteSpace(c)))
            indent = "";

        var docs = node.GetLeadingTrivia()
            .Where(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
            .ToList();

        var rendered = string.Concat(lines.Select(l => indent + "/// " + l + newline)).Replace("///  ", "/// ");
        if (docs.Count == 0)
            return new Edit(lineStart, lineStart, rendered);

        var start = LineStart(text, docs[0].FullSpan.Start);
        var end = docs[^1].FullSpan.End;
        var existing = text[start..end];

        if (keepHandWritten && !IsGenerated(existing) && existing.Trim().Length > 0)
            return new Edit(start, end, existing);

        if (!IsGenerated(existing) && Normalize(existing) != Normalize(rendered) && !IsJunk(existing))
            Replaced.Add($"{label}: {Normalize(existing)}");

        return new Edit(start, end, rendered);
    }

    /// <summary>Text the generator itself would have written (so not someone's hand-written doc).</summary>
    private static bool IsGenerated(string doc)
        => doc.Contains("<para>Calls <c>", StringComparison.Ordinal)
           || doc.Contains("Returned by <see cref=", StringComparison.Ordinal)
           || doc.Contains("Part of <see cref=", StringComparison.Ordinal);

    /// <summary>The old placeholder docs: just the route, empty tags.</summary>
    private static bool IsJunk(string doc)
    {
        var text = Normalize(doc);
        text = System.Text.RegularExpressions.Regex.Replace(text, "<(param name=\"\\w+\"|returns)>\\s*</(param|returns)>|<returns />", "");
        text = System.Text.RegularExpressions.Regex.Replace(text, "</?summary>", "").Trim();
        return text.Length == 0 || System.Text.RegularExpressions.Regex.IsMatch(text, @"^[/{}\w\-]+/?$");
    }

    private static string Normalize(string doc)
        => System.Text.RegularExpressions.Regex.Replace(doc.Replace("///", " "), @"\s+", " ").Trim();

    private static int LineStart(string text, int position)
    {
        var i = text.LastIndexOf('\n', Math.Max(0, position - 1));
        return i < 0 ? 0 : i + 1;
    }

    private static string Apply(string text, List<Edit> edits)
    {
        var sb = new StringBuilder(text);
        foreach (var edit in edits.OrderByDescending(e => e.Start))
            sb.Remove(edit.Start, edit.End - edit.Start).Insert(edit.Start, edit.Text);
        return sb.ToString();
    }

    // ---- line building ---------------------------------------------------------

    private static List<string> Summary(string text)
    {
        var lines = new List<string>();
        Block(lines, "summary", text);
        return lines;
    }

    private static void Block(List<string> lines, string tag, string text)
    {
        var oneLine = $"<{tag}>{text}</{tag}>";
        if (oneLine.Length <= Width - 12)
        {
            lines.Add(oneLine);
            return;
        }
        lines.Add($"<{tag}>");
        lines.AddRange(DocText.Wrap(text, Width - 12));
        lines.Add($"</{tag}>");
    }

    private static void Para(List<string> lines, string text) => Inline(lines, "<para>", text, "</para>");

    private static void Inline(List<string> lines, string open, string text, string close)
    {
        var wrapped = DocText.Wrap(open + text + close, Width - 12).ToList();
        lines.AddRange(wrapped);
    }
}
