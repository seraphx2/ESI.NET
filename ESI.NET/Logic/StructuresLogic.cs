using ESI.NET.Models.Structures;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// The 2026 structure types - skyhooks, sovereignty hubs, mercenary dens.
    /// Corporation views need <c>esi-structures.read_corporation.v1</c>, the
    /// character's mercenary dens need <c>esi-structures.read_character.v1</c>;
    /// the raidable-skyhook list is public.
    /// </summary>
    public class StructuresLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Structures endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public StructuresLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>Listing of all Skyhooks.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/structures/skyhooks</c>.</para>
        /// <para>Requires the <c>esi-structures.read_corporation.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Station_Manager</c> corporation role.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SkyhookList"/>.</returns>
        public async Task<EsiResponse<SkyhookList>> Skyhooks(EsiCallOptions options)
            => await Execute<SkyhookList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/skyhooks/",
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of a Skyhook.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/structures/skyhooks/{skyhook_id}</c>.</para>
        /// <para>Requires the <c>esi-structures.read_corporation.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Station_Manager</c> corporation role.</para>
        /// </remarks>
        /// <param name="skyhookId">The ID of the Skyhook.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Skyhook"/>.</returns>
        public async Task<EsiResponse<Skyhook>> Skyhook(long skyhookId, EsiCallOptions options)
            => await Execute<Skyhook>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/skyhooks/{skyhook_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "skyhook_id", skyhookId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Listing of all Sovereignty Hubs.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/structures/sovereignty-hubs</c>.</para>
        /// <para>Requires the <c>esi-structures.read_corporation.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SovereigntyHubList"/>.</returns>
        public async Task<EsiResponse<SovereigntyHubList>> SovereigntyHubs(EsiCallOptions options)
            => await Execute<SovereigntyHubList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/sovereignty-hubs/",
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of a Sovereignty Hub.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/structures/sovereignty-hubs/{sovereignty_hub_id}</c>.</para>
        /// <para>Requires the <c>esi-structures.read_corporation.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Station_Manager</c> corporation role.</para>
        /// </remarks>
        /// <param name="sovereigntyHubId">The ID of the Sovereignty Hub.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SovereigntyHub"/>.</returns>
        public async Task<EsiResponse<SovereigntyHub>> SovereigntyHub(long sovereigntyHubId, EsiCallOptions options)
            => await Execute<SovereigntyHub>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/sovereignty-hubs/{sovereignty_hub_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "sovereignty_hub_id", sovereigntyHubId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Listing of all Mercenary Dens.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/structures/mercenary-dens</c>.</para>
        /// <para>Requires the <c>esi-structures.read_character.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MercenaryDenList"/>.</returns>
        public async Task<EsiResponse<MercenaryDenList>> MercenaryDens(EsiCallOptions options)
            => await Execute<MercenaryDenList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/structures/mercenary-dens/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of a Mercenary Den.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /characters/{character_id}/structures/mercenary-dens/{mercenary_den_id}</c>.</para>
        /// <para>Requires the <c>esi-structures.read_character.v1</c> scope.</para>
        /// </remarks>
        /// <param name="mercenaryDenId">The ID of the Mercenary Den.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MercenaryDen"/>.</returns>
        public async Task<EsiResponse<MercenaryDen>> MercenaryDen(long mercenaryDenId, EsiCallOptions options)
            => await Execute<MercenaryDen>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/structures/mercenary-dens/{mercenary_den_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "mercenary_den_id", mercenaryDenId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List (upcoming) raidable Skyhooks. Listing of all Skyhooks that currently or will
        /// shortly be raidable.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /skyhooks/raidable</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="RaidableSkyhookList"/>.</returns>
        public async Task<EsiResponse<RaidableSkyhookList>> RaidableSkyhooks(EsiCallOptions options = null)
            => await Execute<RaidableSkyhookList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/skyhooks/raidable/",
                options: options).ConfigureAwait(false);
    }
}
