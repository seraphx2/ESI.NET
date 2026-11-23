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

        /// <summary>
        /// Creates the Universe endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public UniverseLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Get names and categories for a set of IDs. Resolve a set of IDs to names and categories.
        /// Supported ID's for resolving are: Characters, Corporations, Alliances, Stations, Solar
        /// Systems, Constellations, Regions, Types, Factions.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /universe/names</c>.</para>
        /// </remarks>
        /// <param name="anyIds">IDs of any kind (characters, corporations, alliances, stations,
        /// systems, regions, constellations, types, factions) to resolve to names (at most
        /// 1000).</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ResolvedInfo"/>.</returns>
        public async Task<EsiResponse<List<ResolvedInfo>>> Names(IReadOnlyList<long> anyIds, EsiCallOptions options = null)
            => await Execute<List<ResolvedInfo>>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/names/", body: (anyIds ?? throw new System.ArgumentNullException(nameof(anyIds))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Bulk names to IDs. Resolve a set of names to IDs in the following categories: agents,
        /// alliances, characters, constellations, corporations factions, inventory_types, regions,
        /// stations, and systems. Only exact matches will be returned. All names searched for are
        /// cached for 12 hours.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /universe/ids</c>.</para>
        /// </remarks>
        /// <param name="names">Names to resolve (at most 500). Only exact matches are returned, in
        /// categories such as characters, corporations, alliances, systems, stations and item
        /// types.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="IDLookup"/>.</returns>
        public async Task<EsiResponse<IDLookup>> IDs(IReadOnlyList<string> names, EsiCallOptions options = null)
            => await Execute<IDLookup>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/ids/", body: (names ?? throw new System.ArgumentNullException(nameof(names))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>List all public structures.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /universe/structures</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Structures(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/structures/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get structure information. Returns information on requested structure if you are on the
        /// ACL. Otherwise, returns "Forbidden" for all inputs.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /universe/structures/{structure_id}</c>.</para>
        /// <para>Requires the <c>esi-universe.read_structures.v1</c> scope.</para>
        /// </remarks>
        /// <param name="structureId">An Eve structure ID.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Structure"/>.</returns>
        public async Task<EsiResponse<Structure>> Structure(long structureId, EsiCallOptions options)
            => await Execute<Structure>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/universe/structures/{structure_id}/", replacements: new Dictionary<string, string>()
            {
                { "structure_id", structureId.ToString(CultureInfo.InvariantCulture) }
            }, options: options).ConfigureAwait(false);


        /// <summary>
        /// Get system jumps. Get the number of jumps in solar systems within the last hour ending
        /// at the timestamp of the Last-Modified header, excluding wormhole space. Only systems
        /// with jumps will be listed.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /universe/system_jumps</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Jumps"/>.</returns>
        public async Task<EsiResponse<List<Jumps>>> Jumps(EsiCallOptions options = null)
            => await Execute<List<Jumps>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/system_jumps/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get system kills. Get the number of ship, pod and NPC kills per solar system within the
        /// last hour ending at the timestamp of the Last-Modified header, excluding wormhole space.
        /// Only systems with kills will be listed.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /universe/system_kills</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Kills"/>.</returns>
        public async Task<EsiResponse<List<Kills>>> Kills(EsiCallOptions options = null)
            => await Execute<List<Kills>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/system_kills/",
                options: options).ConfigureAwait(false);
    }
}
