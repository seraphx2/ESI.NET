using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET
{
    /// <summary>
    /// What an ESI call returned: the status, the body read into <see cref="Data"/> (or ESI's error
    /// text on <see cref="Message"/>), and ESI's response headers. Every Logic method returns one.
    /// A non-success status does not throw: check <see cref="StatusCode"/>.
    /// </summary>
    /// <typeparam name="T">The model the body is read into.</typeparam>
    public class EsiResponse<T>
    {
        /// <summary>
        /// Reads <paramref name="response"/> and projects its headers and body onto an
        /// <see cref="EsiResponse{T}"/>. The response is disposed before this returns.
        /// </summary>
        internal static async Task<EsiResponse<T>> CreateAsync(HttpResponseMessage response, string path, CancellationToken cancellationToken = default)
        {
            try
            {
                string body = null;

                if (response.StatusCode != HttpStatusCode.NoContent && response.Content != null)
#if NET
                    body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
                    body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

                return new EsiResponse<T>(response, path, body);
            }
            finally
            {
                response.Dispose();
            }
        }

        /// <summary>
        /// The <c>403</c> for a call refused before it was sent because the token lacks the route's
        /// scope (see <see cref="EsiConfig.ValidateScopes"/>). It looks like ESI's own refusal, so
        /// code that handles a 403 keeps working, and <see cref="MissingScope"/> says which scope.
        /// </summary>
        internal static EsiResponse<T> ForMissingScope(string method, string template, string scope, long characterId, string[] granted)
            => new EsiResponse<T>
            {
                StatusCode = HttpStatusCode.Forbidden,
                Endpoint = template,
                MissingScope = scope,
                Message = $"{method} {template} requires the '{scope}' scope, which the access token for character {characterId} was not granted. "
                          + "Request it at SSO login and have the character log in again; ESI.NET did not send the request. "
                          + $"Granted scopes: {(granted.Length == 0 ? "(none)" : string.Join(" ", granted))}.",
            };

        private EsiResponse() { }

        private EsiResponse(HttpResponseMessage response, string path, string body)
        {
            StatusCode = response.StatusCode;
            Endpoint = path.Split('|')[1];

            // Headers are metadata: a malformed one is dropped (left null/empty),
            // never allowed to cost the caller the body.
            if (Guid.TryParse(Header(response, "X-ESI-Request-ID"), out var requestId))
                RequestId = requestId;

            Pages = IntHeader(response, "X-Pages");

            var etag = Header(response, "ETag");
            if (etag != null)
                ETag = etag.Replace("\"", string.Empty);

            // HttpContentHeaders already parses these RFC 1123 dates for us (and
            // does it correctly - a raw DateTime.Parse(headerString) is culture-
            // sensitive and can misparse or throw under a non-default locale).
            // Content can be null on .NET Framework (netstandard2.0), e.g. for a 204.
            Expires = response.Content?.Headers.Expires?.UtcDateTime;
            LastModified = response.Content?.Headers.LastModified?.UtcDateTime;

            ErrorLimitRemain = IntHeader(response, "X-Esi-Error-Limit-Remain");
            ErrorLimitReset = IntHeader(response, "X-Esi-Error-Limit-Reset");

            if (Enum.TryParse(Header(response, Http.EsiCacheHandler.StatusHeader), out EsiCacheStatus cache))
                Cache = cache;

            if (response.StatusCode == HttpStatusCode.NoContent)
                Message = "No Content";
            else if (response.StatusCode == HttpStatusCode.OK ||
                     response.StatusCode == HttpStatusCode.Created)
                Data = ReadData(body);
            else if (response.StatusCode == HttpStatusCode.NotModified)
                Message = "Not Modified";
            else
                Message = ReadError(body);

            if (response.StatusCode == HttpStatusCode.Forbidden)
                Message = WithRoleHint(path, Message);
        }

        /// <summary>
        /// A <c>403</c> from a route that needs a corporation role is usually the role, not the
        /// token: say which roles would do. (A missing scope never gets this far - see
        /// <see cref="MissingScope"/>.)
        /// </summary>
        private static string WithRoleHint(string path, string message)
        {
            var parts = path.Split('|');
            var requirement = parts.Length == 2 ? EsiEndpointRequirements.Find(parts[0], parts[1]) : null;
            if (requirement == null || requirement.Roles.Count == 0)
                return message;

            var hint = requirement.Roles.Count == 1
                ? $"This endpoint requires the character to hold the {requirement.Roles[0]} corporation role."
                : $"This endpoint requires the character to hold one of these corporation roles: {string.Join(", ", requirement.Roles)}.";
            return string.IsNullOrWhiteSpace(message) ? hint : $"{message} ({hint})";
        }

        /// <summary>
        /// ESI answers 200/201 with JSON - an object, an array, or a bare scalar (a
        /// wallet balance, a CSPA cost). An empty body leaves <c>Data</c> at its
        /// default. Anything else that does not read as <typeparamref name="T"/>
        /// throws <see cref="EsiDeserializationException"/>: returning a "successful"
        /// response with no data would hide the failure until a null dereference
        /// somewhere far from the cause.
        /// </summary>
        private T ReadData(string body)
        {
            // Trim: some endpoints end the body with a newline.
            var json = body?.Trim();
            if (string.IsNullOrEmpty(json))
                return default;

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException) when (typeof(T) == typeof(string))
            {
                // A string model holds any text, so a plain-text body loses nothing.
                return (T)(object)body;
            }
            catch (JsonException ex)
            {
                throw new EsiDeserializationException(Endpoint, StatusCode, RequestId, typeof(T), body, ex);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException) && !(ex is EsiDeserializationException))
            {
                // Not only JsonException: a converter from the app's JsonConvert.DefaultSettings,
                // or a model setter, can throw anything (FormatException, ...). The caller still
                // gets the documented exception, with the endpoint, request id and body.
                throw new EsiDeserializationException(Endpoint, StatusCode, RequestId, typeof(T), body, ex);
            }
        }

        /// <summary>
        /// ESI error bodies are <c>{"error": "..."}</c>. Anything that isn't (an HTML
        /// page from a gateway in front of ESI, an empty body) is passed through
        /// as-is: the status code is the real signal, the body is only detail.
        /// </summary>
        private static string ReadError(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return body;

            try
            {
                return JsonConvert.DeserializeAnonymousType(body, new { error = (string)null })?.error ?? body;
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                // Not only JsonException: a converter from the app's JsonConvert.DefaultSettings can
                // throw anything. The error body is only detail, so fall back to it as-is.
                return body;
            }
        }

        private static string Header(HttpResponseMessage response, string name)
            => response.Headers.TryGetValues(name, out var values) ? values.FirstOrDefault() : null;

        private static int? IntHeader(HttpResponseMessage response, string name)
            => int.TryParse(Header(response, name), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : (int?)null;

        /// <summary>ESI's <c>X-ESI-Request-ID</c>; quote it when reporting a problem to CCP. <see cref="Guid.Empty"/> if ESI sent none.</summary>
        public Guid RequestId { get; set; }

        /// <summary>The HTTP status ESI answered with.</summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>The route template that was called, e.g. <c>/characters/{character_id}/wallet/</c>.</summary>
        public string Endpoint { get; set; }

        /// <summary>Always <c>"latest"</c>; it predates compatibility dates and means nothing now.</summary>
        [Obsolete("Meaningless since requests pin a compatibility date - see EsiVersion.CompatibilityDate. Will be removed with the next compatibility-date release.")]
        public string Version { get; set; } = "latest";
        /// <summary>When ESI's cached copy of this response expires (UTC): asking again before then returns the same data.</summary>
        public DateTime? Expires { get; set; }

        /// <summary>When the data last changed (UTC), if ESI says.</summary>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// ESI's entity tag for this response, without quotes (a weak tag keeps its <c>W/</c>). Pass it
        /// back as <see cref="EsiCallOptions.IfNoneMatch"/> to get <c>304 Not Modified</c> if nothing changed.
        /// </summary>
        public string ETag { get; set; }

        /// <summary>Errors left in ESI's error budget for this window (<c>X-Esi-Error-Limit-Remain</c>).</summary>
        public int? ErrorLimitRemain { get; set; }

        /// <summary>Seconds until ESI's error budget resets (<c>X-Esi-Error-Limit-Reset</c>).</summary>
        public int? ErrorLimitReset { get; set; }

        /// <summary>For a paginated endpoint, the number of pages (<c>X-Pages</c>); request them with <see cref="EsiCallOptions.Page"/>.</summary>
        public int? Pages { get; set; }

        /// <summary>
        /// ESI's error text for a failed call (or the raw body when it was not ESI's JSON error), and
        /// <c>No Content</c> / <c>Not Modified</c> for a <c>204</c> / <c>304</c>. Null on success.
        /// </summary>
        public string Message { get; set; }

        /// <summary>The body read into <typeparamref name="T"/>, for a <c>200</c> / <c>201</c>. Default otherwise.</summary>
        public T Data { get; set; }

        /// <summary>
        /// Set when ESI.NET refused the call itself, without sending it, because the access token
        /// lacks this SSO scope (<see cref="StatusCode"/> is then <c>403</c> and <see cref="Message"/>
        /// lists the scopes the token does carry). Add the scope to the ones you request at SSO
        /// login and have the character log in again - an existing token never gains scopes.
        /// Null for every response ESI actually sent. See <see cref="EsiConfig.ValidateScopes"/>.
        /// </summary>
        public string MissingScope { get; set; }

        /// <summary>
        /// Whether this response came from the response cache (<c>AddEsiCaching()</c>):
        /// <see cref="EsiCacheStatus.Hit"/> or <see cref="EsiCacheStatus.Revalidated"/> mean ESI
        /// sent no body. <see cref="EsiCacheStatus.None"/> when caching is off.
        /// </summary>
        public EsiCacheStatus Cache { get; set; }

        /// <summary>
        /// No longer set. A body that does not fit the model now throws
        /// <see cref="EsiDeserializationException"/>, and an unreadable error body is
        /// passed through on <see cref="Message"/>.
        /// </summary>
        [Obsolete("Never set: a 200/201 body that does not fit the model throws EsiDeserializationException instead. Will be removed with the next compatibility-date release.")]
        public Exception Exception { get; set; }
    }
}
