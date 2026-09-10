using Newtonsoft.Json;

namespace ESI.NET.Models.Routes
{
    /// <summary>Result of <see cref="ESI.NET.Logic.RoutesLogic.Map"/> — the ordered solar-system ids from origin to destination.</summary>
    public class RouteResult
    {
        [JsonProperty("route")]
        public long[] Route { get; set; }
    }
}
