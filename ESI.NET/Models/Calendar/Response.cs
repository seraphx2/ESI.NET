using ESI.NET.Enumerations;
using Newtonsoft.Json;

namespace ESI.NET.Models.Calendar
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CalendarLogic.Responses"/>.</summary>
    public class Response
    {
        /// <summary>ID of the invited character.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>
        /// The character's response to the event: <c>accepted</c>, <c>declined</c>,
        /// <c>tentative</c>, or <c>not_responded</c>.
        /// </summary>
        [JsonProperty("event_response")]
        public EventResponse EventResponse { get; set; }
    }
}
