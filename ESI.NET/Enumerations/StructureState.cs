using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ESI.NET.Enumerations
{
    /// <summary>Where an Upwell structure is in its anchoring and reinforcement cycle.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum StructureState
    {
        /// <summary>Anchoring, and vulnerable while it does.</summary>
        [EnumMember(Value = "anchor_vulnerable")] AnchorVulnerable,
        /// <summary>Anchoring.</summary>
        [EnumMember(Value = "anchoring")] Anchoring,
        /// <summary>Its shield was broken; reinforced until the armor timer.</summary>
        [EnumMember(Value = "armor_reinforce")] ArmorReinforce,
        /// <summary>Out of armor reinforcement; its armor can be attacked.</summary>
        [EnumMember(Value = "armor_vulnerable")] ArmorVulnerable,
        /// <summary>Just deployed, and vulnerable.</summary>
        [EnumMember(Value = "deploy_vulnerable")] DeployVulnerable,
        /// <summary>Being fitted after anchoring; cannot be attacked.</summary>
        [EnumMember(Value = "fitting_invulnerable")] FittingInvulnerable,
        /// <summary>Its armor was broken; reinforced until the hull timer.</summary>
        [EnumMember(Value = "hull_reinforce")] HullReinforce,
        /// <summary>Out of hull reinforcement; it can be destroyed.</summary>
        [EnumMember(Value = "hull_vulnerable")] HullVulnerable,
        /// <summary>A legacy state ESI may still report.</summary>
        [EnumMember(Value = "online_deprecated")] OnlineDeprecated,
        /// <summary>Coming online after anchoring, and vulnerable while it does.</summary>
        [EnumMember(Value = "onlining_vulnerable")] OnliningVulnerable,
        /// <summary>Online and in its vulnerability window; its shield can be attacked.</summary>
        [EnumMember(Value = "shield_vulnerable")] ShieldVulnerable,
        /// <summary>Not anchored.</summary>
        [EnumMember(Value = "unanchored")] Unanchored,
        /// <summary>A state ESI reported that has no meaning here.</summary>
        [EnumMember(Value = "unknown")] Unknown
    }
}
