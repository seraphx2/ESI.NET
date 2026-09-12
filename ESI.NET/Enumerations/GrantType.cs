using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum GrantType
    {
        [EnumMember(Value="authorization_code")] /**/ AuthorizationCode,
        [EnumMember(Value="refresh_token")]      /**/ RefreshToken
    }
}
