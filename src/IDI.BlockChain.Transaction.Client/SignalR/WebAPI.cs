using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IDI.Core.Extensions;

namespace IDI.BlockChain.Transaction.Client.SignalR
{
    public class WebAPI
    {
        public static async Task<T> Get<T>(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers["Type"] = "GET";
                client.Headers["Accept"] = "application/json";
                client.Encoding = Encoding.UTF8;
                //client.DownloadStringCompleted += (sender, e) =>
                //{
                //    var obj = e.Result;
                //};
                string json = await client.DownloadStringTaskAsync(new Uri($"{Configure.Api}/{url}"));

                return json.To<T>();
            }
        }
    }
}
