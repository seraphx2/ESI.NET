using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Industry;

namespace ESI.NET.Interfaces.Logic
{
    public interface IIndustryLogic
    {
        /// <summary>
        /// /industry/facilities/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Facility>>> Facilities(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /industry/systems/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<SolarSystem>>> SolarSystemCostIndices(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Job>>> JobsForCharacter(bool include_completed = false, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mining/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Entry>>> MiningLedger(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Observer>>> Observers(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporation/{corporation_id}/mining/observers/{observer_id}/
        /// </summary>
        /// <param name="observer_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ObserverInfo>>> ObservedMining(long observer_id, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/industry/jobs/
        /// </summary>
        /// <param name="include_completed"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Job>>> JobsForCorporation(bool include_completed = false, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporation/{corporation_id}/mining/extractions/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Extraction>>> Extractions(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}