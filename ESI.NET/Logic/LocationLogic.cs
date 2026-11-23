using ESI.NET.Models.Location;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Location endpoints. Reach them through <see cref="EsiClient.Location"/>.
    /// </summary>
    public class LocationLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Location endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public LocationLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Get character location. Get the current location of a character, including the station
        /// or structure it is docked in.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/location</c>.</para>
        /// <para>Requires the <c>esi-location.read_location.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Location"/>.</returns>
        public async Task<EsiResponse<Location>> Location(EsiCallOptions options)
            => await Execute<Location>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/location/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get current ship. Get the ship a character is currently in.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/ship</c>.</para>
        /// <para>Requires the <c>esi-location.read_ship_type.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Ship"/>.</returns>
        public async Task<EsiResponse<Ship>> Ship(EsiCallOptions options)
            => await Execute<Ship>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/ship/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character online. Get whether a character is online, and its login statistics.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/online</c>.</para>
        /// <para>Requires the <c>esi-location.read_online.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Activity"/>.</returns>
        public async Task<EsiResponse<Activity>> Online(EsiCallOptions options)
            => await Execute<Activity>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/online/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}