using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    //[Table(Name = "Transaction")]

    public class Transaction
    {
        public int Id { get; set; }

        [Display(Name = "Card PAN")]

        public string CardPAN { get; set; }
        public decimal Amount { get; set; }  //exclusive of the transaction fee
        [Display(Name = "Transaction Fee")]

        public string TransactionFee { get; set; }  //A fee charged, by the acquirer to the issuer, for transaction activity, in the currency of the amount, transaction.
        [Display(Name = "Processing Fee")]

        public string ProcessingFee { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Message Type Identifier")]

        public string MTI { get; set; }     //Message Type Identifier
        [Display(Name = "System Trace Audit Number")]

        public string STAN { get; set; }    //System Trace Audit Number
        [Display(Name = "Channel Code")]

        public string ChannelCode { get; set; }
        [Display(Name = "Original Data Element")]

        public string OriginalDataElement { get; set; }
        [Display(Name = "Transaction Type Code")]

        public string TransactionTypeCode { get; set; }
        [Display(Name = "Payer Account")]

        public string Account1 { get; set; }    //"from" account number (payer)
        [Display(Name = "Payee Account")]

        public string Account2 { get; set; }    //"To" account number (payee)
        [Display(Name = "Fee Code")]

        public string FeeCode { get; set; }

        [Display(Name = "Response Code")]

        public string ResponseCode { get; set; }
        [Display(Name = "Response Description")]

        public string ResponseDescription { get; set; }
        public bool IsReversePending { get; set; }
        public bool IsReversed { get; set; }
        public SourceNode SourceNode { get; set; }
        [Display(Name = "Source Node")]

        public int SourceNodeId { get; set; }
        [Display(Name = "Acquirer")]

        public string Acquirer { get; set; }
        [Display(Name = "Issuer")]

        public string Issuer { get; set; }

    }
}
