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
using MySwitchProcessor.Utility;
using Trx.Messaging.Iso8583;


namespace MySwitchProcessor
{
    class Program
    {
        public static SchemeRepository schemeRepo = new SchemeRepository();
        public static ComboRepository comboRepo = new ComboRepository();
        public static List<Scheme> AllSchemes = schemeRepo.GetAll().ToList();
        public static List<Combo> AllCombos = comboRepo.GetAll().ToList();
        public static List<SourceNode> AllSourceNodes = new SourceNodeRepository().GetByStatus(Status.Active).ToList();   //select only the active nodes
        public static List<SinkNode> AllSinkNodes = new SinkNodeRepository().GetByStatus(Status.Active).ToList();

        public static List<ClientPeer> ClientPeers = new List<ClientPeer>();
        public static List<ListenerPeer> listenerPeers = new List<ListenerPeer>();
        //for testing purpose, sourceNode = TestSim(ATM), sinkNode = Globus
        static void Main(string[] args)
        {
            //Database.SetInitializer<ApplicationDbContext>(null);

            Console.WriteLine("Initializing all nodes");
            InitializeNodes();
            Console.ReadLine();
            Console.WriteLine("Transaction ended");
            Console.Read();
        }

        static bool testRegex(string word, string pattern)
        {
            return Regex.IsMatch(word, pattern);
        }

        public static void ConnectSinkNode(int sinkNodeId)
        {
            try
            {
                SinkNode sink = new SinkNodeRepository().Get(sinkNodeId);
                Processor.ClientPeer(sink, ClientPeers);
                sink.Status = Status.Active;
                new SinkNodeRepository().Update(sink);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ConnectSinkNode()
        {
            try
            {
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
        public static void ConnectSinkNode(Iso8583Message msg)
        {
            string acquInst = msg.Fields[MessageField.ACQUIRER_INSTITUTION_CODE_FIELD].ToString();
            var getFinInst = new FinancialInstitutionRepository().GetByInstitutionCode(acquInst);
            var sink = new SinkNodeRepository().Get(getFinInst.SinkNodeId);
            try
            {
                    Processor.ClientPeer(sink, ClientPeers);
                    sink.Status = Status.Active;
                    new SinkNodeRepository().Update(sink);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
