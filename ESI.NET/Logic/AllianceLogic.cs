using ESI.NET.Models;
using ESI.NET.Models.Alliance;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Alliance endpoints. Reach them through <see cref="EsiClient.Alliance"/>.
    /// </summary>
    public class AllianceLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Alliance endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public AllianceLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>List all active player alliances.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /alliances</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> All(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/",
                options: options).ConfigureAwait(false);


        /// <summary>Public information about an alliance.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /alliances/{alliance_id}</c>.</para>
        /// </remarks>
        /// <param name="allianceId">The ID of the alliance.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Alliance"/>.</returns>
        public async Task<EsiResponse<Alliance>> Information(long allianceId, EsiCallOptions options = null)
            => await Execute<Alliance>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/{alliance_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// List alliance's corporations. List all current member corporations of an alliance.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /alliances/{alliance_id}/corporations</c>.</para>
        /// </remarks>
        /// <param name="allianceId">The ID of the alliance.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Corporations(long allianceId, EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/{alliance_id}/corporations/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get alliance icon. Get the icon urls for a alliance This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /alliances/{alliance_id}/icons</c>.</para>
        /// </remarks>
        /// <param name="allianceId">The ID of the alliance.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Images"/>.</returns>
        public async Task<EsiResponse<Images>> Icons(long allianceId, EsiCallOptions options = null)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/{alliance_id}/icons/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

    }
}