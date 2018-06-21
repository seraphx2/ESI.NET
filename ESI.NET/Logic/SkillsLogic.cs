using ESI.NET.Models.Skills;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class SkillsLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int character_id;

        public SkillsLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/attributes/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Attributes>> Attributes()
            => await Execute<Attributes>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/attributes/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/skills/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<SkillDetails>> List()
            => await Execute<SkillDetails>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/skills/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/skillqueue/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<SkillQueueItem>>> Queue()
            => await Execute<List<SkillQueueItem>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/skillqueue/", token: _data.Token);
    }
}