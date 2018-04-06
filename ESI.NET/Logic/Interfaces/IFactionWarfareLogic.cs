using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.FactionWarfare;

namespace ESI.NET.Logic.Interfaces
{
    public interface IFactionWarfareLogic
    {
        Task<ApiResponse<Leaderboards<FactionTotal>>> Leaderboads();
        Task<ApiResponse<Leaderboards<CharacterTotal>>> LeaderboardsForCharacters();
        Task<ApiResponse<Leaderboards<CorporationTotal>>> LeaderboardsForCorporations();
        Task<ApiResponse<List<War>>> List();
        Task<ApiResponse<List<Stat>>> Stats();
        Task<ApiResponse<Stat>> StatsForCharacter();
        Task<ApiResponse<Stat>> StatsForCorporation();
        Task<ApiResponse<List<FactionWarfareSystem>>> Systems();
    }
}