using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Killmails;

namespace ESI.NET.Logic.Interfaces
{
    public interface IKillmailsLogic
    {
        Task<ApiResponse<List<Killmail>>> ForCharacter(int? max_kill_id = 0, int max_count = 50);
        Task<ApiResponse<List<Killmail>>> ForCorporation(int? max_kill_id = 0);
        Task<ApiResponse<Information>> Information(string killmail_hash, int killmail_id);
    }
}