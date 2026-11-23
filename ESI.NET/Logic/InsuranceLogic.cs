using ESI.NET.Models.Insurance;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Insurance endpoints. Reach them through <see cref="EsiClient.Insurance"/>.
    /// </summary>
    public class InsuranceLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Insurance endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public InsuranceLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// List insurance levels. Return available insurance levels for all ship types.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /insurance/prices</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Insurance"/>.</returns>
        public async Task<EsiResponse<List<Insurance>>> Levels(EsiCallOptions options = null)
            => await Execute<List<Insurance>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/insurance/prices/",
                options: options).ConfigureAwait(false);

    }
}