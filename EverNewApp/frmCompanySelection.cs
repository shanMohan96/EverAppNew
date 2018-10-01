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
    public partial class frmCompanySelection : Form
    {
        string sPageName = "Company Details";
        DAL dl = new DAL();

        public frmCompanySelection()
        {
            InitializeComponent();
        }

        private void frmCompanySelection_Load(object sender, EventArgs e)
        {
           /// Datalayer.SetButtion(btnCreate);
            Datalayer.SetButtion(btnExit);
           // Datalayer.SetButtion(btnSelection);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            PopulateData();
        }

        private void frmCompanySelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");

        }
        void PopulateData()
        {
            DataTable dtCompanyDate = new DataTable();
            dtCompanyDate = dl.SelectMethod("SELECT TM_COMPAYID,TM_NAME FROM TM_COMPANY WHERE TM_ACTIVE=1");
            if (dtCompanyDate.Rows.Count > 0)
            {
                if (dtCompanyDate.Rows.Count == 1)
                {
                    int TM_COMPAYID = 0;
                    int.TryParse(Convert.ToString(dtCompanyDate.Rows[0]["TM_COMPAYID"]), out TM_COMPAYID);
                                      
                    Datalayer.iT001_COMPANYID = TM_COMPAYID;

                    this.Close();

                    MDIMaster fmMain = new MDIMaster();
                    fmMain.Show();
                                     
                    
                }
                else
                    dgDisplayData.DataSource = dtCompanyDate;
            }
            else
                dgDisplayData.DataSource = null;
        }

        void Filldata()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int iTL01_SCHOOLID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["TM_COMPAYID"].Value.ToString(), out iTL01_SCHOOLID);

                Datalayer.iT001_COMPANYID = iTL01_SCHOOLID;

                MDIMaster fmMain = new MDIMaster();
                fmMain.Show();

                this.Hide();
            }
            else
            {
                Datalayer.InformationMessageBox(Datalayer.sMeessgeSelection);
            }
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;
            dgDisplayData.Columns["TM_NAME"].HeaderText = "Name";


            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3); ;
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.EnableHeadersVisualStyles = false;
        }

        private void btnSelection_Click(object sender, EventArgs e)
        {
            Filldata();
        }

        private void dgDisplayData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Filldata();
        }

        private void dgDisplayData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Filldata();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            frmAddUpdateCompany fmAdd = new frmAddUpdateCompany();
            fmAdd.Show();

            this.Hide();
        }


    }
}
