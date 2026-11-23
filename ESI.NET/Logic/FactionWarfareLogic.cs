using ESI.NET.Models.FactionWarfare;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Faction Warfare endpoints. Reach them through
    /// <see cref="EsiClient.FactionWarfare"/>.
    /// </summary>
    public class FactionWarfareLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Faction Warfare endpoints. <see cref="EsiClient"/> does this; there is no
        /// need to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public FactionWarfareLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Data about which NPC factions are at war. Data about which NPC factions are at war This
        /// route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /fw/wars</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="War"/>.</returns>
        public async Task<EsiResponse<List<War>>> List(EsiCallOptions options = null)
            => await Execute<List<War>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/wars/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// An overview of statistics about factions involved in faction warfare. Statistical
        /// overviews of factions involved in faction warfare This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /fw/stats</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Stat"/>.</returns>
        public async Task<EsiResponse<List<Stat>>> Stats(EsiCallOptions options = null)
            => await Execute<List<Stat>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/stats/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Ownership of faction warfare systems. An overview of the current ownership of faction
        /// warfare solar systems.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /fw/systems</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="FactionWarfareSystem"/>.</returns>
        public async Task<EsiResponse<List<FactionWarfareSystem>>> Systems(EsiCallOptions options = null)
            => await Execute<List<FactionWarfareSystem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/systems/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// List of the top factions in faction warfare. Top 4 leaderboard of factions for kills and
        /// victory points separated by total, last week and yesterday This route expires daily at
        /// 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /fw/leaderboards</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Leaderboards{FactionTotal}"/>.</returns>
        public async Task<EsiResponse<Leaderboards<FactionTotal>>> Leaderboads(EsiCallOptions options = null)
            => await Execute<Leaderboards<FactionTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/leaderboards/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// List of the top corporations in faction warfare. Top 10 leaderboard of corporations for
        /// kills and victory points separated by total, last week and yesterday This route expires
        /// daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /fw/leaderboards/corporations</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Leaderboards{CorporationTotal}"/>.</returns>
        public async Task<EsiResponse<Leaderboards<CorporationTotal>>> LeaderboardsForCorporations(EsiCallOptions options = null)
            => await Execute<Leaderboards<CorporationTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/leaderboards/corporations/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// List of the top pilots in faction warfare. Top 100 leaderboard of pilots for kills and
        /// victory points separated by total, last week and yesterday This route expires daily at
        /// 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /fw/leaderboards/characters</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Leaderboards{CharacterTotal}"/>.</returns>
        public async Task<EsiResponse<Leaderboards<CharacterTotal>>> LeaderboardsForCharacters(EsiCallOptions options = null)
            => await Execute<Leaderboards<CharacterTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/leaderboards/characters/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Overview of a corporation involved in faction warfare. Statistics about a corporation
        /// involved in faction warfare This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/fw/stats</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_fw_stats.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Stat"/>.</returns>
        public async Task<EsiResponse<Stat>> StatsForCorporation(EsiCallOptions options)
            => await Execute<Stat>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/fw/stats/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Overview of a character involved in faction warfare. Statistical overview of a character
        /// involved in faction warfare This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/fw/stats</c>.</para>
        /// <para>Requires the <c>esi-characters.read_fw_stats.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Stat"/>.</returns>
        public async Task<EsiResponse<Stat>> StatsForCharacter(EsiCallOptions options)
            => await Execute<Stat>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fw/stats/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}