using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.AccessLists"/>.</summary>
    public class AccessListRefList
    {
        /// <summary>List of Access Lists.</summary>
        [JsonProperty("access_lists")]
        public List<AccessListRef> AccessLists { get; set; } = new List<AccessListRef>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Character.AccessListRefList.AccessLists"/>.
    /// </summary>
    public class AccessListRef
    {
        /// <summary>Access List's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }
    }

    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.AccessList"/>.</summary>
    public class AccessList
    {
        /// <summary>The Access List's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The Access List's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The Access List's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>The Access List's membership.</summary>
        [JsonProperty("membership")]
        public AccessListMembership Membership { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Character.AccessList.Membership"/>.</summary>
    public class AccessListMembership
    {
        /// <summary>Whether everyone is allowed unless blocked.</summary>
        [JsonProperty("allow_everyone")]
        public bool AllowEveryone { get; set; }

        /// <summary>Alliances in the Access List.</summary>
        [JsonProperty("alliances")]
        public List<AccessListAllianceEntry> Alliances { get; set; } = new List<AccessListAllianceEntry>();

        /// <summary>Characters in the Access List.</summary>
        [JsonProperty("characters")]
        public List<AccessListCharacterEntry> Characters { get; set; } = new List<AccessListCharacterEntry>();

        /// <summary>Corporations in the Access List.</summary>
        [JsonProperty("corporations")]
        public List<AccessListCorporationEntry> Corporations { get; set; } = new List<AccessListCorporationEntry>();
    }

    /// <summary>Unspecified | Allowed | Blocked | Manager | Admin</summary>
    public class AccessListAllianceEntry
    {
        /// <summary>Alliance's ID.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>
        /// Alliance's access. One of <c>Unspecified</c>, <c>Allowed</c>, <c>Blocked</c>,
        /// <c>Manager</c>, <c>Admin</c>.
        /// </summary>
        [JsonProperty("access")]
        public string Access { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Character.AccessListMembership.Characters"/>.
    /// </summary>
    public class AccessListCharacterEntry
    {
        /// <summary>Character's ID.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>
        /// Character's access. One of <c>Unspecified</c>, <c>Allowed</c>, <c>Blocked</c>,
        /// <c>Manager</c>, <c>Admin</c>.
        /// </summary>
        [JsonProperty("access")]
        public string Access { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Character.AccessListMembership.Corporations"/>.
    /// </summary>
    public class AccessListCorporationEntry
    {
        /// <summary>Corporation's ID.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>
        /// Corporation's access. One of <c>Unspecified</c>, <c>Allowed</c>, <c>Blocked</c>,
        /// <c>Manager</c>, <c>Admin</c>.
        /// </summary>
        [JsonProperty("access")]
        public string Access { get; set; }
    }
}
