using IDI.BlockChain.Models.Base;

namespace IDI.BlockChain.Domain.Transaction
{
    public static class Extensions
    {
        public static decimal Remain(this TradeOrder order)
        {
            return order.Size - order.Volume;
        }
    }
}
