using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Insurance;

namespace ESI.NET.Interfaces.Logic
{
    public interface IInsuranceLogic
    {
        /// <summary>
        /// /insurance/prices/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Insurance>>> Levels(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}