using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Loyalty;

namespace ESI.NET.Logic.Interfaces
{
    public interface ILoyaltyLogic
    {
        Task<ApiResponse<List<Offer>>> Offers(int corporation_id);
        Task<ApiResponse<List<Points>>> Points();
    }
}