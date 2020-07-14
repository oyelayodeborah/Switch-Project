using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezSwitch
{
    public class ServiceInstaller
    {
        static string GetRootUrlFromAppConfig()
        {
            return System.Configuration.ConfigurationManager.AppSettings["baseAddress"];
        }
        IDisposable webServer;
        private string baseAddress = GetRootUrlFromAppConfig();

        public ServiceInstaller()
        {
        }

        public void Start()
        {
            this.webServer = WebApp.Start<Startup>(url: baseAddress);
        }

        public void Stop()
        {
            this.webServer.Dispose();
        }

    }
}
