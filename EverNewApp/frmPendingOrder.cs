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
    public partial class frmPendingOrder : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmPendingOrder()
        {
            InitializeComponent();
        }

        private void frmManageJobWorkInvoice_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnExit);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbName, "c");

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            //PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnExit, "ctrl + X");
        }

        private void frmManageJobWorkInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //if (dgDisplayData.SelectedRows.Count > 0)
                //    dgDisplayData.ClearSelection();
                //else
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
            int iPartyID = 0, TM02_MAIN_PRODUCTSIZEID = 0;
            string sPartyID = "", sTM02_MAIN_PRODUCTSIZEID = "";
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iPartyID);

            if (iPartyID > 0)
                sPartyID = iPartyID.ToString();
            if (TM02_MAIN_PRODUCTSIZEID > 0)
                sTM02_MAIN_PRODUCTSIZEID = TM02_MAIN_PRODUCTSIZEID.ToString();

            //MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            //List<USP_VP_GET_PANDING_ORDERResult> lstCategory = new List<USP_VP_GET_PANDING_ORDERResult>();
            //lstCategory = MyDa.USP_VP_GET_PANDING_ORDER(sPartyID, Datalayer.iT001_COMPANYID.ToString()).ToList();
            //dgDisplayData.DataSource = lstCategory;

            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_GET_PANDING_ORDER '" + sPartyID + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dt.Rows.Count > 0)
            {

                //var orderedRows = from row in dt.AsEnumerable()
                //                  orderby row.Field<DateTime>("Invoice_Date")
                //                  select row;
                //DataTable tblOrdered = orderedRows.CopyToDataTable();
                DataView dv = new DataView(dt);
                dv.RowFilter = "TOTALPENDING >0";

                //  dgDisplayData.DataSource = dv.ToTable();

                ReportDocument RptDoc = new ReportDocument();

                RptDoc.Load(Application.StartupPath + @"\Report\rptPendingOrderListing.rpt");
                RptDoc.SetDataSource(dv.ToTable());

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Pending Order Report";

                Report.frmReportViwer fmReport = new Report.frmReportViwer();
                fmReport.Show();

                ////dgDisplayData.Columns["TM01_PRODUCTID"].Visible = false;
                ////dgDisplayData.Columns["TM02_PRODUCTSIZEID"].Visible = false;
                ////dgDisplayData.Columns["T001_ACCOUNTID"].Visible = false;

                //dgDisplayData.Columns["Account_Name"].HeaderText = "Name";
                //dgDisplayData.Columns["Product_No"].HeaderText = "No";
                //dgDisplayData.Columns["NAME"].HeaderText = "Name";
                //dgDisplayData.Columns["SIZE"].HeaderText = "Finish";
                //dgDisplayData.Columns["QTY"].HeaderText = "Order Qty";
                //dgDisplayData.Columns["SALE"].HeaderText = "Displaced";
                //dgDisplayData.Columns["TOTALPENDING"].HeaderText = "Pending";

                //dgDisplayData.Columns["Account_Name"].DisplayIndex = 0;
                //dgDisplayData.Columns["Product_No"].DisplayIndex = 1;
                //dgDisplayData.Columns["NAME"].DisplayIndex = 2;
                //dgDisplayData.Columns["SIZE"].DisplayIndex = 3;
                //dgDisplayData.Columns["QTY"].DisplayIndex = 4;
                //dgDisplayData.Columns["SALE"].DisplayIndex = 5;
                //dgDisplayData.Columns["TOTALPENDING"].DisplayIndex = 6;


                //this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
                //dgDisplayData.EnableHeadersVisualStyles = false;

                //// dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                //dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //dgDisplayData.ColumnHeadersHeight = 30;
                //dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                //dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // dgDisplayData.ClearSelection();
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
