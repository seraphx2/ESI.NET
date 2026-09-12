using ESI.NET.Enumerations;
using ESI.NET.Models.Fleets;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class FleetsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public FleetsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /fleets/{fleet_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Settings>> Settings(long fleetId, EsiCallOptions options)
            => await Execute<Settings>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/fleets/{fleet_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="motd"></param>
        /// <param name="isFreeMove"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> UpdateSettings(long fleetId, string motd = null, bool? isFreeMove = null, EsiCallOptions options = null)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/fleets/{fleet_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildUpdateSettingsObject(motd, isFreeMove),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/fleet/
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<FleetInfo>> FleetInfo(EsiCallOptions options)
            => await Execute<FleetInfo>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fleet/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/members/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Member>>> Members(long fleetId, EsiCallOptions options)
            => await Execute<List<Member>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/fleets/{fleet_id}/members/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/members/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="characterId"></param>
        /// <param name="role"></param>
        /// <param name="wingId"></param>
        /// <param name="squadId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> InviteCharacter(long fleetId, long characterId, FleetRole role, long wingId = 0, long squadId = 0, EsiCallOptions options = null)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/fleets/{fleet_id}/members/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildFleetInviteObject(characterId, role, wingId, squadId),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/members/{member_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="memberId"></param>
        /// <param name="role"></param>
        /// <param name="wingId"></param>
        /// <param name="squadId"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<string>> MoveCharacter(long fleetId, long memberId, FleetRole role, long wingId = 0, long squadId = 0, EsiCallOptions options = null)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/fleets/{fleet_id}/members/{member_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "member_id", memberId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildFleetInviteObject(options.Character.CharacterID, role, wingId, squadId),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/members/{member_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> KickCharacter(long fleetId, long memberId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/fleets/{fleet_id}/members/{member_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "member_id", memberId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/wings/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Wing>>> Wings(long fleetId, EsiCallOptions options)
            => await Execute<List<Wing>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/fleets/{fleet_id}/wings/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/wings/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<NewWing>> CreateWing(long fleetId, EsiCallOptions options)
            => await Execute<NewWing>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/fleets/{fleet_id}/wings/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="wingId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// /fleets/{fleet_id}/wings/{wing_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="wingId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> DeleteWing(long fleetId, long wingId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/fleets/{fleet_id}/wings/{wing_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "wing_id", wingId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/squads/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="wingId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<NewSquad>> CreateSquad(long fleetId, long wingId, EsiCallOptions options)
            => await Execute<NewSquad>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/fleets/{fleet_id}/wings/{wing_id}/squads/",
                replacements: new Dictionary<string, string>()
                {
                    { "fleet_id", fleetId.ToString(CultureInfo.InvariantCulture) },
                    { "wing_id", wingId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /fleets/{fleet_id}/squads/{squad_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="squadId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// /fleets/{fleet_id}/squads/{squad_id}/
        /// </summary>
        /// <param name="fleetId"></param>
        /// <param name="squadId"></param>
        /// <returns></returns>
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
        /// Dynamically builds the required structure for a fleet invite or move
        /// </summary>
        /// <param name="character_id"></param>
        /// <param name="role"></param>
        /// <param name="wing_id"></param>
        /// <param name="squad_id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
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