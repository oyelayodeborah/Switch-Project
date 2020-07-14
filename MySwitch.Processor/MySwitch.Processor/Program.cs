using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx.Messaging.FlowControl;
using MySwitch.Data.Repositories;
using MySwitch.Core.Models;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Net.Sockets;
using System.Net;

namespace MySwitch.Processor
{
    class Program
    {
        public static SchemeRepository schemeRepo = new SchemeRepository();
        public static List<Scheme> AllSchemes = schemeRepo.GetAll().ToList();
        public static List<SourceNode> AllSourceNodes = new SourceNodeRepository().GetByStatus(Status.Active).ToList();   //select only the active nodes
        public static List<SinkNode> AllSinkNodes = new SinkNodeRepository().GetByStatus(Status.Active).ToList();
        

        public static List<ClientPeer> ClientPeers = new List<ClientPeer>();
        //for testing purpose, sourceNode = TestSim(ATM), sinkNode = Globus
        static void Main(string[] args)
        {
            //Database.SetInitializer<ApplicationDbContext>(null);

            Console.WriteLine("Initializing all nodes");
            InitializeNodes();
            Console.Read();
        }

        static bool testRegex(string word, string pattern)
        {
            return Regex.IsMatch(word, pattern);
        }

        public static void Connects(string ipAddress, int port)
        {
            //TcpClient tcpClient = new TcpClient();
            //tcpClient.Connect(ipAddress, port);
            SocketPermission permission = new SocketPermission(NetworkAccess.Connect,
                   TransportType.Tcp, ipAddress, port);
            Console.WriteLine(permission.IsUnrestricted());
            Console.WriteLine("Permission settled");
            Socket s = new Socket(AddressFamily.InterNetwork,
        SocketType.Stream,
        ProtocolType.Tcp);

            Console.WriteLine("Establishing Connection to {0}",
                ipAddress);
            s.Connect(ipAddress, port);
            Console.WriteLine("Connection established");
        }

        private static void InitializeNodes()
        {
            try
            {
                foreach (var source in AllSourceNodes)
                {
                    Console.WriteLine("About to call ListenerPeer method....");
                    Processor.ListenerPeer(source);
                    source.Status = Status.Active;
                    new SourceNodeRepository().Update(source);
                }
                foreach (var sink in AllSinkNodes)
                {
                    Processor.ClientPeer(sink, ClientPeers);
                    sink.Status = Status.Active;
                    new SinkNodeRepository().Update(sink);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
