using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CharacterLogic.MercenaryTacticalOperations"/>.
    /// </summary>
    public class MercenaryTacticalOperationList
    {
        /// <summary>List of available operations.</summary>
        [JsonProperty("operations")]
        public List<MercenaryTacticalOperationRef> Operations { get; set; } = new List<MercenaryTacticalOperationRef>();
    }

    /// <summary>
    /// Part of
    /// <see cref="ESI.NET.Models.Character.MercenaryTacticalOperationList.Operations"/>.
    /// </summary>
    public class MercenaryTacticalOperationRef
    {
        /// <summary>Operation's ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>ID of Mercenary Den offering this operation.</summary>
        [JsonProperty("mercenary_den_id")]
        public long MercenaryDenId { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CharacterLogic.MercenaryTacticalOperation"/>.
    /// </summary>
    public class MercenaryTacticalOperation
    {
        /// <summary>Operation's ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>ID of Mercenary Den offering this operation.</summary>
        [JsonProperty("mercenary_den_id")]
        public long MercenaryDenId { get; set; }

        /// <summary>Operation's dungeon type ID.</summary>
        [JsonProperty("dungeon_type_id")]
        public long DungeonTypeId { get; set; }

        /// <summary>Moment the operation will expire.</summary>
        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        /// <summary>
        /// Operation's state. One of <c>Unspecified</c>, <c>Available</c>, <c>Started</c>,
        /// <c>Completed</c>, <c>Expired</c>, <c>Removed</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
