using ESI.NET.Models.Structures;
using System.Collections.Generic;
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
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString() } },
                options: options);

        /// <summary>/corporations/{corporation_id}/structures/skyhooks/{skyhook_id}/</summary>
        public async Task<EsiResponse<Skyhook>> Skyhook(long skyhook_id, EsiCallOptions options)
            => await Execute<Skyhook>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/skyhooks/{skyhook_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() },
                    { "skyhook_id", skyhook_id.ToString() }
                },
                options: options);

        /// <summary>/corporations/{corporation_id}/structures/sovereignty-hubs/</summary>
        public async Task<EsiResponse<SovereigntyHubList>> SovereigntyHubs(EsiCallOptions options)
            => await Execute<SovereigntyHubList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/sovereignty-hubs/",
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString() } },
                options: options);

        /// <summary>/corporations/{corporation_id}/structures/sovereignty-hubs/{sovereignty_hub_id}/</summary>
        public async Task<EsiResponse<SovereigntyHub>> SovereigntyHub(long sovereignty_hub_id, EsiCallOptions options)
            => await Execute<SovereigntyHub>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/sovereignty-hubs/{sovereignty_hub_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() },
                    { "sovereignty_hub_id", sovereignty_hub_id.ToString() }
                },
                options: options);

        /// <summary>/characters/{character_id}/structures/mercenary-dens/</summary>
        public async Task<EsiResponse<MercenaryDenList>> MercenaryDens(EsiCallOptions options)
            => await Execute<MercenaryDenList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/structures/mercenary-dens/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString() } },
                options: options);

        /// <summary>/characters/{character_id}/structures/mercenary-dens/{mercenary_den_id}/</summary>
        public async Task<EsiResponse<MercenaryDen>> MercenaryDen(long mercenary_den_id, EsiCallOptions options)
            => await Execute<MercenaryDen>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/structures/mercenary-dens/{mercenary_den_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() },
                    { "mercenary_den_id", mercenary_den_id.ToString() }
                },
                options: options);

        /// <summary>/skyhooks/raidable/ - public list of skyhooks currently raidable.</summary>
        public async Task<EsiResponse<RaidableSkyhookList>> RaidableSkyhooks(EsiCallOptions options = null)
            => await Execute<RaidableSkyhookList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/skyhooks/raidable/",
                options: options);
    }
}
