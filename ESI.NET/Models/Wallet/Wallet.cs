using Newtonsoft.Json;

namespace ESI.NET.Models.Wallet
{
    public class Wallet
    {
        [JsonProperty("division")]
        public long Division { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }
    }
}
