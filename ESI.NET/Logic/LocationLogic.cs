using ESI.NET.Models.Location;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class LocationLogic : ILocationLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id;

        public LocationLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/location/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Location>> Location(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Location>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/location/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/ship/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Ship>> Ship(string eTag = null, CancellationToken cancellationToken = default)
            => await Execute<Ship>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/ship/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/online/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Activity>> Online(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Activity>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/online/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                token: _data.Token);
    }
}