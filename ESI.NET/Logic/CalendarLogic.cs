using ESI.NET.Enumerations;
using ESI.NET.Models.Calendar;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class CalendarLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public CalendarLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/calendar/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<CalendarItem>>> Events(EsiCallOptions options)
            => await Execute<List<CalendarItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Event>> Event(long event_id, EsiCallOptions options)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/{event_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "event_id", event_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Event>> Respond(long event_id, EventResponse eventResponse, EsiCallOptions options)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/characters/{character_id}/calendar/{event_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "event_id", event_id.ToString(CultureInfo.InvariantCulture) }
                },
                body: new
                {
                    response = eventResponse.ToEsiValue()
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Response>>> Responses(long event_id, EsiCallOptions options)
            => await Execute<List<Response>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/{event_id}/attendees/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "event_id", event_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}