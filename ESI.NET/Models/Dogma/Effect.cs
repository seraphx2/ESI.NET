using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Dogma
{
    public class Effect
    {
        [JsonProperty("effect_id", Required = Required.Always)]
        public int EffectId { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }
    }
}
