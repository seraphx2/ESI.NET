using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Planet
    {
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }

        /// <summary>
        /// Only returned in /universe/planets/{planet_id}/
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Only returned in /universe/planets/{planet_id}/
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// Only returned in /universe/planets/{planet_id}/
        /// </summary>
        [JsonProperty("position")]
        public Position Position { get; set; }

        /// <summary>
        /// Only returned in /universe/planets/{planet_id}/
        /// </summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }


        /// <summary>
        /// Only returned in /universe/systems/{system_id}/
        /// </summary>
        [JsonProperty("asteroid_belts")]
        public long[] AsteroidBelts { get; set; }

        /// <summary>
        /// Only returned in /universe/systems/{system_id}/
        /// </summary>
        [JsonProperty("moons")]
        public long[] Moons { get; set; }
    }
}