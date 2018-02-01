using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Alliance
{
    public class Information
    {
        [JsonProperty("alliance_name")]
        public string Name { get; set; }

        [JsonProperty("date_founded")]
        public DateTime DateFounded { get; set; }

        [JsonProperty("executor_corp")]
        public long ExecutorCorporation { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }
    }
}
