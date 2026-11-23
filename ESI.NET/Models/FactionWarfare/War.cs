using Newtonsoft.Json;

namespace ESI.NET.Models.FactionWarfare
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FactionWarfareLogic.List"/>.</summary>
    public class War
    {
        /// <summary>ID of the faction at war with <see cref="AgainstId"/>.</summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>The faction ID of the enemy faction.</summary>
        [JsonProperty("against_id")]
        public long AgainstId { get; set; }
    }
}