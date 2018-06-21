using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class UserInterfaceLogic
    {
        private HttpClient _client;
        private ESIConfig _config;

        public UserInterfaceLogic(HttpClient client, ESIConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /ui/openwindow/marketdetails/
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> MarketDetails(int type_id)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, "/ui/openwindow/marketdetails/", new string[]
            {
                $"type_id={type_id}"
            });

        /// <summary>
        /// /ui/openwindow/contract/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Contract(int contract_id)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, "/ui/openwindow/contract/", new string[]
            {
                $"contract_id={contract_id}"
            });

        /// <summary>
        /// /ui/openwindow/information/
        /// </summary>
        /// <param name="target_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Information(int target_id)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, "/ui/openwindow/information/", new string[]
            {
                $"target_id={target_id}"
            });

        /// <summary>
        /// /ui/autopilot/waypoint/
        /// </summary>
        /// <param name="destination_id"></param>
        /// <param name="add_to_beginning"></param>
        /// <param name="clear_other_waypoints"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Waypoint(long destination_id, bool add_to_beginning = false, bool clear_other_waypoints = false)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, "/ui/autopilot/waypoint/", new string[]
            {
                $"destination_id={destination_id}",
                $"add_to_beginning={add_to_beginning}",
                $"clear_other_waypoints={clear_other_waypoints}"
            });

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
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, "/ui/openwindow/newmail/", body: new
            {
                subject = subject,
                body = body,
                recipients = recipients
            });
    }
}