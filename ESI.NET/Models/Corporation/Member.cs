using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    public class Member
    {
        [JsonProperty("character_id")]
        public int CharacterId { get; set; }
    }
}
