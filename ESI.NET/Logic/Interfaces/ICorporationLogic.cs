using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models;
using ESI.NET.Models.Corporation;

namespace ESI.NET.Logic.Interfaces
{
    public interface ICorporationLogic
    {
        Task<ApiResponse<List<AllianceHistory>>> AllianceHistory(int corporation_id);
        Task<ApiResponse<List<Blueprint>>> Blueprints(int page = 1);
        Task<ApiResponse<List<ContainerLog>>> ContainerLogs(int page = 1);
        Task<ApiResponse<Divisions>> Divisions();
        Task<ApiResponse<List<Facility>>> Facilities();
        Task<ApiResponse<Images>> Icons(int corporation_id);
        Task<ApiResponse<Information>> Information(int corporation_id);
        Task<ApiResponse<List<Medal>>> Medals(int page = 1);
        Task<ApiResponse<List<IssuedMedal>>> MedalsIssued(int page = 1);
        Task<ApiResponse<int>> MemberLimit();
        Task<ApiResponse<List<Member>>> Members();
        Task<ApiResponse<List<MemberTitles>>> MemberTitles();
        Task<ApiResponse<List<MemberInfo>>> MemberTracking();
        Task<ApiResponse<List<Corporation>>> Names(List<int> corporation_ids);
        Task<ApiResponse<List<int>>> NpcCorps();
        Task<ApiResponse<Outpost>> Outpost(int outpost_id);
        Task<ApiResponse<int[]>> Outposts(int page = 1);
        Task<ApiResponse<List<CharacterRoles>>> Roles();
        Task<ApiResponse<List<CharacterRolesHistory>>> RolesHistory();
        Task<ApiResponse<List<Shareholder>>> Shareholders(int page = 1);
        Task<ApiResponse<Standing>> Standings(int page = 1);
        Task<ApiResponse<StarbaseInfo>> Starbase(int starbase_id, int system_id);
        Task<ApiResponse<List<Starbase>>> Starbases(int page = 1);
        Task<ApiResponse<List<Structure>>> Structures(int page = 1);
        Task<ApiResponse<List<Title>>> Titles();
        Task<ApiResponse<string>> UpdateStructureVulnerability(long structure_id, object new_schedule);
    }
}