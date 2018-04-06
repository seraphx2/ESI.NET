using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.PlanetaryInteraction;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class PlanetaryInteractionLogic : IPlanetaryInteractionLogic
    {
        private ESIConfig _config;
        private int character_id, corporation_id;

        public PlanetaryInteractionLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
                corporation_id = _config.AuthorizedCharacter.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/planets/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Planet>>> Colonies()
            => await Execute<List<Planet>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/planets/");

        /// <summary>
        /// /characters/{character_id}/planets/{planet_id}/
        /// </summary>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ColonyLayout>> ColonyLayout(int planet_id)
            => await Execute<ColonyLayout>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/planets/{planet_id}/");

        /// <summary>
        /// /corporations/{corporation_id}/customs_offices/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<CustomsOffice>>> CorporationCustomsOffices()
            => await Execute<List<CustomsOffice>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/customs_offices/");

        /// <summary>
        /// /universe/schematics/{schematic_id}/
        /// </summary>
        /// <param name="schematic_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Schematic>> SchematicInformation(int schematic_id)
            => await Execute<Schematic>(_config, RequestSecurity.Public, RequestMethod.GET, $"/universe/schematics/{schematic_id}/");
    }
}