using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ESI.NET
{
    /// <summary>What an authenticated ESI route needs: one SSO scope and, for some, a corporation role.</summary>
    internal sealed class EsiEndpointRequirement
    {
        public EsiEndpointRequirement(string scope, string[] roles)
        {
            Scope = scope;
            Roles = roles;
        }

        /// <summary>The SSO scope the access token must carry.</summary>
        public string Scope { get; }

        /// <summary>
        /// Corporation roles, any one of which the character must hold in game. Empty for most
        /// routes. ESI.NET cannot check these before the call (that would need its own scoped
        /// request), so they only explain a <c>403</c>.
        /// </summary>
        public IReadOnlyList<string> Roles { get; }
    }

    /// <summary>
    /// The route -&gt; scope / role table, generated from ESI's spec into
    /// <c>EsiEndpointRequirements.g.cs</c> by <c>tools/SpecCheck --write-docs</c>.
    /// </summary>
    internal static partial class EsiEndpointRequirements
    {
        private static readonly string[] NoRoles = Array.Empty<string>();
        private static readonly char[] Space = { ' ' };

        /// <summary>
        /// The requirement for <paramref name="method"/> + <paramref name="template"/> (the route as
        /// a Logic method passes it, e.g. <c>/characters/{character_id}/wallet/</c>), or null for a
        /// public route.
        /// </summary>
        public static EsiEndpointRequirement Find(HttpMethod method, string template)
            => Find(method.Method, template);

        /// <inheritdoc cref="Find(HttpMethod, string)"/>
        public static EsiEndpointRequirement Find(string method, string template)
        {
            if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(template))
                return null;
            return Table.TryGetValue(method.ToUpperInvariant() + " " + Normalize(template), out var requirement)
                ? requirement
                : null;
        }

        /// <summary>Whether the space-separated <paramref name="granted"/> scopes include <paramref name="scope"/>.</summary>
        public static bool IsGranted(string granted, string scope)
        {
            foreach (var candidate in Split(granted))
                if (string.Equals(candidate, scope, StringComparison.Ordinal))
                    return true;
            return false;
        }

        /// <summary>The space-separated scopes of a token as a list.</summary>
        public static string[] Split(string granted)
            => (granted ?? string.Empty).Split(Space, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>Leading slash, no trailing slash - the form the table is keyed by.</summary>
        private static string Normalize(string path)
        {
            if (path[0] != '/')
                path = "/" + path;
            return path.Length > 1 ? path.TrimEnd('/') : path;
        }
    }
}
