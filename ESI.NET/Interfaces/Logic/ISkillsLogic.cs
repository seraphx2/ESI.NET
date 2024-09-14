using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Skills;

namespace ESI.NET.Interfaces.Logic
{
    public interface ISkillsLogic
    {
        /// <summary>
        /// /characters/{character_id}/attributes/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Attributes>> Attributes(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/skills/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<SkillDetails>> List(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/skillqueue/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<SkillQueueItem>>> Queue(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}