using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static void GetRequestToken()
        {
            var client = new RestClient("http://localhost:8003/");
            
            //client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator()
        }
    }
}
