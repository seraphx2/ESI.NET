using ESI.NET.Enumerations;
using ESI.NET.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class SearchLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public SearchLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/search/
        /// </summary>
        /// <param name="search">The string to search on</param>
        /// <param name="categories">Type of entities to search for</param>
        /// <param name="isStrict">Whether the search should be a strict match</param>
        /// <param name="language">Language to use in the response</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<SearchResults>> Query(string search, SearchCategory categories, EsiCallOptions options, bool isStrict = false, string language = "en-us")
        {
            var categoryList = categories.ToEsiValue();

            var response = await Execute<SearchResults>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/search/",
                options: options,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[] {
                    $"search={search}",
                    $"categories={categoryList}",
                    $"strict={isStrict}",
                    $"language={language}"
                }).ConfigureAwait(false);

            return response;
        }
    }
}
