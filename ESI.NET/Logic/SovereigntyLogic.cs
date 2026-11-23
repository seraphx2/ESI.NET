using ESI.NET.Models.Sovereignty;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Sovereignty endpoints. Reach them through <see cref="EsiClient.Sovereignty"/>.
    /// </summary>
    public class SovereigntyLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Sovereignty endpoints. <see cref="EsiClient"/> does this; there is no need
        /// to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public SovereigntyLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>Shows sovereignty data for campaigns.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /sovereignty/campaigns</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Campaign"/>.</returns>
        public async Task<EsiResponse<List<Campaign>>> Campaigns(EsiCallOptions options = null)
            => await Execute<List<Campaign>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/sovereignty/campaigns/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// List sovereignty details for K-space systems. Listing of sovereignty details for all
        /// K-space systems in New Eden.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /sovereignty/systems</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SovereigntySystems"/>.</returns>
        public async Task<EsiResponse<SovereigntySystems>> Systems(EsiCallOptions options = null)
            => await Execute<SovereigntySystems>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/sovereignty/systems/",
                options: options).ConfigureAwait(false);
    }
}