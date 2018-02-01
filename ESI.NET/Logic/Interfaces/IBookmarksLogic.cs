using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Bookmarks;

namespace ESI.NET.Logic.Interfaces
{
    public interface IBookmarksLogic
    {
        Task<ApiResponse<List<Folder>>> FoldersForCharacter(int page = 1);
        Task<ApiResponse<List<Folder>>> FoldersForCorporation(int page = 1);
        Task<ApiResponse<List<Bookmark>>> ForCharacter(int page = 1);
        Task<ApiResponse<List<Bookmark>>> ForCorporation(int page = 1);
    }
}