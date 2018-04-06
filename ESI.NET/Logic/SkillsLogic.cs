using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Skills;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class SkillsLogic : ISkillsLogic
    {
        private ESIConfig _config;
        private int character_id;

        public SkillsLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
                character_id = _config.AuthorizedCharacter.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/attributes/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Attributes>> Attributes()
            => await Execute<Attributes>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/attributes/");

        /// <summary>
        /// /characters/{character_id}/skills/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<SkillDetails>> List()
            => await Execute<SkillDetails>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/skills/");

        /// <summary>
        /// /characters/{character_id}/skillqueue/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<SkillQueueItem>>> Queue()
            => await Execute<List<SkillQueueItem>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/skillqueue/");
    }
}