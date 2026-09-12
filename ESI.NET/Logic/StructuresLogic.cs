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

        public StructuresLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>/corporations/{corporation_id}/structures/skyhooks/</summary>
        public async Task<EsiResponse<SkyhookList>> Skyhooks(EsiCallOptions options)
            => await Execute<SkyhookList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/skyhooks/",
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>/corporations/{corporation_id}/structures/skyhooks/{skyhook_id}/</summary>
        public async Task<EsiResponse<Skyhook>> Skyhook(long skyhookId, EsiCallOptions options)
            => await Execute<Skyhook>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/skyhooks/{skyhook_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "skyhook_id", skyhookId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>/corporations/{corporation_id}/structures/sovereignty-hubs/</summary>
        public async Task<EsiResponse<SovereigntyHubList>> SovereigntyHubs(EsiCallOptions options)
            => await Execute<SovereigntyHubList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/sovereignty-hubs/",
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>/corporations/{corporation_id}/structures/sovereignty-hubs/{sovereignty_hub_id}/</summary>
        public async Task<EsiResponse<SovereigntyHub>> SovereigntyHub(long sovereigntyHubId, EsiCallOptions options)
            => await Execute<SovereigntyHub>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/sovereignty-hubs/{sovereignty_hub_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "sovereignty_hub_id", sovereigntyHubId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>/characters/{character_id}/structures/mercenary-dens/</summary>
        public async Task<EsiResponse<MercenaryDenList>> MercenaryDens(EsiCallOptions options)
            => await Execute<MercenaryDenList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/structures/mercenary-dens/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>/characters/{character_id}/structures/mercenary-dens/{mercenary_den_id}/</summary>
        public async Task<EsiResponse<MercenaryDen>> MercenaryDen(long mercenaryDenId, EsiCallOptions options)
            => await Execute<MercenaryDen>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/structures/mercenary-dens/{mercenary_den_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "mercenary_den_id", mercenaryDenId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>/skyhooks/raidable/ - public list of skyhooks currently raidable.</summary>
        public async Task<EsiResponse<RaidableSkyhookList>> RaidableSkyhooks(EsiCallOptions options = null)
            => await Execute<RaidableSkyhookList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/skyhooks/raidable/",
                options: options).ConfigureAwait(false);
    }
}
