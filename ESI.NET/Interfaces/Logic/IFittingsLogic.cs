using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Fittings;

namespace ESI.NET.Interfaces.Logic
{
    public interface IFittingsLogic
    {
        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Fitting>>> List(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <param name="fitting"></param>
        /// <returns></returns>
        Task<EsiResponse<NewFitting>> Add(object fitting, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/fittings/{fitting_id}/
        /// </summary>
        /// <param name="fitting_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> Delete(int fitting_id, CancellationToken cancellationToken = default);
    }
}