using System.Text;
using System.Threading.Tasks;
using IDI.BlockChain.Models.Transaction;
using IDI.Core.Extensions;
using Microsoft.AspNetCore.Sockets;

namespace IDI.BlockChain.Transaction.Client.EndPoints
{
    public class QuotationEndPoint : EndPoint
    {
        //public ConnectionList Connections { get; } = new ConnectionList();

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            QuotationTicker.Instance.Connections.Add(connection);
            //await Broadcast($"{connection.ConnectionId} connected ({connection.Metadata[ConnectionMetadataNames.Transport]})");

            try
            {
                while (await connection.Transport.In.WaitToReadAsync())
                {
                    if (connection.Transport.In.TryRead(out var buffer))
                    {
                        // We can avoid the copy here but we'll deal with that later
                        var group = Encoding.UTF8.GetString(buffer).To<Group>();

                        if (group != null)
                        {
                            Clients.Instance.Join(group, connection.ConnectionId);
                        }
                        //text = $"{connection.ConnectionId}: {text}";
                        //await Broadcast(Encoding.UTF8.GetBytes(text));
                    }
                }
            }
            finally
            {
                QuotationTicker.Instance.Connections.Remove(connection);
                Clients.Instance.Remove(connection.ConnectionId);
                //await Broadcast($"{connection.ConnectionId} disconnected ({connection.Metadata[ConnectionMetadataNames.Transport]})");
            }
        }

        //private Task Broadcast(string text)
        //{
        //    return Broadcast(Encoding.UTF8.GetBytes(text));
        //}

        //private Task Broadcast(byte[] payload)
        //{
        //    var tasks = new List<Task>(Connections.Count);

        //    foreach (var c in Connections)
        //    {
        //        tasks.Add(c.Transport.Out.WriteAsync(payload));
        //    }

        //    return Task.WhenAll(tasks);
        //}
    }
}
