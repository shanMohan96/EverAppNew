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
    public partial class frmAddUpdateLoan : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        DAL dl = new DAL();
        public static string sPageName = "Loan Details";

        public frmAddUpdateLoan()
        {
            InitializeComponent();
        }

        private void frmAddUpdateParty_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillEmployeeList(cmbEmployee);

            PopauteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(button1, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnSave, "ctrl + S");
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
            if (Datalayer.iT15_LOANID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T15_LOAN WHERE T15_LOANID=" + Datalayer.iT15_LOANID);
                if (dt.Rows.Count > 0)
                {
                    cmbEmployee.SelectedValue = dt.Rows[0]["T14_WORKERID"].ToString();
                    txtAmount.Text = dt.Rows[0]["T14_AMOUNT"].ToString();
                    dtpDate.Text = dt.Rows[0]["T14_DATE"].ToString();
                    txtDetails.Text = dt.Rows[0]["T14_DETAILS"].ToString();

                    cmbEmployee.Focus();
                }
            }
        }

        void AddUpateBank()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(cmbEmployee.Text.Trim()))
                {
                    ep1.SetError(cmbEmployee, "Name is Required..");
                    cmbEmployee.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
                {
                    ep1.SetError(txtAmount, "Amount is Required..");
                    txtAmount.Focus();
                    return;
                }

                int T14_WORKERID = 0, TM04_BANKID = 0;
                decimal dT14_AMOUNT = 0;
                int.TryParse(cmbEmployee.SelectedValue.ToString(), out T14_WORKERID);
                decimal.TryParse(txtAmount.Text.Trim(), out dT14_AMOUNT);

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATE_LOAN(Datalayer.iT15_LOANID, T14_WORKERID, dtpDate.Value, dT14_AMOUNT, txtDetails.Text.Trim(), Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iT15_LOANID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.iT15_LOANID == 0)
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
            Datalayer.iT15_LOANID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            cmbEmployee.Focus();
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
            Datalayer.iT15_LOANID = 0;
        }
       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManageLoan fm = new frmManageLoan();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }
    }
}
