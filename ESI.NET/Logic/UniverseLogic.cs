using ESI.NET.Models.Universe;
using System.Collections.Generic;
using System.Globalization;
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
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/categories/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Categories(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/categories/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/categories/{category_id}/
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Category>> Category(long categoryId, EsiCallOptions options = null)
            => await Execute<Category>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/categories/{category_id}/", replacements: new Dictionary<string, string>()
            {
                { "category_id", categoryId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/constellations/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Constellations(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/constellations/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/constellations/{constellation_id}/
        /// </summary>
        /// <param name="constellationId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Constellation>> Constellation(long constellationId, EsiCallOptions options = null)
            => await Execute<Constellation>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/constellations/{constellation_id}/", replacements: new Dictionary<string, string>()
            {
                { "constellation_id", constellationId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/factions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Faction>>> Factions(EsiCallOptions options = null)
            => await Execute<List<Faction>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/factions/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/graphics/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Graphics(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/graphics/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/graphics/{graphic_id}/
        /// </summary>
        /// <param name="graphicId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Graphic>> Graphic(long graphicId, EsiCallOptions options = null)
            => await Execute<Graphic>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/graphics/{graphic_id}/", replacements: new Dictionary<string, string>()
            {
                { "graphic_id", graphicId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/groups/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Groups(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/groups/",
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /universe/groups/{group_id}/
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Group>> Group(long groupId, EsiCallOptions options = null)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/groups/{group_id}/", replacements: new Dictionary<string, string>()
            {
                { "group_id", groupId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/moons/{moon_id}/
        /// </summary>
        /// <param name="moonId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Moon>> Moon(long moonId, EsiCallOptions options = null)
            => await Execute<Moon>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/moons/{moon_id}/", replacements: new Dictionary<string, string>()
            {
                { "moon_id", moonId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/names/
        /// </summary>
        /// <param name="anyIds">The ids to resolve; Supported IDs for resolving are: Characters, Corporations, Alliances, Stations, Solar Systems, Constellations, Regions, Types.</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ResolvedInfo>>> Names(List<long> anyIds, EsiCallOptions options = null)
            => await Execute<List<ResolvedInfo>>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/names/", body: (anyIds ?? throw new System.ArgumentNullException(nameof(anyIds))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/ids/
        /// </summary>
        /// <param name="names">Resolve a set of names to IDs in the following categories: agents, alliances, characters, constellations, corporations factions, inventory_types, regions, stations, and systems. Only exact matches will be returned. All names searched for are cached for 12 hours.</param>
        /// <returns></returns>
        public async Task<EsiResponse<IDLookup>> IDs(List<string> names, EsiCallOptions options = null)
            => await Execute<IDLookup>(_client, _config, RequestSecurity.Public, HttpMethod.Post, "/universe/ids/", body: (names ?? throw new System.ArgumentNullException(nameof(names))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/planets/{planet_id}/
        /// </summary>
        /// <param name="planetId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Planet>> Planet(long planetId, EsiCallOptions options = null)
            => await Execute<Planet>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/planets/{planet_id}/", replacements: new Dictionary<string, string>()
            {
                { "planet_id", planetId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/races/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Race>>> Races(EsiCallOptions options = null)
            => await Execute<List<Race>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/races/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/regions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Regions(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/regions/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/regions/{region_id}/
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Region>> Region(long regionId, EsiCallOptions options = null)
            => await Execute<Region>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/regions/{region_id}/", replacements: new Dictionary<string, string>()
            {
                { "region_id", regionId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/stations/{station_id}/
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Station>> Station(long stationId, EsiCallOptions options = null)
            => await Execute<Station>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/stations/{station_id}/", replacements: new Dictionary<string, string>()
            {
                { "station_id", stationId.ToString(CultureInfo.InvariantCulture) }
            },
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
        /// /universe/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Systems(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/systems/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/systems/{system_id}/
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<SolarSystem>> System(long systemId, EsiCallOptions options = null)
            => await Execute<SolarSystem>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/systems/{system_id}/", replacements: new Dictionary<string, string>()
            {
                { "system_id", systemId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/types/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Types(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/types/",
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /universe/types/{type_id}/
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Type>> Type(long typeId, EsiCallOptions options = null)
            => await Execute<Type>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/types/{type_id}/", replacements: new Dictionary<string, string>()
            {
                { "type_id", typeId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/stargates/{stargate_id}/
        /// </summary>
        /// <param name="stargateId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Stargate>> Stargate(long stargateId, EsiCallOptions options = null)
            => await Execute<Stargate>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/stargates/{stargate_id}/", replacements: new Dictionary<string, string>()
            {
                { "stargate_id", stargateId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


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


        /// <summary>
        /// /universe/stars/{star_id}/
        /// </summary>
        /// <param name="starId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Star>> Star(long starId, EsiCallOptions options = null)
            => await Execute<Star>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/stars/{star_id}/", replacements: new Dictionary<string, string>()
            {
                { "star_id", starId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/ancestries/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Ancestry>>> Ancestries(EsiCallOptions options = null)
            => await Execute<List<Ancestry>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/ancestries/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /universe/asteroid_belts/{asteroid_belt_id}/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<AsteroidBelt>> AsteroidBelt(long asteroidBeltId, EsiCallOptions options = null)
            => await Execute<AsteroidBelt>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/universe/asteroid_belts/{asteroid_belt_id}/", replacements: new Dictionary<string, string>()
            {
                { "asteroid_belt_id", asteroidBeltId.ToString(CultureInfo.InvariantCulture) }
            },
                options: options).ConfigureAwait(false);

    }
}
