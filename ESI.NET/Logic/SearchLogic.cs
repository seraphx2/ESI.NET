using ESI.NET.Enumerations;
using ESI.NET.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Search endpoints. Reach them through <see cref="EsiClient.Search"/>.
    /// </summary>
    public class SearchLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Search endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public SearchLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Search on a string. Search for entities that match a given sub-string.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/search</c>.</para>
        /// <para>Requires the <c>esi-search.search_structures.v1</c> scope.</para>
        /// </remarks>
        /// <param name="search">The string to search on.</param>
        /// <param name="categories">Type of entities to search for.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <param name="isStrict">Only return exact matches of <c>search</c> when true; otherwise
        /// also match names that start with it.</param>
        /// <param name="language">The language <c>search</c> is in, e.g. <c>en</c>, <c>de</c>,
        /// <c>fr</c>, <c>ja</c>, <c>ru</c>, <c>zh</c>, <c>ko</c>, <c>es</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SearchResults"/>.</returns>
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
