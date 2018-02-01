using ESI.NET.Models.Killmails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESI.NET.Logic.Interfaces
{
    public interface IWarsLogic
    {
        Task<ApiResponse<List<int>>> All(long max_war_id = 0);
        Task<ApiResponse<Models.Wars.Information>> Information(int war_id);
        Task<ApiResponse<List<Killmail>>> Kills(int war_id, int page = 1);
    }
}