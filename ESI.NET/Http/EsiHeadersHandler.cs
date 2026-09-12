using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// Adds the headers ESI expects to every outgoing request: <c>X-User-Agent</c> (from
    /// <see cref="EsiConfig.UserAgent"/>) and <c>Accept: application/json</c>. Content-encoding is
    /// left to the primary handler's automatic decompression rather than a manual
    /// <c>Accept-Encoding</c> header. Used by the <c>AddEsi</c> DI pipeline.
    /// </summary>
    public sealed class EsiHeadersHandler : DelegatingHandler
    {
        private readonly string _userAgent;

        public EsiHeadersHandler(IOptions<EsiConfig> config)
        {
            Guard.NotNull(config, nameof(config));

            _userAgent = config.Value?.UserAgent;
            if (string.IsNullOrWhiteSpace(_userAgent))
                throw new ArgumentException(
                    "EsiConfig.UserAgent is required. Set it to something that identifies your app " +
                    "(character and/or project name) so CCP can contact you rather than cut off ESI access.");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Guard.NotNull(request, nameof(request));

            if (!request.Headers.Contains("X-User-Agent"))
                request.Headers.Add("X-User-Agent", _userAgent);

            if (request.Headers.Accept.Count == 0)
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
