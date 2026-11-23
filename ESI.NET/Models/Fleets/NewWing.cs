using Newtonsoft.Json;

namespace ESI.NET.Models.Fleets
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FleetsLogic.CreateWing"/>.</summary>
    public class NewWing
    {
        /// <summary>The wing_id of the newly created wing.</summary>
        [JsonProperty("wing_id")]
        public long WingId { get; set; }
    }
}
