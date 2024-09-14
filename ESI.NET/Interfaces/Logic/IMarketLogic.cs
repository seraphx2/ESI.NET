using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models.Market;

namespace ESI.NET.Interfaces.Logic
{
    public interface IMarketLogic
    {
        /// <summary>
        /// /markets/prices/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Price>>> Prices(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /markets/{region_id}/orders/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="order_type"></param>
        /// <param name="page"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Order>>> RegionOrders(
            int region_id,
            MarketOrderType order_type = MarketOrderType.All,
            int page = 1,
            int? type_id = null,
            string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /markets/{region_id}/history/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Statistic>>> TypeHistoryInRegion(int region_id, int type_id,
            string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /markets/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Order>>> StructureOrders(long structure_id, int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /markets/groups/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Groups(string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /markets/groups/{market_group_id}/
        /// </summary>
        /// <param name="market_group_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Group>> Group(int market_group_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/orders/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Order>>> CharacterOrders(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/orders/history/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Order>>> CharacterOrderHistory(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /markets/{region_id}/types/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Types(int region_id, int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Order>>> CorporationOrders(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Order>>> CorporationOrderHistory(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}