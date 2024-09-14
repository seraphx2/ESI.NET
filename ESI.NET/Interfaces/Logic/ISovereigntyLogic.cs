using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Sovereignty;

namespace ESI.NET.Interfaces.Logic
{
    public interface ISovereigntyLogic
    {
        /// <summary>
        /// /sovereignty/campaigns/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Campaign>>> Campaigns(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /sovereignty/map/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<SystemSovereignty>>> Systems(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /sovereignty/structures/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Structure>>> Structures(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}