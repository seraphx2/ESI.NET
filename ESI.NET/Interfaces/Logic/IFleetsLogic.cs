using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models.Fleets;

namespace ESI.NET.Interfaces.Logic
{
    public interface IFleetsLogic
    {
        /// <summary>
        /// /fleets/{fleet_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Settings>> Settings(long fleet_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="motd"></param>
        /// <param name="is_free_move"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> UpdateSettings(long fleet_id, string motd = null,
            bool? is_free_move = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/fleet/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<FleetInfo>> FleetInfo(string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/members/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Member>>> Members(long fleet_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/members/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="character_id"></param>
        /// <param name="role"></param>
        /// <param name="wing_id"></param>
        /// <param name="squad_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> InviteCharacter(long fleet_id, int character_id, FleetRole role,
            long wing_id = 0, long squad_id = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/members/{member_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="member_id"></param>
        /// <param name="role"></param>
        /// <param name="wing_id"></param>
        /// <param name="squad_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> MoveCharacter(long fleet_id, int member_id, FleetRole role,
            long wing_id = 0, long squad_id = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/members/{member_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="member_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> KickCharacter(long fleet_id, int member_id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/wings/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Wing>>> Wings(long fleet_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/wings/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <returns></returns>
        Task<EsiResponse<NewWing>> CreateWing(long fleet_id, CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="wing_id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> RenameWing(long fleet_id, long wing_id, string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="wing_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> DeleteWing(long fleet_id, long wing_id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/wings/{wing_id}/squads/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="wing_id"></param>
        /// <returns></returns>
        Task<EsiResponse<NewSquad>> CreateSquad(long fleet_id, long wing_id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/squads/{squad_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="squad_id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> RenameSquad(long fleet_id, long squad_id, string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /fleets/{fleet_id}/squads/{squad_id}/
        /// </summary>
        /// <param name="fleet_id"></param>
        /// <param name="squad_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> DeleteSquad(long fleet_id, long squad_id,
            CancellationToken cancellationToken = default);
    }
}