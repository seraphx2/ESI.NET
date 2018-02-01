using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Industry;

namespace ESI.NET.Logic.Interfaces
{
    public interface IIndustryLogic
    {
        Task<ApiResponse<List<Extraction>>> Extractions();
        Task<ApiResponse<List<Facility>>> Facilities();
        Task<ApiResponse<List<Job>>> JobsForCharacter(bool include_completed = false);
        Task<ApiResponse<List<Job>>> JobsForCorporation(bool include_completed = false, int page = 1);
        Task<ApiResponse<List<Entry>>> MiningLedger(int page = 1);
        Task<ApiResponse<List<ObserverInfo>>> ObservedMining(int observer_id, int page = 1);
        Task<ApiResponse<List<Observer>>> Observers(int page = 1);
        Task<ApiResponse<List<SolarSystem>>> SolarSystemCostIndices();
    }
}