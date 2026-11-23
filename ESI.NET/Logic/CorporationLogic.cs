using ESI.NET.Models;
using ESI.NET.Models.Corporation;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Corporation endpoints. Reach them through <see cref="EsiClient.Corporation"/>.
    /// </summary>
    public class CorporationLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Corporation endpoints. <see cref="EsiClient"/> does this; there is no need
        /// to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public CorporationLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Get npc corporations. Get a list of npc corporations This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/npccorps</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> NpcCorps(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/npccorps/",
                options: options).ConfigureAwait(false);


        /// <summary>Public information about a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}</c>.</para>
        /// </remarks>
        /// <param name="corporationId">The ID of the corporation.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Corporation"/>.</returns>
        public async Task<EsiResponse<Corporation>> Information(long corporationId, EsiCallOptions options = null)
            => await Execute<Corporation>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/{corporation_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporationId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get alliance history. Get a list of all the alliances a corporation has been a member
        /// of.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/alliancehistory</c>.</para>
        /// </remarks>
        /// <param name="corporationId">The ID of the corporation.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="AllianceHistory"/>.</returns>
        public async Task<EsiResponse<List<AllianceHistory>>> AllianceHistory(long corporationId, EsiCallOptions options = null)
            => await Execute<List<AllianceHistory>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/{corporation_id}/alliancehistory/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporationId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>Returns a list of blueprints the corporation owns.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/blueprints</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_blueprints.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Blueprint"/>.</returns>
        public async Task<EsiResponse<List<Blueprint>>> Blueprints(EsiCallOptions options)
            => await Execute<List<Blueprint>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/blueprints/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get all corporation ALSC logs. Returns logs recorded in the past seven days from all
        /// audit log secure containers (ALSC) owned by a given corporation.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/containers/logs</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_container_logs.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ContainerLog"/>.</returns>
        public async Task<EsiResponse<List<ContainerLog>>> ContainerLogs(EsiCallOptions options)
            => await Execute<List<ContainerLog>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/containers/logs/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation divisions. Return corporation hangar and wallet division names, only
        /// show if a division is not using the default name.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/divisions</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_divisions.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Divisions"/>.</returns>
        public async Task<EsiResponse<Divisions>> Divisions(EsiCallOptions options)
            => await Execute<Divisions>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/divisions/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return a corporation's facilities.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/facilities</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_facilities.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Factory_Manager</c> corporation role.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Facility"/>.</returns>
        public async Task<EsiResponse<List<Facility>>> Facilities(EsiCallOptions options)
            => await Execute<List<Facility>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/facilities/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get the icon urls for a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/icons</c>.</para>
        /// </remarks>
        /// <param name="corporationId">The ID of the corporation.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Images"/>.</returns>
        public async Task<EsiResponse<Images>> Icons(long corporationId, EsiCallOptions options = null)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/{corporation_id}/icons/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporationId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>Returns a corporation's medals.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/medals</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_medals.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Medal"/>.</returns>
        public async Task<EsiResponse<List<Medal>>> Medals(EsiCallOptions options)
            => await Execute<List<Medal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/medals/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Returns medals issued by a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/medals/issued</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_medals.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="IssuedMedal"/>.</returns>
        public async Task<EsiResponse<List<IssuedMedal>>> MedalsIssued(EsiCallOptions options)
            => await Execute<List<IssuedMedal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/medals/issued/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation members. Return the current member list of a corporation, the token's
        /// character need to be a member of the corporation.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/members</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_corporation_membership.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Members(EsiCallOptions options)
            => await Execute<long[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/members/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation member limit. Return a corporation's member limit, not including CEO
        /// himself.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/members/limit</c>.</para>
        /// <para>Requires the <c>esi-corporations.track_members.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <c>long</c> value.</returns>
        public async Task<EsiResponse<long>> MemberLimit(EsiCallOptions options)
            => await Execute<long>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/members/limit/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Returns a corporation's members' titles.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/members/titles</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_titles.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="MemberTitles"/>.</returns>
        public async Task<EsiResponse<List<MemberTitles>>> MemberTitles(EsiCallOptions options)
            => await Execute<List<MemberTitles>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/members/titles/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Track corporation members. Returns additional information about a corporation's members
        /// which helps tracking their activities.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/membertracking</c>.</para>
        /// <para>Requires the <c>esi-corporations.track_members.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="MemberInfo"/>.</returns>
        public async Task<EsiResponse<List<MemberInfo>>> MemberTracking(EsiCallOptions options)
            => await Execute<List<MemberInfo>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/membertracking/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation member roles. Return the roles of all members if the character has the
        /// personnel manager role or any grantable role.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/roles</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_corporation_membership.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="CharacterRoles"/>.</returns>
        public async Task<EsiResponse<List<CharacterRoles>>> Roles(EsiCallOptions options)
            => await Execute<List<CharacterRoles>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/roles/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation member roles history. Return how roles have changed for a coporation's
        /// members, up to a month.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/roles/history</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_corporation_membership.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="CharacterRolesHistory"/>.</returns>
        public async Task<EsiResponse<List<CharacterRolesHistory>>> RolesHistory(EsiCallOptions options)
            => await Execute<List<CharacterRolesHistory>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/roles/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return the current shareholders of a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/shareholders</c>.</para>
        /// <para>Requires the <c>esi-wallet.read_corporation_wallets.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Shareholder"/>.</returns>
        public async Task<EsiResponse<List<Shareholder>>> Shareholders(EsiCallOptions options)
            => await Execute<List<Shareholder>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/shareholders/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation standings. Return corporation standings from agents, NPC corporations,
        /// and factions.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/standings</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_standings.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Standing"/>.</returns>
        public async Task<EsiResponse<List<Standing>>> Standings(EsiCallOptions options)
            => await Execute<List<Standing>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/standings/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Returns list of corporation starbases (POSes).</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/starbases</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_starbases.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Starbase"/>.</returns>
        public async Task<EsiResponse<List<Starbase>>> Starbases(EsiCallOptions options)
            => await Execute<List<Starbase>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/starbases/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get starbase (POS) detail. Returns various settings and fuels of a starbase (POS).
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/starbases/{starbase_id}</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_starbases.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="starbaseId">An EVE starbase (POS) ID.</param>
        /// <param name="systemId">The solar system this starbase (POS) is located in,.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="StarbaseInfo"/>.</returns>
        public async Task<EsiResponse<StarbaseInfo>> Starbase(long starbaseId, long systemId, EsiCallOptions options)
            => await Execute<StarbaseInfo>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/starbases/{starbase_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "starbase_id", starbaseId.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[]
                {
                    $"system_id={systemId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation structures. Get a list of corporation structures. This route's version
        /// includes the changes to structures detailed in this blog:
        /// https://www.eveonline.com/article/upwell-2.0-structures-changes-coming-on-february-13th.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/structures</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_structures.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Station_Manager</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Structure"/>.</returns>
        public async Task<EsiResponse<List<Structure>>> Structures(EsiCallOptions options)
            => await Execute<List<Structure>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Returns a corporation's titles.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/titles</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_titles.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Title"/>.</returns>
        public async Task<EsiResponse<List<Title>>> Titles(EsiCallOptions options)
            => await Execute<List<Title>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/titles/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        // ---- Corporation Projects (scope: esi-corporations.read_projects.v1) ----

        /// <summary>
        /// List corporation projects. Listing of all (active) corporation projects.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/projects</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_projects.v1</c> scope.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="state">Filter by state.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ProjectList"/>.</returns>
        public async Task<EsiResponse<ProjectList>> Projects(string state = null, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ProjectList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/projects/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: BuildCursorParams(("state", state), ("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of a corporation project.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/projects/{project_id}</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_projects.v1</c> scope.</para>
        /// </remarks>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Project"/>.</returns>
        public async Task<EsiResponse<Project>> Project(string projectId, EsiCallOptions options)
            => await Execute<Project>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/projects/{project_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "project_id", projectId }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List project contributors. Listing of all contributors to a corporation project.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/projects/{project_id}/contributors</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_projects.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Project_Manager</c> corporation role.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ProjectContributors"/>.</returns>
        public async Task<EsiResponse<ProjectContributors>> ProjectContributors(string projectId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ProjectContributors>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/projects/{project_id}/contributors/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "project_id", projectId }
                },
                parameters: BuildCursorParams(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>Show your contribution to a corporation project.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/projects/{project_id}/contribution/{character_id}</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_projects.v1</c> scope.</para>
        /// </remarks>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="characterId">The ID of the character.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ProjectContribution"/>.</returns>
        public async Task<EsiResponse<ProjectContribution>> ProjectContribution(string projectId, long characterId, EsiCallOptions options)
            => await Execute<ProjectContribution>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/projects/{project_id}/contribution/{character_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "project_id", projectId },
                    { "character_id", characterId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        private static string[] BuildCursorParams(params (string Key, string Value)[] pairs)
        {
            var list = new List<string>();
            foreach (var (key, value) in pairs)
                if (!string.IsNullOrEmpty(value))
                    list.Add($"{key}={value}");
            return list.Count > 0 ? list.ToArray() : null;
        }
    }
}