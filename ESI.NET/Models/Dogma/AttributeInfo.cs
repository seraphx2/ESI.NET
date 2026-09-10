using Newtonsoft.Json;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// Full dogma attribute definition from <c>/dogma/attributes/{attribute_id}/</c>.
    /// The id+value pair as it appears on an item is <see cref="Attribute"/>.
    /// </summary>
    public class AttributeInfo
    {
        [JsonProperty("attribute_id")]
        public int AttributeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon_id")]
        public int IconId { get; set; }

        [JsonProperty("default_value")]
        public double DefaultValue { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("unit_id")]
        public int UnitId { get; set; }

        [JsonProperty("stackable")]
        public bool Stackable { get; set; }

        [JsonProperty("high_is_good")]
        public bool HighIsGood { get; set; }
    }
}
