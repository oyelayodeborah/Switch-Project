﻿using MySwitch.Core.Models;
using MySwitch.Data.Repositories;
using MySwitchProcessor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx.Messaging;
using Trx.Messaging.Iso8583;
using Trx.Messaging.FlowControl;
using System.Threading;

namespace MySwitchProcessor
{
    public class TransactionManager
    {
        static List<Scheme> allSchemes = Program.AllSchemes;
        public static Iso8583Message ProcessIncommingMessage(int sourceId, Iso8583Message originalMessage)
        {
            try
            {
                string InstitutionCode1 = Convert.ToString(originalMessage.Fields[32]);
                string InstitutionCode2 = Convert.ToString(originalMessage.Fields[33]);
                var getFinInst = new FinancialInstitutionRepository().GetByInstitutionCode(InstitutionCode1);
                var sink = new SinkNodeRepository().Get(getFinInst.SinkNodeId);
                string transactionTypeCode = originalMessage.Fields[MessageField.TRANSACTION_TYPE_FIELD].ToString().Substring(0, 2);
                Iso8583Message replyFromFEP = null;
                ClientPeer clientPeer = null;
                if (transactionTypeCode.Equals(TransactionTypeCode.INTER_BANK_TRANSFER) && InstitutionCode1==null && InstitutionCode2==null)
                {
                        originalMessage.Fields.Add(39, ResponseCode.INVALID_TRANSACTION);
                        return originalMessage;
                }
                if (transactionTypeCode!=null && InstitutionCode1==null)
                {
                        originalMessage.Fields.Add(39, ResponseCode.INVALID_TRANSACTION);
                        return originalMessage;
                }
                SourceNode sourceNode = new SourceNodeRepository().Get(sourceId);

                Processor.GetFinancialInstitution(originalMessage);

                //Configure Original Data Element
                DateTime transmissionDate = DateTime.UtcNow;

                //Configuring the original data element (intended for transaction matching (e.g. to identify a transaction for correction or reversal)
                //consists of 5 data elements: MTI (1-4), STAN (5-10), Date&Time (11-20), AcquirerInsttutionCode (21-31), ForwardingInstCode(32-42)
                string transactionDate = string.Format("{0}{1}",
                         string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day),
                         string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                         transmissionDate.Minute, transmissionDate.Second));

                //string originalDataElement = $"{originalMessage.MessageTypeIdentifier}{originalMessage.Fields[11]}{transactionDate}";
                string originalDataElement = originalMessage.MessageTypeIdentifier.ToString() + originalMessage.Fields[11] + transactionDate;
                //if the originalDataElement is empty (for non-reversal), add it
                if (!(originalMessage.Fields.Contains(MessageField.ORIGINAL_DATA_ELEMENT_FIELD)))
                {
                    originalMessage.Fields.Add(MessageField.ORIGINAL_DATA_ELEMENT_FIELD, originalDataElement);
                }
                MessageLogger.LogMessage("Validating message...");

                //check if it's a reversal message
                if (originalMessage.MessageTypeIdentifier == MTI.REVERSAL_ADVICE || originalMessage.MessageTypeIdentifier == MTI.REPEAT_REVERSAL_ADVICE)
                {
                    MessageLogger.LogMessage("\nReversal message!");
                    //confirm that this transaction actually needs to be reversed 
                    bool isReversal;
                    Iso8583Message reversalMessage = GetReversalMessage(originalMessage, out isReversal);

                    if (!isReversal)
                    {
                        MessageLogger.LogMessage("Invalid Reversal transaction");
                        return reversalMessage;
                    }
                    //continue if it's a reversal message
                    originalMessage = reversalMessage;
                }

                string cardPan = originalMessage.Fields[MessageField.CARD_PAN_FIELD].ToString();
                decimal amount = Convert.ToDecimal(originalMessage[MessageField.AMOUNT_FIELD].Value);      //The amount is in kobo
                string channelCode = originalMessage.Fields[MessageField.CHANNEL_ID_FIELD].Value.ToString().Substring(0, 2);
                string tTypeCode = originalMessage.Fields[MessageField.TRANSACTION_TYPE_FIELD].Value.ToString().Substring(0, 2);    //first two positions for the transaction type
                string expiryDate = originalMessage.Fields[MessageField.EXPIRY_DATE_FIELD].Value.ToString();
                DateTime expiryDateOfCard = ConvertToDateTime(expiryDate);
                Iso8583Message responseMessage;     //rsponse message to return after performing some checks

