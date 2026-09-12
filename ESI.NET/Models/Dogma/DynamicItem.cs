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
        [JsonProperty("created_by")]
        public long CreatedBy { get; set; }

        [JsonProperty("dogma_attributes")]
        public List<Attribute> DogmaAttributes { get; set; } = new List<Attribute>();

        [JsonProperty("dogma_effects")]
        public List<Effect> DogmaEffects { get; set; } = new List<Effect>();

        [JsonProperty("mutator_type_id")]
        public long MutatorTypeId { get; set; }

        [JsonProperty("source_type_id")]
        public long SourceTypeId { get; set; }
    }
}
