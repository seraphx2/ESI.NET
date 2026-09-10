using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Character
{
    public class AccessListRefList
    {
        [JsonProperty("access_lists")]
        public List<AccessListRef> AccessLists { get; set; } = new List<AccessListRef>();
    }

    public class AccessListRef
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public class AccessList
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("membership")]
        public AccessListMembership Membership { get; set; }
    }

    public class AccessListMembership
    {
        [JsonProperty("allow_everyone")]
        public bool AllowEveryone { get; set; }

        [JsonProperty("alliances")]
        public List<AccessListAllianceEntry> Alliances { get; set; } = new List<AccessListAllianceEntry>();

        [JsonProperty("characters")]
        public List<AccessListCharacterEntry> Characters { get; set; } = new List<AccessListCharacterEntry>();

        [JsonProperty("corporations")]
        public List<AccessListCorporationEntry> Corporations { get; set; } = new List<AccessListCorporationEntry>();
    }

    /// <summary>Unspecified | Allowed | Blocked | Manager | Admin</summary>
    public class AccessListAllianceEntry
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }
    }

    public class AccessListCharacterEntry
    {
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }
    }

    public class AccessListCorporationEntry
    {
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }
    }
}
