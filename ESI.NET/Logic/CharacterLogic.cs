using ESI.NET.Models;
using ESI.NET.Models.Character;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Character endpoints. Reach them through <see cref="EsiClient.Character"/>.
    /// </summary>
    public class CharacterLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Character endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public CharacterLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Character affiliation. Bulk lookup of character IDs to corporation, alliance and
        /// faction.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/affiliation</c>.</para>
        /// </remarks>
        /// <param name="characterIds">IDs of the characters to look up (at most 1000).</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Affiliation"/>.</returns>
        public async Task<EsiResponse<List<Affiliation>>> Affiliation(long[] characterIds, EsiCallOptions options = null)
            => await Execute<List<Affiliation>>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/characters/affiliation/",
                body: characterIds,
                options: options).ConfigureAwait(false);


        /// <summary>Public information about a character.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}</c>.</para>
        /// </remarks>
        /// <param name="characterId">The ID of the character.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Information"/>.</returns>
        public async Task<EsiResponse<Information>> Information(long characterId, EsiCallOptions options = null)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/characters/{character_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", characterId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get agents research. Return a list of agents research information for a character. The
        /// formula for finding the current research points with an agent is: currentPoints =
        /// remainderPoints + pointsPerDay * days(currentTime - researchStartDate).
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/agents_research</c>.</para>
        /// <para>Requires the <c>esi-characters.read_agents_research.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Agent"/>.</returns>
        public async Task<EsiResponse<List<Agent>>> AgentsResearch(EsiCallOptions options)
            => await Execute<List<Agent>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/agents_research/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return a list of blueprints the character owns.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/blueprints</c>.</para>
        /// <para>Requires the <c>esi-characters.read_blueprints.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Blueprint"/>.</returns>
        public async Task<EsiResponse<List<Blueprint>>> Blueprints(EsiCallOptions options)
            => await Execute<List<Blueprint>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/blueprints/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation history. Get a list of all the corporations a character has been a
        /// member of.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/corporationhistory</c>.</para>
        /// </remarks>
        /// <param name="characterId">The ID of the character.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="CorporationHistory"/>.</returns>
        public async Task<EsiResponse<List<CorporationHistory>>> CorporationHistory(long characterId, EsiCallOptions options = null)
            => await Execute<List<CorporationHistory>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/characters/{character_id}/corporationhistory/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", characterId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Calculate a CSPA charge cost. Takes a source character ID in the url and a set of target
        /// character ID's in the body, returns a CSPA charge cost.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/{character_id}/cspa</c>.</para>
        /// <para>Requires the <c>esi-characters.read_contacts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="characterIds">The ID of the character.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <c>decimal</c> value.</returns>
        public async Task<EsiResponse<decimal>> CSPA(object characterIds, EsiCallOptions options)
            => await Execute<decimal>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/cspa/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: characterIds,
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get jump fatigue. Return a character's jump activation and fatigue information.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/fatigue</c>.</para>
        /// <para>Requires the <c>esi-characters.read_fatigue.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Fatigue"/>.</returns>
        public async Task<EsiResponse<Fatigue>> Fatigue(EsiCallOptions options)
            => await Execute<Fatigue>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fatigue/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return a list of medals the character has.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/medals</c>.</para>
        /// <para>Requires the <c>esi-characters.read_medals.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Medal"/>.</returns>
        public async Task<EsiResponse<List<Medal>>> Medals(EsiCallOptions options)
            => await Execute<List<Medal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/medals/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return character notifications.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/notifications</c>.</para>
        /// <para>Requires the <c>esi-characters.read_notifications.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Notification"/>.</returns>
        public async Task<EsiResponse<List<Notification>>> Notifications(EsiCallOptions options)
            => await Execute<List<Notification>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/notifications/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get new contact notifications. Return notifications about having been added to someone's
        /// contact list.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/notifications/contacts</c>.</para>
        /// <para>Requires the <c>esi-characters.read_notifications.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ContactNotification"/>.</returns>
        public async Task<EsiResponse<List<ContactNotification>>> ContactNotifications(EsiCallOptions options)
            => await Execute<List<ContactNotification>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/notifications/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character portraits. Get portrait urls for a character This route expires daily at
        /// 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/portrait</c>.</para>
        /// </remarks>
        /// <param name="characterId">The ID of the character.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Images"/>.</returns>
        public async Task<EsiResponse<Images>> Portrait(long characterId, EsiCallOptions options = null)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/characters/{character_id}/portrait/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", characterId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>Returns a character's corporation roles.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/roles</c>.</para>
        /// <para>Requires the <c>esi-characters.read_corporation_roles.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Roles"/>.</returns>
        public async Task<EsiResponse<Roles>> Roles(EsiCallOptions options)
            => await Execute<Roles>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/roles/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get standings. Return character standings from agents, NPC corporations, and factions.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/standings</c>.</para>
        /// <para>Requires the <c>esi-characters.read_standings.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Standing"/>.</returns>
        public async Task<EsiResponse<List<Standing>>> Standings(EsiCallOptions options)
            => await Execute<List<Standing>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/standings/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get character corporation titles. Returns a character's titles.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/titles</c>.</para>
        /// <para>Requires the <c>esi-characters.read_titles.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Title"/>.</returns>
        public async Task<EsiResponse<List<Title>>> Titles(EsiCallOptions options)
            => await Execute<List<Title>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/titles/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// List Access Lists. Lists all Access Lists the character is Manager or Admin of.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/access-lists</c>.</para>
        /// <para>Requires the <c>esi-access.read_lists.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="AccessListRefList"/>.</returns>
        public async Task<EsiResponse<AccessListRefList>> AccessLists(EsiCallOptions options)
            => await Execute<AccessListRefList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/access-lists/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of an Access List.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/access-lists/{access_list_id}</c>.</para>
        /// <para>Requires the <c>esi-access.read_lists.v1</c> scope.</para>
        /// </remarks>
        /// <param name="accessListId">The ID of the Access List.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="AccessList"/>.</returns>
        public async Task<EsiResponse<AccessList>> AccessList(long accessListId, EsiCallOptions options)
            => await Execute<AccessList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/access-lists/{access_list_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "access_list_id", accessListId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List Mercenary Tactical Operations. Listing of all Mercenary Tactical Operations for the
        /// character.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/mercenary-tactical-operations</c>.</para>
        /// <para>Requires the <c>esi-activities.read_character.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MercenaryTacticalOperationList"/>.</returns>
        public async Task<EsiResponse<MercenaryTacticalOperationList>> MercenaryTacticalOperations(EsiCallOptions options)
            => await Execute<MercenaryTacticalOperationList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mercenary-tactical-operations/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of a Mercenary Tactical Operation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /characters/{character_id}/mercenary-tactical-operations/{operation_id}</c>.</para>
        /// <para>Requires the <c>esi-activities.read_character.v1</c> scope.</para>
        /// </remarks>
        /// <param name="operationId">The ID of the operation.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MercenaryTacticalOperation"/>.</returns>
        public async Task<EsiResponse<MercenaryTacticalOperation>> MercenaryTacticalOperation(string operationId, EsiCallOptions options)
            => await Execute<MercenaryTacticalOperation>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mercenary-tactical-operations/{operation_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "operation_id", operationId }
                },
                options: options).ConfigureAwait(false);
    }
}