using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IDI.Core.Extensions;

namespace IDI.BlockChain.Transaction.Client.EndPoints
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
                string json = await client.DownloadStringTaskAsync(new Uri($"http://192.168.0.137:17528/api/{url}"));
                //string json = await client.DownloadStringTaskAsync(new Uri($"{Configure.Api}/{url}"));

                return json.To<T>();
            }
        }
    }
}
