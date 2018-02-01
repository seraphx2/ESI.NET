using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Assets;

namespace ESI.NET.Logic.Interfaces
{
    public interface IAssetsLogic
    {
        Task<ApiResponse<List<Item>>> ForCharacter(int page = 1);
        Task<ApiResponse<List<Item>>> ForCorporation(int page = 1);
        Task<ApiResponse<List<ItemLocation>>> LocationsForCharacter(List<long> item_ids);
        Task<ApiResponse<List<ItemLocation>>> LocationsForCorporation(List<long> item_ids);
        Task<ApiResponse<List<ItemName>>> NamesForCharacter(List<long> item_ids);
        Task<ApiResponse<List<ItemName>>> NamesForCorporation(List<long> item_ids);
    }
}