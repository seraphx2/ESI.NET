using Newtonsoft.Json;
    
namespace ESI.NET.Models
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.AllianceLogic.Icons"/>,
    /// <see cref="ESI.NET.Logic.CharacterLogic.Portrait"/>, and
    /// <see cref="ESI.NET.Logic.CorporationLogic.Icons"/>.
    /// </summary>
    public class Images
    {
        /// <summary>URL of the 512 × 512 pixel image.</summary>
        [JsonProperty("px512x512")]
        public string x512 { get; set; }

        /// <summary>URL of the 256 × 256 pixel image.</summary>
        [JsonProperty("px256x256")]
        public string x256 { get; set; }

        /// <summary>URL of the 128 × 128 pixel image.</summary>
        [JsonProperty("px128x128")]
        public string x128 { get; set; }

        /// <summary>URL of the 64 × 64 pixel image.</summary>
        [JsonProperty("px64x64")]
        public string x64 { get; set; }
    }
}
