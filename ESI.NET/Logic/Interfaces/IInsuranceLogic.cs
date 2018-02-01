using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Insurance;

namespace ESI.NET.Logic.Interfaces
{
    public interface IInsuranceLogic
    {
        Task<ApiResponse<List<Insurance>>> Levels();
    }
}