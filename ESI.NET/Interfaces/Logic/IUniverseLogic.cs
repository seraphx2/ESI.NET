using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Universe;

namespace ESI.NET.Interfaces.Logic
{
    public interface IUniverseLogic
    {
        /// <summary>
        /// /universe/bloodlines/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Bloodline>>> Bloodlines(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/categories/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Categories(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/categories/{category_id}/
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Category>> Category(int category_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/constellations/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Constellations(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/constellations/{constellation_id}/
        /// </summary>
        /// <param name="constellation_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Constellation>> Constellation(int constellation_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/factions/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Faction>>> Factions(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/graphics/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Graphics(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/graphics/{graphic_id}/
        /// </summary>
        /// <param name="graphic_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Graphic>> Graphic(int graphic_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/groups/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Groups(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Group>> Group(int group_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/moons/{moon_id}/
        /// </summary>
        /// <param name="moon_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Moon>> Moon(int moon_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/names/
        /// </summary>
        /// <param name="any_ids">The ids to resolve; Supported IDs for resolving are: Characters, Corporations, Alliances, Stations, Solar Systems, Constellations, Regions, Types.</param>
        /// <returns></returns>
        Task<EsiResponse<List<ResolvedInfo>>> Names(List<int> any_ids, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/ids/
        /// </summary>
        /// <param name="names">Resolve a set of names to IDs in the following categories: agents, alliances, characters, constellations, corporations factions, inventory_types, regions, stations, and systems. Only exact matches will be returned. All names searched for are cached for 12 hours.</param>
        /// <returns></returns>
        Task<EsiResponse<IDLookup>> IDs(List<string> names, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/planets/{planet_id}/
        /// </summary>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Planet>> Planet(int planet_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/races/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Race>>> Races(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/regions/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Regions(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/regions/{region_id}/
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Region>> Region(int region_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/stations/{station_id}/
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Station>> Station(int station_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/structures/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<long[]>> Structures(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Structure>> Structure(long structure_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/systems/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Systems(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/systems/{system_id}/
        /// </summary>
        /// <param name="system_id"></param>
        /// <returns></returns>
        Task<EsiResponse<SolarSystem>> System(int system_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/types/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Types(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/types/{type_id}/
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Type>> Type(int type_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/stargates/{stargate_id}/
        /// </summary>
        /// <param name="stargate_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Stargate>> Stargate(int stargate_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/system_jumps/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Jumps>>> Jumps(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/system_kills/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Kills>>> Kills(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/stars/{star_id}/
        /// </summary>
        /// <param name="star_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Star>> Star(int star_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/ancestries/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Ancestry>>> Ancestries(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/asteroid_belts/{asteroid_belt_id}/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<AsteroidBelt>>> AsteroidBelt(int asteroid_belt_id, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}