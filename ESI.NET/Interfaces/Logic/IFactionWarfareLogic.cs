using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.FactionWarfare;

namespace ESI.NET.Interfaces.Logic
{
    public interface IFactionWarfareLogic
    {
        /// <summary>
        /// /fw/wars/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<War>>> List(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fw/stats/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Stat>>> Stats(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fw/systems/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<FactionWarfareSystem>>> Systems(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// fw/leaderboards/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Leaderboards<FactionTotal>>> Leaderboads(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fw/leaderboards/corporations/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Leaderboards<CorporationTotal>>> LeaderboardsForCorporations(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fw/leaderboards/characters/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Leaderboards<CharacterTotal>>> LeaderboardsForCharacters(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Stat>> StatsForCorporation(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Stat>> StatsForCharacter(string eTag = null,
            CancellationToken cancellationToken = default);
    }
}