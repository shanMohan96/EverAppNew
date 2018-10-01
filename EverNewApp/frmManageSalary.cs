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
    public partial class frmManageSalary : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmManageSalary()
        {
            InitializeComponent();
        }

        private void frmManagePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillEmployeeList(cmbName);
            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnAdd, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnEdit, "ctrl + E");
            t1.SetToolTip(btnDelete, "ctrl + D");
        }

        private void frmManagePurchase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (dgDisplayData.SelectedRows.Count > 0)
                    dgDisplayData.ClearSelection();
                else
                    this.Close();
            }
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
            if (e.Control && e.KeyCode == Keys.E)
                btnEdit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.N)
                btnAdd_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                btnExit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.D)
                btnDelete_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateSalary fmAdd = new frmAddUpdateSalary();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            string TM02_PARTYID = "";
            int iTM02_PARTYID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iTM02_PARTYID);

            if (iTM02_PARTYID > 0)
                TM02_PARTYID = iTM02_PARTYID.ToString();

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_SALARYResult> lst = new List<USP_VP_GET_SALARYResult>();
            lst = MyDa.USP_VP_GET_SALARY(TM02_PARTYID, dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            dgDisplayData.Columns["T16_SALARYID"].Visible = false;
            dgDisplayData.Columns["T14_WORKERID"].Visible = false;
            dgDisplayData.Columns["T16_TOTAL_DAY"].Visible = false;
            dgDisplayData.Columns["T16_TOTAL_HOURS"].Visible = false;
            dgDisplayData.Columns["T16_DAY_PRICE"].Visible = false;
            dgDisplayData.Columns["T16_HOURS_PRICE"].Visible = false;
            dgDisplayData.Columns["T16_DETAILS"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;
            dgDisplayData.Columns["T14_PHONE_NO"].Visible = false;

            dgDisplayData.Columns["T14_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T16_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T16_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["T16_NET_SALARY"].HeaderText = "Salary";
            dgDisplayData.Columns["T16_LOAN_AMOUNT"].HeaderText = "Loan";

            dgDisplayData.Columns["T14_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T16_DATE"].DisplayIndex = 1;
            dgDisplayData.Columns["T16_NET_SALARY"].DisplayIndex = 2;
            dgDisplayData.Columns["T16_LOAN_AMOUNT"].DisplayIndex = 3;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns["T14_NAME"].Width = 350;
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        void EditData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int ID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["T16_SALARYID"].Value.ToString(), out ID);

                Datalayer.iT16_SALARYID = ID;

                frmAddUpdateSalary frmAddSTD = new frmAddUpdateSalary();
                frmAddSTD.MdiParent = this.MdiParent;
                frmAddSTD.Show();

                this.Close();
            }
            else
            {
                Datalayer.InformationMessageBox(Datalayer.sMeessgeSelection);
            }
        }

        private void dgDisplayData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                EditData();
            if (e.KeyCode == Keys.Delete)
                DelelteData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DelelteData();
        }

        void DelelteData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {

                if (Datalayer.ShowQuestMsg(Datalayer.sMessageConfirmation))
                {
                    try
                    {
                        int ID = 0;
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T16_SALARYID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        string Sq = "DELETE FROM T16_SALARY WHERE T16_SALARYID=" + ID;
                        DAL dl = new DAL();
                        if (dl.ExecuteMethod(Sq))
                        {
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Loan Details");
                    }
                    catch (Exception)
                    {
                        Datalayer.InformationMessageBox(Datalayer.sMessageForainKey);
                        return;
                    }
                }
            }
            else
            {
                Datalayer.InformationMessageBox(Datalayer.sMeessgeSelection);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();
        }

    }
}
