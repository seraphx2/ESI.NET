using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Roles"/>.</summary>
    public class CharacterRoles
    {
        /// <summary>ID of the member.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>Corporation roles that may be granted to others, at every location.</summary>
        [JsonProperty("grantable_roles")]
        public string[] GrantableRoles { get; set; }

        /// <summary>
        /// Corporation roles that may be granted to others, at the member's base.
        /// </summary>
        [JsonProperty("grantable_roles_at_base")]
        public string[] GrantableRolesAtBase { get; set; }

        /// <summary>
        /// Corporation roles that may be granted to others, at the corporation's headquarters.
        /// </summary>
        [JsonProperty("grantable_roles_at_hq")]
        public string[] GrantableRolesAtHq { get; set; }

        /// <summary>Corporation roles that may be granted to others, at other locations.</summary>
        [JsonProperty("grantable_roles_at_other")]
        public string[] GrantableRolesAtOther { get; set; }

        /// <summary>Corporation roles the member holds that apply at every location.</summary>
        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        /// <summary>Corporation roles that apply at the member's base.</summary>
        [JsonProperty("roles_at_base")]
        public string[] RolesAtBase { get; set; }

        /// <summary>Corporation roles that apply at the corporation's headquarters.</summary>
        [JsonProperty("roles_at_hq")]
        public string[] RolesAtHq { get; set; }

        /// <summary>Corporation roles that apply at other locations.</summary>
        [JsonProperty("roles_at_other")]
        public string[] RolesAtOther { get; set; }
    }
}
