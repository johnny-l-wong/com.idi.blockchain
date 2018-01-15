using System.Collections.Generic;
using System.Threading.Tasks;
using IDI.BlockChain.Common.Enums;
using IDI.BlockChain.Models.Transaction;
using Microsoft.AspNetCore.SignalR;

namespace IDI.BlockChain.Transaction.Client.SignalR
{
    public class QuotationHub : Hub
    {
        public QuotationHub()
        {
            QuotationTicker.Instance.BroadcastQuotation = BroadcastQuotation;
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

        private async Task BroadcastQuotation(Dictionary<KLineRange, Quotation> quotations)
        {
            foreach (var kvp in quotations)
            {
                var groupName = $"kline/{kvp.Value.Symbol}/{kvp.Value.Range}";

                await Clients.Group(groupName).InvokeAsync("quotationUpdated", kvp.Value);
            }
        }
    }
}
