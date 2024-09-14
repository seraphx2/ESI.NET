using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Assets;

namespace ESI.NET.Interfaces.Logic
{
    public interface IAssetsLogic
    {
        /// <summary>
        /// /characters/{character_id}/assets/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Item>>> ForCharacter(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/assets/locations/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ItemLocation>>> LocationsForCharacter(List<long> item_ids,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/assets/names/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ItemName>>> NamesForCharacter(List<long> item_ids,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/assets/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Item>>> ForCorporation(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/assets/locations/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ItemLocation>>> LocationsForCorporation(List<long> item_ids,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/assets/names/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ItemName>>> NamesForCorporation(List<long> item_ids,
            CancellationToken cancellationToken = default);
    }
}