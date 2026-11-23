using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.Roles"/>.</summary>
    public class Roles
    {
        /// <summary>Corporation roles the character holds that apply at every location.</summary>
        [JsonProperty("roles")]
        public string[] MainRoles { get; set; }

        /// <summary>Corporation roles that apply at the character's base.</summary>
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
