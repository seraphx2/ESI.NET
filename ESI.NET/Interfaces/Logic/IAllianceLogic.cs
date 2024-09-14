using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models;
using ESI.NET.Models.Alliance;

namespace ESI.NET.Interfaces.Logic
{
    public interface IAllianceLogic
    {
        /// <summary>
        /// /alliances/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> All(string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /alliances/{alliance_id}/
        /// </summary>
        /// <param name="allianceId"></param>
        /// <returns></returns>
        Task<EsiResponse<Alliance>> Information(int alliance_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /alliances/{alliance_id}/corporations/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Corporations(int alliance_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /alliances/{alliance_id}/icons/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Images>> Icons(int alliance_id, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}