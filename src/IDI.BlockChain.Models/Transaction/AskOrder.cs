﻿using IDI.BlockChain.Common.Enums;
using IDI.BlockChain.Models.Base;

namespace IDI.BlockChain.Models.Transaction
{
    /// <summary>
    /// 交易应单
    /// </summary>
    public class AskOrder : TradeOrder
    {
        public AskOrder(int uid, decimal price, decimal size) : base(uid, price, size, TranType.Ask) { }
    }
}