                if (expiryDateOfCard <= DateTime.Now)    //card expired
                {
                    responseMessage = SetReponseMessage(originalMessage, ResponseCode.EXPIRED_CARD.ToString());
                    MessageLogger.LogMessage("Expired card");
                    return responseMessage;
                }
                //check the amount, only for balance enq should the amount be zero
                if (amount <= 0 && tTypeCode != TransactionTypeCode.BALANCE_ENQUIRY.ToString())
                {
                    responseMessage = SetReponseMessage(originalMessage, ResponseCode.INVALID_AMOUNT.ToString());         //Invalid amount
                    MessageLogger.LogMessage("Invalid Amount");
                    return responseMessage;
                }

                Combo theCombo = null;
                Scheme theScheme = null;
                sourceNode.Schemes = Program.AllSchemes;
                var sourceNodeSchemes = sourceNode.Schemes;
                string pan = originalMessage.Fields[MessageField.CARD_PAN_FIELD].Value.ToString();
                //var route = new RouteRepository().GetByCardPAN(pan.Substring(0, 6));
                var route = new RouteRepository().GetByCardPAN(pan);
                route = new RouteRepository().GetBySinkNodeId(sink.Id);
                if (route == null)
                {
                    MessageLogger.LogMessage("Route is null.");
                    responseMessage = SetReponseMessage(originalMessage, ResponseCode.ROUTING_ERROR.ToString());
                    return responseMessage;
                }
                var routeId = route.FirstOrDefault().Id;
                var routess = routeId;
                //get the sourcenode by the route
                theScheme = sourceNodeSchemes.Where(s => s.RouteId == routeId).FirstOrDefault();
                theScheme.Combos = Program.AllCombos;

                if (theScheme == null)
                {
                    MessageLogger.LogMessage("No such scheme");
                    responseMessage = SetReponseMessage(originalMessage, ResponseCode.TRANSACTION_NOT_PERMITTED_ON_TERMINAL.ToString());
                    return responseMessage;
                }
                var trantypeId = new TransactionTypeRepository().GetByCode(tTypeCode);
                var chanId = new ChannelRepository().GetByCode(channelCode);
                //theCombo = theScheme.Combos.Where(c => c.TransactionType.Code.Equals(tTypeCode) && c.Channel.Code.Equals(channelCode)).FirstOrDefault();
                theCombo = theScheme.Combos.Where(c => c.TransactionTypeId.Equals(trantypeId.Id) && c.ChannelId.Equals(chanId.Id)).FirstOrDefault();
                var good = theCombo;
                if (theCombo == null)   //of course, scheme also = null
                {
                    MessageLogger.LogMessage("Invalid transaction type-channel-fee combo");
                    responseMessage = SetReponseMessage(originalMessage, ResponseCode.TRANSACTION_NOT_PERMITTED_ON_TERMINAL.ToString()); // Transaction not allowed on terminal                 
                    return responseMessage;
                }

                var getSinkNodeId = route.FirstOrDefault().SinkNodeId;

                var sinkNnode = new SinkNodeRepository().Get(getSinkNodeId);
                if (sinkNnode == null)
                {
                    MessageLogger.LogMessage("Sink node is null.");
                    responseMessage = SetReponseMessage(originalMessage, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE.ToString());
                    return responseMessage;
                }

                //At this point, everything is assumed to be okay
                var getFeeId = theCombo.FeeId;
                Fee fee = new FeeRepository().Get(getFeeId);
                if (fee == null)
                {
                    MessageLogger.LogMessage("Unacceptable transaction fee");
                    responseMessage = SetReponseMessage(originalMessage, ResponseCode.UNACCEPTABLE_TRANSACTION_FEE.ToString());  // Transaction type not allowed in this scheme                
                    return responseMessage;
                }

                //calculate the transaction fee and embed in the  message
                originalMessage = SetTransactionFee(originalMessage, ComputeFee(fee, amount));


