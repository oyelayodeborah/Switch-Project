using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BankONETransactionSimulator;

namespace BankONETrans.WindowsForms
{
    public partial class SwitcTrxSim : Form
    {
        public SwitcTrxSim()
        {
            InitializeComponent();
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                MessageBox.Show("Input XML is required");
            }
            if (string.IsNullOrEmpty(ddlTrxType.Text))
            {
                MessageBox.Show("transaction type is required");
            }

            string trxType = string.Empty;
            switch (ddlTrxType.Text)
            {
                case "Bills Payment":
                    trxType = "51";
                    break;
                case "Recharge":
                    trxType = "00";
                    break;
                case "Funds Transfer":
                    trxType = "40";
                    break;
                case "Cashout":
                    trxType = "01";
                    break;
                case "Mfb Settlement":
                    trxType = "50";
                    break;
            }

            txtOutput.Text = new SwitchProcessor(trxType, txtInput.Text).ResponseMessage;
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
    }
}
