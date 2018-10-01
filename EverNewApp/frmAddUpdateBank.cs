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
    public partial class frmAddUpdateBank : Form
    {
        MyDabaseDataContext MyDa;
        public static string sPageName = "Bank Details";

        public frmAddUpdateBank()
        {
            InitializeComponent();
        }

        private void frmAddUpdateParty_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);


            PopauteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(button1, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnSave, "ctrl + S");
        }

        DAL dl = new DAL();
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
            if (Datalayer.T010_PACKINGSLIPID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM TM04_BANK WHERE TM04_BANKID=" + Datalayer.T010_PACKINGSLIPID);
                if (dt.Rows.Count > 0)
                {
                    txtBank.Text = dt.Rows[0]["TM04_NAME"].ToString();
                    txtAccount.Text = dt.Rows[0]["TM04_ACCONT_NAME"].ToString();
                    txtIFSC.Text = dt.Rows[0]["TM04_IFSCCODE"].ToString();
                    txtBranch.Text = dt.Rows[0]["TM04_BRANCH"].ToString();
                    txtDetails.Text = dt.Rows[0]["TM04_DETAILS"].ToString();

                    txtBank.Focus();
                }
            }
        }

        void AddUpateBank()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(txtBank.Text.Trim()))
                {
                    ep1.SetError(txtBank, "Name is Required..");
                    txtBank.Focus();
                    return;
                }

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATE_BANK(Datalayer.T010_PACKINGSLIPID, txtBank.Text.Trim(), txtAccount.Text.Trim(), txtIFSC.Text.Trim(), txtBranch.Text.Trim(), txtDetails.Text.Trim(), Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.T010_PACKINGSLIPID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.T010_PACKINGSLIPID == 0)
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
            Datalayer.T010_PACKINGSLIPID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            txtBank.Focus();
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
            Datalayer.T010_PACKINGSLIPID = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            

            frmManageBank fm = new frmManageBank();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }
    }
}
