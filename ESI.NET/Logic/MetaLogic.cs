using ESI.NET.Models.Meta;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>ESI's own metadata - all public.</summary>
    public class MetaLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Meta endpoints. <see cref="EsiClient"/> does this; there is no need to call
        /// it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public MetaLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>Get the changelog of this API.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /meta/changelog</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MetaChangelog"/>.</returns>
        public async Task<EsiResponse<MetaChangelog>> Changelog(EsiCallOptions options = null)
            => await Execute<MetaChangelog>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/changelog/",
                options: options).ConfigureAwait(false);

        /// <summary>Get a list of compatibility dates.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /meta/compatibility-dates</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MetaCompatibilityDates"/>.</returns>
        public async Task<EsiResponse<MetaCompatibilityDates>> CompatibilityDates(EsiCallOptions options = null)
            => await Execute<MetaCompatibilityDates>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/compatibility-dates/",
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get the name of ESI. Get the name ESI is currently going by, plus the full and glorious
        /// history of every name it has ever had. The three letters have never once meant the same
        /// thing twice.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /meta/name</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MetaName"/>.</returns>
        public async Task<EsiResponse<MetaName>> Name(EsiCallOptions options = null)
            => await Execute<MetaName>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/name/",
                options: options).ConfigureAwait(false);

        /// <summary>Get health status. Get the health status of each API route.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /meta/status</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MetaStatus"/>.</returns>
        public async Task<EsiResponse<MetaStatus>> Status(EsiCallOptions options = null)
            => await Execute<MetaStatus>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/status/",
                options: options).ConfigureAwait(false);
    }
}
