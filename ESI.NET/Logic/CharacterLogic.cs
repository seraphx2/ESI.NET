using ESI.NET.Models;
using ESI.NET.Models.Character;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class CharacterLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public CharacterLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/affiliation/
        /// </summary>
        /// <param name="characterIds">dynamic = long</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Affiliation>>> Affiliation(long[] character_ids, EsiCallOptions options = null)
            => await Execute<List<Affiliation>>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/characters/affiliation/",
                body: character_ids,
                options: options);


        /// <summary>
        /// /characters/{character_id}/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Information>> Information(long character_id, EsiCallOptions options = null)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/characters/{character_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /characters/{character_id}/agents_research/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Agent>>> AgentsResearch(EsiCallOptions options)
            => await Execute<List<Agent>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/agents_research/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/blueprints/
        /// </summary>
        /// <param name="page">Which page of results to return</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Blueprint>>> Blueprints(EsiCallOptions options)
            => await Execute<List<Blueprint>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/blueprints/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/corporationhistory/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<CorporationHistory>>> CorporationHistory(long character_id, EsiCallOptions options = null)
            => await Execute<List<CorporationHistory>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/characters/{character_id}/corporationhistory/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /characters/{character_id}/cspa/
        /// </summary>
        /// <param name="character_ids">The target characters to calculate the charge for</param>
        /// <returns></returns>
        public async Task<EsiResponse<decimal>> CSPA(object character_ids, EsiCallOptions options)
            => await Execute<decimal>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/cspa/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                body: character_ids,
                options: options);

        /// <summary>
        /// /characters/{character_id}/fatigue/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Fatigue>> Fatigue(EsiCallOptions options)
            => await Execute<Fatigue>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fatigue/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/medals/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Medal>>> Medals(EsiCallOptions options)
            => await Execute<List<Medal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/medals/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/notifications/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Notification>>> Notifications(EsiCallOptions options)
            => await Execute<List<Notification>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/notifications/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/notifications/contacts/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContactNotification>>> ContactNotifications(EsiCallOptions options)
            => await Execute<List<ContactNotification>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/notifications/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/portrait/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Images>> Portrait(long character_id, EsiCallOptions options = null)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/characters/{character_id}/portrait/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /characters/{character_id}/roles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Roles>> Roles(EsiCallOptions options)
            => await Execute<Roles>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/roles/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/standings/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Standing>>> Standings(EsiCallOptions options)
            => await Execute<List<Standing>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/standings/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Title>>> Titles(EsiCallOptions options)
            => await Execute<List<Title>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/titles/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);


        /// <summary>
        /// /characters/{character_id}/access-lists/ - scope esi-access.read_lists.v1
        /// </summary>
        public async Task<EsiResponse<AccessListRefList>> AccessLists(EsiCallOptions options)
            => await Execute<AccessListRefList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/access-lists/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/access-lists/{access_list_id}/ - scope esi-access.read_lists.v1
        /// </summary>
        public async Task<EsiResponse<AccessList>> AccessList(long access_list_id, EsiCallOptions options)
            => await Execute<AccessList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/access-lists/{access_list_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() },
                    { "access_list_id", access_list_id.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/mercenary-tactical-operations/ - scope esi-activities.read_character.v1
        /// </summary>
        public async Task<EsiResponse<MercenaryTacticalOperationList>> MercenaryTacticalOperations(EsiCallOptions options)
            => await Execute<MercenaryTacticalOperationList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mercenary-tactical-operations/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/mercenary-tactical-operations/{operation_id}/ - scope esi-activities.read_character.v1
        /// </summary>
        public async Task<EsiResponse<MercenaryTacticalOperation>> MercenaryTacticalOperation(string operation_id, EsiCallOptions options)
            => await Execute<MercenaryTacticalOperation>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mercenary-tactical-operations/{operation_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() },
                    { "operation_id", operation_id }
                },
                options: options);
    }
}