using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.UniverseLogic.Kills"/>.</summary>
    public class Kills
    {
        /// <summary>ID of the solar system.</summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        /// <summary>Number of player ships killed in this system.</summary>
        [JsonProperty("ship_kills")]
        public long ShipKills { get; set; }

        /// <summary>Number of NPC ships killed in this system.</summary>
        [JsonProperty("npc_kills")]
        public long NpcKills { get; set; }

        /// <summary>Number of pods killed in this system.</summary>
        [JsonProperty("pod_kills")]
        public long PodKills { get; set; }
    }
}
