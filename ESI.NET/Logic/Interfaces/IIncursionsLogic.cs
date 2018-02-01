using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Incursions;

namespace ESI.NET.Logic.Interfaces
{
    public interface IIncursionsLogic
    {
        Task<ApiResponse<List<Incursion>>> All();
    }
}