using ESI.NET.Models.PlanetaryInteraction;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class PlanetaryInteractionLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public PlanetaryInteractionLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/planets/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Planet>>> Colonies(EsiCallOptions options)
            => await Execute<List<Planet>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/planets/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/planets/{planet_id}/
        /// </summary>
        /// <param name="planetId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<ColonyLayout>> ColonyLayout(long planetId, EsiCallOptions options)
            => await Execute<ColonyLayout>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/planets/{planet_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "planet_id", planetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/customs_offices/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<CustomsOffice>>> CorporationCustomsOffices(EsiCallOptions options)
            => await Execute<List<CustomsOffice>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/customs_offices/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /universe/schematics/{schematic_id}/
        /// </summary>
        /// <param name="schematicId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Schematic>> SchematicInformation(long schematicId, EsiCallOptions options = null)
            => await Execute<Schematic>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/schematics/{schematic_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "schematic_id", schematicId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

    }
}