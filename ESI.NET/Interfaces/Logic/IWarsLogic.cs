using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Wars;

namespace ESI.NET.Interfaces.Logic
{
    public interface IWarsLogic
    {
        /// <summary>
        /// /wars/
        /// </summary>
        /// <param name="max_war_id">Only return wars with ID smaller than this</param>
        /// <returns></returns>
        Task<EsiResponse<int[]>> All(long max_war_id = 0, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /wars/{warId}/
        /// </summary>
        /// <param name="war_id"></param>
        /// <returns></returns>
        Task<EsiResponse<War>> Information(int war_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /wars/{warId}/killmails/
        /// </summary>
        /// <param name="war_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Models.Killmails.Killmail>>> Kills(int war_id, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default);
    }
}