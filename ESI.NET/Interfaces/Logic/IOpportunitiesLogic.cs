using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET.Interfaces.Logic
{
    public interface IOpportunitiesLogic
    {
        /// <summary>
        /// /opportunities/groups/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Groups(string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /opportunities/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Models.Opportunities.Group>> Group(int group_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /opportunities/tasks/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Tasks(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /opportunities/tasks/{task_id}/
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Models.Opportunities.Task>> Task(int task_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/opportunities/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Models.Opportunities.CompletedTask>>> CompletedTasks(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}