using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class UserInterfaceLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public UserInterfaceLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /ui/openwindow/marketdetails/
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> MarketDetails(long typeId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/openwindow/marketdetails/",
                parameters: new string[]
                {
                    $"type_id={typeId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /ui/openwindow/contract/
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Contract(long contractId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/openwindow/contract/",
                parameters: new string[]
                {
                    $"contract_id={contractId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /ui/openwindow/information/
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Information(long targetId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/openwindow/information/",
                parameters: new string[]
                {
                    $"target_id={targetId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /ui/autopilot/waypoint/
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="addToBeginning"></param>
        /// <param name="clearOtherWaypoints"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Waypoint(long destinationId, bool addToBeginning = false, bool clearOtherWaypoints = false, EsiCallOptions options = null)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/autopilot/waypoint/",
                parameters: new string[]
                {
                    $"destination_id={destinationId}",
                    $"add_to_beginning={addToBeginning}",
                    $"clear_other_waypoints={clearOtherWaypoints}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /ui/openwindow/newmail/
        /// </summary>
        /// <param name="subject">max length: 1000</param>
        /// <param name="body">max length: 10000</param>
        /// <param name="recipients">max: 50; this can be any of the following id types: character, corporation, alliance, mailing list; only multiple character ids can be specified</param>
        /// <param name="to_mailing_list_id"></param>
        /// <param name="to_corp_or_alliance_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> NewMail(string subject, string body, long[] recipients, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/openwindow/newmail/",
                body: new
                {
                    subject,
                    body,
                    recipients
                },
                options: options).ConfigureAwait(false);
    }
}