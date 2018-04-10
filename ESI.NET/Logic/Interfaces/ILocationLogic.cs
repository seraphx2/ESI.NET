using ESI.NET.Models.Location;
using System.Threading.Tasks;

namespace ESI.NET.Logic.Interfaces
{
    public interface ILocationLogic
    {
        Task<ApiResponse<Location>> Location();
        Task<ApiResponse<Activity>> Online();
        Task<ApiResponse<Ship>> Ship();
    }
}