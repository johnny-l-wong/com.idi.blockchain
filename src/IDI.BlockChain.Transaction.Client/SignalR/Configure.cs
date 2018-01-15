﻿using System.Collections.Generic;

namespace IDI.BlockChain.Transaction.Client.SignalR
{
    public class Configure
    {
        public const int QuotationUpdateInterval = 1000;

        public const string Api = "http://localhost:17528/api";

        public static class Symbol
        {
            public static List<string> All = new List<string> { "btc/usdt" };
        }
    }
}
