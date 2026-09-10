using ESI.NET.Models.Universe;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
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
        /// /universe/bloodlines/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bloodline>>> Bloodlines(EsiCallOptions options = null)
            => await Execute<List<Bloodline>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/bloodlines/",
                options: options);


        /// <summary>
        /// /universe/categories/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Categories(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/categories/",
                options: options);


        /// <summary>
        /// /universe/categories/{category_id}/
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Category>> Category(int category_id, EsiCallOptions options = null)
            => await Execute<Category>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/categories/{category_id}/", replacements: new Dictionary<string, string>()
            {
                { "category_id", category_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/constellations/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Constellations(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/constellations/",
                options: options);


        /// <summary>
        /// /universe/constellations/{constellation_id}/
        /// </summary>
        /// <param name="constellation_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Constellation>> Constellation(int constellation_id, EsiCallOptions options = null)
            => await Execute<Constellation>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/constellations/{constellation_id}/", replacements: new Dictionary<string, string>()
            {
                { "constellation_id", constellation_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/factions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Faction>>> Factions(EsiCallOptions options = null)
            => await Execute<List<Faction>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/factions/",
                options: options);


        /// <summary>
        /// /universe/graphics/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Graphics(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/graphics/",
                options: options);


        /// <summary>
        /// /universe/graphics/{graphic_id}/
        /// </summary>
        /// <param name="graphic_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Graphic>> Graphic(int graphic_id, EsiCallOptions options = null)
            => await Execute<Graphic>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/graphics/{graphic_id}/", replacements: new Dictionary<string, string>()
            {
                { "graphic_id", graphic_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/groups/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Groups(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/groups/",
                options: options);

        /// <summary>
        /// /universe/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Group>> Group(int group_id, EsiCallOptions options = null)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/groups/{group_id}/", replacements: new Dictionary<string, string>()
            {
                { "group_id", group_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/moons/{moon_id}/
        /// </summary>
        /// <param name="moon_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Moon>> Moon(int moon_id, EsiCallOptions options = null)
            => await Execute<Moon>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/moons/{moon_id}/", replacements: new Dictionary<string, string>()
            {
                { "moon_id", moon_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/names/
        /// </summary>
        /// <param name="any_ids">The ids to resolve; Supported IDs for resolving are: Characters, Corporations, Alliances, Stations, Solar Systems, Constellations, Regions, Types.</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ResolvedInfo>>> Names(List<int> any_ids, EsiCallOptions options = null)
            => await Execute<List<ResolvedInfo>>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/names/", body: any_ids.ToArray(),
                options: options);


        /// <summary>
        /// /universe/ids/
        /// </summary>
        /// <param name="names">Resolve a set of names to IDs in the following categories: agents, alliances, characters, constellations, corporations factions, inventory_types, regions, stations, and systems. Only exact matches will be returned. All names searched for are cached for 12 hours.</param>
        /// <returns></returns>
        public async Task<EsiResponse<IDLookup>> IDs(List<string> names, EsiCallOptions options = null)
            => await Execute<IDLookup>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/ids/", body: names.ToArray(),
                options: options);


        /// <summary>
        /// /universe/planets/{planet_id}/
        /// </summary>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Planet>> Planet(int planet_id, EsiCallOptions options = null)
            => await Execute<Planet>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/planets/{planet_id}/", replacements: new Dictionary<string, string>()
            {
                { "planet_id", planet_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/races/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Race>>> Races(EsiCallOptions options = null)
            => await Execute<List<Race>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/races/",
                options: options);


        /// <summary>
        /// /universe/regions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Regions(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/regions/",
                options: options);


        /// <summary>
        /// /universe/regions/{region_id}/
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Region>> Region(int region_id, EsiCallOptions options = null)
            => await Execute<Region>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/regions/{region_id}/", replacements: new Dictionary<string, string>()
            {
                { "region_id", region_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/stations/{station_id}/
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Station>> Station(int station_id, EsiCallOptions options = null)
            => await Execute<Station>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/stations/{station_id}/", replacements: new Dictionary<string, string>()
            {
                { "station_id", station_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Structures(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/structures/",
                options: options);


        /// <summary>
        /// /universe/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Structure>> Structure(long structure_id, EsiCallOptions options)
            => await Execute<Structure>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/universe/structures/{structure_id}/", replacements: new Dictionary<string, string>()
            {
                { "structure_id", structure_id.ToString() }
            }, options: options);

        /// <summary>
        /// /universe/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Systems(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/systems/",
                options: options);


        /// <summary>
        /// /universe/systems/{system_id}/
        /// </summary>
        /// <param name="system_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<SolarSystem>> System(int system_id, EsiCallOptions options = null)
            => await Execute<SolarSystem>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/systems/{system_id}/", replacements: new Dictionary<string, string>()
            {
                { "system_id", system_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/types/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Types(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/types/",
                options: options);

        /// <summary>
        /// /universe/types/{type_id}/
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Type>> Type(int type_id, EsiCallOptions options = null)
            => await Execute<Type>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/types/{type_id}/", replacements: new Dictionary<string, string>()
            {
                { "type_id", type_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/stargates/{stargate_id}/
        /// </summary>
        /// <param name="stargate_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Stargate>> Stargate(int stargate_id, EsiCallOptions options = null)
            => await Execute<Stargate>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/stargates/{stargate_id}/", replacements: new Dictionary<string, string>()
            {
                { "stargate_id", stargate_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/system_jumps/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Jumps>>> Jumps(EsiCallOptions options = null)
            => await Execute<List<Jumps>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/system_jumps/",
                options: options);


        /// <summary>
        /// /universe/system_kills/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Kills>>> Kills(EsiCallOptions options = null)
            => await Execute<List<Kills>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/system_kills/",
                options: options);


        /// <summary>
        /// /universe/stars/{star_id}/
        /// </summary>
        /// <param name="star_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Star>> Star(int star_id, EsiCallOptions options = null)
            => await Execute<Star>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/stars/{star_id}/", replacements: new Dictionary<string, string>()
            {
                { "star_id", star_id.ToString() }
            },
                options: options);


        /// <summary>
        /// /universe/ancestries/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Ancestry>>> Ancestries(EsiCallOptions options = null)
            => await Execute<List<Ancestry>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/ancestries/",
                options: options);


        /// <summary>
        /// /universe/asteroid_belts/{asteroid_belt_id}/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<AsteroidBelt>> AsteroidBelt(int asteroid_belt_id, EsiCallOptions options = null)
            => await Execute<AsteroidBelt>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/asteroid_belts/{asteroid_belt_id}/", replacements: new Dictionary<string, string>()
            {
                { "asteroid_belt_id", asteroid_belt_id.ToString() }
            },
                options: options);

    }
}
