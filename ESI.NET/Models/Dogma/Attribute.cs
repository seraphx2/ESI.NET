using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Dogma
{
    public class Attribute
    {
        [JsonProperty("attribute_id", Required = Required.Always)]
        public int AttributeId { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }
}
