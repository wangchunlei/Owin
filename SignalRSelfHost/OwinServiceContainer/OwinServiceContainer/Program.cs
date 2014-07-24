using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace OwinServiceContainer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var os = new OwinService();
            if (Environment.UserInteractive)
            {
                os.Start(args);
            }
            else
            {
                ServiceBase.Run(os);
            }

        }
    }
}
