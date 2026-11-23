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
        /// <summary>
        /// ID of the dogma effect. Details are in the SDE or
        /// <see cref="ESI.NET.Logic.DogmaLogic.Effect"/>.
        /// </summary>
        [JsonProperty("effect_id")]
        public long EffectId { get; set; }

        /// <summary>The effect's internal name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The name the game client shows.</summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>The effect's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>ID of the icon the game client shows for it.</summary>
        [JsonProperty("icon_id")]
        public long IconId { get; set; }

        /// <summary>
        /// The effect's category number: passive, active, targeted, area, online, overload, and so
        /// on.
        /// </summary>
        [JsonProperty("effect_category")]
        public long EffectCategory { get; set; }

        /// <summary>ID of the dogma expression evaluated when the effect starts.</summary>
        [JsonProperty("pre_expression")]
        public long PreExpression { get; set; }

        /// <summary>ID of the dogma expression evaluated when the effect stops.</summary>
        [JsonProperty("post_expression")]
        public long PostExpression { get; set; }

        /// <summary>Whether using the effect on someone is an act of aggression.</summary>
        [JsonProperty("is_offensive")]
        public bool IsOffensive { get; set; }

        /// <summary>Whether the effect helps its target (remote repair, for example).</summary>
        [JsonProperty("is_assistance")]
        public bool IsAssistance { get; set; }

        /// <summary>
        /// Whether a module with this effect cannot be set to repeat automatically.
        /// </summary>
        [JsonProperty("disallow_auto_repeat")]
        public bool DisallowAutoRepeat { get; set; }

        /// <summary>Whether it is shown in the game client.</summary>
        [JsonProperty("published")]
        public bool Published { get; set; }

        /// <summary>Whether the effect can be used while in warp.</summary>
        [JsonProperty("is_warp_safe")]
        public bool IsWarpSafe { get; set; }

        /// <summary>Whether the chance of success depends on range.</summary>
        [JsonProperty("range_chance")]
        public bool RangeChance { get; set; }

        /// <summary>Whether success is a chance roll, as for ECM.</summary>
        [JsonProperty("electronic_chance")]
        public bool ElectronicChance { get; set; }

        /// <summary>ID of the attribute that holds the cycle time.</summary>
        [JsonProperty("duration_attribute_id")]
        public long DurationAttributeId { get; set; }

        /// <summary>ID of the attribute that holds the tracking speed.</summary>
        [JsonProperty("tracking_speed_attribute_id")]
        public long TrackingSpeedAttributeId { get; set; }

        /// <summary>ID of the attribute that holds the capacitor cost per cycle.</summary>
        [JsonProperty("discharge_attribute_id")]
        public long DischargeAttributeId { get; set; }

        /// <summary>ID of the attribute that holds the optimal range.</summary>
        [JsonProperty("range_attribute_id")]
        public long RangeAttributeId { get; set; }

        /// <summary>ID of the attribute that holds the falloff range.</summary>
        [JsonProperty("falloff_attribute_id")]
        public long FalloffAttributeId { get; set; }

        /// <summary>The attribute changes the effect applies.</summary>
        [JsonProperty("modifiers")]
        public List<Modifier> Modifiers { get; set; } = new List<Modifier>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Dogma.EffectInfo.Modifiers"/>.</summary>
    public class Modifier
    {
        /// <summary>
        /// How the modifier picks the items it changes, e.g. <c>ItemModifier</c>,
        /// <c>LocationGroupModifier</c>, or <c>LocationRequiredSkillModifier</c>.
        /// </summary>
        [JsonProperty("func")]
        public string Func { get; set; }

        /// <summary>
        /// Whose attribute is changed, e.g. <c>shipID</c>, <c>charID</c>, <c>itemID</c>, or
        /// <c>targetID</c>.
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; set; }

        /// <summary>ID of the attribute that is changed.</summary>
        [JsonProperty("modified_attribute_id")]
        public long ModifiedAttributeId { get; set; }

        /// <summary>ID of the attribute whose value is applied.</summary>
        [JsonProperty("modifying_attribute_id")]
        public long ModifyingAttributeId { get; set; }

        /// <summary>
        /// ID of the effect involved, for modifiers whose <see cref="Func"/> needs one.
        /// </summary>
        [JsonProperty("effect_id")]
        public long EffectId { get; set; }

        /// <summary>
        /// How the value is applied, as a dogma operator number (add, multiply, post-percent, and
        /// so on).
        /// </summary>
        [JsonProperty("operator")]
        public long Operator { get; set; }
    }
}
