using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CorporationLogic.RolesHistory"/>.
    /// </summary>
    public class CharacterRolesHistory
    {
        /// <summary>When the roles changed.</summary>
        [JsonProperty("changed_at")]
        public DateTime ChangedAt { get; set; }

        /// <summary>The character whose roles are changed.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>ID of the character who issued this change.</summary>
        [JsonProperty("issuer_id")]
        public long IssuerId { get; set; }

        /// <summary>The roles after the change.</summary>
        [JsonProperty("new_roles")]
        public string[] NewRoles { get; set; }

        /// <summary>The roles before the change.</summary>
        [JsonProperty("old_roles")]
        public string[] OldRoles { get; set; }

        /// <summary>
        /// Which set of roles changed: <c>roles</c>, <c>grantable_roles</c>, or one of their
        /// <c>_at_hq</c> / <c>_at_base</c> / <c>_at_other</c> variants.
        /// </summary>
        [JsonProperty("role_type")]
        public string RoleType { get; set; }
    }
}
