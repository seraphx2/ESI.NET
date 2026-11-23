using ESI.NET.Models.Status;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Status endpoints. Reach them through <see cref="EsiClient.Status"/>.
    /// </summary>
    public class StatusLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Status endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public StatusLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>Get the server's status. Current status of the EVE Online cluster.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /status</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Status"/>.</returns>
        public async Task<EsiResponse<Status>> Retrieve(EsiCallOptions options = null)
            => await Execute<Status>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/status/",
                options: options).ConfigureAwait(false);

    }
}