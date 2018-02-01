using ESI.NET.Enumerations;
using ESI.NET.Logic.Interfaces;
using ESI.NET.Models;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class SearchLogic : ISearchLogic
    {
        private ESIConfig _config;
        private int character_id;

        public SearchLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /search/ and /characters/{character_id}/search/
        /// </summary>
        /// <param name="search">The string to search on</param>
        /// <param name="categories">Type of entities to search for</param>
        /// <param name="isStrict">Whether the search should be a strict match</param>
        /// <param name="language">Language to use in the response</param>
        /// <returns></returns>
        public async Task<ApiResponse<SearchResults>> Query(RequestSecurity security, string search, SearchCategory categories, bool isStrict = false, string language = "en-us")
        {
            var categoryList = categories.ToString().Replace(" ", "");

            var endpoint = "/search/";
            if (security == RequestSecurity.Authenticated)
                endpoint = $"/characters/{character_id}/search/";

            var response = await Execute<SearchResults>(_config, security, RequestMethod.GET, endpoint, new string[] {
                $"search={search}",
                $"categories={categoryList}",
                $"strict={isStrict}",
                $"language={language}"
            });

            return response;
        }
    }
}
