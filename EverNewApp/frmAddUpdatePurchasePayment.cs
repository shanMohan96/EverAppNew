using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EverNewApp
{
    public partial class frmAddUpdatePurchasePayment : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        DAL dl = new DAL();
        public static string sPageName = "Payment Details";

        public frmAddUpdatePurchasePayment()
        {
            InitializeComponent();
        }

        private void frmAddUpdateParty_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);
            

            dbo.FillAccountList(cmbName, Datalayer.sPaymentMode);
            dbo.FillBankList(cmbBank);

            cmbType.SelectedIndex = 0;
            cmbPaymentType.SelectedIndex = 0;

          

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(button1, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnSave, "ctrl + S");

            if (Datalayer.sPaymentMode == "R")
            {
                lblTitle.Text = "Bill Receive Details";
                cmbPaymentType.SelectedItem = "Receive";
                dbo.FillAccountList(cmbName, "c");
            }
            else
            {
                dbo.FillAccountList(cmbName, "p");
                lblTitle.Text = "Bill Payment Details";
                cmbPaymentType.SelectedItem = "Payment";
            }

            PopauteData();
        }

        private void frmAddUpdateParty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");

            if (e.Control && e.KeyCode == Keys.S)
                btnSave_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.N)
                button1_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                btnExit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void PopauteData()
        {
            if (Datalayer.iT009_PURCHASEPAYMENTID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T009_PAYMENT WHERE T009_PAYMENTID=" + Datalayer.iT009_PURCHASEPAYMENTID);
                if (dt.Rows.Count > 0)
                {
                    cmbBank.SelectedValue = dt.Rows[0]["TM04_BANKID"];
                    cmbName.SelectedValue = dt.Rows[0]["T001_ACCOUNTID"];
                    cmbType.SelectedItem = dt.Rows[0]["T009_TYPE"].ToString();
                    txtAmount.Text = dt.Rows[0]["T009_AMOUNT"].ToString();
                    txtDetails.Text = dt.Rows[0]["TM04_DETAILS"].ToString();

                    cmbName.Focus();
                }
            }
        }

        void AddUpateBank()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(cmbName.Text.Trim()))
                {
                    ep1.SetError(cmbName, "Name is Required..");
                    cmbName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbType.Text.Trim()))
                {
                    ep1.SetError(cmbType, "Type is Required..");
                    cmbType.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbPaymentType.Text.Trim()))
                {
                    ep1.SetError(cmbPaymentType, "Type is Required..");
                    cmbPaymentType.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
                {
                    ep1.SetError(txtAmount, "Amount is Required..");
                    txtAmount.Focus();
                    return;
                }

                int T001_ACCOUNTID = 0, TM04_BANKID = 0, T009_AMOUNT = 0;
                string T009_TYPE = cmbType.SelectedItem.ToString();
                string T009_PAYMENT_TYPE = cmbPaymentType.SelectedItem.ToString();
                int.TryParse(cmbName.SelectedValue.ToString(), out T001_ACCOUNTID);
                int.TryParse(Convert.ToString(cmbBank.SelectedValue), out TM04_BANKID);
                int.TryParse(txtAmount.Text.Trim(), out T009_AMOUNT);

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATE_PAYMENT(Datalayer.iT009_PURCHASEPAYMENTID, dtpDate.Value, T001_ACCOUNTID, T009_TYPE, T009_PAYMENT_TYPE, TM04_BANKID, T009_AMOUNT, txtDetails.Text.Trim(), Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iT009_PURCHASEPAYMENTID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.iT009_PURCHASEPAYMENTID == 0)
                        Datalayer.DosenotInsertMessageBox(sPageName);
                    else
                        Datalayer.DosenotUpdateMessageBox(sPageName);
                }

                ResetData();
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void ResetData()
        {
            Datalayer.iT009_PURCHASEPAYMENTID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            cmbName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpateBank();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ResetData();
        }

        private void frmAddUpdateParty_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.iT009_PURCHASEPAYMENTID = 0;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem.ToString() == "CASH")
                cmbBank.Enabled = false;
            else
                cmbBank.Enabled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManagePurchasePayment fm = new frmManagePurchasePayment();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
