using Newtonsoft.Json;

namespace ESI.NET.Models.FactionWarfare
{
    public class War
    {
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("against_id")]
        public long AgainstId { get; set; }
    }
}