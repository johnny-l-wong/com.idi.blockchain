using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDI.BlockChain.Common.Enums;
using IDI.BlockChain.Models.Transaction;
using IDI.Core.Common;
using IDI.Core.Utils;

namespace IDI.BlockChain.Transaction.Client.SignalR
{
    public enum QuotationState
    {
        Open,
        Close
    }

    public sealed class QuotationTicker : Singleton<QuotationTicker>
    {
        private readonly List<string> SYMBOLS = Configure.Symbol.All;
        private readonly TimeSpan UPDATE_INTERVAL_QUOTATION = TimeSpan.FromMilliseconds(Configure.QuotationUpdateInterval);
        private readonly object LOCK_STATE = new object();
        private readonly object LOCK_QUOTATION = new object();
        private volatile QuotationState quotationState;
        private Timer timerQuotation;

        public Func<Dictionary<KLineRange, Quotation>, Task> BroadcastQuotation;

        public QuotationState QuotationState
        {
            get { return quotationState; }
            private set { quotationState = value; }
        }

        private QuotationTicker() { }

        public void Open()
        {
            lock (LOCK_STATE)
            {
                if (quotationState != QuotationState.Open)
                {
                    timerQuotation = new Timer(UpdateQuotation, null, UPDATE_INTERVAL_QUOTATION, UPDATE_INTERVAL_QUOTATION);

                    QuotationState = QuotationState.Open;

                    //BroadcastMarketStateChange(QuotationState.Open);
                }
            }
        }

        private void UpdateQuotation(object state)
        {
            lock (LOCK_QUOTATION)
            {
                foreach (var symbol in SYMBOLS)
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
                var task = WebAPI.Get<Result<Quotation>>($"{symbol}/{range}");

                task.Wait();

                if (task.Result.Status == ResultStatus.Success)
                {
                    quotations.Add(range, task.Result.Data);
                }
            }

            return false;
        }

        private bool TryUpdateQuotationCache(Quotation cache, Quotation current)
        {
            cache.KLine = current.KLine;
            cache.Symbol = current.Symbol;
            cache.Success = current.Success;

            return true;
        }
    }
}
