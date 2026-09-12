using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Calendar
{
    public class CalendarItem
    {
        [JsonProperty("event_date")]
        public DateTime EventDate { get; set; }

        [JsonProperty("event_id")]
        public long EventId { get; set; }

        [JsonProperty("event_response")]
        public string EventResponse { get; set; }

        [JsonProperty("importance")]
        public long Importance { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
