using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models.Calendar;

namespace ESI.NET.Interfaces.Logic
{
    public interface ICalendarLogic
    {
        /// <summary>
        /// /characters/{character_id}/calendar/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<CalendarItem>>> Events(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Event>> Event(int event_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/calendar/{event_id}/
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        Task<EsiResponse<Event>> Respond(int event_id, EventResponse eventResponse,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Response>>> Responses(int event_id, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}