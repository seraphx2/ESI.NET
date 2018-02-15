using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Universe;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class UniverseLogic : IUniverseLogic
    {
        private ESIConfig _config;

        public UniverseLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /universe/bloodlines/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bloodline>>> Bloodlines()
        {
            var endpoint = "/universe/bloodlines/";
            var response = await Execute<List<Bloodline>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/categories/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Categories()
        {
            var endpoint = "/universe/categories/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/categories/{category_id}/
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Category>> Category(int category_id)
        {
            var endpoint = $"/universe/categories/{category_id}/";
            var response = await Execute<Category>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/constellations/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Constellations()
        {
            var endpoint = "/universe/constellations/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/constellations/{constellation_id}/
        /// </summary>
        /// <param name="constellation_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Constellation>> Constellation(int constellation_id)
        {
            var endpoint = $"/universe/constellations/{constellation_id}/";
            var response = await Execute<Constellation>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/factions/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Faction>>> Factions()
        {
            var endpoint = "/universe/factions/";
            var response = await Execute<List<Faction>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/graphics/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Graphics()
        {
            var endpoint = "/universe/graphics/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/graphics/{graphic_id}/
        /// </summary>
        /// <param name="graphic_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Graphic>> Graphic(int graphic_id)
        {
            var endpoint = $"/universe/graphics/{graphic_id}/";
            var response = await Execute<Graphic>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Groups()
        {
            var endpoint = "/universe/groups/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Group>> Group(int group_id)
        {
            var endpoint = $"/universe/groups/{group_id}/";
            var response = await Execute<Group>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/moons/{moon_id}/
        /// </summary>
        /// <param name="moon_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Moon>> Moon(int moon_id)
        {
            var endpoint = $"/universe/moons/{moon_id}/";
            var response = await Execute<Moon>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/names/
        /// </summary>
        /// <param name="any_ids">The ids to resolve; Supported IDs for resolving are: Characters, Corporations, Alliances, Stations, Solar Systems, Constellations, Regions, Types.</param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ResolvedInfo>>> Names(List<long> any_ids)
        {
            var endpoint = "/universe/names/";
            var response = await Execute<List<ResolvedInfo>>(_config, RequestSecurity.Public, RequestMethod.POST, endpoint, body: any_ids.ToArray());

            return response;
        }

        /// <summary>
        /// /universe/ids/
        /// </summary>
        /// <param name="names">Resolve a set of names to IDs in the following categories: agents, alliances, characters, constellations, corporations factions, inventory_types, regions, stations, and systems. Only exact matches will be returned. All names searched for are cached for 12 hours.</param>
        /// <returns></returns>
        public async Task<ApiResponse<IDLookup>> IDs(List<string> names)
        {
            var endpoint = "/universe/ids/";
            var response = await Execute<IDLookup>(_config, RequestSecurity.Public, RequestMethod.POST, endpoint, body: names.ToArray());

            return response;
        }

        /// <summary>
        /// /universe/planets/{planet_id}/
        /// </summary>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Planet>> Planet(int planet_id)
        {
            var endpoint = $"/universe/planets/{planet_id}/";
            var response = await Execute<Planet>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/races/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Race>>> Races()
        {
            var endpoint = "/universe/races/";
            var response = await Execute<List<Race>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/regions/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<int[]>> Regions()
        {
            var endpoint = "/universe/regions/";
            var response = await Execute<int[]>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/regions/{region_id}/
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Region>> Region(int region_id)
        {
            var endpoint = $"/universe/regions/{region_id}/";
            var response = await Execute<Region>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/stations/{station_id}/
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Station>> Station(int station_id)
        {
            var endpoint = $"/universe/stations/{station_id}/";
            var response = await Execute<Station>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<long[]>> Structures()
        {
            var endpoint = "/universe/structures/";
            var response = await Execute<long[]>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Structure>> Structure(long structure_id)
        {
            var endpoint = $"/universe/structures/{structure_id}/";
            var response = await Execute<Structure>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<int[]>> Systems()
        {
            var endpoint = "/universe/systems/";
            var response = await Execute<int[]>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/systems/{system_id}/
        /// </summary>
        /// <param name="system_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<SolarSystem>> System(int system_id)
        {
            var endpoint = $"/universe/systems/{system_id}/";
            var response = await Execute<SolarSystem>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/types/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<int[]>> Types()
        {
            var endpoint = "/universe/types/";
            var response = await Execute<int[]>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/types/{type_id}/
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Type>> Type(int type_id)
        {
            var endpoint = $"/universe/types/{type_id}/";
            var response = await Execute<Type>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/stargates/{stargate_id}/
        /// </summary>
        /// <param name="stargate_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Stargate>> Stargate(int stargate_id)
        {
            var endpoint = $"/universe/stargates/{stargate_id}/";
            var response = await Execute<Stargate>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/system_jumps/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Jumps>>> Jumps()
        {
            var endpoint = "/universe/system_jumps/";
            var response = await Execute<List<Jumps>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/system_kills/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Kills>>> Kills()
        {
            var endpoint = "/universe/system_kills/";
            var response = await Execute<List<Kills>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/stars/{star_id}/
        /// </summary>
        /// <param name="star_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Star>> Star(int star_id)
        {
            var endpoint = $"/universe/stars/{star_id}/";
            var response = await Execute<Star>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /universe/ancestries/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Ancestry>>> Ancestries()
        {
            var endpoint = $"/universe/ancestries/";
            var response = await Execute<List<Ancestry>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }
    }
}
