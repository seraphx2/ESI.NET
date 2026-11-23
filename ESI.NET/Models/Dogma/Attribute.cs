using Newtonsoft.Json;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// A dogma attribute as it sits on an item — its id and value — as returned inside
    /// <c>/dogma/dynamic/items/{type_id}/{item_id}/</c>.
    /// The full descriptive record is <see cref="AttributeInfo"/> (<c>/dogma/attributes/{attribute_id}/</c>).
    /// </summary>
    public class Attribute
    {
        /// <summary>
        /// ID of the dogma attribute. Details are in the SDE or
        /// <see cref="ESI.NET.Logic.DogmaLogic.Attribute"/>.
        /// </summary>
        [JsonProperty("attribute_id")]
        public long AttributeId { get; set; }

        /// <summary>The attribute's value on this item.</summary>
        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
