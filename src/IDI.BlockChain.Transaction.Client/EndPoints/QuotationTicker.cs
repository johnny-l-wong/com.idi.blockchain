using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDI.BlockChain.Models.Transaction;
using IDI.Core.Common;
using IDI.Core.Extensions;
using IDI.Core.Utils;
using Microsoft.AspNetCore.Sockets;

namespace IDI.BlockChain.Transaction.Client.EndPoints
{
    public enum QuotationState
    {
        Closed,
        Open
    }

    public sealed class QuotationTicker : Singleton<QuotationTicker>
    {
        //private readonly static Lazy<QuotationTicker> instance = new Lazy<QuotationTicker>(() => new QuotationTicker());
        private readonly object opening = new object();
        private readonly object pushing = new object();
        private volatile QuotationState state;
        private Timer timer;

        //public static QuotationTicker Instance => instance.Value;

        public QuotationState State
        {
            get { return state; }
            private set { state = value; }
        }

        public ConnectionList Connections { get; } = new ConnectionList();

        private QuotationTicker()
        {
            lock (opening)
            {
                if (state != QuotationState.Open)
                {
                    var interval = TimeSpan.FromMilliseconds(Configure.QuotationUpdateInterval);

                    timer = new Timer(PushQuotation, null, interval, interval);

                    State = QuotationState.Open;
                }
            }
        }

        private void PushQuotation(object state)
        {
            lock (pushing)
            {
                foreach (var group in Clients.Instance.Groups)
                {
                    if (TryGetQuotation(group, out Quotation quotation))
                    {
                        Broadcast(group, quotation);
                    }
                }
            }
        }

        private bool TryGetQuotation(Group group, out Quotation quotation)
        {
            quotation = default(Quotation);

            try
            {
                var task = WebAPI.Get<Result<Quotation>>($"trans/quotation/{group.Symbol}/{(uint)group.Range}");

                task.Wait();

                if (task.Result.Status == ResultStatus.Success)
                {
                    quotation = task.Result.Data;
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Broadcast(Group group, Quotation quotation)
        {
            var connections = Clients.Instance.Connections(group);

            if (connections.Count > 0)
                Broadcast(connections, quotation.ToJson());
        }

        private Task Broadcast(List<string> connections, string json)
        {
            return Broadcast(connections, Encoding.UTF8.GetBytes(json));
        }

        private Task Broadcast(List<string> connections, byte[] payload)
        {
            var tasks = new List<Task>(connections.Count);

            foreach (var connection in Connections)
            {
                if (connections.Contains(connection.ConnectionId))
                    tasks.Add(connection.Transport.Out.WriteAsync(payload));
            }

            return Task.WhenAll(tasks);
        }
    }
}
