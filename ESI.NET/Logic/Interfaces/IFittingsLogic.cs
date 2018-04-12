using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Fittings;

namespace ESI.NET.Logic.Interface
{
    public interface IFittingsLogic
    {
        Task<ApiResponse<NewFitting>> Add(object fitting);
        Task<ApiResponse<string>> Delete(int fitting_id);
        Task<ApiResponse<List<Fitting>>> List();
    }
}