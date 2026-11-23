using Newtonsoft.Json;

namespace ESI.NET.Models.Wallet
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.WalletLogic.CorporationWallets"/>.
    /// </summary>
    public class Wallet
    {
        /// <summary>The wallet division's number, 1 to 7.</summary>
        [JsonProperty("division")]
        public long Division { get; set; }

        /// <summary>ISK in the wallet division.</summary>
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
    }
}
