using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace OwinClientApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GetRequestToken();
            Console.ReadKey(false);
        }

        private static void GetRequestToken()
        {
            var client =
                new RestClient(
                    "https://192.168.70.132:8060//api/BaseCaseCaseObjectAPI/GetSetting?code=pagecounterconfig&usercode=");
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest(Method.POST);
            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    var response = client.Execute(request);
                    Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff") + ":" + response.Content);
                });
            }
            //client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator()
        }
    }
}