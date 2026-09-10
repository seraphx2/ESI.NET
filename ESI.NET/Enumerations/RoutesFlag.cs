using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>Routing preference for <see cref="ESI.NET.Logic.RoutesLogic.Map"/>.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum RoutesFlag
    {
        [EnumMember(Value = "Shorter")]    /**/ Shorter,
        [EnumMember(Value = "Safer")]      /**/ Safer,
        [EnumMember(Value = "LessSecure")] /**/ LessSecure
    }
}
