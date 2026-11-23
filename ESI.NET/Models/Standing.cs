using Newtonsoft.Json;

namespace ESI.NET.Models
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CharacterLogic.Standings"/> and
    /// <see cref="ESI.NET.Logic.CorporationLogic.Standings"/>.
    /// </summary>
    public class Standing
    {
        /// <summary>ID of the agent, NPC corporation, or faction the standing is with.</summary>
        [JsonProperty("from_id")]
        public long FromId { get; set; }

        /// <summary>
        /// What <see cref="FromId"/> is: <c>agent</c>, <c>npc_corp</c>, or <c>faction</c>.
        /// </summary>
        [JsonProperty("from_type")]
        public string FromType { get; set; }

        /// <summary>The standing, from -10 to 10.</summary>
        [JsonProperty("standing")]
        public decimal Value { get; set; }
    }
}
