using ESI.NET.Models.Industry;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Industry endpoints. Reach them through <see cref="EsiClient.Industry"/>.
    /// </summary>
    public class IndustryLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Industry endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public IndustryLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>Return a list of industry facilities.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /industry/facilities</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Facility"/>.</returns>
        public async Task<EsiResponse<List<Facility>>> Facilities(EsiCallOptions options = null)
            => await Execute<List<Facility>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/industry/facilities/",
                options: options).ConfigureAwait(false);


        /// <summary>Return cost indices for solar systems.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /industry/systems</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="SolarSystem"/>.</returns>
        public async Task<EsiResponse<List<SolarSystem>>> SolarSystemCostIndices(EsiCallOptions options = null)
            => await Execute<List<SolarSystem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/industry/systems/",
                options: options).ConfigureAwait(false);


        /// <summary>List industry jobs placed by a character.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/industry/jobs</c>.</para>
        /// <para>Requires the <c>esi-industry.read_character_jobs.v1</c> scope.</para>
        /// </remarks>
        /// <param name="includeCompleted">Whether to retrieve completed character industry jobs.
        /// Only includes jobs from the past 90 days.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Job"/>.</returns>
        public async Task<EsiResponse<List<Job>>> JobsForCharacter(bool includeCompleted = false, EsiCallOptions options = null)
            => await Execute<List<Job>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/industry/jobs/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[]
                {
                    $"include_completed={includeCompleted}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Character mining ledger. Paginated record of all mining done by a character for the past
        /// 30 days.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/mining</c>.</para>
        /// <para>Requires the <c>esi-industry.read_character_mining.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Entry"/>.</returns>
        public async Task<EsiResponse<List<Entry>>> MiningLedger(EsiCallOptions options)
            => await Execute<List<Entry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mining/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Corporation mining observers. Paginated list of all entities capable of observing and
        /// recording mining for a corporation.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporation/{corporation_id}/mining/observers</c>.</para>
        /// <para>Requires the <c>esi-industry.read_corporation_mining.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Accountant</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Observer"/>.</returns>
        public async Task<EsiResponse<List<Observer>>> Observers(EsiCallOptions options)
            => await Execute<List<Observer>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporation/{corporation_id}/mining/observers/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Observed corporation mining. Paginated record of all mining seen by an observer.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporation/{corporation_id}/mining/observers/{observer_id}</c>.</para>
        /// <para>Requires the <c>esi-industry.read_corporation_mining.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Accountant</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="observerId">A mining observer id.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ObserverInfo"/>.</returns>
        public async Task<EsiResponse<List<ObserverInfo>>> ObservedMining(long observerId, EsiCallOptions options)
            => await Execute<List<ObserverInfo>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporation/{corporation_id}/mining/observers/{observer_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "observer_id", observerId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>List industry jobs run by a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/industry/jobs</c>.</para>
        /// <para>Requires the <c>esi-industry.read_corporation_jobs.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Factory_Manager</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="includeCompleted">Whether to retrieve completed corporation industry jobs.
        /// Only includes jobs from the past 90 days.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Job"/>.</returns>
        public async Task<EsiResponse<List<Job>>> JobsForCorporation(bool includeCompleted = false, EsiCallOptions options = null)
            => await Execute<List<Job>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/industry/jobs/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[]
                {
                    $"include_completed={includeCompleted}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Moon extraction timers. Extraction timers for all moon chunks being extracted by
        /// refineries belonging to a corporation.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporation/{corporation_id}/mining/extractions</c>.</para>
        /// <para>Requires the <c>esi-industry.read_corporation_mining.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Station_Manager</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Extraction"/>.</returns>
        public async Task<EsiResponse<List<Extraction>>> Extractions(EsiCallOptions options)
            => await Execute<List<Extraction>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporation/{corporation_id}/mining/extractions/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}