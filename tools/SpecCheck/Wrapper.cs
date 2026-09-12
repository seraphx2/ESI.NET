using System.Reflection;
using ESI.NET;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ESI.NET.Tools.SpecCheck;

/// <summary>One endpoint the wrapper implements, as found in <c>ESI.NET/Logic/*.cs</c>.</summary>
public sealed record ImplementedEndpoint(
    string Class,          // "AllianceLogic"
    string Method,         // "Information"
    string HttpMethod,     // "GET"
    string Path,           // "/alliances/{alliance_id}" - normalized
    bool Authenticated,
    string ModelTypeText,  // "Alliance" / "List<Bloodline>" - the Execute<T> arg as written
    Type? ModelType)       // T resolved by reflection (for Tier 2); null if the join failed
{
    public string Key => $"{HttpMethod} {Path}";
    public string LooseKey => $"{HttpMethod} {Spec.ParamAgnostic(Path)}";
}

/// <summary>
/// Extracts the implemented endpoint set from source. The HTTP method, route and
/// security live inside each Logic method body (invisible to reflection), so those
/// come from a Roslyn syntax walk; the response model <c>T</c> comes from reflecting
/// the built <c>ESI.NET</c> assembly and is joined back on (class, method).
/// </summary>
public sealed class Wrapper
{
    public IReadOnlyList<ImplementedEndpoint> Endpoints { get; }
    public IReadOnlyList<string> Warnings { get; }

    private Wrapper(List<ImplementedEndpoint> endpoints, List<string> warnings)
    {
        Endpoints = endpoints;
        Warnings = warnings;
    }

    public static Wrapper Scan(string logicDirectory, Assembly esiAssembly)
    {
        var warnings = new List<string>();
        var models = ReflectResponseModels(esiAssembly);
        var endpoints = new List<ImplementedEndpoint>();
        var seen = new HashSet<(string, string, string, string)>();

        foreach (var file in Directory.EnumerateFiles(logicDirectory, "*.cs").OrderBy(f => f))
        {
            var name = Path.GetFileName(file);
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file).GetCompilationUnitRoot();

            foreach (var call in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                if (!IsExecuteCall(call, out var typeArgs))
                    continue;

                var method = call.Ancestors().OfType<MethodDeclarationSyntax>().FirstOrDefault();
                var type = call.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (method is null || type is null)
                    continue;

                var className = type.Identifier.Text;
                var methodName = method.Identifier.Text;
                models.TryGetValue((className, methodName), out var modelType);
                if (modelType is null)
                    warnings.Add($"{name}: {className}.{methodName} - no matching EsiResponse<T> method on the built assembly");

                // Routes are string literals in almost every Logic method. SearchLogic.Query
                // picks its endpoint from a local at runtime, so fall back to reading every
                // "/..."-shaped literal in the method body.
                List<(string HttpMethod, string Endpoint, bool Authenticated)> found;
                if (TryReadArguments(call, out var security, out var httpMethod, out var endpoint))
                {
                    found = new() { (httpMethod, endpoint, security == "Authenticated") };
                }
                else
                {
                    found = FallbackFromBody(method);
                    if (found.Count == 0)
                    {
                        warnings.Add($"{name}: {className}.{methodName} - could not parse Execute(...) arguments");
                        continue;
                    }
                    warnings.Add($"{name}: {className}.{methodName} - route(s) read heuristically from the method body "
                                 + $"({string.Join(", ", found.Select(f => f.Endpoint))}); security may be approximate");
                }

                foreach (var (m, ep, auth) in found)
                {
                    var path = Spec.NormalizePath(ep);
                    if (seen.Add((className, methodName, m, path)))
                        endpoints.Add(new ImplementedEndpoint(className, methodName, m, path, auth, typeArgs, modelType));
                }
            }
        }

