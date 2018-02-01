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
        {
            var endpoint = $"/characters/{character_id}/planets/";
            var response = await Execute<List<Planet>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/planets/{planet_id}/
        /// </summary>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ColonyLayout>> ColonyLayout(int planet_id)
        {
            var endpoint = $"/characters/{character_id}/planets/{planet_id}/";
            var response = await Execute<ColonyLayout>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/customs_offices/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<CustomsOffice>>> CorporationCustomsOffices()
        {
            var endpoint = $"/corporations/{corporation_id}/customs_offices/";
            var response = await Execute<List<CustomsOffice>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/schematics/{schematic_id}/
        /// </summary>
        /// <param name="schematic_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Schematic>> SchematicInformation(int schematic_id)
        {
            var endpoint = $"/universe/schematics/{schematic_id}/";
            var response = await Execute<Schematic>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        
    }
}