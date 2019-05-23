using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AuthVersion
    {
        [EnumMember(Value = "v1")]  /**/ v1,
        [EnumMember(Value = "v2")]  /**/ v2
    }
}
