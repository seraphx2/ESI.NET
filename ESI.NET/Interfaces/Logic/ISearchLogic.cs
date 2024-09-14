using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models;

namespace ESI.NET.Interfaces.Logic
{
    public interface ISearchLogic
    {
        /// <summary>
        /// /search/ and /characters/{character_id}/search/
        /// </summary>
        /// <param name="search">The string to search on</param>
        /// <param name="categories">Type of entities to search for</param>
        /// <param name="isStrict">Whether the search should be a strict match</param>
        /// <param name="language">Language to use in the response</param>
        /// <returns></returns>
        Task<EsiResponse<SearchResults>> Query(SearchType type, string search, SearchCategory categories,
            bool isStrict = false, string language = "en-us", string eTag = null,
            CancellationToken cancellationToken = default);
    }
}