        return new Wrapper(endpoints, warnings);
    }

    /// <summary>(class, method) -&gt; T for every <c>Logic</c> method returning <c>Task&lt;EsiResponse&lt;T&gt;&gt;</c>.</summary>
    private static Dictionary<(string, string), Type> ReflectResponseModels(Assembly assembly)
    {
        var map = new Dictionary<(string, string), Type>();

        Type?[] types;
        try { types = assembly.GetTypes(); }
        catch (ReflectionTypeLoadException ex) { types = ex.Types; }

        foreach (var type in types)
        {
            if (type is null || !type.IsClass || type.Namespace != "ESI.NET.Logic" || !type.Name.EndsWith("Logic"))
                continue;

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var returnType = method.ReturnType;
                if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                    continue;

                var inner = returnType.GetGenericArguments()[0];
                if (!inner.IsGenericType || inner.GetGenericTypeDefinition() != typeof(EsiResponse<>))
                    continue;

                map[(type.Name, method.Name)] = inner.GetGenericArguments()[0];
            }
        }
        return map;
    }

    /// <summary>
    /// Last resort when the <c>Execute</c> call takes its route/security from locals: every
    /// string literal in the method that starts with <c>/</c> is treated as an endpoint,
    /// the (single) <c>HttpMethod.x</c> in the method as the verb, and the presence of
    /// <c>RequestSecurity.Authenticated</c> anywhere in the method as "authenticated".
    /// </summary>
    private static List<(string HttpMethod, string Endpoint, bool Authenticated)> FallbackFromBody(MethodDeclarationSyntax method)
    {
        var verb = method.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
            .Where(m => m.Expression.ToString().Split('.').Last() == "HttpMethod")
            .Select(m => m.Name.Identifier.Text.ToUpperInvariant())
            .FirstOrDefault() ?? "GET";

        var authenticated = method.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
            .Any(m => m.Expression.ToString().Split('.').Last() == "RequestSecurity"
                      && m.Name.Identifier.Text == "Authenticated");

        return method.DescendantNodes().OfType<LiteralExpressionSyntax>()
            .Where(l => l.IsKind(SyntaxKind.StringLiteralExpression) && l.Token.ValueText.StartsWith('/'))
            .Select(l => l.Token.ValueText)
            .Distinct()
            .Select(ep => (verb, ep, authenticated))
            .ToList();
    }

    private static bool IsExecuteCall(InvocationExpressionSyntax call, out string typeArgs)
    {
        typeArgs = "";
        var generic = call.Expression switch
        {
            GenericNameSyntax g => g,                                         // Execute<T>(...)  via using static
            MemberAccessExpressionSyntax { Name: GenericNameSyntax g } => g,  // EsiRequest.Execute<T>(...)
            _ => null,
        };
        if (generic is null || generic.Identifier.Text != "Execute")
            return false;

        typeArgs = string.Join(", ", generic.TypeArgumentList.Arguments.Select(a => a.ToString()));
        return true;
    }

    /// <summary>
    /// Positional args are <c>(client, config, RequestSecurity.x, HttpMethod.y, "endpoint", ...)</c>.
    /// Falls back to scanning every argument if that exact shape is not present.
    /// </summary>
    private static bool TryReadArguments(InvocationExpressionSyntax call, out string security, out string httpMethod, out string endpoint)
    {
        security = httpMethod = endpoint = "";
        var positional = call.ArgumentList.Arguments.Where(a => a.NameColon is null).ToList();

        if (positional.Count >= 5
            && positional[2].Expression is MemberAccessExpressionSyntax s && Receiver(s) == "RequestSecurity"
            && positional[3].Expression is MemberAccessExpressionSyntax h && Receiver(h) == "HttpMethod"
            && positional[4].Expression is LiteralExpressionSyntax lit && lit.IsKind(SyntaxKind.StringLiteralExpression))
        {
            security = s.Name.Identifier.Text;
            httpMethod = h.Name.Identifier.Text.ToUpperInvariant();
            endpoint = lit.Token.ValueText;
            return true;
        }

        foreach (var arg in call.ArgumentList.Arguments)
        {
            if (arg.Expression is MemberAccessExpressionSyntax member)
            {
                var receiver = Receiver(member);
                if (receiver == "RequestSecurity") security = member.Name.Identifier.Text;
                else if (receiver == "HttpMethod") httpMethod = member.Name.Identifier.Text.ToUpperInvariant();
            }
            else if (endpoint.Length == 0
                     && arg.Expression is LiteralExpressionSyntax l
                     && l.IsKind(SyntaxKind.StringLiteralExpression))
            {
                endpoint = l.Token.ValueText;
            }
        }
        return security.Length > 0 && httpMethod.Length > 0 && endpoint.Length > 0;

        static string Receiver(MemberAccessExpressionSyntax m) => m.Expression.ToString().Split('.').Last();
    }
}
