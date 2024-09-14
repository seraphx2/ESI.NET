using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Clones;

namespace ESI.NET.Interfaces.Logic
{
    public interface IClonesLogic
    {
        /// <summary>
        /// /characters/{character_id}/clones/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Clones>> List(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/implants/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Implants(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}