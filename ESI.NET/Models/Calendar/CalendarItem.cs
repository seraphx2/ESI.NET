using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Calendar
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CalendarLogic.Events"/>.</summary>
    public class CalendarItem
    {
        /// <summary>When the event starts.</summary>
        [JsonProperty("event_date")]
        public DateTime EventDate { get; set; }

        /// <summary>ID of the calendar event.</summary>
        [JsonProperty("event_id")]
        public long EventId { get; set; }

        /// <summary>
        /// The character's response to the event: <c>accepted</c>, <c>declined</c>,
        /// <c>tentative</c>, or <c>not_responded</c>.
        /// </summary>
        [JsonProperty("event_response")]
        public string EventResponse { get; set; }

        /// <summary>The event's importance: <c>0</c> normal, <c>1</c> important.</summary>
        [JsonProperty("importance")]
        public long Importance { get; set; }

        /// <summary>The event's title.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
