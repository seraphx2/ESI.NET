using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    public class Cspa
    {
        [JsonProperty("cost")]
        public decimal Cost { get; set; }
    }
}
