using ESI.NET.Logic.Interfaces;
using ESI.NET.Models;
using ESI.NET.Models.Corporation;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class CorporationLogic : ICorporationLogic
    {
        private ESIConfig _config;
        private int corporation_id;

        public CorporationLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                corporation_id = _config.AuthorizedCharacter.CorporationID;
            }
        }

        /// <summary>
        /// /corporations/{corporation_id}/
        /// </summary>
        /// <param name="corporation_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Information>> Information(int corporation_id)
            => await Execute<Information>(_config, RequestSecurity.Public, RequestMethod.GET, $"/corporations/{corporation_id}/");

        /// <summary>
        /// /corporations/{corporation_id}/alliancehistory/
        /// </summary>
        /// <param name="corporation_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<AllianceHistory>>> AllianceHistory(int corporation_id)
            => await Execute<List<AllianceHistory>>(_config, RequestSecurity.Public, RequestMethod.GET, $"/corporations/{corporation_id}/alliancehistory/");

        /// <summary>
        /// /corporations/names/
        /// </summary>
        /// <param name="corporation_ids"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Corporation>>> Names(List<int> corporation_ids)
            => await Execute<List<Corporation>>(_config, RequestSecurity.Public, RequestMethod.GET, "/corporations/names/", new string[]
            {
                $"corporation_ids={string.Join(",", corporation_ids)}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/members/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Member>>> Members()
            => await Execute<List<Member>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/members/");

        /// <summary>
        /// /corporations/{corporation_id}/roles/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<CharacterRoles>>> Roles()
            => await Execute<List<CharacterRoles>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/roles/");

        /// <summary>
        /// /corporations/{corporation_id}/roles/history/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<CharacterRolesHistory>>> RolesHistory()
            => await Execute<List<CharacterRolesHistory>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/roles/history/");

        /// <summary>
        /// /corporations/{corporation_id}/icons/
        /// </summary>
        /// <param name="corporationId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Images>> Icons(int corporation_id)
            => await Execute<Images>(_config, RequestSecurity.Public, RequestMethod.GET, $"/corporations/{corporation_id}/icons/");

        /// <summary>
        /// /corporations/npccorps/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> NpcCorps()
            => await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, "/corporations/npccorps/");

        /// <summary>
        /// /corporations/{corporation_id}/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Structure>>> Structures(int page = 1)
            => await Execute<List<Structure>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/structures/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> UpdateStructureVulnerability(long structure_id, object new_schedule)
            => await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/corporations/{corporation_id}/structures/{structure_id}/", body: new_schedule);

        /// <summary>
        /// /corporations/{corporation_id}/membertracking/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<MemberInfo>>> MemberTracking()
            => await Execute<List<MemberInfo>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/membertracking/");

        /// <summary>
        /// /corporations/{corporation_id}/divisions/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Divisions>> Divisions()
            => await Execute<Divisions>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/divisions/");

        /// <summary>
        /// /corporations/{corporation_id}/members/limit/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<int>> MemberLimit()
            => await Execute<int>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/members/limit/");

        /// <summary>
        /// /corporations/{corporation_id}/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Title>>> Titles()
            => await Execute<List<Title>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/titles/");

        /// <summary>
        /// /corporations/{corporation_id}/members/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<MemberTitles>>> MemberTitles()
            => await Execute<List<MemberTitles>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/members/titles/");

        /// <summary>
        /// /corporations/{corporation_id}/blueprints/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Blueprint>>> Blueprints(int page = 1)
            => await Execute<List<Blueprint>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/blueprints/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/standings/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Standing>> Standings(int page = 1)
            => await Execute<Standing>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/standings/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/starbases/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Starbase>>> Starbases(int page = 1)
            => await Execute<List<Starbase>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/starbases/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/starbases/{starbase_id}/
        /// </summary>
        /// <param name="starbase_id"></param>
        /// <param name="system_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<StarbaseInfo>> Starbase(int starbase_id, int system_id)
            => await Execute<StarbaseInfo>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/starbases/{starbase_id}/", new string[]
            {
                $"system_id={system_id}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/containers/logs/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ContainerLog>>> ContainerLogs(int page = 1)
            => await Execute<List<ContainerLog>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/containers/logs/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/facilities/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Facility>>> Facilities()
            => await Execute<List<Facility>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/facilities/");

        /// <summary>
        /// /corporations/{corporation_id}/medals/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Medal>>> Medals(int page = 1)
            => await Execute<List<Medal>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/medals/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/medals/issued/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<IssuedMedal>>> MedalsIssued(int page = 1)
            => await Execute<List<IssuedMedal>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/medals/issued/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/outposts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<int[]>> Outposts(int page = 1)
            => await Execute<int[]>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/outposts/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/outposts/{outpost_id}/
        /// </summary>
        /// <param name="outpost_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Outpost>> Outpost(int outpost_id)
            => await Execute<Outpost>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/outposts/{outpost_id}/");

        /// <summary>
        /// /corporations/{corporation_id}/shareholders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Shareholder>>> Shareholders(int page = 1)
            => await Execute<List<Shareholder>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/shareholders/", new string[]
            {
                $"page={page}"
            });
    }
}