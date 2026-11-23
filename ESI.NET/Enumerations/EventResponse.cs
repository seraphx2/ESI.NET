using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>A character's response to a calendar event.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum EventResponse
    {
        /// <summary>Will attend.</summary>
        [EnumMember(Value="accepted")]      /**/ Accepted,
        /// <summary>Will not attend.</summary>
        [EnumMember(Value="declined")]      /**/ Declined,
        /// <summary>Has not answered (only ever read, never sent).</summary>
        [EnumMember(Value="not_responded")] /**/ NotResponded,
        /// <summary>Might attend.</summary>
        [EnumMember(Value="tentative")]     /**/ Tentative
    }
}