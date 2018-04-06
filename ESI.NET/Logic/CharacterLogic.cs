using ESI.NET.Logic.Interfaces;
using ESI.NET.Models;
using ESI.NET.Models.Character;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class CharacterLogic : ICharacterLogic
    {
        private ESIConfig _config;
        private int character_id;

        public CharacterLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
                character_id = _config.AuthorizedCharacter.CharacterID;
        }

        /// <summary>
        /// /characters/affiliation/
        /// </summary>
        /// <param name="characterIds">dynamic = long</param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Affiliation>>> Affiliation(List<int> character_ids)
            => await Execute<List<Affiliation>>(_config, RequestSecurity.Public, RequestMethod.POST, "/characters/affiliation/", body: character_ids.ToArray());

        /// <summary>
        /// /characters/names/
        /// </summary>
        /// <param name="characterIds"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Character>>> Names(List<int> character_ids)
            => await Execute<List<Character>>(_config, RequestSecurity.Public, RequestMethod.GET, "/characters/names/", new string[]
            {
                $"character_ids={string.Join(",", character_ids)}"
            });

        /// <summary>
        /// /characters/{character_id}/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Information>> Information(int character_id)
            => await Execute<Information>(_config, RequestSecurity.Public, RequestMethod.GET, $"/characters/{character_id}/");

        /// <summary>
        /// /characters/{character_id}/agents_research/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Agent>>> AgentsResearch()
            => await Execute<List<Agent>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/agents_research/");

        /// <summary>
        /// /characters/{character_id}/blueprints/
        /// </summary>
        /// <param name="page">Which page of results to return</param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Blueprint>>> Blueprints(int page = 1)
            => await Execute<List<Blueprint>>(_config, RequestSecurity.Public, RequestMethod.GET, $"/characters/{character_id}/blueprints/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /characters/{character_id}/chat_channels/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<ChatChannel>>> ChatChannels()
            => await Execute<List<ChatChannel>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/chat_channels/");

        /// <summary>
        /// /characters/{character_id}/corporationhistory/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Corporation>>> CorporationHistory(int character_id)
            => await Execute<List<Corporation>>(_config, RequestSecurity.Public, RequestMethod.GET, $"/characters/{character_id}/corporationhistory/");

        /// <summary>
        /// /characters/{character_id}/cspa/
        /// </summary>
        /// <param name="character_ids">The target characters to calculate the charge for</param>
        /// <returns></returns>
        public async Task<ApiResponse<CSPA>> CalculateCSPA(object character_ids)
            => await Execute<CSPA>(_config, RequestSecurity.Authenticated, RequestMethod.POST, $"/characters/{character_id}/cspa/", body: character_ids);

        /// <summary>
        /// /characters/{character_id}/fatigue/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Fatigue>> Fatigue()
            => await Execute<Fatigue>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/fatigue/");

        /// <summary>
        /// /characters/{character_id}/medals/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Medal>>> Medals()
            => await Execute<List<Medal>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/medals/");

        /// <summary>
        /// /characters/{character_id}/notifications/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Notification>>> Notifications()
            => await Execute<List<Notification>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/notifications/");

        /// <summary>
        /// /characters/{character_id}/notifications/contacts/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<ContactNotification>>> ContactNotifications()
            => await Execute<List<ContactNotification>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/notifications/contacts/");

        /// <summary>
        /// /characters/{character_id}/portrait/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Images>> Portrait(int character_id)
            => await Execute<Images>(_config, RequestSecurity.Public, RequestMethod.GET, $"/characters/{character_id}/portrait/");

        /// <summary>
        /// /characters/{character_id}/roles/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<string>>> Roles()
            => await Execute<List<string>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/roles/");

        /// <summary>
        /// /characters/{character_id}/standings/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Standing>>> Standings()
            => await Execute<List<Standing>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/standings/");

        /// <summary>
        /// /characters/{character_id}/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Stats>>> Stats()
            => await Execute<List<Stats>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/stats/");

        /// <summary>
        /// /characters/{character_id}/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Title>>> Titles()
            => await Execute<List<Title>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/titles/");
    }
}