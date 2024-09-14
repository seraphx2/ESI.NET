using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET.Interfaces.Logic
{
    public interface IUserInterfaceLogic
    {
        /// <summary>
        /// /ui/openwindow/marketdetails/
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> MarketDetails(int type_id, CancellationToken cancellationToken = default);

        /// <summary>
        /// /ui/openwindow/contract/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> Contract(int contract_id, CancellationToken cancellationToken = default);

        /// <summary>
        /// /ui/openwindow/information/
        /// </summary>
        /// <param name="target_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> Information(int target_id, CancellationToken cancellationToken = default);

        /// <summary>
        /// /ui/autopilot/waypoint/
        /// </summary>
        /// <param name="destination_id"></param>
        /// <param name="add_to_beginning"></param>
        /// <param name="clear_other_waypoints"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> Waypoint(long destination_id, bool add_to_beginning = false,
            bool clear_other_waypoints = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// /ui/openwindow/newmail/
        /// </summary>
        /// <param name="subject">max length: 1000</param>
        /// <param name="body">max length: 10000</param>
        /// <param name="recipients">max: 50; this can be any of the following id types: character, corporation, alliance, mailing list; only multiple character ids can be specified</param>
        /// <param name="to_mailing_list_id"></param>
        /// <param name="to_corp_or_alliance_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> NewMail(string subject, string body, int[] recipients,
            CancellationToken cancellationToken = default);
    }
}