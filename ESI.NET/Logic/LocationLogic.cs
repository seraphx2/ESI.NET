using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Location;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class LocationLogic : ILocationLogic
    {
        private ESIConfig _config;
        private int character_id;

        public LocationLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
                character_id = _config.AuthorizedCharacter.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/location/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Location>> Location()
            => await Execute<Location>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/location/");

        /// <summary>
        /// /characters/{character_id}/ship/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Ship>> Ship()
            => await Execute<Ship>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/ship/");

        /// <summary>
        /// /characters/{character_id}/online/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Activity>> Online()
            => await Execute<Activity>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/online/");
    }
}