using ESI.NET.Models.Location;
using ESI.NET.Models.SSO;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class LocationLogic : _BaseLogic
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
        public async Task<EsiResponse<Location>> Location()
            => await Execute<Location>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/location/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/ship/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Ship>> Ship()
            => await Execute<Ship>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/ship/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/online/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Activity>> Online()
            => await Execute<Activity>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/online/", token: _data.Token);
    }
}