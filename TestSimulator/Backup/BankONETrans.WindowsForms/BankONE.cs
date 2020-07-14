using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BankONETrans.WindowsForms
{
    public partial class BankONE : Form
    {
        public BankONE()
        {
            InitializeComponent();
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCardPAN.Text))
            {
                MessageBox.Show("Card PAN is required");
            }
            if (string.IsNullOrEmpty(ddlMon.Text) || string.IsNullOrEmpty(ddlYr.Text))
            {
                MessageBox.Show("Xpiry date is required");
            }
            if (string.IsNullOrEmpty(ddlTrxType.Text))
            {
                MessageBox.Show("transaction type is required");
            }

            string mon = string.Empty;
            #region Get Month
            switch (ddlMon.Text)
            {
                case "Janunary":
                    mon = "01";
                    break;
                case "February":
                    mon = "02";
                    break;
                case "March":
                    mon = "03";
                    break;
                case "April":
                    mon = "04";
                    break;
                case "May":
                    mon = "05";
                    break;
                case "June":
                    mon = "06";
                    break;
                case "July":
                    mon = "07";
                    break;
                case "August":
                    mon = "08";
                    break;
                case "September":
                    mon = "09";
                    break;
                case "October":
                    mon = "10";
                    break;
                case "November":
                    mon = "11";
                    break;
                case "December":
                    mon = "12";
                    break;
            }
            #endregion

            string trxType = string.Empty;
            switch (ddlTrxType.Text)
            {           
                case "Balance Enquiry":
                    trxType = "31";
                    break;
                case "Cash Withdrawal":
                    trxType = "01";
                    break;
                case "Sign On":

                    break;
            }

            DateTime xpiryDt = new DateTime(int.Parse(ddlYr.Text), int.Parse(mon), 1);
            long amount = long.Parse(txtAmount.Text);

            if (ddlTrxType.Text == "Reversal")
            {
                txtOutput.Text = new BankONETransactionSimulator.Processor(txtCardPAN.Text, xpiryDt, "01", amount,true).ResponseMessage;
            }
            else
            {
                txtOutput.Text = new BankONETransactionSimulator.Processor(txtCardPAN.Text, xpiryDt, trxType, amount,false).ResponseMessage;
            }
        }
    }
}
