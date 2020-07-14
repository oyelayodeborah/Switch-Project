using BezSwitch.Utilities;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezSwitch
{
    class Program
    {
        static void Main(string[] args)
        {
            Common _env = new Common();
            string baseAdd = _env.GetRootUrl();
            #region Prod
            //HostFactory.Run(x => {
            //    x.Service<ServiceInstaller>(p => {
            //        p.ConstructUsing(name => new ServiceInstaller());
            //        p.WhenStarted(tc => tc.Start());
            //        p.WhenStopped(tc => tc.Stop());
            //    });

            //    x.RunAsLocalSystem();
            //    x.SetDescription("BillsTransfer service self host");
            //    x.SetDisplayName("BillsTransferServiceAPI");
            //    x.SetServiceName("BillsTransferServiceAPI");
            //});

            #endregion

            #region Dev
            using (WebApp.Start<Startup>(url: baseAdd))
            {
                Console.ReadLine();
            }

            #endregion
        }
    }
}
