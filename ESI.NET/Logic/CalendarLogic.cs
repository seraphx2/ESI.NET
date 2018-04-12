using ESI.NET.Enumerations;
using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Calendar;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class CalendarLogic : ICalendarLogic
    {
        private ESIConfig _config;
        private int character_id, corporation_id;

        public CalendarLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
                corporation_id = config.AuthorizedCharacter.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/calendar/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Event>>> Events()
            => await Execute<List<Event>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/");

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Event>> Event(int event_id)
            => await Execute<Event>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/{event_id}/");

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Event>> Respond(int event_id, EventResponse eventResponse)
        {
            var response = await Execute<Event>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/characters/{character_id}/calendar/{event_id}/", body: new
            {
                response = eventResponse.ToString()
            });

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["PUT|/characters/{character_id}/calendar/{event_id}/"];

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Response>>> Responses(int event_id)
            => await Execute<List<Response>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/{event_id}/attendees/");
    }
}