using ESI.NET.Enumerations;
using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Fleets;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class FleetsLogic : IFleetsLogic
    {
        private ESIConfig _config;
        private int character_id;

        public FleetsLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /fleets/{fleet_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Settings>> Settings(long fleet_id)
            => await Execute<Settings>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/fleets/{fleet_id}/");

        /// <summary>
        /// /fleets/{fleet_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="motd"></param>
        /// <param name="is_free_move"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> UpdateSettings(long fleet_id, string motd = null, bool? is_free_move = null)
        {
            dynamic body = null;

            if (motd != null)
                body = new { motd = motd };
            if (is_free_move != null)
                body = new { is_free_move = is_free_move };
            if (motd != null && is_free_move != null)
                body = new { motd = motd, is_free_move = is_free_move };

            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/fleets/{fleet_id}/", body: body);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["PUT|/fleets/{fleet_id}/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/fleet/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<FleetInfo>> FleetInfo()
            => await Execute<FleetInfo>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/fleet/");

        /// <summary>
        /// /fleets/{fleet_id}/members/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Member>>> Members(long fleet_id)
            => await Execute<List<Member>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/fleets/{fleet_id}/members/");

        /// <summary>
        /// /fleets/{fleet_id}/members/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="character_id"></param>
        /// <param name="role"></param>
        /// <param name="wing_id"></param>
        /// <param name="squad_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> InviteCharacter(long fleet_id, int character_id, FleetRole role, long wing_id = 0, long squad_id = 0)
        {
            dynamic body = null;
            body = BuildFleetInvite(character_id, role, wing_id, squad_id, body);

            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.POST, $"/fleets/{fleet_id}/members/", body: body);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["POST|/fleets/{fleet_id}/members/"];

            return response;
        }

        /// <summary>
        /// /fleets/{fleet_id}/members/{member_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="member_id"></param>
        /// <param name="role"></param>
        /// <param name="wing_id"></param>
        /// <param name="squad_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> MoveCharacter(long fleet_id, int member_id, FleetRole role, long wing_id = 0, long squad_id = 0)
        {
            dynamic body = null;
            body = BuildFleetInvite(character_id, role, wing_id, squad_id, body);

            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/fleets/{fleet_id}/members/{member_id}/", body: body);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["PUT|/fleets/{fleet_id}/members/{member_id}/"];

            return response;
        }

        /// <summary>
        /// /fleets/{fleet_id}/members/{member_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="member_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> KickCharacter(long fleet_id, int member_id)
        {
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/fleets/{fleet_id}/members/{member_id}/");

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/fleets/{fleet_id}/members/{member_id}/"];

            return response;
        }

        /// <summary>
        /// /fleets/{fleet_id}/wings/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Wing>>> Wings(long fleet_id)
        {
            var response = await Execute<List<Wing>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/fleets/{fleet_id}/wings/");

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/fleets/{fleet_id}/members/{member_id}/"];

            return response;
        }

        /// <summary>
        /// /fleets/{fleet_id}/wings/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<NewWing>> CreateWing(long fleet_id)
            => await Execute<NewWing>(_config, RequestSecurity.Authenticated, RequestMethod.POST, $"/fleets/{fleet_id}/wings/");

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="wing_id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> RenameWing(long fleet_id, long wing_id, string name)
        {
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/fleets/{fleet_id}/wings/{wing_id}/", body: new
            {
                name = name
            });

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["PUT|/fleets/{fleet_id}/wings/{wing_id}/"];

            return response;
        }

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="wing_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> DeleteWing(long fleet_id, long wing_id)
        {
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/fleets/{fleet_id}/wings/{wing_id}/");

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/fleets/{fleet_id}/wings/{wing_id}/"];

            return response;
        }

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/squads/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="wing_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<NewSquad>> CreateSquad(long fleet_id, long wing_id)
            => await Execute<NewSquad>(_config, RequestSecurity.Authenticated, RequestMethod.POST, $"/fleets/{fleet_id}/wings/{wing_id}/squads/");

        /// <summary>
        /// /fleets/{fleet_id}/squads/{squad_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="squad_id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> RenameSquad(long fleet_id, long squad_id, string name)
        {
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/fleets/{fleet_id}/squads/{squad_id}/", body: new
            {
                name = name
            });

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["PUT|/fleets/{fleet_id}/squads/{squad_id}/"];

            return response;
        }

        /// <summary>
        /// /fleets/{fleet_id}/squads/{squad_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="squad_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> DeleteSquad(long fleet_id, long squad_id)
        {
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/fleets/{fleet_id}/squads/{squad_id}/");

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/fleets/{fleet_id}/squads/{squad_id}/"];

            return response;
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
        private static dynamic BuildFleetInvite(int character_id, FleetRole role, long wing_id, long squad_id, dynamic body)
        {
            if (role == FleetRole.fleet_commander)
            {
                body = new
                {
                    character_id = character_id,
                    role = role.ToString()
                };
            }
            else if (role == FleetRole.wing_commander)
            {
                body = new
                {
                    character_id = character_id,
                    role = role.ToString(),
                    wing_id = wing_id
                };
            }
            else if (role == FleetRole.squad_commander || role == FleetRole.squad_member)
            {
                body = new
                {
                    character_id = character_id,
                    role = role.ToString(),
                    wing_id = wing_id,
                    squad_id = squad_id
                };
            }

            return body;
        }
    }
}