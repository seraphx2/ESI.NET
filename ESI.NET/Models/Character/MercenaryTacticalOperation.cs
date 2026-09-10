using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Character
{
    public class MercenaryTacticalOperationList
    {
        [JsonProperty("operations")]
        public List<MercenaryTacticalOperationRef> Operations { get; set; } = new List<MercenaryTacticalOperationRef>();
    }

    public class MercenaryTacticalOperationRef
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mercenary_den_id")]
        public long MercenaryDenId { get; set; }
    }

    public class MercenaryTacticalOperation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mercenary_den_id")]
        public long MercenaryDenId { get; set; }

        [JsonProperty("dungeon_type_id")]
        public long DungeonTypeId { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        /// <summary>Unspecified | Available | Started | Completed | Expired | Removed</summary>
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
