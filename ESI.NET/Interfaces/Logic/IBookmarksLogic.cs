using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Bookmarks;

namespace ESI.NET.Interfaces.Logic
{
    public interface IBookmarksLogic
    {
        /// <summary>
        /// /characters/{character_id}/bookmarks/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Bookmark>>> ForCharacter(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/bookmarks/folders/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Folder>>> FoldersForCharacter(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/bookmarks/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Bookmark>>> ForCorporation(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/bookmarks/folders/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Folder>>> FoldersForCorporation(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}