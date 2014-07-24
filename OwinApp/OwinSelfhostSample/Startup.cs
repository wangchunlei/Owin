using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using OwinSelfhostSample.Extensions;

namespace OwinSelfhostSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            //GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new AssembliesResolver());
            //Configure Web API for self-host
            var config = new HttpConfiguration();

            config.Services.Replace(typeof(IAssembliesResolver), new AssembliesResolver());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            appBuilder.UseAuthentication();
            appBuilder.UseWebApi(config);
        }
    }
}
