using Newtonsoft.Json;

namespace ESI.NET.Models.SSO
{
    /// <summary>The tokens EVE SSO issues, from <see cref="ESI.NET.SsoLogic.GetToken"/>. Verify them with <see cref="ESI.NET.SsoLogic.Verify"/>.</summary>
    public class SsoToken
    {
        /// <summary>The access token (a JWT) for authenticated ESI calls.</summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        /// <summary>The token type, <c>Bearer</c>.</summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        /// <summary>Seconds until the access token expires (usually 1200).</summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        /// <summary>
        /// The refresh token. EVE SSO rotates it on every refresh: always store the newest one.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
