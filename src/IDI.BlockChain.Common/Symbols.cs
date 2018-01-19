using System.Collections.Generic;

namespace IDI.BlockChain.Common
{
    public class Symbol
    {
        public class Key
        {
            public static string BTCUSDT = "BTC-USDT";
        }

        public static List<string> All = new List<string>
        {
            Key.BTCUSDT
        };
    }
}
