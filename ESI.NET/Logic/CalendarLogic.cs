using ESI.NET.Enumerations;
using ESI.NET.Models.Calendar;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Calendar endpoints. Reach them through <see cref="EsiClient.Calendar"/>.
    /// </summary>
    public class CalendarLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Calendar endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public CalendarLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// List calendar event summaries. Get 50 event summaries from the calendar. If no
        /// from_event ID is given, the resource will return the next 50 chronological event
        /// summaries from now. If a from_event ID is specified, it will return the next 50
        /// chronological event summaries from after that event.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/calendar</c>.</para>
        /// <para>Requires the <c>esi-calendar.read_calendar_events.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="CalendarItem"/>.</returns>
        public async Task<EsiResponse<List<CalendarItem>>> Events(EsiCallOptions options)
            => await Execute<List<CalendarItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get an event. Get all the information for a specific event.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/calendar/{event_id}</c>.</para>
        /// <para>Requires the <c>esi-calendar.read_calendar_events.v1</c> scope.</para>
        /// </remarks>
        /// <param name="eventId">The id of the event requested.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Event"/>.</returns>
        public async Task<EsiResponse<Event>> Event(long eventId, EsiCallOptions options)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/{event_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "event_id", eventId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Respond to an event. Set your response status to an event.</summary>
        /// <remarks>
        /// <para>Calls <c>PUT /characters/{character_id}/calendar/{event_id}</c>.</para>
        /// <para>Requires the <c>esi-calendar.respond_calendar_events.v1</c> scope.</para>
        /// </remarks>
        /// <param name="eventId">The ID of the event requested.</param>
        /// <param name="eventResponse">The response to give: accepted, declined, or
        /// tentative.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<Event>> Respond(long eventId, EventResponse eventResponse, EsiCallOptions options)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/characters/{character_id}/calendar/{event_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "event_id", eventId.ToString(CultureInfo.InvariantCulture) }
                },
                body: new
                {
                    response = eventResponse.ToEsiValue()
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get attendees. Get all invited attendees for a given event.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/calendar/{event_id}/attendees</c>.</para>
        /// <para>Requires the <c>esi-calendar.read_calendar_events.v1</c> scope.</para>
        /// </remarks>
        /// <param name="eventId">The id of the event requested.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Response"/>.</returns>
        public async Task<EsiResponse<List<Response>>> Responses(long eventId, EsiCallOptions options)
            => await Execute<List<Response>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/{event_id}/attendees/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "event_id", eventId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}