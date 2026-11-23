using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>The OAuth grant type of an SSO token request.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum GrantType
    {
        /// <summary>Exchange the authorization code from the SSO callback for tokens.</summary>
        [EnumMember(Value="authorization_code")] /**/ AuthorizationCode,
        /// <summary>Exchange a refresh token for a new access token.</summary>
        [EnumMember(Value="refresh_token")]      /**/ RefreshToken
    }
}
