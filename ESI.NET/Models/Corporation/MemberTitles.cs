using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    public class MemberTitles
    {
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("titles")]
        public long[] Titles { get; set; }

    }
}
