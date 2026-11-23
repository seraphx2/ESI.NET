using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>Categories to search in. Flags: combine them with <c>|</c>.</summary>
    [Flags]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum SearchCategory
    {
        /// <summary>Agents.</summary>
        [EnumMember(Value="agent")]          /**/ Agent = 1,
        /// <summary>Alliances.</summary>
        [EnumMember(Value="alliance")]       /**/ Alliance = 2,
        /// <summary>Characters.</summary>
        [EnumMember(Value="character")]      /**/ Character = 4,
        /// <summary>Constellations.</summary>
        [EnumMember(Value="constellation")]  /**/ Constellation = 8,
        /// <summary>Corporations.</summary>
        [EnumMember(Value="corporation")]    /**/ Corporation = 16,
        /// <summary>Factions.</summary>
        [EnumMember(Value="faction")]        /**/ Faction = 32,
        /// <summary>Item types.</summary>
        [EnumMember(Value="inventory_type")] /**/ InventoryType = 64,
        /// <summary>Regions.</summary>
        [EnumMember(Value="region")]         /**/ Region = 128,
        /// <summary>Solar systems.</summary>
        [EnumMember(Value="solar_system")]   /**/ SolarSystem = 256,
        /// <summary>NPC stations.</summary>
        [EnumMember(Value="station")]        /**/ Station = 512,
        /// <summary>Player structures the character has access to.</summary>
        [EnumMember(Value="structure")]      /**/ Structure = 1024
    }
}
