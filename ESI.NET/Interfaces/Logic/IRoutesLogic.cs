using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Enumerations;

namespace ESI.NET.Interfaces.Logic
{
    public interface IRoutesLogic
    {
        /// <summary>
        /// /route/{origin}/{destination}/
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="flag"></param>
        /// <param name="avoid"></param>
        /// <param name="connections"></param>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Map(
            int origin,
            int destination,
            RoutesFlag flag = RoutesFlag.Shortest,
            int[] avoid = null,
            int[] connections = null,
            string eTag = null,
            CancellationToken cancellationToken = default);
    }
}