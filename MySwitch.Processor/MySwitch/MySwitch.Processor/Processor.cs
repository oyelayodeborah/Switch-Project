using MySwitch.Core.Models;
using MySwitch.Data.Repositories;
using MySwitchProcessor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace MySwitchProcessor
{
    public class Processor
    {
        static void Client_RequestCancelled(object sender, PeerRequestCancelledEventArgs e)
        {
            ///
            Console.WriteLine("Peer request cancelled");
        }

        static void Client_RequestDone(object sender, PeerRequestDoneEventArgs e)
        {
            //
            Console.WriteLine("Peer request done");
        }


        public static void ClientPeer(SinkNode sinkNodeToConnect, List<ClientPeer> clientPeers)    //join conn
        {
            ClientPeer client = new ClientPeer(sinkNodeToConnect.Id.ToString(),
                                new TwoBytesNboHeaderChannel(new Iso8583Ascii1987BinaryBitmapMessageFormatter(), sinkNodeToConnect.IPAddress, Convert.ToInt32(sinkNodeToConnect.Port)), new BasicMessagesIdentifier(11, 41));
            client.RequestDone += new PeerRequestDoneEventHandler(Client_RequestDone);
            client.RequestCancelled += new PeerRequestCancelledEventHandler(Client_RequestCancelled);
            client.Connected += client_Connected;
            client.Receive += new PeerReceiveEventHandler(Client_Receive);
            clientPeers.Add(client);
            client.Connect();

            Console.WriteLine("Waiting for connection..");
        }

        static void client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Client Connected");
        }

        public static void ListenerPeer(SourceNode sourceNode)     //create conn
        {
            var port = Convert.ToInt32(sourceNode.Port);
            Console.WriteLine($"POrt is {port}");
            TcpListener tcpListener = new TcpListener(port);
            tcpListener.LocalInterface = sourceNode.IPAddress;
            Console.WriteLine($"Logging IP Address --------- {tcpListener.LocalInterface} ------- {port}");
            ListenerPeer listener = new ListenerPeer(sourceNode.Id.ToString(),
                     new TwoBytesNboHeaderChannel(new Iso8583Ascii1987BinaryBitmapMessageFormatter()),
                     new BasicMessagesIdentifier(11, 41),
                     tcpListener);
            Console.WriteLine($"Successfully set up stuff ---- {port}");
            //Program.Connects(tcpListener.LocalInterface, port);
            listener.Receive += Listener_Receive;
            listener.Connected += listener_Connected;
            
            listener.Connect();
            Console.WriteLine("Listening for connection.. on " + sourceNode.Port);
        }

        public static void listener_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Listener connected");
        }
        public static void Listener_Receive(object sender, ReceiveEventArgs e)
        {
            //MessageLogger.LogMessage("Receiving new Message...");
            Console.WriteLine("Receiving new Message...");
            var listenerPeer = sender as ListenerPeer;       //ATM
            int sourceNodeId = Convert.ToInt32(listenerPeer.Name);  //the sourceNode's ID was used as the name of the listenerPeer
            Iso8583Message theIsoMessage = e.Message as Iso8583Message;
            //Log the incoming message before validating it
            TransactionManager.LogTransaction(theIsoMessage, new SourceNodeRepository().Get(sourceNodeId));
            //MessageLogger.LogMessage("Validating incoming message...");
            Console.WriteLine("Validating incoming message...");

            Iso8583Message responseFromFEP = TransactionManager.ProcessIncommingMessage(sourceNodeId, theIsoMessage);

            //Log the response message as a transaction wether successful or not
            TransactionManager.LogTransaction(responseFromFEP, new SourceNodeRepository().Get(sourceNodeId));


            //MessageLogger.LogMessage("Sending response back to the source...");
            Console.WriteLine("Sending response back to the source...");
            SendMessage(listenerPeer, responseFromFEP);    //sends response msg back to the source, ATM in this case            
            //listenerPeer.Close();
            //listenerPeer.Dispose();
        }

        static void Client_Receive(object sender, ReceiveEventArgs e)
        {
            ClientPeer clientPeer = sender as ClientPeer;
            //logger.Log("Connected to ==> " + clientPeer.Name);

            Iso8583Message receivedMsg = e.Message as Iso8583Message;
        }
        static Message SendMessage(Peer peer, Message msg)
        {
            int maxRetries = 3; int numberOfRetries = 1;
            while (numberOfRetries < maxRetries)
            {
                if (peer.IsConnected)
                {
                    break;
                }
                peer.Close();
                numberOfRetries++;
                peer.Connect();
                Thread.Sleep(2000);
            }

            if (peer.IsConnected)
            {
                try
                {
                    var request = new PeerRequest(peer, msg);

                    request.Send();

                    //At this point, the message has been sent to the SINK for processing
                    int serverTimeout = 100000000;
                    request.WaitResponse(serverTimeout);

                    var response = request.ResponseMessage;
                    return response;
                }
                catch (Exception ex)
                {
                    msg.Fields.Add(39, "06"); // ERROR
                    MessageLogger.LogError("Error sending message " + ex.Message + "   Inner Exception:  " + ex.InnerException);
                    return msg;
                }
            }
            else
            {
                msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE); // ERROR
                return msg;
            }

        }
    }
}