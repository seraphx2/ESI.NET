using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ESI.NET.Enumerations
{
    /// <summary>What kind of entity an ID resolved by <see cref="ESI.NET.Logic.UniverseLogic.Names"/> is.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ResolvedInfoCategory
    {
        /// <summary>An alliance.</summary>
        [EnumMember(Value = "alliance")] Alliance,
        /// <summary>A character.</summary>
        [EnumMember(Value = "character")] Character,
        /// <summary>A constellation.</summary>
        [EnumMember(Value = "constellation")] Constellation,
        /// <summary>A corporation.</summary>
        [EnumMember(Value = "corporation")] Corporation,
        /// <summary>An item type.</summary>
        [EnumMember(Value = "inventory_type")] InventoryType,
        /// <summary>A region.</summary>
        [EnumMember(Value = "region")] Region,
        /// <summary>A solar system.</summary>
        [EnumMember(Value = "solar_system")] SolarSystem,
        /// <summary>An NPC station.</summary>
        [EnumMember(Value = "station")] Station,
        /// <summary>An NPC faction.</summary>
        [EnumMember(Value = "faction")] Faction
    }
}
