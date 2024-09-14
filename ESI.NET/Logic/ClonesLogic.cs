using ESI.NET.Models.Clones;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class ClonesLogic : IClonesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id;

        public ClonesLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/clones/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Clones>> List(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Clones>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/clones/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/implants/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Implants(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/implants/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                token: _data.Token);
    }
}