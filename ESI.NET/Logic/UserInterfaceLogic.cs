using ESI.NET.Logic.Interfaces;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class UserInterfaceLogic : IUserInterfaceLogic
    {
        private ESIConfig _config;

        public UserInterfaceLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /ui/openwindow/marketdetails/
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> MarketDetails(int type_id)
        {
            var endpoint = "/ui/openwindow/marketdetails/";
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, new string[]
            {
                $"type_id={type_id}"
            });

            return response;
        }

        /// <summary>
        /// /ui/openwindow/contract/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Contract(int contract_id)
        {
            var endpoint = "/ui/openwindow/contract/";
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, new string[]
            {
                $"contract_id={contract_id}"
            });

            return response;
        }

        /// <summary>
        /// /ui/openwindow/information/
        /// </summary>
        /// <param name="target_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Information(int target_id)
        {
            var endpoint = "/ui/openwindow/information/";
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, new string[]
            {
                $"target_id={target_id}"
            });

            return response;
        }

        /// <summary>
        /// /ui/autopilot/waypoint/
        /// </summary>
        /// <param name="destination_id"></param>
        /// <param name="add_to_beginning"></param>
        /// <param name="clear_other_waypoints"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Waypoint(long destination_id, bool add_to_beginning = false, bool clear_other_waypoints = false)
        {
            var endpoint = "/ui/autopilot/waypoint/";
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, new string[]
            {
                $"destination_id={destination_id}",
                $"add_to_beginning={add_to_beginning}",
                $"clear_other_waypoints={clear_other_waypoints}"
            });

            return response;
        }

        /// <summary>
        /// /ui/openwindow/newmail/
        /// </summary>
        /// <param name="subject">max length: 1000</param>
        /// <param name="body">max length: 10000</param>
        /// <param name="recipients">max: 50; this can be any of the following id types: character, corporation, alliance, mailing list; only multiple character ids can be specified</param>
        /// <param name="to_mailing_list_id"></param>
        /// <param name="to_corp_or_alliance_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> NewMail(string subject, string body, int[] recipients)
        {
            var endpoint = "/ui/openwindow/newmail/";
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, body: new
            {
                subject = subject,
                body = body,
                recipients = recipients
            });

            return response;
        }
    }
}
