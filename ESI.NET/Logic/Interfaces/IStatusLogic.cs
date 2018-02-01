using System.Threading.Tasks;
using ESI.NET.Models.Status;

namespace ESI.NET.Logic.Interfaces
{
    public interface IStatusLogic
    {
        Task<ApiResponse<Status>> Retrieve();
    }
}