                clientPeer = Program.ClientPeers.Where(p => p.Name.Equals(theScheme.Route.SinkNodeId.ToString())).FirstOrDefault();
                if (clientPeer == null)
                {
                    MessageLogger.LogMessage("Clientpeer is null");
                    originalMessage = SetReponseMessage(originalMessage, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE.ToString());
                    return originalMessage;
                }
                ////////
                
            replyFromFEP = SendMessageToFEP(clientPeer, originalMessage);        //sends message to the client                    
            return replyFromFEP;
            }
            catch (Exception ex)
            {
                MessageLogger.LogError("Error ocurred while processing the incoming message");
                originalMessage.Fields.Add(39, ResponseCode.INVALID_TRANSACTION);
                return originalMessage;
            }
        }

        public static Iso8583Message SendMessageToFEP(Peer peer, Iso8583Message msg)
        {
            if (msg == null)
            {
                MessageLogger.LogMessage("iso Message is null");
                SetReponseMessage(msg, ResponseCode.INVALID_RESPONSE.ToString());
            }

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
                    int serverTimeout = 60000000;          //60 secs
                    request.WaitResponse(serverTimeout);
                    if (request.Expired)
                    {
                        MessageLogger.LogMessage("Connection timeout.");
                        return SetReponseMessage(msg, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE); //Response received too late
                    }

                    var response = request.ResponseMessage;
                    return response as Iso8583Message;
                }
                catch (Exception ex)
                {
                    MessageLogger.LogError("Error sending message: " + ex.Message);
                    return SetReponseMessage(msg, ResponseCode.ERROR);
                }
            }
            else
            {
                MessageLogger.LogMessage("\n Could not connect to the Sink Node.");
                return SetReponseMessage(msg, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE.ToString()); //Issuer inoperative
            }
        }

        static decimal ComputeFee(Fee fee, decimal transactionAmount)
        {
            decimal charge = 0;
            var PercentageOfTransaction = (Convert.ToInt32(fee.PercentOfTransaction));
            var Maximum = (Convert.ToInt32(fee.Maximum));
            var Minimum = (Convert.ToInt32(fee.Minimum));
            var FlatAmmount = (Convert.ToDecimal(fee.FlatAmount));

            if (PercentageOfTransaction == 0)   //this means a flat amount is set
            {
                charge = FlatAmmount;
            }
            else
            {
                double percentage = PercentageOfTransaction;
                charge = ((decimal)percentage * transactionAmount) > Maximum ? Maximum : (((decimal)percentage * transactionAmount) < Minimum ? Minimum : (decimal)percentage * transactionAmount);
            }
            return charge * 100;    //converts charge in Naira to kobo
        }

        static Iso8583Message SetTransactionFee(Iso8583Message msg, decimal charge)
        {
            msg.Fields.Add(MessageField.TRANSACTION_FEE_FIELD, Math.Round(charge, 2).ToString());
            return msg;
        }

        static DateTime ConvertToDateTime(string dateString)
        {
            string year = dateString.Substring(0, 2);
            string month = dateString.Substring(2, 2);

            string expiry = month + "-01-" + year;
            DateTime expiryDate = DateTime.Now;
            if (DateTime.TryParse(expiry, out expiryDate))
            {
                return expiryDate;
            }
            return expiryDate;
        }

        public static void LogTransaction(Iso8583Message msg, SourceNode srcNode = null, bool needReversal = false)
        {
            var transactionTypeCode = msg.Fields[MessageField.TRANSACTION_TYPE_FIELD].ToString().Substring(0, 2);
            var channelCode = msg.Fields[MessageField.CHANNEL_ID_FIELD].ToString().Substring(0, 1);
            var cardPan = msg.Fields[MessageField.CARD_PAN_FIELD].ToString();
            string responseCode = string.Empty;             //the response to send back is not yet known
            var transmissionDate = DateTime.UtcNow;
            string originalDataElement = msg.Fields.Contains(MessageField.ORIGINAL_DATA_ELEMENT_FIELD) ? msg.Fields[MessageField.ORIGINAL_DATA_ELEMENT_FIELD].ToString() : string.Empty;
            //int originalDataElementLength = originalDataElement.Length;

            try
            {
                Transaction transactionToLog = new Transaction();
                transactionToLog.MTI = msg.MessageTypeIdentifier.ToString();
                transactionToLog.CardPAN = msg.Fields[MessageField.CARD_PAN_FIELD].ToString();
                transactionToLog.STAN = msg.Fields[MessageField.STAN_FIELD].ToString();
                transactionToLog.Amount = Convert.ToDecimal(msg.Fields[MessageField.AMOUNT_FIELD].ToString());
                transactionToLog.ChannelCode = msg.Fields[MessageField.CHANNEL_ID_FIELD].ToString();
                transactionToLog.ChannelCode = channelCode;
                transactionToLog.TransactionTypeCode = transactionTypeCode;
                transactionToLog.SourceNodeId = srcNode.Id;
                transactionToLog.Date = transmissionDate;
                transactionToLog.Acquirer = msg.Fields[MessageField.ACQUIRER_INSTITUTION_CODE_FIELD].ToString();
                if (msg.Fields[MessageField.ISSUER_INSTITUTION_CODE_FIELD].ToString() != null && msg.Fields[MessageField.ISSUER_INSTITUTION_CODE_FIELD].ToString() != "" 
                    && !transactionTypeCode.Equals( TransactionTypeCode.INTRA_BANK_TRANSFER))
                {
                    transactionToLog.Issuer = msg.Fields[MessageField.ISSUER_INSTITUTION_CODE_FIELD].ToString();
                }
                else
                {
                    transactionToLog.Issuer = msg.Fields[MessageField.ACQUIRER_INSTITUTION_CODE_FIELD].ToString();
                }
                

                if (msg.Fields.Contains(MessageField.TRANSACTION_FEE_FIELD))
                {
                    //transactionToLog.TransactionFee = Convert.ToDecimal(msg.Fields[MessageField.TRANSACTION_FEE_FIELD].ToString());
                    transactionToLog.TransactionFee = msg.Fields[MessageField.TRANSACTION_FEE_FIELD].ToString();
                }

                if (msg.Fields.Contains(MessageField.PROCESSING_FEE_FIELD))
                {
                    //transactionToLog.ProcessingFee = Convert.ToDecimal(msg.Fields[MessageField.PROCESSING_FEE_FIELD].ToString());
                    transactionToLog.ProcessingFee = msg.Fields[MessageField.PROCESSING_FEE_FIELD].ToString();
               }


                if (msg.Fields.Contains(MessageField.RESPONSE_FIELD))
                {
                    responseCode = msg.Fields[MessageField.RESPONSE_FIELD].Value.ToString();

                    transactionToLog.ResponseCode = responseCode;
                    transactionToLog.ResponseDescription = MessageDefinition.GetResponseDescription(responseCode);
                    if (responseCode == "")
                    {
                        transactionToLog.ResponseCode = "00";
                        transactionToLog.ResponseDescription = "Successful";
                    }
                    
                }
                transactionToLog.Account1 = msg.Fields[MessageField.FROM_ACCOUNT_ID_FIELD].ToString();
                transactionToLog.Account2 = msg.Fields[MessageField.TO_ACCOUNT_ID_FIELD].ToString();
                transactionToLog.IsReversePending = needReversal;

                if (msg.MessageTypeIdentifier == MTI.REVERSAL_ADVICE_RESPONSE && responseCode == "00")   //successful reversal
                {
                    transactionToLog.IsReversed = true;
                }
                try
                {
                    var originalDataElementt = originalDataElement.TrimStart('0');
                    var regenerateOriginalDataElement = originalDataElementt;
                    regenerateOriginalDataElement = regenerateOriginalDataElement.Substring(0, 3);
                    if (transactionToLog.MTI == "200" && regenerateOriginalDataElement == "200")
                    {
                        transactionToLog.OriginalDataElement = originalDataElementt;      //removes all leading zeros
                    }
                    else if (transactionToLog.MTI == "210")
                    {
                        regenerateOriginalDataElement = originalDataElementt.Remove(0, 3);
                        transactionToLog.OriginalDataElement = transactionToLog.MTI + regenerateOriginalDataElement;
                    }
                    else if (transactionToLog.MTI == "420" || transactionToLog.MTI == "421" || transactionToLog.MTI == "430")
                    {
                        transactionToLog.OriginalDataElement = originalDataElementt + "R";
                    }
                    var isReversed = transactionToLog.IsReversed;
                    var getResponseCode = transactionToLog.ResponseCode;
                    if (isReversed==true && getResponseCode == "00")
                    {
                        var getTransaction = new TransactionRepository().GetByOriginalDataElement(originalDataElement);

                        Transaction updateTransaction = new Transaction();
                        updateTransaction = new TransactionRepository().Get(getTransaction.Id);
                        updateTransaction.IsReversed = true;
                        new TransactionRepository().Update(updateTransaction);
                    }
                    new TransactionRepository().Save(transactionToLog);
                    

                    MessageLogger.LogMessage("Transaction logged! -> ODE= " + transactionToLog.OriginalDataElement + " Amount: " + transactionToLog.Amount + " Date: " + transactionToLog.Date);
                }
                catch (Exception ex)
                {
                    MessageLogger.LogError("Transaction log failed! " + ex.Message + "   Inner Exception:  " + ex.InnerException);
                }
            }
            catch (Exception ex)
            {
                MessageLogger.LogError("Transaction log failed " + ex.Message + "   Inner Exception:  " + ex.InnerException);
            }
        }

        private static Iso8583Message GetReversalMessage(Iso8583Message isoMsg, out bool isReversal)
        {
            isReversal = true;
            string originalDataElement;
            try
            {
                originalDataElement = isoMsg.Fields[MessageField.ORIGINAL_DATA_ELEMENT_FIELD].ToString().TrimStart('0');
            }
            catch (Exception ex)
            {
                MessageLogger.LogError("Original data element is empty1 " + ex.Message + "   Inner Exception:  " + ex.InnerException);
                isReversal = false;
                SetReponseMessage(isoMsg, ResponseCode.INVALID_TRANSACTION);
                isoMsg.MessageTypeIdentifier = MTI.REVERSAL_ADVICE_RESPONSE;
                return isoMsg;
            }

            Transaction transaction = new TransactionRepository().GetByOriginalDataElement(originalDataElement);
            MessageLogger.LogMessage("Original Data Element: " + originalDataElement);
            if (transaction == null)
            {
                MessageLogger.LogMessage("\n Transaction log not found");
                isReversal = false;
                SetReponseMessage(isoMsg, ResponseCode.UNABLE_TO_LOCATE_RECORD.ToString());
                isoMsg.MessageTypeIdentifier = MTI.REVERSAL_ADVICE_RESPONSE;
                return isoMsg;
            }
            if (transaction.IsReversed)
            {
                MessageLogger.LogMessage("\n Transaction has already been reversed");
                isReversal = false;
                SetReponseMessage(isoMsg, ResponseCode.DUPLICATE_TRANSACTION.ToString());
                return isoMsg;
            }
            
            MessageLogger.LogMessage("\n Continue with reversal");
            isoMsg.Fields.Add(MessageField.FROM_ACCOUNT_ID_FIELD, transaction.Account1);
            isoMsg.Fields.Add(MessageField.TO_ACCOUNT_ID_FIELD, transaction.Account2);
            isoMsg.Fields.Add(MessageField.AMOUNT_FIELD, transaction.Amount.ToString());
            isoMsg.Fields.Add(MessageField.TRANSACTION_FEE_FIELD, transaction.TransactionFee.ToString());
            isoMsg.Fields.Add(MessageField.PROCESSING_FEE_FIELD, transaction.ProcessingFee.ToString());
            isoMsg.Fields.Add(MessageField.ISSUER_INSTITUTION_CODE_FIELD, transaction.Issuer.ToString());
            isoMsg.Fields.Add(MessageField.ACQUIRER_INSTITUTION_CODE_FIELD, transaction.Acquirer.ToString());
            //get the transaction type int he msg and replace the first two elements by the Transaction code of the logged transaction
            string processingCode = isoMsg.Fields[MessageField.TRANSACTION_TYPE_FIELD].ToString();
            processingCode = transaction.TransactionTypeCode + processingCode.Substring(2, processingCode.Length - 2);
            isoMsg.Fields.Add(MessageField.TRANSACTION_TYPE_FIELD, processingCode);    //error

            return isoMsg;
        }

        public static Iso8583Message SetReponseMessage(Iso8583Message msg, string code)
        {
            msg.SetResponseMessageTypeIdentifier();
            msg.Fields.Add(MessageField.RESPONSE_FIELD, code);
            return msg;
        }
    }

}
