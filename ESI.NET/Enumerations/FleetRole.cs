using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>A member's position in a fleet.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum FleetRole
    {
        /// <summary>Commands the whole fleet.</summary>
        [EnumMember(Value="fleet_commander")] /**/ FleetCommander,
        /// <summary>Commands a wing.</summary>
        [EnumMember(Value="wing_commander")]  /**/ WingCommander,
        /// <summary>Commands a squad.</summary>
        [EnumMember(Value="squad_commander")] /**/ SquadCommander,
        /// <summary>A member of a squad.</summary>
        [EnumMember(Value="squad_member")]    /**/ SquadMember
    }
}
