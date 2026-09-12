using ESI.NET.Models.Skills;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class SkillsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public SkillsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/attributes/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Attributes>> Attributes(EsiCallOptions options)
            => await Execute<Attributes>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/attributes/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/skills/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<SkillDetails>> List(EsiCallOptions options)
            => await Execute<SkillDetails>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/skills/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/skillqueue/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<SkillQueueItem>>> Queue(EsiCallOptions options)
            => await Execute<List<SkillQueueItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/skillqueue/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}