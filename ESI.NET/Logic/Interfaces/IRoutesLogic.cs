using System.Threading.Tasks;
using ESI.NET.Enumerations;

namespace ESI.NET.Logic.Interfaces
{
    public interface IRoutesLogic
    {
        Task<ApiResponse<int[]>> Map(int origin, int destination, RoutesFlag flag = RoutesFlag.shortest, int[] avoid = null, int[] connections = null);
    }
}