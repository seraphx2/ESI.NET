using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Calendar;

namespace ESI.NET.Logic.Interfaces
{
    public interface ICalendarLogic
    {
        Task<ApiResponse<Event>> Event(int event_id);
        Task<ApiResponse<List<Event>>> Events();
        Task<ApiResponse<List<Response>>> Responses(int event_id);
    }
}