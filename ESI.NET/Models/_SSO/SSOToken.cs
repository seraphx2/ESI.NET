using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.SSO
{
    public class SSOToken
    {
        [JsonProperty("access_token")]
        public string Value { get; set; }

        [JsonProperty("token_type")]
        public string Type { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string Refresh { get; set; }

        public DateTime Expires { get; set; }
    }
}
