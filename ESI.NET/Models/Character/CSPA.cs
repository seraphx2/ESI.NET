using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    public class CSPA
    {
        [JsonProperty("cost")]
        public decimal Cost { get; set; }
    }
}
