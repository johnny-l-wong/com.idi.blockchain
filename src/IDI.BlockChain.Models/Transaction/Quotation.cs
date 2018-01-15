using IDI.BlockChain.Common.Enums;
using Newtonsoft.Json;

namespace IDI.BlockChain.Models.Transaction
{
    public class Quotation
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("range")]
        public KLineRange Range { get; set; }

        [JsonProperty("data")]
        public KLine KLine { get; set; } = new KLine();

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
