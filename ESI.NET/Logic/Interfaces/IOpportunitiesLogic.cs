using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Opportunities;

namespace ESI.NET.Logic.Interfaces
{
    public interface IOpportunitiesLogic
    {
        Task<ApiResponse<List<CompletedTask>>> CompletedTasks();
        Task<ApiResponse<Group>> Group(int group_id);
        Task<ApiResponse<List<int>>> Groups();
        Task<ApiResponse<Models.Opportunities.Task>> Task(int task_id);
        Task<ApiResponse<List<int>>> Tasks();
    }
}