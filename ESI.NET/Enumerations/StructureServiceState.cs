using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ESI.NET.Enumerations
{
    /// <summary>The state of a structure's service module.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum StructureServiceState
    {
        /// <summary>The service is running.</summary>
        [EnumMember(Value = "online")] Online,
        /// <summary>The service is installed but offline.</summary>
        [EnumMember(Value = "offline")] Offline,
        /// <summary>The service was taken offline and is being cleaned up.</summary>
        [EnumMember(Value = "cleanup")] Cleanup
    }
}
