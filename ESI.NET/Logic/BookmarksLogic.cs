using ESI.NET.Models.Bookmarks;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class BookmarksLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public BookmarksLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/bookmarks/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bookmark>>> ForCharacter(EsiCallOptions options)
            => await Execute<List<Bookmark>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/bookmarks/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/bookmarks/folders/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Folder>>> FoldersForCharacter(EsiCallOptions options)
            => await Execute<List<Folder>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/bookmarks/folders/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/bookmarks/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bookmark>>> ForCorporation(EsiCallOptions options)
            => await Execute<List<Bookmark>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/bookmarks/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/bookmarks/folders/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Folder>>> FoldersForCorporation(EsiCallOptions options)
            => await Execute<List<Folder>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/bookmarks/folders/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);
    }
}