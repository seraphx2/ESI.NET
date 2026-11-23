using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// Response of <c>/dogma/dynamic/items/{type_id}/{item_id}/</c> — a mutated (abyssal) item:
    /// its source and mutator types, who created it, and the rolled dogma attributes/effects.
    /// </summary>
    public class DynamicItem
    {
        /// <summary>The ID of the character who created the item.</summary>
        [JsonProperty("created_by")]
        public long CreatedBy { get; set; }

        /// <summary>The item's attribute values after mutation.</summary>
        [JsonProperty("dogma_attributes")]
        public List<Attribute> DogmaAttributes { get; set; } = new List<Attribute>();

        /// <summary>The item's dogma effects.</summary>
        [JsonProperty("dogma_effects")]
        public List<Effect> DogmaEffects { get; set; } = new List<Effect>();

        /// <summary>The type ID of the mutator used to generate the dynamic item.</summary>
        [JsonProperty("mutator_type_id")]
        public long MutatorTypeId { get; set; }

        /// <summary>
        /// The type ID of the source item the mutator was applied to create the dynamic item.
        /// </summary>
        [JsonProperty("source_type_id")]
        public long SourceTypeId { get; set; }
    }
}
