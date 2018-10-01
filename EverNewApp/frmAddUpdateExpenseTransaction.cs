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
    public partial class frmAddUpdateExpenseTransaction : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        DAL dl = new DAL();
        public static string sPageName = "Expense Details";

        public frmAddUpdateExpenseTransaction()
        {
            InitializeComponent();
        }

        private void frmAddUpdateParty_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillExpenseMaster(cmbExpense);

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
            if (Datalayer.iT013_EXPENSETRANSACATIONID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T013_EXPENSETRANSACATION WHERE T013_EXPENSETRANSACATIONID=" + Datalayer.iT013_EXPENSETRANSACATIONID);
                if (dt.Rows.Count > 0)
                {
                    cmbExpense.SelectedValue = dt.Rows[0]["TM05_EXPENSEID"].ToString();
                    txtAmount.Text = dt.Rows[0]["T013_AMOUNT"].ToString();
                    dtpDate.Text = dt.Rows[0]["T013_DATE"].ToString();
                    txtDetails.Text = dt.Rows[0]["T013_DETAILS"].ToString();

                    cmbExpense.Focus();
                }
            }
        }

        void AddUpateBank()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(cmbExpense.Text.Trim()))
                {
                    ep1.SetError(cmbExpense, "Name is Required..");
                    cmbExpense.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
                {
                    ep1.SetError(txtAmount, "Amount is Required..");
                    txtAmount.Focus();
                    return;
                }

                int TM05_EXPENSEID = 0, TM04_BANKID = 0, T009_AMOUNT = 0;
                int.TryParse(cmbExpense.SelectedValue.ToString(), out TM05_EXPENSEID);
                int.TryParse(txtAmount.Text.Trim(), out T009_AMOUNT);

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATE_EXPENSE_TRANSACTION(Datalayer.iT013_EXPENSETRANSACATIONID,TM05_EXPENSEID, dtpDate.Value, T009_AMOUNT, txtDetails.Text.Trim(), Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iT013_EXPENSETRANSACATIONID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.iT013_EXPENSETRANSACATIONID == 0)
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
            Datalayer.iT013_EXPENSETRANSACATIONID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            cmbExpense.Focus();
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
       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManageExpenseTransaction fm = new frmManageExpenseTransaction();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }
    }
}
