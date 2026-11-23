using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// The innermost handler <c>AddEsiCaching()</c> adds: stamps the moment a request actually
    /// leaves for ESI, after the error-limit wait and any token refresh. <see cref="EsiCacheHandler"/>
    /// measures the round trip from that stamp when it estimates ESI's clock from the response's
    /// <c>Date</c>.
    /// </summary>
    internal sealed class EsiSendClockHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            EsiRequestState.SetSentAt(request, Stopwatch.GetTimestamp());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
