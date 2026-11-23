using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>Which side of the market to list orders for.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum MarketOrderType
    {
        /// <summary>Buy and sell orders.</summary>
        [EnumMember(Value="all")]  /**/ All,
        /// <summary>Buy orders only.</summary>
        [EnumMember(Value="buy")]  /**/ Buy,
        /// <summary>Sell orders only.</summary>
        [EnumMember(Value="sell")] /**/ Sell
    }
}
