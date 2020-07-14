using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitchProcessor.Utility
{
    public static class TransactionTypeCode
    {
        public const string CASH_WITHDRAWAL = "01";
        public const string PAYMENT_FROM_ACCOUNT = "50";
        public const string PAYMENT_BY_DEPOSIT = "51";
        public const string INTRA_BANK_TRANSFER = "24";
        public const string INTER_BANK_TRANSFER = "71";
        public const string BALANCE_ENQUIRY = "31";
        public const string FUND_TRANSFER = "41";
    }
}
