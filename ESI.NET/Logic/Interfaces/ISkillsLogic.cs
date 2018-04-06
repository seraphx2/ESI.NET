using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Skills;

namespace ESI.NET.Logic.Interfaces
{
    public interface ISkillsLogic
    {
        Task<ApiResponse<Attributes>> Attributes();
        Task<ApiResponse<SkillDetails>> List();
        Task<ApiResponse<List<SkillQueueItem>>> Queue();
    }
}