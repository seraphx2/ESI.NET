using Newtonsoft.Json;

namespace ESI.NET.Models.Fleets
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FleetsLogic.CreateSquad"/>.</summary>
    public class NewSquad
    {
        /// <summary>The squad_id of the newly created squad.</summary>
        [JsonProperty("squad_id")]
        public long SquadId { get; set; }
    }
}
