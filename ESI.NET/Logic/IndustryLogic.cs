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
            => await Execute<List<Facility>>(_config, RequestSecurity.Public, RequestMethod.GET, "/industry/facilities/");

        /// <summary>
        /// /industry/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<SolarSystem>>> SolarSystemCostIndices()
            => await Execute<List<SolarSystem>>(_config, RequestSecurity.Public, RequestMethod.GET, "/industry/systems/");

        /// <summary>
        /// /characters/{character_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Job>>> JobsForCharacter(bool include_completed = false)
            => await Execute<List<Job>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/industry/jobs/", new string[]
            {
                $"include_completed={include_completed}"
            });

        /// <summary>
        /// /characters/{character_id}/mining/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Entry>>> MiningLedger(int page = 1)
            => await Execute<List<Entry>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/mining/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Observer>>> Observers(int page = 1)
            => await Execute<List<Observer>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporation/{corporation_id}/mining/observers/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/{observer_id}/
        /// </summary>
        /// <param name="observer_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ObserverInfo>>> ObservedMining(int observer_id, int page = 1)
            => await Execute<List<ObserverInfo>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporation/{corporation_id}/mining/observers/{observer_id}/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Job>>> JobsForCorporation(bool include_completed = false, int page = 1)
            => await Execute<List<Job>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/industry/jobs/", new string[]
            {
                $"include_completed={include_completed}",
                $"page={page}"
            });

        /// <summary>
        /// /corporation/{corporation_id}/mining/extractions/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Extraction>>> Extractions()
            => await Execute<List<Extraction>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporation/{corporation_id}/mining/extractions/");
    }
}