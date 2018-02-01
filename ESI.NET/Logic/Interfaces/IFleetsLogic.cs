using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models.Fleets;

namespace ESI.NET.Logic.Interfaces
{
    public interface IFleetsLogic
    {
        Task<ApiResponse<NewSquad>> CreateSquad(long fleet_id, long wing_id);
        Task<ApiResponse<NewWing>> CreateWing(long fleet_id);
        Task<ApiResponse<string>> DeleteSquad(long fleet_id, long squad_id);
        Task<ApiResponse<string>> DeleteWing(long fleet_id, long wing_id);
        Task<ApiResponse<FleetInfo>> FleetInfo();
        Task<ApiResponse<string>> InviteCharacter(long fleet_id, int character_id, FleetRole role, long wing_id = 0, long squad_id = 0);
        Task<ApiResponse<string>> KickCharacter(long fleet_id, int member_id);
        Task<ApiResponse<List<Member>>> Members(long fleet_id);
        Task<ApiResponse<string>> MoveCharacter(long fleet_id, int member_id, FleetRole role, long wing_id = 0, long squad_id = 0);
        Task<ApiResponse<string>> RenameSquad(long fleet_id, long squad_id, string name);
        Task<ApiResponse<string>> RenameWing(long fleet_id, long wing_id, string name);
        Task<ApiResponse<Settings>> Settings(long fleet_id);
        Task<ApiResponse<string>> UpdateSettings(long fleet_id, string motd = null, bool? is_free_move = null);
        Task<ApiResponse<List<Wing>>> Wings(long fleet_id);
    }
}