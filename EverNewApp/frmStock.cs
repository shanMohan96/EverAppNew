using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace EverNewApp
{
    public partial class frmStock : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmStock()
        {
            InitializeComponent();
        }

        private void frmManageJobWorkInvoice_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnExit);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);


            dbo.FillItemName(cmbName);
            // dbo.FillItemSize(cmbName, cmbMainItemSize);

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnExit, "ctrl + X");
        }

        private void frmManageJobWorkInvoice_KeyDown(object sender, KeyEventArgs e)
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
            if (e.Control && e.KeyCode == Keys.N)
                btnAdd_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                btnExit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateProductItem fmAdd = new frmAddUpdateProductItem();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            //DAL dl = new DAL();

            //DataTable dt = new DataTable();
            //dt = dl.SelectMethod("SELECT T007.T007_DATE ,STUFF((SELECT    ' Item:'+ CAST( TM01_NAME AS VARCHAR(10))+' Qty:'+CAST(T008_QTY AS VARCHAR(10)) +'\n' [text()] FROM T008_SALEITEM  T008 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T008.TM01_PRODUCTID WHERE T008.T007_SALEID = T007.T007_SALEID FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,1,' ') List_Output FROM T007_SALE T007");
            //dgDisplayData.DataSource = dt;
            //dgDisplayData.Columns["List_Output"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgDisplayData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //return;

            int iPartyID = 0, TM02_MAIN_PRODUCTSIZEID = 0;
            string sPartyID = "", sTM02_MAIN_PRODUCTSIZEID = "";
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iPartyID);
            //if (!string.IsNullOrEmpty(cmbMainItemSize.Text.Trim()))
            //    int.TryParse(cmbMainItemSize.SelectedValue.ToString(), out TM02_MAIN_PRODUCTSIZEID);

            if (iPartyID > 0)
                sPartyID = iPartyID.ToString();
            if (TM02_MAIN_PRODUCTSIZEID > 0)
                sTM02_MAIN_PRODUCTSIZEID = TM02_MAIN_PRODUCTSIZEID.ToString();

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_STOCKResult> lstCategory = new List<USP_VP_GET_STOCKResult>();
            lstCategory = MyDa.USP_VP_GET_STOCK(sPartyID, sTM02_MAIN_PRODUCTSIZEID, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lstCategory;

            //  dgDisplayData.Columns["TM01_NO"].HeaderText = "No";
            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Name";
            //  dgDisplayData.Columns["TM02_SIZE"].HeaderText = "Size";
            dgDisplayData.Columns["T006_STOCK"].HeaderText = "Qty";
            dgDisplayData.Columns["T006_UNIT"].HeaderText = "Unit";

            //  dgDisplayData.Columns["TM01_NO"].DisplayIndex = 0;
            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 0;
            // dgDisplayData.Columns["TM02_SIZE"].DisplayIndex = 2;
            dgDisplayData.Columns["T006_STOCK"].DisplayIndex = 1;
            dgDisplayData.Columns["T006_UNIT"].DisplayIndex = 2;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;
            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }




        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;
            //if (cmbMainItemSize.Items.Count > 0)
            //    cmbMainItemSize.SelectedIndex = -1;

            PopualteData();
        }

    }
}
