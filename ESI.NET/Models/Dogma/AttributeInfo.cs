using Newtonsoft.Json;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// Full dogma attribute definition from <c>/dogma/attributes/{attribute_id}/</c>.
    /// The id+value pair as it appears on an item is <see cref="Attribute"/>.
    /// </summary>
    public class AttributeInfo
    {
        /// <summary>
        /// ID of the dogma attribute. Details are in the SDE or
        /// <see cref="ESI.NET.Logic.DogmaLogic.Attribute"/>.
        /// </summary>
        [JsonProperty("attribute_id")]
        public long AttributeId { get; set; }

        /// <summary>The attribute's internal name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The attribute's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>ID of the icon the game client shows for it.</summary>
        [JsonProperty("icon_id")]
        public long IconId { get; set; }

        /// <summary>The value an item has when it does not set this attribute.</summary>
        [JsonProperty("default_value")]
        public double DefaultValue { get; set; }

        /// <summary>Whether it is shown in the game client.</summary>
        [JsonProperty("published")]
        public bool Published { get; set; }

        /// <summary>The name the game client shows.</summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// ID of the unit the value is measured in (metres, seconds, percent, ...). Units are
        /// listed in the SDE.
        /// </summary>
        [JsonProperty("unit_id")]
        public long UnitId { get; set; }

        /// <summary>
        /// Whether several modifiers of this attribute stack in full. When false, stacking
        /// penalties apply.
        /// </summary>
        [JsonProperty("stackable")]
        public bool Stackable { get; set; }

        /// <summary>
        /// Whether a higher value is better (the client colours improvements green).
        /// </summary>
        [JsonProperty("high_is_good")]
        public bool HighIsGood { get; set; }
    }
}
