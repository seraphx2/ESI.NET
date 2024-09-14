using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Loyalty;

namespace ESI.NET.Interfaces.Logic
{
    public interface ILoyaltyLogic
    {
        /// <summary>
        /// /loyalty/stores/{corporation_id}/offers/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Offer>>> Offers(int corporation_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/loyalty/points/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Points>>> Points(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}