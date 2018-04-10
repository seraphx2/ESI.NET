using ESI.NET.Models.Clones;
using System.Threading.Tasks;

namespace ESI.NET.Logic.Interfaces
{
    public interface IClones
    {
        Task<ApiResponse<int[]>> Implants();
        Task<ApiResponse<Clones>> List();
    }
}