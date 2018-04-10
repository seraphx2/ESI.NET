using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models;
using ESI.NET.Models.Alliance;

namespace ESI.NET.Logic.Interfaces
{
    public interface IAllianceLogic
    {
        Task<ApiResponse<List<int>>> All();
        Task<ApiResponse<List<int>>> Corporations(int alliance_id);
        Task<ApiResponse<Images>> Icons(int alliance_id);
        Task<ApiResponse<Information>> Information(int alliance_id);
        Task<ApiResponse<List<Alliance>>> Names(int[] alliance_ids);
    }
}