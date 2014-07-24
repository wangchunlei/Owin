using Domas.DAP.ADF.Notifier;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace OwinServiceContainer
{
    public partial class OwinService : ServiceBase
    {
        public OwinService()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("OwinSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "OwinSource", "消息日志");
            }
            eventLog1.Source = "OwinSource";
            eventLog1.Log = "消息日志";
        }

        IDisposable disposable;
        public void Start(string[] args)
        {
            this.OnStart(args);
        }
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("服务正在启动");
            var port = ConfigurationManager.AppSettings["port"];
            if (string.IsNullOrEmpty(port))
            {
                port = "8088";
            }
            var url = string.Format("http://*:{0}", port);
            var startOptions = new StartOptions(url);
            try
            {
                disposable = WebApp.Start<Startup>(startOptions);
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(ex.GetBaseException().ToString(), EventLogEntryType.Error);
                eventLog1.WriteEntry(ex.GetBaseException().StackTrace);
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                throw;
            }
            eventLog1.WriteEntry("服务启动完成");
            //var type = typeof(NotifierServer);
        }

        protected override void OnStop()
        {
            disposable.Dispose();
            eventLog1.WriteEntry("服务正常停止");
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
            //app.UseWelcomePage();
        }
    }
}
