using ESI.NET.Models.Universe;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// The dynamic <c>/universe/*</c> routes. The static-data routes (types, groups,
    /// systems, regions, races, ...) were removed from ESI at compatibility date
    /// <c>2026-11-19</c>; that data is now served by the SDE at
    /// <c>https://sde.evetech.net</c>.
    /// </summary>
    public class UniverseLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public UniverseLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /universe/names/
        /// </summary>
        /// <param name="anyIds">The ids to resolve; Supported IDs for resolving are: Characters, Corporations, Alliances, Stations, Solar Systems, Constellations, Regions, Types.</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ResolvedInfo>>> Names(IReadOnlyList<long> anyIds, EsiCallOptions options = null)
            => await Execute<List<ResolvedInfo>>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/names/", body: (anyIds ?? throw new System.ArgumentNullException(nameof(anyIds))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/ids/
        /// </summary>
        /// <param name="names">Resolve a set of names to IDs in the following categories: agents, alliances, characters, constellations, corporations factions, inventory_types, regions, stations, and systems. Only exact matches will be returned. All names searched for are cached for 12 hours.</param>
        /// <returns></returns>
        public async Task<EsiResponse<IDLookup>> IDs(IReadOnlyList<string> names, EsiCallOptions options = null)
            => await Execute<IDLookup>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/ids/", body: (names ?? throw new System.ArgumentNullException(nameof(names))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Structures(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/structures/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/structures/{structure_id}/
        /// </summary>
        /// <param name="structureId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Structure>> Structure(long structureId, EsiCallOptions options)
            => await Execute<Structure>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/universe/structures/{structure_id}/", replacements: new Dictionary<string, string>()
            {
                { "structure_id", structureId.ToString(CultureInfo.InvariantCulture) }
            }, options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/system_jumps/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Jumps>>> Jumps(EsiCallOptions options = null)
            => await Execute<List<Jumps>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/system_jumps/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/system_kills/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Kills>>> Kills(EsiCallOptions options = null)
            => await Execute<List<Kills>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/system_kills/",
                options: options).ConfigureAwait(false);
    }
}
