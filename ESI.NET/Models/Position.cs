using Newtonsoft.Json;

namespace ESI.NET.Models
{
    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Assets.ItemLocation.Position"/>,
    /// <see cref="ESI.NET.Models.Killmails.Victim.Position"/>, and
    /// <see cref="ESI.NET.Models.Universe.Structure.Position"/>.
    /// </summary>
    public class Position
    {
        /// <summary>Position along the X axis, in metres.</summary>
        [JsonProperty("x")]
        public double X { get; set; }

        /// <summary>Position along the Y axis, in metres.</summary>
        [JsonProperty("y")]
        public double Y { get; set; }

        /// <summary>Position along the Z axis, in metres.</summary>
        [JsonProperty("z")]
        public double Z { get; set; }
    }
}
