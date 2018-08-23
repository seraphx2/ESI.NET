using ESI.NET.Enumerations;
using ESI.NET.Models;
using ESI.NET.Models.SSO;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class SearchLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int character_id;

        public SearchLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /search/ and /characters/{character_id}/search/
        /// </summary>
        /// <param name="search">The string to search on</param>
        /// <param name="categories">Type of entities to search for</param>
        /// <param name="isStrict">Whether the search should be a strict match</param>
        /// <param name="language">Language to use in the response</param>
        /// <returns></returns>
        public async Task<EsiResponse<SearchResults>> Query(RequestSecurity security, string search, SearchCategory categories, bool isStrict = false, string language = "en-us")
        {
            var categoryList = categories.ToEsiValue();

            var endpoint = "/search/";
            if (security == RequestSecurity.Authenticated)
                endpoint = $"/characters/{character_id}/search/";

            var response = await Execute<SearchResults>(_client, _config, security, RequestMethod.GET, endpoint, new string[] {
                $"search={search}",
                $"categories={categoryList}",
                $"strict={isStrict}",
                $"language={language}"
            }, token: _data?.Token);

            return response;
        }
    }
}
