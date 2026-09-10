using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Kills
    {
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        [JsonProperty("ship_kills")]
        public long ShipKills { get; set; }

        [JsonProperty("npc_kills")]
        public long NpcKills { get; set; }

        [JsonProperty("pod_kills")]
        public long PodKills { get; set; }
    }
}
