using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Status;

namespace ESI.NET.Interfaces.Logic
{
    public interface IStatusLogic
    {
        Task<EsiResponse<Status>> Retrieve(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}