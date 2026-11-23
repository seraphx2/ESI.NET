using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>Routing preference for <see cref="ESI.NET.Logic.RoutesLogic.Map"/>.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum RoutePreference
    {
        /// <summary>The fewest jumps.</summary>
        [EnumMember(Value = "Shorter")]    /**/ Shorter,
        /// <summary>Avoid low-sec and null-sec systems where possible.</summary>
        [EnumMember(Value = "Safer")]      /**/ Safer,
        /// <summary>Prefer low-sec and null-sec systems.</summary>
        [EnumMember(Value = "LessSecure")] /**/ LessSecure
    }
}
