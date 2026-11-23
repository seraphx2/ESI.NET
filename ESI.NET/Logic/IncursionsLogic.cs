using ESI.NET.Models.Incursions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Incursions endpoints. Reach them through <see cref="EsiClient.Incursions"/>.
    /// </summary>
    public class IncursionsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Incursions endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public IncursionsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>Return a list of current incursions.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /incursions</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Incursion"/>.</returns>
        public async Task<EsiResponse<List<Incursion>>> All(EsiCallOptions options = null)
            => await Execute<List<Incursion>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/incursions/",
                options: options).ConfigureAwait(false);

    }
}