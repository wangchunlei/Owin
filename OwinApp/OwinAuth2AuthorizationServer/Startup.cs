using Microsoft.Owin;
using Owin;
using OwinOAuth2AuthorizationServer;

[assembly: OwinStartup(typeof(Startup))]

namespace OwinOAuth2AuthorizationServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
