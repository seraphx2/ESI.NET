using Newtonsoft.Json;

namespace ESI.NET.Models.Alliance
{
    public class Alliance
    {
        [JsonProperty("alliance_id")]
        public int Id { get; set; }

        [JsonProperty("alliance_name")]
        public string Name { get; set; }
    }
}
