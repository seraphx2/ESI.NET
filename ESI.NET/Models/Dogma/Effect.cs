using Newtonsoft.Json;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// A dogma effect as it sits on an item — its id and whether it is the default — as returned
    /// inside <c>/universe/types/{type_id}/</c> and <c>/dogma/dynamic/items/{type_id}/{item_id}/</c>.
    /// The full descriptive record is <see cref="EffectInfo"/> (<c>/dogma/effects/{effect_id}/</c>).
    /// </summary>
    public class Effect
    {
        [JsonProperty("effect_id")]
        public long EffectId { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }
    }
}
