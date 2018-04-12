using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Contacts;

namespace ESI.NET.Logic.Interfaces
{
    public interface IContactsLogic
    {
        Task<ApiResponse<int[]>> Add(int contact_id, decimal standing, int? label_id = null, bool? watched = null);
        Task<ApiResponse<string>> Delete(int[] contact_ids);
        Task<ApiResponse<List<Label>>> Labels();
        Task<ApiResponse<List<Contact>>> List(int page = 1);
        Task<ApiResponse<List<Contact>>> ListForAlliance(int page = 1);
        Task<ApiResponse<List<Contact>>> ListForCorporation(int page = 1);
        Task<ApiResponse<string>> Update(int contact_id, decimal standing, int? label_id = null, bool? watched = null);
    }
}