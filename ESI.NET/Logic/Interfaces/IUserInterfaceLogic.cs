using System.Threading.Tasks;

namespace ESI.NET.Logic.Interfaces
{
    public interface IUserInterfaceLogic
    {
        Task<ApiResponse<string>> Contract(int contract_id);
        Task<ApiResponse<string>> Information(int target_id);
        Task<ApiResponse<string>> MarketDetails(int type_id);
        Task<ApiResponse<string>> NewMail(string subject, string body, int[] recipients);
        Task<ApiResponse<string>> Waypoint(long destination_id, bool add_to_beginning = false, bool clear_other_waypoints = false);
    }
}