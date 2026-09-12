using ESI.NET.Models.Location;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class LocationLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public LocationLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/location/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Location>> Location(EsiCallOptions options)
            => await Execute<Location>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/location/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/ship/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Ship>> Ship(EsiCallOptions options)
            => await Execute<Ship>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/ship/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/online/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Activity>> Online(EsiCallOptions options)
            => await Execute<Activity>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/online/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}