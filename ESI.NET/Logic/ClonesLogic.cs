using ESI.NET.Models.Clones;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class ClonesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public ClonesLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/clones/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Clones>> List(EsiCallOptions options)
            => await Execute<Clones>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/clones/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/implants/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Implants(EsiCallOptions options)
            => await Execute<long[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/implants/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options).ConfigureAwait(false);
    }
}