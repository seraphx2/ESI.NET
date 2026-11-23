using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>The EVE server a request is sent to (the <c>X-Tenant</c> header).</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum DataSource
    {
        /// <summary>The public test server.</summary>
        [EnumMember(Value = "singularity")] /**/ Singularity,
        /// <summary>The live server. The default.</summary>
        [EnumMember(Value = "tranquility")] /**/ Tranquility,
        /// <summary>The server for China, run separately by NetEase.</summary>
        [EnumMember(Value = "serenity")]    /**/ Serenity
    }
}
