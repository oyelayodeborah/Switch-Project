namespace BankONETrans.WindowsForms
{
    partial class BankONE
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtCardPAN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlMon = new System.Windows.Forms.ComboBox();
            this.ddlYr = new System.Windows.Forms.ComboBox();
            this.ddlTrxType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGO = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Card PAN:";
            // 
            // txtCardPAN
            // 
            this.txtCardPAN.Location = new System.Drawing.Point(71, 30);
            this.txtCardPAN.Name = "txtCardPAN";
            this.txtCardPAN.Size = new System.Drawing.Size(184, 20);
            this.txtCardPAN.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Xpiry Date:";
            // 
            // ddlMon
            // 
            this.ddlMon.FormattingEnabled = true;
            this.ddlMon.Items.AddRange(new object[] {
            "Janunary",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.ddlMon.Location = new System.Drawing.Point(73, 58);
            this.ddlMon.Name = "ddlMon";
            this.ddlMon.Size = new System.Drawing.Size(80, 21);
            this.ddlMon.TabIndex = 3;
            // 
            // ddlYr
            // 
            this.ddlYr.FormattingEnabled = true;
            this.ddlYr.Items.AddRange(new object[] {
            "2020",
            "2021",
            "2022",
            "2023"});
            this.ddlYr.Location = new System.Drawing.Point(159, 58);
            this.ddlYr.Name = "ddlYr";
            this.ddlYr.Size = new System.Drawing.Size(61, 21);
            this.ddlYr.TabIndex = 4;
            // 
            // ddlTrxType
            // 
            this.ddlTrxType.FormattingEnabled = true;
            this.ddlTrxType.Items.AddRange(new object[] {
            "Balance Enquiry",
            "Cash Withdrawal",
            "Sign On",
            "Reversal",
            "Intra-Bank Transfer",
            "Inter-Bank Transfer"});
            this.ddlTrxType.Location = new System.Drawing.Point(103, 89);
            this.ddlTrxType.Name = "ddlTrxType";
            this.ddlTrxType.Size = new System.Drawing.Size(117, 21);
            this.ddlTrxType.TabIndex = 6;
            this.ddlTrxType.SelectedIndexChanged += new System.EventHandler(this.ddlTrxType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Transaction Type:";
            // 
            // btnGO
            // 
            this.btnGO.Location = new System.Drawing.Point(103, 164);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(75, 23);
            this.btnGO.TabIndex = 7;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.AcceptsReturn = true;
            this.txtOutput.Location = new System.Drawing.Point(16, 222);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(239, 282);
            this.txtOutput.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Output:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(86, 206);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(79, 13);
            this.lblTime.TabIndex = 10;
            this.lblTime.Text = "12:00:00 AM";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Amount:";
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(71, 117);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(149, 20);
            this.txtAmount.TabIndex = 12;
            this.txtAmount.Text = "0";
            this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BankONE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 516);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.ddlTrxType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddlYr);
            this.Controls.Add(this.ddlMon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCardPAN);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "BankONE";
            this.Text = "Bank ONE Transaction Simuator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCardPAN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlMon;
        private System.Windows.Forms.ComboBox ddlYr;
        private System.Windows.Forms.ComboBox ddlTrxType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAmount;
    }
}

