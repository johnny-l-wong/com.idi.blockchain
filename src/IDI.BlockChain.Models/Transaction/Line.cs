using System;
using IDI.BlockChain.Common.Enums;

namespace IDI.BlockChain.Models.Transaction
{
    public class Line
    {
        public KLineRange Range { get; set; }

        public DateTime TimeScale { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }
    }
}
