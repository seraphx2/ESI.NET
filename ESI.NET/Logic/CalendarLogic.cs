using ESI.NET.Enumerations;
using ESI.NET.Models.Calendar;
using System.Collections.Generic;
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
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Event>> Event(int event_id, EsiCallOptions options)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/{event_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() },
                    { "event_id", event_id.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Event>> Respond(int event_id, EventResponse eventResponse, EsiCallOptions options)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/characters/{character_id}/calendar/{event_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() },
                    { "event_id", event_id.ToString() }
                },
                body: new
                {
                    response = eventResponse.ToEsiValue()
                },
                options: options);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Response>>> Responses(int event_id, EsiCallOptions options)
            => await Execute<List<Response>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/calendar/{event_id}/attendees/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() },
                    { "event_id", event_id.ToString() }
                },
                options: options);
    }
}