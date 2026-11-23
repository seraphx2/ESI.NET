using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's User Interface endpoints. Reach them through
    /// <see cref="EsiClient.UserInterface"/>.
    /// </summary>
    public class UserInterfaceLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the User Interface endpoints. <see cref="EsiClient"/> does this; there is no
        /// need to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public UserInterfaceLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Open Market Details. Open the market details window for a specific typeID inside the
        /// client.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /ui/openwindow/marketdetails</c>.</para>
        /// <para>Requires the <c>esi-ui.open_window.v1</c> scope.</para>
        /// </remarks>
        /// <param name="typeId">The item type to open in market window.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> MarketDetails(long typeId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/openwindow/marketdetails/",
                parameters: new string[]
                {
                    $"type_id={typeId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>Open the contract window inside the client.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /ui/openwindow/contract</c>.</para>
        /// <para>Requires the <c>esi-ui.open_window.v1</c> scope.</para>
        /// </remarks>
        /// <param name="contractId">The contract to open.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> Contract(long contractId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/openwindow/contract/",
                parameters: new string[]
                {
                    $"contract_id={contractId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Open Information Window. Open the information window for a character, corporation or
        /// alliance inside the client.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /ui/openwindow/information</c>.</para>
        /// <para>Requires the <c>esi-ui.open_window.v1</c> scope.</para>
        /// </remarks>
        /// <param name="targetId">The target to open.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> Information(long targetId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/ui/openwindow/information/",
                parameters: new string[]
                {
                    $"target_id={targetId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>Set Autopilot Waypoint. Set a solar system as autopilot waypoint.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /ui/autopilot/waypoint</c>.</para>
        /// <para>Requires the <c>esi-ui.write_waypoint.v1</c> scope.</para>
        /// </remarks>
        /// <param name="destinationId">The destination to travel to, can be solar system, station
        /// or structure's id.</param>
        /// <param name="addToBeginning">Whether this solar system should be added to the beginning
        /// of all waypoints.</param>
        /// <param name="clearOtherWaypoints">Whether clean other waypoints beforing adding this
        /// one.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
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
        /// Open New Mail Window. Open the New Mail window, according to settings from the request
        /// if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /ui/openwindow/newmail</c>.</para>
        /// <para>Requires the <c>esi-ui.open_window.v1</c> scope.</para>
        /// </remarks>
        /// <param name="subject">The subject to fill in.</param>
        /// <param name="body">The body to fill in.</param>
        /// <param name="recipients">Character IDs to fill in as recipients.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
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