using Newtonsoft.Json;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// A dogma attribute as it sits on an item — its id and value — as returned inside
    /// <c>/universe/types/{type_id}/</c> and <c>/dogma/dynamic/items/{type_id}/{item_id}/</c>.
    /// The full descriptive record is <see cref="AttributeInfo"/> (<c>/dogma/attributes/{attribute_id}/</c>).
    /// </summary>
    public class Attribute
    {
        [JsonProperty("attribute_id")]
        public long AttributeId { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
