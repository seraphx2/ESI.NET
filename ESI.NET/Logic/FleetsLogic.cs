using ESI.NET.Enumerations;
using ESI.NET.Models.Fleets;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Fleets endpoints. Reach them through <see cref="EsiClient.Fleets"/>.
    /// </summary>
    public class FleetsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Fleets endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public FleetsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>Get fleet information. Return details about a fleet.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /fleets/{fleet_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.read_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Settings"/>.</returns>
        public async Task<EsiResponse<Settings>> Settings(long fleetId, EsiCallOptions options)
            => await Execute<Settings>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/fleets/{fleet_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Update settings about a fleet.</summary>
        /// <remarks>
        /// <para>Calls <c>PUT /fleets/{fleet_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="motd">New fleet MOTD in CCP flavoured HTML.</param>
        /// <param name="isFreeMove">Should free-move be enabled in the fleet.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> UpdateSettings(long fleetId, string motd = null, bool? isFreeMove = null, EsiCallOptions options = null)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/fleets/{fleet_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildUpdateSettingsObject(motd, isFreeMove),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character fleet info. Return the fleet ID the character is in, if any.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/fleet</c>.</para>
        /// <para>Requires the <c>esi-fleets.read_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="FleetInfo"/>.</returns>
        public async Task<EsiResponse<FleetInfo>> FleetInfo(EsiCallOptions options)
            => await Execute<FleetInfo>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fleet/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return information about fleet members.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /fleets/{fleet_id}/members</c>.</para>
        /// <para>Requires the <c>esi-fleets.read_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Member"/>.</returns>
        public async Task<EsiResponse<List<Member>>> Members(long fleetId, EsiCallOptions options)
            => await Execute<List<Member>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/fleets/{fleet_id}/members/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Create fleet invitation. Invite a character into the fleet. If a character has a CSPA
        /// charge set it is not possible to invite them to the fleet using ESI.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /fleets/{fleet_id}/members</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="characterId">The character you want to invite.</param>
        /// <param name="role">If a character is invited with the <c>fleet_commander</c> role,
        /// neither <c>wing_id</c> or <c>squad_id</c> should be specified. If a character is invited
        /// with the <c>wing_commander</c> role, only <c>wing_id</c> should be specified. If a
        /// character is invited with the <c>squad_commander</c> role, both <c>wing_id</c> and
        /// <c>squad_id</c> should be specified. If a character is invited with the
        /// <c>squad_member</c> role, <c>wing_id</c> and <c>squad_id</c> should either both be
        /// specified or not specified at all. If they aren’t specified, the invited character will
        /// join any squad with available positions.</param>
        /// <param name="wingId">ID of the wing to put the character in, or 0 for none. Which roles
        /// need it is set out under <c>role</c>.</param>
        /// <param name="squadId">ID of the squad to put the character in, or 0 for none. Which
        /// roles need it is set out under <c>role</c>.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> InviteCharacter(long fleetId, long characterId, FleetRole role, long wingId = 0, long squadId = 0, EsiCallOptions options = null)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/fleets/{fleet_id}/members/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildFleetInviteObject(characterId, role, wingId, squadId),
                options: options).ConfigureAwait(false);

        /// <summary>Move a fleet member around.</summary>
        /// <remarks>
        /// <para>Calls <c>PUT /fleets/{fleet_id}/members/{member_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="memberId">The character ID of a member in this fleet.</param>
        /// <param name="role">If a character is moved to the <c>fleet_commander</c> role, neither
        /// <c>wing_id</c> or <c>squad_id</c> should be specified. If a character is moved to the
        /// <c>wing_commander</c> role, only <c>wing_id</c> should be specified. If a character is
        /// moved to the <c>squad_commander</c> role, both <c>wing_id</c> and <c>squad_id</c> should
        /// be specified. If a character is moved to the <c>squad_member</c> role, both
        /// <c>wing_id</c> and <c>squad_id</c> should be specified.</param>
        /// <param name="wingId">ID of the wing to put the character in, or 0 for none. Which roles
        /// need it is set out under <c>role</c>.</param>
        /// <param name="squadId">ID of the squad to put the character in, or 0 for none. Which
        /// roles need it is set out under <c>role</c>.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> MoveCharacter(long fleetId, long memberId, FleetRole role, long wingId = 0, long squadId = 0, EsiCallOptions options = null)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/fleets/{fleet_id}/members/{member_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "member_id", memberId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildFleetInviteObject(options.Character.CharacterID, role, wingId, squadId),
                options: options).ConfigureAwait(false);

        /// <summary>Kick a fleet member.</summary>
        /// <remarks>
        /// <para>Calls <c>DELETE /fleets/{fleet_id}/members/{member_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="memberId">The character ID of a member in this fleet.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> KickCharacter(long fleetId, long memberId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/fleets/{fleet_id}/members/{member_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "member_id", memberId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get fleet wings. Return information about wings in a fleet.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /fleets/{fleet_id}/wings</c>.</para>
        /// <para>Requires the <c>esi-fleets.read_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Wing"/>.</returns>
        public async Task<EsiResponse<List<Wing>>> Wings(long fleetId, EsiCallOptions options)
            => await Execute<List<Wing>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/fleets/{fleet_id}/wings/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Create a new wing in a fleet.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /fleets/{fleet_id}/wings</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="NewWing"/>.</returns>
        public async Task<EsiResponse<NewWing>> CreateWing(long fleetId, EsiCallOptions options)
            => await Execute<NewWing>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/fleets/{fleet_id}/wings/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Rename a fleet wing.</summary>
        /// <remarks>
        /// <para>Calls <c>PUT /fleets/{fleet_id}/wings/{wing_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="wingId">ID of the wing to put the character in, or 0 for none. Which roles
        /// need it is set out under <c>role</c>.</param>
        /// <param name="name">The wing's new name (at most 10 characters).</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> RenameWing(long fleetId, long wingId, string name, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/fleets/{fleet_id}/wings/{wing_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "wing_id", wingId.ToString(CultureInfo.InvariantCulture) }
                },
                body: new
                {
                    name
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Delete fleet wing. Delete a fleet wing, only empty wings can be deleted. The wing may
        /// contain squads, but the squads must be empty.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>DELETE /fleets/{fleet_id}/wings/{wing_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="wingId">ID of the wing to put the character in, or 0 for none. Which roles
        /// need it is set out under <c>role</c>.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> DeleteWing(long fleetId, long wingId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/fleets/{fleet_id}/wings/{wing_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "wing_id", wingId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Create a new squad in a fleet.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /fleets/{fleet_id}/wings/{wing_id}/squads</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="wingId">The wing_id to create squad in.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="NewSquad"/>.</returns>
        public async Task<EsiResponse<NewSquad>> CreateSquad(long fleetId, long wingId, EsiCallOptions options)
            => await Execute<NewSquad>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/fleets/{fleet_id}/wings/{wing_id}/squads/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "wing_id", wingId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Rename a fleet squad.</summary>
        /// <remarks>
        /// <para>Calls <c>PUT /fleets/{fleet_id}/squads/{squad_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="squadId">ID of the squad to put the character in, or 0 for none. Which
        /// roles need it is set out under <c>role</c>.</param>
        /// <param name="name">The squad's new name (at most 10 characters).</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> RenameSquad(long fleetId, long squadId, string name, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/fleets/{fleet_id}/squads/{squad_id}/", replacements: new Dictionary<string, string>()
            {
                { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                { "squad_id", squadId.ToString(CultureInfo.InvariantCulture) }
            }, body: new
            {
                name
            }, options: options).ConfigureAwait(false);

        /// <summary>
        /// Delete fleet squad. Delete a fleet squad, only empty squads can be deleted.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>DELETE /fleets/{fleet_id}/squads/{squad_id}</c>.</para>
        /// <para>Requires the <c>esi-fleets.write_fleet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fleetId">ID for a fleet.</param>
        /// <param name="squadId">ID of the squad to put the character in, or 0 for none. Which
        /// roles need it is set out under <c>role</c>.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> DeleteSquad(long fleetId, long squadId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/fleets/{fleet_id}/squads/{squad_id}/", replacements: new Dictionary<string, string>()
            {
                { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                { "squad_id", squadId.ToString(CultureInfo.InvariantCulture) }
            }, options: options).ConfigureAwait(false);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="motd"></param>
        /// <param name="is_free_move"></param>
        /// <returns></returns>
        private static dynamic BuildUpdateSettingsObject(string motd, bool? is_free_move)
        {
            dynamic body = null;

            if (motd != null)
                body = new { motd };
            if (is_free_move != null)
                body = new { is_free_move };
            if (motd != null && is_free_move != null)
                body = new { motd, is_free_move };

            return body;
        }

        /// <summary>
        /// Builds the request body for a fleet invite or move: only the IDs the role needs are sent.
        /// </summary>
        private static dynamic BuildFleetInviteObject(long character_id, FleetRole role, long wing_id, long squad_id)
        {
            dynamic body = null;

            if (role == FleetRole.FleetCommander)
                body = new { character_id, role = role.ToEsiValue() };

            else if (role == FleetRole.WingCommander)
                body = new { character_id, role = role.ToEsiValue(), wing_id };

            else if (role == FleetRole.SquadCommander || role == FleetRole.SquadMember)
                body = new { character_id, role = role.ToEsiValue(), wing_id, squad_id };

            return body;
        }
    }
}