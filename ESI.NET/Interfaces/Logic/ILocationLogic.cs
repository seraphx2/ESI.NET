using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Location;

namespace ESI.NET.Interfaces.Logic
{
    public interface ILocationLogic
    {
        /// <summary>
        /// /characters/{character_id}/location/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Location>> Location(string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/ship/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Ship>> Ship(string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/online/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Activity>> Online(string eTag = null, CancellationToken cancellationToken = default);
    }
}