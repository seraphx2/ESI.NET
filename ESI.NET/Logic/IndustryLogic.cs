using ESI.NET.Models.Industry;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class IndustryLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public IndustryLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /industry/facilities/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Facility>>> Facilities(EsiCallOptions options = null)
            => await Execute<List<Facility>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/industry/facilities/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /industry/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<SolarSystem>>> SolarSystemCostIndices(EsiCallOptions options = null)
            => await Execute<List<SolarSystem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/industry/systems/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /characters/{character_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Job>>> JobsForCharacter(bool include_completed = false, EsiCallOptions options = null)
            => await Execute<List<Job>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/industry/jobs/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                parameters: new string[]
                {
                    $"include_completed={include_completed}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/mining/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Entry>>> MiningLedger(EsiCallOptions options)
            => await Execute<List<Entry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mining/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Observer>>> Observers(EsiCallOptions options)
            => await Execute<List<Observer>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporation/{corporation_id}/mining/observers/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/{observer_id}/
        /// </summary>
        /// <param name="observer_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ObserverInfo>>> ObservedMining(long observer_id, EsiCallOptions options)
            => await Execute<List<ObserverInfo>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporation/{corporation_id}/mining/observers/{observer_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() },
                    { "observer_id", observer_id.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Job>>> JobsForCorporation(bool include_completed = false, EsiCallOptions options = null)
            => await Execute<List<Job>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/industry/jobs/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                parameters: new string[]
                {
                    $"include_completed={include_completed}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporation/{corporation_id}/mining/extractions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Extraction>>> Extractions(EsiCallOptions options)
            => await Execute<List<Extraction>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporation/{corporation_id}/mining/extractions/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options).ConfigureAwait(false);
    }
}