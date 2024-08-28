using ESI.NET.Models.SSO;
using ESI.NET.Models.Universe;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class UniverseLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;

        public UniverseLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;
        }

        /// <summary>
        /// /universe/bloodlines/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bloodline>>> Bloodlines(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Bloodline>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/bloodlines/",
                eTag: eTag, cancellationToken: cancellationToken );

        /// <summary>
        /// /universe/categories/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Categories(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/categories/",
                eTag: eTag, cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/categories/{category_id}/
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Category>> Category(int category_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Category>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/categories/{category_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "category_id", category_id.ToString() }
                });

        /// <summary>
        /// /universe/constellations/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Constellations(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/constellations/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/constellations/{constellation_id}/
        /// </summary>
        /// <param name="constellation_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Constellation>> Constellation(int constellation_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Constellation>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/constellations/{constellation_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken, replacements: new Dictionary<string, string>()
                {
                    { "constellation_id", constellation_id.ToString() }
                });

        /// <summary>
        /// /universe/factions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Faction>>> Factions(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Faction>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/factions/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/graphics/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Graphics(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/graphics/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/graphics/{graphic_id}/
        /// </summary>
        /// <param name="graphic_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Graphic>> Graphic(int graphic_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Graphic>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/graphics/{graphic_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken, replacements: new Dictionary<string, string>()
                {
                    { "graphic_id", graphic_id.ToString() }
                });

        /// <summary>
        /// /universe/groups/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Groups(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/groups/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                parameters: new string[]
                {
                    $"page={page}"
                });

        /// <summary>
        /// /universe/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Group>> Group(int group_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/groups/{group_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken, replacements: new Dictionary<string, string>()
                {
                    { "group_id", group_id.ToString() }
                });

        /// <summary>
        /// /universe/moons/{moon_id}/
        /// </summary>
        /// <param name="moon_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Moon>> Moon(int moon_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Moon>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/moons/{moon_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "moon_id", moon_id.ToString() }
                });

        /// <summary>
        /// /universe/names/
        /// </summary>
        /// <param name="any_ids">The ids to resolve; Supported IDs for resolving are: Characters, Corporations, Alliances, Stations, Solar Systems, Constellations, Regions, Types.</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ResolvedInfo>>> Names(List<int> any_ids, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<ResolvedInfo>>(_client, _config, RequestSecurity.Public, HttpMethod.Post,
                "/universe/names/",
                cancellationToken: cancellationToken,
                body: any_ids.ToArray());

        /// <summary>
        /// /universe/ids/
        /// </summary>
        /// <param name="names">Resolve a set of names to IDs in the following categories: agents, alliances, characters, constellations, corporations factions, inventory_types, regions, stations, and systems. Only exact matches will be returned. All names searched for are cached for 12 hours.</param>
        /// <returns></returns>
        public async Task<EsiResponse<IDLookup>> IDs(List<string> names, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<IDLookup>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/ids/",
                cancellationToken: cancellationToken,
                body: names.ToArray());

        /// <summary>
        /// /universe/planets/{planet_id}/
        /// </summary>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Planet>> Planet(int planet_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Planet>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/planets/{planet_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "planet_id", planet_id.ToString() }
                });

        /// <summary>
        /// /universe/races/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Race>>> Races(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Race>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/races/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/regions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Regions(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/regions/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/regions/{region_id}/
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Region>> Region(int region_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Region>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/regions/{region_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", region_id.ToString() }
                });

        /// <summary>
        /// /universe/stations/{station_id}/
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Station>> Station(int station_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Station>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/stations/{station_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "station_id", station_id.ToString() }
                });

        /// <summary>
        /// /universe/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Structures(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/structures/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Structure>> Structure(long structure_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Structure>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/universe/structures/{structure_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "structure_id", structure_id.ToString() }
                }, token: _data.Token);

        /// <summary>
        /// /universe/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Systems(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/systems/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/systems/{system_id}/
        /// </summary>
        /// <param name="system_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<SolarSystem>> System(int system_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<SolarSystem>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/systems/{system_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "system_id", system_id.ToString() }
                });

        /// <summary>
        /// /universe/types/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Types(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/types/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                parameters: new string[]
                {
                    $"page={page}"
                });

        /// <summary>
        /// /universe/types/{type_id}/
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Type>> Type(int type_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Type>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/types/{type_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "type_id", type_id.ToString() }
                });

        /// <summary>
        /// /universe/stargates/{stargate_id}/
        /// </summary>
        /// <param name="stargate_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Stargate>> Stargate(int stargate_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Stargate>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/stargates/{stargate_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "stargate_id", stargate_id.ToString() }
                });

        /// <summary>
        /// /universe/system_jumps/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Jumps>>> Jumps(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Jumps>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/system_jumps/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/system_kills/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Kills>>> Kills(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Kills>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/system_kills/", eTag: eTag, cancellationToken: cancellationToken);

        /// <summary>
        /// /universe/stars/{star_id}/
        /// </summary>
        /// <param name="star_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Star>> Star(int star_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Star>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/stars/{star_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "star_id", star_id.ToString() }
                });

        /// <summary>
        /// /universe/ancestries/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Ancestry>>> Ancestries(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Ancestry>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/ancestries/",
                eTag: eTag,
                cancellationToken: cancellationToken
            );

        /// <summary>
        /// /universe/asteroid_belts/{asteroid_belt_id}/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<AsteroidBelt>>> AsteroidBelt(int asteroid_belt_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<AsteroidBelt>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/universe/asteroid_belts/{asteroid_belt_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "asteroid_belt_id", asteroid_belt_id.ToString() }
                });
    }
}