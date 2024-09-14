using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Incursions;

namespace ESI.NET.Interfaces.Logic
{
    public interface IIncursionsLogic
    {
        /// <summary>
        /// /incursions/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Incursion>>> All(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}