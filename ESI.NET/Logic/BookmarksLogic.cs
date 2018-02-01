using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Bookmarks;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class BookmarksLogic : IBookmarksLogic
    {
        private ESIConfig _config;
        private int corporation_id, character_id;

        public BookmarksLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                corporation_id = _config.AuthorizedCharacter.CorporationID;
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/bookmarks/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bookmark>>> ForCharacter(int page = 1)
        {
            var endpoint = $"/characters/{character_id}/bookmarks/";
            var response = await Execute<List<Bookmark>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/bookmarks/folders/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Folder>>> FoldersForCharacter(int page = 1)
        {
            var endpoint = $"/characters/{character_id}/bookmarks/folders/";
            var response = await Execute<List<Folder>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/bookmarks/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bookmark>>> ForCorporation(int page = 1)
        {
            var endpoint = $"/corporations/{corporation_id}/bookmarks/";
            var response = await Execute<List<Bookmark>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/bookmarks/folders/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Folder>>> FoldersForCorporation(int page = 1)
        {
            var endpoint = $"/corporations/{corporation_id}/bookmarks/folders/";
            var response = await Execute<List<Folder>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }
    }
}
