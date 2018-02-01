using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Industry;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class IndustryLogic : IIndustryLogic
    {
        private ESIConfig _config;
        private int corporation_id, character_id;

        public IndustryLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                corporation_id = _config.AuthorizedCharacter.CorporationID;
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /industry/facilities/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Facility>>> Facilities()
        {
            var endpoint = "/industry/facilities/";
            var response = await Execute<List<Facility>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /industry/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<SolarSystem>>> SolarSystemCostIndices()
        {
            var endpoint = "/industry/systems/";
            var response = await Execute<List<SolarSystem>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Job>>> JobsForCharacter(bool include_completed = false)
        {
            var endpoint = $"/characters/{character_id}/industry/jobs/";
            var response = await Execute<List<Job>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"include_completed={include_completed}"
            });

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mining/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Entry>>> MiningLedger(int page = 1)
        {
            var endpoint = $"/characters/{character_id}/mining/";
            var response = await Execute<List<Entry>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Observer>>> Observers(int page = 1)
        {
            var endpoint = $"/corporation/{corporation_id}/mining/observers/";
            var response = await Execute<List<Observer>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/{observer_id}/
        /// </summary>
        /// <param name="observer_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ObserverInfo>>> ObservedMining(int observer_id, int page = 1)
        {
            var endpoint = $"/corporation/{corporation_id}/mining/observers/{observer_id}/";
            var response = await Execute<List<ObserverInfo>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }
        /// <summary>
        /// /corporations/{corporation_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Job>>> JobsForCorporation(bool include_completed = false, int page = 1)
        {
            var endpoint = $"/corporations/{corporation_id}/industry/jobs/";
            var response = await Execute<List<Job>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"include_completed={include_completed}",
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporation/{corporation_id}/mining/extractions/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Extraction>>> Extractions()
        {
            var endpoint = $"/corporation/{corporation_id}/mining/extractions/";
            var response = await Execute<List<Extraction>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }
    }
}
