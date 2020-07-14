using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BezSwitchTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = null;
            try 
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234);
                listener = new TcpListener(ep);
                listener.Start();

                Console.WriteLine(@"  
                ===================================================  
                       Started listening requests at: {0}:{1}  
                ===================================================",
                ep.Address, ep.Port);

                Byte[] bytes = new Byte[256];
                String data = null;

                // Run the loop continously; this is the server.  
                while (true)
                {
                    Console.Write("Waiting for connection...");
                    var sender = listener.AcceptTcpClient();
                    Console.WriteLine("Connected...!");
                    data = null;
                    NetworkStream stream = sender.GetStream();
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = ASCIIEncoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received from ATM source: {0}", data);
                        data = data.ToUpper();
                        // Globos Client
                        SendRequestToGlobos("127.0.0.1", 8585, data);
                    }
                    sender.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: {0}", ex);
            }
            finally
            {
                listener.Stop();
            }
            Console.WriteLine("Hit enter to continue...");
            Console.ReadKey();
        }

        static void SendRequestToGlobos(string server, int port,string message)
        {
            SendToServer(server, port, "Globos", message);
        }

        static void SendToServer(string server, int port, string name,string message)
        {
            try
            {
                //port = 8585;
                TcpClient client = new TcpClient(server, port);
                Byte[] data = ASCIIEncoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                Console.WriteLine("Sent to Server: {0}", name);
                stream.Write(data, 0, data.Length);
                data = new Byte[256];
                string responseData = string.Empty;
                int bytes = stream.Read(data, 0, data.Length);
                responseData = ASCIIEncoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Response from: {0} : {1}", name, responseData);
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("ArgumentNullException: {0}", ex);
            }
            catch (SocketException sex)
            {
                Console.WriteLine("SocketException: {0}", sex);
            }
        }
    }
}
