using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankONETransactionSimulator
{
    public class ConfigurationManager
    {
        public static string NodeName 
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["NodeName"];
            }
        }
        public static string NodeIP
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["NodeIP"];
            }
        }
        public static int NodePort
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["NodePort"]);
            }
        }
        public static string LinkedAccount
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["LinkedAccount"];
            }
        }

    }
}
