using ESI.NET.Models.PlanetaryInteraction;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Planetary Interaction endpoints. Reach them through
    /// <see cref="EsiClient.PlanetaryInteraction"/>.
    /// </summary>
    public class PlanetaryInteractionLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Planetary Interaction endpoints. <see cref="EsiClient"/> does this; there is
        /// no need to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public PlanetaryInteractionLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Get colonies. Returns a list of all planetary colonies owned by a character.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/planets</c>.</para>
        /// <para>Requires the <c>esi-planets.manage_planets.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Planet"/>.</returns>
        public async Task<EsiResponse<List<Planet>>> Colonies(EsiCallOptions options)
            => await Execute<List<Planet>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/planets/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get colony layout. Returns full details on the layout of a single planetary colony,
        /// including links, pins and routes. Note: Planetary information is only recalculated when
        /// the colony is viewed through the client. Information will not update until this criteria
        /// is met.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/planets/{planet_id}</c>.</para>
        /// <para>Requires the <c>esi-planets.manage_planets.v1</c> scope.</para>
        /// </remarks>
        /// <param name="planetId">Planet id of the target planet.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ColonyLayout"/>.</returns>
        public async Task<EsiResponse<ColonyLayout>> ColonyLayout(long planetId, EsiCallOptions options)
            => await Execute<ColonyLayout>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/planets/{planet_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "planet_id", planetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>List customs offices owned by a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/customs_offices</c>.</para>
        /// <para>Requires the <c>esi-planets.read_customs_offices.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="CustomsOffice"/>.</returns>
        public async Task<EsiResponse<List<CustomsOffice>>> CorporationCustomsOffices(EsiCallOptions options)
            => await Execute<List<CustomsOffice>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/customs_offices/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}