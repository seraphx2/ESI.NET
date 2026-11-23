using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>The kind of a contract.</summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ContractType
    {
        /// <summary>A type ESI.NET does not know.</summary>
        [EnumMember(Value = "unknown")]  /**/ Unknown,
        /// <summary>Items (and/or ISK) for items (and/or ISK).</summary>
        [EnumMember(Value = "item_exchange")]  /**/ ItemExchange,
        /// <summary>Items sold to the highest bidder.</summary>
        [EnumMember(Value = "auction")] /**/ Auction,
        /// <summary>Items to be hauled to a destination for a reward.</summary>
        [EnumMember(Value = "courier")] /**/ Courier,
        /// <summary>A loan.</summary>
        [EnumMember(Value = "loan")] /**/ Loan
    }
}
