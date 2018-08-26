using ESI.NET.Enumerations;
using ESI.NET.Models.Calendar;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class CalendarLogic : BaseLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id;

        public CalendarLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/calendar/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Event>>> Events()
            => await Execute<List<Event>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Event>> Event(int event_id)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/{event_id}/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Event>> Respond(int event_id, EventResponse eventResponse)
            => await Execute<Event>(_client, _config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/characters/{character_id}/calendar/{event_id}/", noContent: NoContentMessages["PUT|/characters/{character_id}/calendar/{event_id}/"], body: new
            {
                response = eventResponse.ToEsiValue()
            }, token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Response>>> Responses(int event_id)
            => await Execute<List<Response>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/calendar/{event_id}/attendees/", token: _data.Token);
    }
}