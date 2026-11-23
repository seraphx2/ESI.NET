using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Wars
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.WarsLogic.Information"/>.</summary>
    public class War
    {
        /// <summary>
        /// The aggressor corporation or alliance that declared this war, only contains either
        /// corporation_id or alliance_id.
        /// </summary>
        [JsonProperty("aggressor")]
        public Combatant Aggressor { get; set; }

        /// <summary>
        /// Allied corporations or alliances, each object contains either corporation_id or
        /// alliance_id.
        /// </summary>
        [JsonProperty("allies")]
        public List<Ally> Allies { get; set; }

        /// <summary>Time that the war was declared.</summary>
        [JsonProperty("declared")]
        public DateTime Declared { get; set; }

        /// <summary>
        /// The defending corporation or alliance that declared this war, only contains either
        /// corporation_id or alliance_id.
        /// </summary>
        [JsonProperty("defender")]
        public Combatant Defender { get; set; }

        /// <summary>Time the war ended and shooting was no longer allowed.</summary>
        [JsonProperty("finished")]
        public DateTime Finished { get; set; }

        /// <summary>ID of the specified war.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Was the war declared mutual by both parties.</summary>
        [JsonProperty("mutual")]
        public bool Mutual { get; set; }

        /// <summary>Is the war currently open for allies or not.</summary>
        [JsonProperty("open_for_allies")]
        public bool OpenForAllies { get; set; }

        /// <summary>
        /// Time the war was retracted but both sides could still shoot each other.
        /// </summary>
        [JsonProperty("retracted")]
        public DateTime Retracted { get; set; }

        /// <summary>Time when the war started and both sides could shoot each other.</summary>
        [JsonProperty("started")]
        public DateTime Started { get; set; }
    }



    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Wars.War.Aggressor"/> and
    /// <see cref="ESI.NET.Models.Wars.War.Defender"/>.
    /// </summary>
    public class Combatant
    {
        /// <summary>Alliance ID if and only if the aggressor is an alliance.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>Corporation ID if and only if the aggressor is a corporation.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>ISK value of ships the aggressor has destroyed.</summary>
        [JsonProperty("isk_destroyed")]
        public float IskDestroyed { get; set; }

        /// <summary>The number of ships the aggressor has killed.</summary>
        [JsonProperty("ships_killed")]
        public long ShipsKilled { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Wars.War.Allies"/>.</summary>
    public class Ally
    {
        /// <summary>Alliance ID if and only if this ally is an alliance.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>Corporation ID if and only if this ally is a corporation.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }
    }
}
