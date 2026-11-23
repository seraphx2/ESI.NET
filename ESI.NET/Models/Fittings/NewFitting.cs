using Newtonsoft.Json;

namespace ESI.NET.Models.Fittings
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FittingsLogic.Add"/>.</summary>
    public class NewFitting
    {
        /// <summary>ID of the fitting that was created.</summary>
        [JsonProperty("fitting_id")]
        public long FittingId { get; set; }
    }
}
