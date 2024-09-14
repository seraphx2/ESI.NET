using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Killmails;

namespace ESI.NET.Interfaces.Logic
{
    public interface IKillmailsLogic
    {
        /// <summary>
        /// /characters/{character_id}/killmails/recent/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Killmail>>> ForCharacter(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/killmails/recent/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Killmail>>> ForCorporation(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /killmails/{killmail_id}/{killmail_hash}/
        /// </summary>
        /// <param name="killmail_hash">The killmail hash for verification</param>
        /// <param name="killmail_id">The killmail ID to be queried</param>
        /// <returns></returns>
        Task<EsiResponse<Information>> Information(string killmail_hash, int killmail_id,
            string eTag = null,
            CancellationToken cancellationToken = default);
    }
}