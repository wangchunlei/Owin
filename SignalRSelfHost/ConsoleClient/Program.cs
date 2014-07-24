using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:8088";
            var querystringData = new Dictionary<string, string>();
            //querystringData.Add("test", "11123");

            var hubConnection = new HubConnection(url, querystringData);
            hubConnection.Credentials = CredentialCache.DefaultCredentials;

            Console.Write("连接服务器...，请输入用户名：");
            var user = Console.ReadLine();

            hubConnection.Headers.Add("UserID", user);
            //hubConnection.AddClientCertificate(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(""));

            var hubProxy = hubConnection.CreateHubProxy("NotifierServer");
            hubProxy.On<string>("Received", message =>
            {
                if (!string.IsNullOrEmpty(message))
                {
                    dynamic info = Newtonsoft.Json.JsonConvert.DeserializeObject(message);
                    Console.WriteLine(string.Format("{0}:{1}", info.User, info.Message));
                }
            });
            ServicePointManager.DefaultConnectionLimit = 10;
            hubConnection.Start().Wait();
            Console.WriteLine("连接服务器成功，可以开始聊天了");
            while (true)
            {
                var input = Console.ReadLine();
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                hubProxy.Invoke("Send", user, input);
            }

        }
    }
}
