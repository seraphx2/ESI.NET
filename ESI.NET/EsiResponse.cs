using Newtonsoft.Json;
using System;
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
            try
            {
                StatusCode = response.StatusCode;
                Endpoint = path.Split('|')[1];

                if (response.Headers.Contains("X-ESI-Request-ID"))
                    RequestId = Guid.Parse(response.Headers.GetValues("X-ESI-Request-ID").First());

                if (response.Headers.Contains("X-Pages"))
                    Pages = int.Parse(response.Headers.GetValues("X-Pages").First());

                if (response.Headers.Contains("ETag"))
                    ETag = response.Headers.GetValues("ETag").First().Replace("\"", string.Empty);

                if (response.Content.Headers.Contains("Expires"))
                    Expires = DateTime.Parse(response.Content.Headers.GetValues("Expires").First());

                if (response.Content.Headers.Contains("Last-Modified"))
                    LastModified = DateTime.Parse(response.Content.Headers.GetValues("Last-Modified").First());

                if (response.Headers.Contains("X-Esi-Error-Limit-Remain"))
                    ErrorLimitRemain = int.Parse(response.Headers.GetValues("X-Esi-Error-Limit-Remain").First());

                if (response.Headers.Contains("X-Esi-Error-Limit-Reset"))
                    ErrorLimitReset = int.Parse(response.Headers.GetValues("X-Esi-Error-Limit-Reset").First());

                if (response.StatusCode == HttpStatusCode.NoContent)
                    Message = "No Content";
                else if (response.StatusCode == HttpStatusCode.OK ||
                         response.StatusCode == HttpStatusCode.Created)
                {
                    // ESI returns JSON on 200/201 - an object, an array, or a bare
                    // scalar (a wallet balance, a CSPA cost). Trim first: some
                    // endpoints end the body with a newline, which defeated the old
                    // "{ }" / "[ ]" check and silently left Data null.
                    var json = body.Trim();
                    if (json.Length > 0 && "{[\"-0123456789tfn".IndexOf(json[0]) >= 0)
                        Data = JsonConvert.DeserializeObject<T>(json);
                    else
                        Message = body;
                }
                else if (response.StatusCode == HttpStatusCode.NotModified)
                    Message = "Not Modified";
                else
                    Message = JsonConvert.DeserializeAnonymousType(body, new { error = string.Empty }).error;
            }
            catch (Exception ex)
            {
                Message = body;
                Exception = ex;
            }
        }

        public Guid RequestId { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Endpoint { get; set; }
        public string Version { get; set; } = "latest";
        public DateTime? Expires { get; set; }
        public DateTime? LastModified { get; set; }
        public string ETag { get; set; }
        public int? ErrorLimitRemain { get; set; }
        public int? ErrorLimitReset { get; set; }
        public int? Pages { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Exception Exception { get; set; }
    }
}
