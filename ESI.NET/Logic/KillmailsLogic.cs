using ESI.NET.Models.Killmails;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Killmails endpoints. Reach them through <see cref="EsiClient.Killmails"/>.
    /// </summary>
    public class KillmailsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Killmails endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public KillmailsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Get a character's recent kills and losses. Return a list of a character's kills and
        /// losses going back 90 days.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/killmails/recent</c>.</para>
        /// <para>Requires the <c>esi-killmails.read_killmails.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Killmail"/>.</returns>
        public async Task<EsiResponse<List<Killmail>>> ForCharacter(EsiCallOptions options)
            => await Execute<List<Killmail>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/killmails/recent/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get a corporation's recent kills and losses. Get a list of a corporation's kills and
        /// losses going back 90 days.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/killmails/recent</c>.</para>
        /// <para>Requires the <c>esi-killmails.read_corporation_killmails.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Killmail"/>.</returns>
        public async Task<EsiResponse<List<Killmail>>> ForCorporation(EsiCallOptions options)
            => await Execute<List<Killmail>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/killmails/recent/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get a single killmail. Return a single killmail from its ID and hash.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /killmails/{killmail_id}/{killmail_hash}</c>.</para>
        /// </remarks>
        /// <param name="killmailHash">The killmail hash for verification.</param>
        /// <param name="killmailId">The killmail ID to be queried.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Information"/>.</returns>
        public async Task<EsiResponse<Information>> Information(string killmailHash, long killmailId, EsiCallOptions options = null)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/killmails/{killmail_id}/{killmail_hash}/",
                replacements: new Dictionary<string, string>()
                {
                    { "killmail_id", killmailId.ToString(CultureInfo.InvariantCulture) },
                    { "killmail_hash", killmailHash ?? throw new ArgumentNullException(nameof(killmailHash)) }
                },
                options: options).ConfigureAwait(false);

    }
}