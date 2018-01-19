using System;
using System.Collections.Generic;
using System.Threading;
using IDI.BlockChain.Common;
using IDI.BlockChain.Common.Enums;
using IDI.BlockChain.Models.Transaction;
using IDI.Core.Common;
using IDI.Core.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace IDI.BlockChain.Transaction.Client.SignalR
{
    public enum QuotationState
    {
        Closed,
        Open
    }

    public class QuotationTicker
    {
        private readonly static Lazy<QuotationTicker> instance = new Lazy<QuotationTicker>(() => new QuotationTicker(Runtime.GetService<IHubContext<QuotationHub>>().Clients));
        private readonly IHubClients clients;
        private readonly object lockOpening = new object();
        private readonly object lockUpdating = new object();
        private volatile QuotationState state;
        private Timer timer;

        public static QuotationTicker Instance => instance.Value;

        public QuotationState QuotationState
        {
            get { return state; }
            private set { state = value; }
        }

        public QuotationTicker(IHubClients clients)
        {
            this.clients = clients;
        }

        public void Open()
        {
            lock (lockOpening)
            {
                if (state != QuotationState.Open)
                {
                    var interval = TimeSpan.FromMilliseconds(Configure.QuotationUpdateInterval);

                    timer = new Timer(UpdateQuotation, null, interval, interval);

                    QuotationState = QuotationState.Open;
                }
            }
        }

        private void UpdateQuotation(object state)
        {
            lock (lockUpdating)
            {
                foreach (var symbol in Symbol.All)
                {
                    Dictionary<KLineRange, Quotation> quotations;

                    if (TryGetQuotation(symbol, out quotations))
                    {
                        BroadcastQuotation(quotations);
                    }
                }
            }
        }

        private bool TryGetQuotation(string symbol, out Dictionary<KLineRange, Quotation> quotations)
        {
            quotations = new Dictionary<KLineRange, Quotation>();

            var ranges = Enum.GetValues(typeof(KLineRange));

            foreach (KLineRange range in ranges)
            {
                var task = WebAPI.Get<Result<Quotation>>($"trans/quotation/{symbol}/{(uint)range}");

                task.Wait();

                if (task.Result.Status == ResultStatus.Success)
                {
                    quotations.Add(range, task.Result.Data);
                }
            }

            return quotations.Count > 0;
        }

        private void BroadcastQuotation(Dictionary<KLineRange, Quotation> quotations)
        {
            foreach (var kvp in quotations)
            {
                var groupName = $"kline/{kvp.Value.Symbol}/{kvp.Value.Range}";

                this.clients.Group(groupName).InvokeAsync("quotationUpdated", kvp.Value);
            }
        }
    }
}
