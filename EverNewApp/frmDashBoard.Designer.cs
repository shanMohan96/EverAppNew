namespace EverNewApp
{
    partial class frmDashBoard
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.frmStock = new System.Windows.Forms.Button();
            this.frmSale = new System.Windows.Forms.Button();
            this.frmStockIn = new System.Windows.Forms.Button();
            this.frmPurchase = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.frmAccount = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1268, 394);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.crystalReportViewer1);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F);
            this.groupBox3.Location = new System.Drawing.Point(3, 92);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1260, 297);
            this.groupBox3.TabIndex = 109;
            this.groupBox3.TabStop = false;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(3, 20);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(1254, 274);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.frmStock);
            this.panel2.Controls.Add(this.frmSale);
            this.panel2.Controls.Add(this.frmStockIn);
            this.panel2.Controls.Add(this.frmPurchase);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.frmAccount);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1266, 86);
            this.panel2.TabIndex = 0;
            // 
            // frmStock
            // 
            this.frmStock.BackColor = System.Drawing.Color.Navy;
            this.frmStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.frmStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.frmStock.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frmStock.ForeColor = System.Drawing.Color.White;
            this.frmStock.Image = global::EverNewApp.Properties.Resources.btnlogo;
            this.frmStock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.frmStock.Location = new System.Drawing.Point(1076, 10);
            this.frmStock.Name = "frmStock";
            this.frmStock.Size = new System.Drawing.Size(171, 51);
            this.frmStock.TabIndex = 0;
            this.frmStock.Text = "STOCK DETAILS";
            this.frmStock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.frmStock.UseVisualStyleBackColor = false;
            this.frmStock.Click += new System.EventHandler(this.frmStock_Click);
            // 
            // frmSale
            // 
            this.frmSale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.frmSale.Cursor = System.Windows.Forms.Cursors.Hand;
            this.frmSale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.frmSale.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frmSale.ForeColor = System.Drawing.Color.White;
            this.frmSale.Image = global::EverNewApp.Properties.Resources.btnlogo;
            this.frmSale.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.frmSale.Location = new System.Drawing.Point(894, 10);
            this.frmSale.Name = "frmSale";
            this.frmSale.Size = new System.Drawing.Size(171, 51);
            this.frmSale.TabIndex = 0;
            this.frmSale.Text = "SALE DETAILS";
            this.frmSale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.frmSale.UseVisualStyleBackColor = false;
            this.frmSale.Click += new System.EventHandler(this.frmSale_Click);
            // 
            // frmStockIn
            // 
            this.frmStockIn.BackColor = System.Drawing.Color.Red;
            this.frmStockIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.frmStockIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.frmStockIn.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frmStockIn.ForeColor = System.Drawing.Color.White;
            this.frmStockIn.Image = global::EverNewApp.Properties.Resources.btnlogo;
            this.frmStockIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.frmStockIn.Location = new System.Drawing.Point(712, 10);
            this.frmStockIn.Name = "frmStockIn";
            this.frmStockIn.Size = new System.Drawing.Size(171, 51);
            this.frmStockIn.TabIndex = 0;
            this.frmStockIn.Text = "STOCK IN";
            this.frmStockIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.frmStockIn.UseVisualStyleBackColor = false;
            this.frmStockIn.Click += new System.EventHandler(this.frmStockIn_Click_1);
            // 
            // frmPurchase
            // 
            this.frmPurchase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.frmPurchase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.frmPurchase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.frmPurchase.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frmPurchase.ForeColor = System.Drawing.Color.White;
            this.frmPurchase.Image = global::EverNewApp.Properties.Resources.btnlogo;
            this.frmPurchase.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.frmPurchase.Location = new System.Drawing.Point(530, 10);
            this.frmPurchase.Name = "frmPurchase";
            this.frmPurchase.Size = new System.Drawing.Size(171, 51);
            this.frmPurchase.TabIndex = 0;
            this.frmPurchase.Text = "PURCHASE";
            this.frmPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.frmPurchase.UseVisualStyleBackColor = false;
            this.frmPurchase.Click += new System.EventHandler(this.frmPurchase_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Purple;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Image = global::EverNewApp.Properties.Resources.btnlogo;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(383, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(136, 51);
            this.button2.TabIndex = 0;
            this.button2.Text = "PACKING SLIP";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::EverNewApp.Properties.Resources.btnlogo;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(201, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(171, 51);
            this.button1.TabIndex = 0;
            this.button1.Text = "ORDER";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmAccount
            // 
            this.frmAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.frmAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.frmAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.frmAccount.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frmAccount.ForeColor = System.Drawing.Color.White;
            this.frmAccount.Image = global::EverNewApp.Properties.Resources.btnlogo;
            this.frmAccount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.frmAccount.Location = new System.Drawing.Point(19, 10);
            this.frmAccount.Name = "frmAccount";
            this.frmAccount.Size = new System.Drawing.Size(171, 51);
            this.frmAccount.TabIndex = 0;
            this.frmAccount.Text = "ACCOUNT";
            this.frmAccount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.frmAccount.UseVisualStyleBackColor = false;
            this.frmAccount.Click += new System.EventHandler(this.frmAccount_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmDashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 394);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.Name = "frmDashBoard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDashBoard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmDashBoard_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button frmAccount;
        private System.Windows.Forms.Button frmStock;
        private System.Windows.Forms.Button frmSale;
        private System.Windows.Forms.Button frmStockIn;
        private System.Windows.Forms.Button frmPurchase;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.Timer timer1;
    }
}