using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models.Market;

namespace ESI.NET.Logic.Interfaces
{
    public interface IMarketLogic
    {
        Task<ApiResponse<List<Order>>> CharacterOrders();
        Task<ApiResponse<List<Order>>> CorporationOrders(int page = 1);
        Task<ApiResponse<Group>> Group(int market_group_id);
        Task<ApiResponse<List<int>>> Groups();
        Task<ApiResponse<List<Price>>> Prices();
        Task<ApiResponse<List<Order>>> RegionOrders(int region_id, MarketOrderType order_type = MarketOrderType.all, int page = 1, int? type_id = null);
        Task<ApiResponse<List<Order>>> StructureOrders(long structure_id, int page = 1);
        Task<ApiResponse<List<Statistic>>> TypeHistoryInRegion(int region_id, int type_id);
        Task<ApiResponse<List<int>>> Types(int region_id, int page = 1);
    }
}