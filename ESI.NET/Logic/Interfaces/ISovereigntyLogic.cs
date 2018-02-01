using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Sovereignty;

namespace ESI.NET.Logic.Interfaces
{
    public interface ISovereigntyLogic
    {
        Task<ApiResponse<List<Campaign>>> Campaigns();
        Task<ApiResponse<List<Structure>>> Structures();
        Task<ApiResponse<List<SystemSovereignty>>> Systems();
    }
}