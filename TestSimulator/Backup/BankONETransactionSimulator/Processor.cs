using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trx.Messaging.Iso8583;
using Trx.Messaging;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Channels;
using System.Threading;

namespace BankONETransactionSimulator
{
    public class Processor
    {
        public Processor(string cardPAN, DateTime xpiryDate, string trxType, long amount, bool isReversal)
        {
            Trx.Messaging.Message response = null;
            string responseMsg = string.Empty;
            try
            {
                Iso8583Message requestMsg = GetMessage(cardPAN, xpiryDate, trxType, amount);
                
                if (requestMsg != null)
                {

                    if (isReversal)
                    {
                        requestMsg = GetReversalFrom(requestMsg);
                    }

                    int maxNoRetries = 3;
                    int serverTimeout = 180000;
                    ClientPeer _clientPeer = new ClientPeer(ConfigurationManager.NodeName, new TwoBytesNboHeaderChannel(
                          new Iso8583Ascii1987BinaryBitmapMessageFormatter(), ConfigurationManager.NodeIP,
                          ConfigurationManager.NodePort), new Trx.Messaging.BasicMessagesIdentifier(11));

                    _clientPeer.Connect();
                    Thread.Sleep(1000);

                    int retries = 0;
                    while (retries < maxNoRetries)
                    {
                        if (_clientPeer.IsConnected)
                        {
                            break;
                        }
                        else
                        {
                            _clientPeer.Close();
                            retries++;
                            _clientPeer.Connect();
                        }
                        Thread.Sleep(2000);
                    }


                    PeerRequest request = null;
                    if (_clientPeer.IsConnected)
                    {
                        request = new PeerRequest(_clientPeer, requestMsg);
                        request.Send();
                        request.WaitResponse(serverTimeout);

                        if (request.Expired)
                        {
                            throw new ApplicationException("Connection timeout.");
                        }
                        if (request != null)
                        {
                            response = request.ResponseMessage;
                            ResponseMessage = GetResponseMesage(response as Iso8583Message);
                        }
                        _clientPeer.Close();

                    }
                    else
                    {
                        throw new Exception("Could not connect to the server.");
                    }
                }
            }
            catch (Exception ex)
            {
                ResponseMessage = string.Format(
                            @"
                                {0}, \n
                                {1}.
                            ",
                             ex.Message,
                             ex.StackTrace
                             );
            }
        }
        public string ResponseMessage { get; set; }
        
        private Iso8583Message GetMessage(string cardPAN, DateTime xpiryDate, string trxType, long amount)
        {
            Iso8583Message echoMsg = new Iso8583Message(200);
            try
            {
                
                echoMsg.Fields.Add(2, cardPAN);
                echoMsg.Fields.Add(3, string.Format("{0}2000", trxType));
                echoMsg.Fields.Add(4, (amount * 100).ToString());
                DateTime transmissionDate = DateTime.Now;
                echoMsg.Fields.Add(7, string.Format("{0}{1}",
                    string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day),
                    string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                    transmissionDate.Minute, transmissionDate.Second)));
                echoMsg.Fields.Add(11, "123456");
                echoMsg.Fields.Add(12, string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                    transmissionDate.Minute, transmissionDate.Second));
                echoMsg.Fields.Add(13, string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day));

                echoMsg.Fields.Add(14, xpiryDate.ToString("yyMM"));
                echoMsg.Fields.Add(15, DateTime.Today.ToString("yyMM"));
                echoMsg.Fields.Add(22, "000");
                echoMsg.Fields.Add(25, "00");
                echoMsg.Fields.Add(28, "D00010000");
                echoMsg.Fields.Add(29, "D00005000");
                echoMsg.Fields.Add(30, "C00000000");
                echoMsg.Fields.Add(32, "11112222");
                echoMsg.Fields.Add(35, string.Format("{0}={1:yyMM}", cardPAN, xpiryDate));
                echoMsg.Fields.Add(37, "000019338509");
                echoMsg.Fields.Add(40, "101");
                echoMsg.Fields.Add(41, "10331931");
                echoMsg.Fields.Add(42, "CAIC10331931");
                echoMsg.Fields.Add(43, @"UTB OBALENDE LANG");
                echoMsg.Fields.Add(49, "566");
                echoMsg.Fields.Add(59, "0019338509");
                echoMsg.Fields.Add(102, ConfigurationManager.LinkedAccount);
                echoMsg.Fields.Add(103, "9876543234567878");
                echoMsg.Fields.Add(90, "020012345610100103230001111222200000000000");

                Message inner = new Message();
                inner.Fields.Add(3, "ATMsrc      PRUICCsnk   000119000119ICCGroup    ");
                inner.Fields.Add(20, DateTime.Today.ToString("yyMMdd"));

                echoMsg.Fields.Add(127, inner);
            }
            catch (Exception ex)
            {
                ResponseMessage = string.Format(
                            @"
                                {0}, \n
                                {1}.
                            ",
                             ex.Message,
                             ex.StackTrace
                             );
            }
            return echoMsg;
        }

        private Iso8583Message GetReversalFrom(Iso8583Message originalMsg)
        {
            originalMsg.MessageTypeIdentifier = 421;
            return originalMsg;
        }

        private string GetResponseMesage(Iso8583Message response)
        {
            string reply = string.Empty;
            string trxType = string.Empty;
            try
            {
                switch (response.Fields[3].ToString().Substring(0, 2))
                {
                    case "31":
                        trxType = "Balance Enquiry";
                        break;
                    case "01":
                        trxType = "Cash Withdrawal";
                        break;

                }

                reply = string.Format(
                    @"
                    Card PAN: {0}, 
                    Transaction Type: {1}, 
                    Transaction Amount {2}, 
                    Balance {3}, 
                    Transaction Date: {4},
                    Response Code: {5}.
                 ",
                    response.Fields[2] == null ? string.Empty : response.Fields[2].ToString(),
                    trxType,
                    response.Fields[4] == null ? string.Empty : response.Fields[4].ToString(),
                    response.Fields[54] == null ? string.Empty : response.Fields[54].ToString(),
                    response.Fields[7] == null ? string.Empty : response.Fields[7].ToString(),
                     response.Fields[39] == null ? string.Empty : response.Fields[39].ToString()
                    );
            }
            catch (Exception ex)
            {
                ResponseMessage = string.Format(
                            @"
                                {0}, \n
                                {1}.
                            ",
                             ex.Message,
                             ex.StackTrace
                             );
            }

            return reply;
        }
    }

    
}
