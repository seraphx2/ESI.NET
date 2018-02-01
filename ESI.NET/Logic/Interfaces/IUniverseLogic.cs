using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Universe;

namespace ESI.NET.Logic.Interfaces
{
    public interface IUniverseLogic
    {
        Task<ApiResponse<List<Bloodline>>> Bloodlines();
        Task<ApiResponse<List<int>>> Categories();
        Task<ApiResponse<Category>> Category(int category_id);
        Task<ApiResponse<Constellation>> Constellation(int constellation_id);
        Task<ApiResponse<List<int>>> Constellations();
        Task<ApiResponse<List<Faction>>> Factions();
        Task<ApiResponse<Graphic>> Graphic(int graphic_id);
        Task<ApiResponse<List<int>>> Graphics();
        Task<ApiResponse<Group>> Group(int group_id);
        Task<ApiResponse<List<int>>> Groups();
        Task<ApiResponse<IDLookup>> IDs(List<string> names);
        Task<ApiResponse<List<Jumps>>> Jumps();
        Task<ApiResponse<List<Kills>>> Kills();
        Task<ApiResponse<Moon>> Moon(int moon_id);
        Task<ApiResponse<List<ResolvedInfo>>> Names(List<long> any_ids);
        Task<ApiResponse<Planet>> Planet(int planet_id);
        Task<ApiResponse<List<Race>>> Races();
        Task<ApiResponse<Region>> Region(int region_id);
        Task<ApiResponse<int[]>> Regions();
        Task<ApiResponse<Star>> Star(int star_id);
        Task<ApiResponse<Stargate>> Stargate(int stargate_id);
        Task<ApiResponse<Station>> Station(int station_id);
        Task<ApiResponse<Structure>> Structure(long structure_id);
        Task<ApiResponse<long[]>> Structures();
        Task<ApiResponse<SolarSystem>> System(int system_id);
        Task<ApiResponse<int[]>> Systems();
        Task<ApiResponse<Type>> Type(int type_id);
        Task<ApiResponse<int[]>> Types();
    }
}