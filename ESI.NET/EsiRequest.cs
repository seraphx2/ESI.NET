using ESI.NET.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ESI.NET
{
    internal static class EsiRequest
    {
        public static async Task<EsiResponse<T>> Execute<T>(HttpClient client, EsiConfig config, RequestSecurity security, HttpMethod httpMethod, string endpoint, EsiCallOptions options, Dictionary<string, string> replacements = null, string[] parameters = null, object body = null)
        {
            if (options == null)
                options = new EsiCallOptions();

            var path = $"{httpMethod}|{endpoint}";
            var template = endpoint;

            if (replacements != null)
                foreach (var property in replacements)
                    endpoint = endpoint.Replace($"{{{property.Key}}}", property.Value);

            var url = $"{config.EsiUrl.TrimEnd('/')}{endpoint}";

            //Attach query string parameters
            var query = new List<string>();
            if (parameters != null)
                query.AddRange(parameters);
            if (options.Page.HasValue)
                query.Add($"page={options.Page.Value}");
            if (query.Count > 0)
                url += $"?{string.Join("&", query)}";

            using var request = new HttpRequestMessage(httpMethod, url);

            // ESI is versioned by a frozen dated snapshot, selected per request.
            request.Headers.Add("X-Compatibility-Date", EsiVersion.CompatibilityDate);
            request.Headers.Add("X-Tenant", config.DataSource.ToEsiValue());

            EsiRequestState.MarkEsiRequest(request);
            if (options.BypassCache)
                EsiRequestState.SetBypassCache(request);

            //Attach token to request header if this endpoint requires an authorized character
            if (security == RequestSecurity.Authenticated)
            {
                var token = options.Character?.Token;
                if (string.IsNullOrEmpty(token))
                    throw new ArgumentException("The request endpoint requires SSO authentication; EsiCallOptions.Character (with a valid Token) has not been provided.");
                if (config.ValidateScopes && MissingScope<T>(httpMethod, template, options.Character) is { } refused)
                    return refused;

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Hand the character (and any per-call refresh callback) to EsiTokenRefreshHandler,
                // which refreshes a near-expired token and swaps this header before the send.
                EsiRequestState.SetCharacter(request, options.Character);
                if (options.OnTokenRefreshed != null)
                    EsiRequestState.SetCallback(request, options.OnTokenRefreshed);
            }

            if (!string.IsNullOrWhiteSpace(options.IfNoneMatch))
                request.Headers.Add("If-None-Match", ToEntityTag(options.IfNoneMatch));

            //Serialize post body data
            if (body != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            //Output final object
            var response = await client.SendAsync(request, options.CancellationToken).ConfigureAwait(false);
            return await EsiResponse<T>.CreateAsync(response, path, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// When <paramref name="character"/>'s token lacks the scope the route needs, the
        /// <c>403</c> ESI would answer - built here, before the request, so ESI never sees (and
        /// never counts against the error limit) a call that can only be refused. Null when the
        /// scope is granted, or when the character has no recorded scopes to check against.
        /// </summary>
        private static EsiResponse<T> MissingScope<T>(HttpMethod method, string template, Models.SSO.AuthorizedCharacterData character)
        {
            if (string.IsNullOrWhiteSpace(character.Scopes))
                return null;

            var requirement = EsiEndpointRequirements.Find(method, template);
            if (requirement == null || EsiEndpointRequirements.IsGranted(character.Scopes, requirement.Scope))
                return null;

            return EsiResponse<T>.ForMissingScope(method.Method, template, requirement.Scope, character.CharacterID,
                EsiEndpointRequirements.Split(character.Scopes));
        }

        /// <summary>
        /// Builds a valid <c>If-None-Match</c> value from an ETag in any of the spellings a caller
        /// may hold: <c>abc</c> / <c>"abc"</c> become <c>"abc"</c>, and the weak forms
        /// <c>W/abc</c> (what <see cref="EsiResponse{T}.ETag"/> holds, quotes stripped) /
        /// <c>W/"abc"</c> become <c>W/"abc"</c>. Quoting the weak form whole (<c>"W/abc"</c>)
        /// would be a different, strong tag.
        /// </summary>
        internal static string ToEntityTag(string etag)
        {
            var tag = etag.Trim();
            var weak = tag.StartsWith("W/", StringComparison.Ordinal);
            var opaque = (weak ? tag.Substring(2) : tag).Trim().Trim('"');
            return (weak ? "W/" : string.Empty) + "\"" + opaque + "\"";
        }

        public enum RequestSecurity
        {
            Public,
            Authenticated

        }
    }
}
