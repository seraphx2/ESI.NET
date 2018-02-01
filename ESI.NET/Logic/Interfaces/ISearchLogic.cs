using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models;

namespace ESI.NET.Logic.Interfaces
{
    public interface ISearchLogic
    {
        Task<ApiResponse<SearchResults>> Query(ApiRequest.RequestSecurity security, string search, SearchCategory categories, bool isStrict = false, string language = "en-us");
    }
}