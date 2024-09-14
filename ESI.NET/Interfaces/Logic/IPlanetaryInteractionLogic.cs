using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.PlanetaryInteraction;

namespace ESI.NET.Interfaces.Logic
{
    public interface IPlanetaryInteractionLogic
    {
        /// <summary>
        /// /characters/{character_id}/planets/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Planet>>> Colonies(string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/planets/{planet_id}/
        /// </summary>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        Task<EsiResponse<ColonyLayout>> ColonyLayout(int planet_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/customs_offices/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<CustomsOffice>>> CorporationCustomsOffices(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /universe/schematics/{schematic_id}/
        /// </summary>
        /// <param name="schematic_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Schematic>> SchematicInformation(int schematic_id, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}