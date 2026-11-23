using Newtonsoft.Json;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// A dogma effect as it sits on an item — its id and whether it is the default — as returned
    /// inside <c>/dogma/dynamic/items/{type_id}/{item_id}/</c>.
    /// The full descriptive record is <see cref="EffectInfo"/> (<c>/dogma/effects/{effect_id}/</c>).
    /// </summary>
    public class Effect
    {
        /// <summary>
        /// ID of the dogma effect. Details are in the SDE or
        /// <see cref="ESI.NET.Logic.DogmaLogic.Effect"/>.
        /// </summary>
        [JsonProperty("effect_id")]
        public long EffectId { get; set; }

        /// <summary>
        /// Whether this is the type's default effect (the one a module runs when activated).
        /// </summary>
        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }
    }
}
