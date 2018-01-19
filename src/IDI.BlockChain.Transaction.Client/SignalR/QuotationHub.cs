using System.Threading.Tasks;
using IDI.BlockChain.Common.Enums;
using Microsoft.AspNetCore.SignalR;

namespace IDI.BlockChain.Transaction.Client.SignalR
{
    public class QuotationHub : Hub
    {
        private readonly QuotationTicker ticker;

        public QuotationHub() : this(QuotationTicker.Instance) { }

        public QuotationHub(QuotationTicker ticker)
        {
            this.ticker = ticker;
        }

        public void Open()
        {
            ticker.Open();
        }

        public async Task Subscribe(string symbol, KLineRange range)
        {
            var groupName = $"kline/{symbol}/{range}";

            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        public async Task Unsubscribe(string symbol, KLineRange range)
        {
            var groupName = $"kline/{symbol}/{range}";

            await Groups.RemoveAsync(Context.ConnectionId, groupName);
        }
    }
}
