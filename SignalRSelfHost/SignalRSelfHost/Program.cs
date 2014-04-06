using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Net;

namespace SignalRSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://*:8088";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }

        public bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var listener = (HttpListener)app.Properties[typeof(HttpListener).FullName];
            //listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
            var idProvider = new UserIdProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            
        }
    }

    //[AuthorizeClaims]
    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            //Clients.All.addMessage(name, message);
            //Clients.Others.Received(Newtonsoft.Json.JsonConvert.SerializeObject(new { User = name, Message = message }));
            Clients.User("wangcl").Received(Newtonsoft.Json.JsonConvert.SerializeObject(new { User = name, Message = message }));
        }
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            return base.OnDisconnected();
        }
    }
}
