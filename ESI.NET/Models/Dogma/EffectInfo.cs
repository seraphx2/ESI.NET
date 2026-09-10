using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Dogma
{
    /// <summary>
    /// Full dogma effect definition from <c>/dogma/effects/{effect_id}/</c>.
    /// The id+default pair as it appears on an item is <see cref="Effect"/>.
    /// </summary>
    public class EffectInfo
    {
        [JsonProperty("effect_id")]
        public long EffectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon_id")]
        public long IconId { get; set; }

        [JsonProperty("effect_category")]
        public long EffectCategory { get; set; }

        [JsonProperty("pre_expression")]
        public long PreExpression { get; set; }

        [JsonProperty("post_expression")]
        public long PostExpression { get; set; }

        [JsonProperty("is_offensive")]
        public bool IsOffensive { get; set; }

        [JsonProperty("is_assistance")]
        public bool IsAssistance { get; set; }

        [JsonProperty("disallow_auto_repeat")]
        public bool DisallowAutoRepeat { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("is_warp_safe")]
        public bool IsWarpSafe { get; set; }

        [JsonProperty("range_chance")]
        public bool RangeChance { get; set; }

        [JsonProperty("electronic_chance")]
        public bool ElectronicChance { get; set; }

        [JsonProperty("duration_attribute_id")]
        public long DurationAttributeId { get; set; }

        [JsonProperty("tracking_speed_attribute_id")]
        public long TrackingSpeedAttributeId { get; set; }

        [JsonProperty("discharge_attribute_id")]
        public long DischargeAttributeId { get; set; }

        [JsonProperty("range_attribute_id")]
        public long RangeAttributeId { get; set; }

        [JsonProperty("falloff_attribute_id")]
        public long FalloffAttributeId { get; set; }

        [JsonProperty("modifiers")]
        public List<Modifier> Modifiers { get; set; } = new List<Modifier>();
    }

    public class Modifier
    {
        [JsonProperty("func")]
        public string Func { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("modified_attribute_id")]
        public long ModifiedAttributeId { get; set; }

        [JsonProperty("modifying_attribute_id")]
        public long ModifyingAttributeId { get; set; }

        [JsonProperty("effect_id")]
        public long EffectId { get; set; }

        [JsonProperty("operator")]
        public long Operator { get; set; }
    }
}
