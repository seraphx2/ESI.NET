using ESI.NET.Enumerations;
using ESI.NET.Models.Calendar;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class CalendarLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int character_id, corporation_id;

        public CalendarLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
            {
                character_id = data.CharacterID;
                corporation_id = data.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/calendar/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Event>>> Events()
            => await Execute<List<Event>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Event>> Event(int event_id)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/{event_id}/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Event>> Respond(int event_id, EventResponse eventResponse)
        {
            var response = await Execute<Event>(_client, _config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/characters/{character_id}/calendar/{event_id}/", body: new
            {
                response = eventResponse.ToEsiValue()
            }, token: _data.Token);

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
            => await Execute<List<Response>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/{event_id}/attendees/", token: _data.Token);
    }
}