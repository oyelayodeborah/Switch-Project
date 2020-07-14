using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BezSwitch.Utilities
{
    public class Common
    {
        public string GetSwaggerXMLDirectory()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            var xmlPath = Path.Combine(System.AppContext.BaseDirectory, xmlFile);
            return xmlPath;
        }

        public string GetRootUrl()
        {
            string address = GetRootIPAddressFromAppConfig();
            string port = GetPortFromAppConfig();
            return $"{address}:{port}/";
        }

        public string GetPortFromAppConfig()
        {
            return System.Configuration.ConfigurationManager.AppSettings["port"];
        }

        public string GetRootIPAddressFromAppConfig()
        {
            return System.Configuration.ConfigurationManager.AppSettings["baseAddress"];
        }
    }
}
