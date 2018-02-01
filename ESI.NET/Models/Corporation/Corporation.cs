using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    public class Corporation
    {
        [JsonProperty("corporation_id")]
        public int Id { get; set; }

        [JsonProperty("corporation_name")]
        public string Name { get; set; }
    }
}
