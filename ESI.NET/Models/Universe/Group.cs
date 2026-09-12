using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Group
    {
        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("category_id")]
        public long CategoryId { get; set; }

        [JsonProperty("types")]
        public long[] Types { get; set; }
    }
}
