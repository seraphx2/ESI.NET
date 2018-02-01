using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.PlanetaryInteraction;

namespace ESI.NET.Logic.Interfaces
{
    public interface IPlanetaryInteractionLogic
    {
        Task<ApiResponse<List<Planet>>> Colonies();
        Task<ApiResponse<ColonyLayout>> ColonyLayout(int planet_id);
        Task<ApiResponse<List<CustomsOffice>>> CorporationCustomsOffices();
        Task<ApiResponse<Schematic>> SchematicInformation(int schematic_id);
    }
}