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
    public partial class frmAddUpdateSalary : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        DAL dl = new DAL();
        public static string sPageName = "Salary Details";

        public frmAddUpdateSalary()
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

            PopuatePrice();
            PopuateLoanAmount();

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
            if (Datalayer.iT16_SALARYID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T16_SALARY WHERE T16_SALARYID=" + Datalayer.iT16_SALARYID);
                if (dt.Rows.Count > 0)
                {
                    cmbEmployee.SelectedValue = dt.Rows[0]["T14_WORKERID"].ToString();

                    lblDayPrice.Text = dt.Rows[0]["T16_DAY_PRICE"].ToString();
                    lblHoursPrice.Text = dt.Rows[0]["T16_HOURS_PRICE"].ToString();
                    dtpDate.Text = dt.Rows[0]["T16_DATE"].ToString();
                    txtTotalDay.Text = dt.Rows[0]["T16_TOTAL_DAY"].ToString();
                    txtTotalHours.Text = dt.Rows[0]["T16_TOTAL_HOURS"].ToString();
                    // txtTotal.Text = dt.Rows[0]["T16_TOTAL_SALARY"].ToString();
                    // txtTotalDedaction.Text = dt.Rows[0]["T16_DIDACT"].ToString();
                    txtSalary.Text = dt.Rows[0]["T16_NET_SALARY"].ToString();
                    lblLoanAmount.Text = dt.Rows[0]["T16_LOAN_AMOUNT"].ToString();
                    //  txtPaidAmt.Text = dt.Rows[0]["T16_PAID_SALARY"].ToString();
                    txtDetails.Text = dt.Rows[0]["T16_DETAILS"].ToString();

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
                if (string.IsNullOrEmpty(txtTotalDay.Text.Trim()))
                {
                    ep1.SetError(txtTotalDay, "Amount is Required..");
                    txtTotalDay.Focus();
                    return;
                }

                decimal dTotal = 0, dDedaction = 0, dSalary = 0, dLoan = 0, dPaid = 0; ;
                int dTotalDay = 0, dTotalHours = 0;

                decimal dDayPrice = 0, dHoursPrice = 0;
                int.TryParse(txtTotalDay.Text.Trim(), out dTotalDay);
                int.TryParse(txtTotalHours.Text.Trim(), out dTotalHours);
                decimal.TryParse(lblDayPrice.Text.Trim(), out dDayPrice);
                decimal.TryParse(lblHoursPrice.Text.Trim(), out dHoursPrice);
                //decimal.TryParse(txtTotal.Text.Trim(), out dTotal);
                //  decimal.TryParse(txtTotalDedaction.Text.Trim(), out dDedaction);
                decimal.TryParse(txtSalary.Text.Trim(), out dSalary);
                decimal.TryParse(lblLoanAmount.Text.Trim(), out dLoan);
                //ecimal.TryParse(txtPaidAmt.Text.Trim(), out dPaid);
                int T14_WORKERID = 0;
                int.TryParse(cmbEmployee.SelectedValue.ToString(), out T14_WORKERID);

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATES_SALARY(Datalayer.iT16_SALARYID, T14_WORKERID, dtpDate.Value, dTotalDay, dTotalHours, dDayPrice, dHoursPrice, dSalary, dLoan, txtDetails.Text.Trim(), Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iT16_SALARYID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.iT16_SALARYID == 0)
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
            Datalayer.iT16_SALARYID = 0;
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
            Datalayer.iT16_SALARYID = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManageSalary fm = new frmManageSalary();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }

        private void cmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopuatePrice();
            PopuateLoanAmount();
        }

        void PopuatePrice()
        {
            if (!string.IsNullOrEmpty(cmbEmployee.Text.Trim()))
            {
                int iT14_WORKERID = 0;
                int.TryParse(cmbEmployee.SelectedValue.ToString(), out iT14_WORKERID);
                if (iT14_WORKERID > 0)
                {
                    string sSql = "SELECT T14_DAY_PRICE,T14_HOURS_PRICE FROM T14_WORKER WHERE T14_WORKERID=" + iT14_WORKERID;
                    DataTable dtData = new DataTable();
                    dtData = dl.SelectMethod(sSql);
                    if (dtData.Rows.Count > 0)
                    {
                        lblDayPrice.Text = dtData.Rows[0]["T14_DAY_PRICE"].ToString();
                        lblHoursPrice.Text = dtData.Rows[0]["T14_HOURS_PRICE"].ToString();
                    }
                    else
                    {
                        lblDayPrice.Text = "0";
                        lblHoursPrice.Text = "0";
                    }
                }
            }
        }

        void PopuateLoanAmount()
        {
            if (!string.IsNullOrEmpty(cmbEmployee.Text.Trim()))
            {
                int iT14_WORKERID = 0;
                int.TryParse(cmbEmployee.SelectedValue.ToString(), out iT14_WORKERID);
                if (iT14_WORKERID > 0)
                {
                    decimal dTotalLoan = 0, dPaidLoan = 0;

                    string sSql = "SELECT SUM(T14_AMOUNT) FROM T15_LOAN WHERE T14_WORKERID=" + iT14_WORKERID;
                    DataTable dtData = new DataTable();
                    dtData = dl.SelectMethod(sSql);
                    if (dtData.Rows.Count > 0)
                    {
                        decimal.TryParse(dtData.Rows[0][0].ToString(), out dTotalLoan); ;
                    }

                    string sSqlPaid = "SELECT SUM(T017_AMOUNT) FROM T017_WORKAR_PAYMENT WHERE T14_WORKERID=" + iT14_WORKERID;
                    DataTable dtDataPaid = new DataTable();
                    dtDataPaid = dl.SelectMethod(sSqlPaid);
                    if (dtDataPaid.Rows.Count > 0)
                    {
                        decimal.TryParse(dtDataPaid.Rows[0][0].ToString(), out dPaidLoan); ;
                    }

                    lblLoanAmount.Text = (dTotalLoan - dPaidLoan).ToString();
                }
                else
                    lblLoanAmount.Text = "0";
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            PopulateSalary();
        }

        private void txtHours_TextChanged(object sender, EventArgs e)
        {
            PopulateSalary();

        }

        void PopulateSalary()
        {
            decimal dTotalDay = 0, dTotalHours = 0, dTotal = 0, dDedaction = 0, dSalary = 0, dLoan = 0, dPaid = 0; ;

            decimal dDayPrice = 0, dHoursPrice = 0;
            decimal.TryParse(txtTotalDay.Text.Trim(), out dTotalDay);
            decimal.TryParse(txtTotalHours.Text.Trim(), out dTotalHours);
            decimal.TryParse(lblDayPrice.Text.Trim(), out dDayPrice);
            decimal.TryParse(lblHoursPrice.Text.Trim(), out dHoursPrice);

            dTotal = (dTotalDay * dDayPrice) + (dTotalHours * dHoursPrice);
            //txtTotal.Text = dTotal.ToString();

            //decimal.TryParse(txtTotalDedaction.Text.Trim(), out dDedaction);
            dSalary = dTotal - dDedaction;

            txtSalary.Text = dSalary.ToString();
           // decimal.TryParse(txtLoan.Text.Trim(), out dLoan);
           // dPaid = dSalary - dLoan;

           // txtPaidAmt.Text = dPaid.ToString();
        }

        private void txtTotalDedaction_TextChanged(object sender, EventArgs e)
        {
            PopulateSalary();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            //PopulateSalary();
        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {
            PopulateSalary();
        }

        private void txtLoan_TextChanged(object sender, EventArgs e)
        {
            PopulateSalary();
        }
    }
}
