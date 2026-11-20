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

                if (response.StatusCode != HttpStatusCode.NoContent)
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
            Expires = response.Content.Headers.Expires?.UtcDateTime;
            LastModified = response.Content.Headers.LastModified?.UtcDateTime;

            ErrorLimitRemain = IntHeader(response, "X-Esi-Error-Limit-Remain");
            ErrorLimitReset = IntHeader(response, "X-Esi-Error-Limit-Reset");

            if (response.StatusCode == HttpStatusCode.NoContent)
                Message = "No Content";
            else if (response.StatusCode == HttpStatusCode.OK ||
                     response.StatusCode == HttpStatusCode.Created)
                Data = ReadData(body);
            else if (response.StatusCode == HttpStatusCode.NotModified)
                Message = "Not Modified";
            else
                Message = ReadError(body);
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
            catch (JsonException)
            {
                return body;
            }
        }

        private static string Header(HttpResponseMessage response, string name)
            => response.Headers.TryGetValues(name, out var values) ? values.FirstOrDefault() : null;

        private static int? IntHeader(HttpResponseMessage response, string name)
            => int.TryParse(Header(response, name), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : (int?)null;

        public Guid RequestId { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Endpoint { get; set; }

        /// <summary>Always <c>"latest"</c>; it predates compatibility dates and means nothing now.</summary>
        [Obsolete("Meaningless since requests pin a compatibility date - see EsiVersion.CompatibilityDate. Will be removed with the next compatibility-date release.")]
        public string Version { get; set; } = "latest";
        public DateTime? Expires { get; set; }
        public DateTime? LastModified { get; set; }
        public string ETag { get; set; }
        public int? ErrorLimitRemain { get; set; }
        public int? ErrorLimitReset { get; set; }
        public int? Pages { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        /// <summary>
        /// No longer set. A body that does not fit the model now throws
        /// <see cref="EsiDeserializationException"/>, and an unreadable error body is
        /// passed through on <see cref="Message"/>.
        /// </summary>
        [Obsolete("Never set: a 200/201 body that does not fit the model throws EsiDeserializationException instead. Will be removed with the next compatibility-date release.")]
        public Exception Exception { get; set; }
    }
}
