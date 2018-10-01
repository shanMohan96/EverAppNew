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
    public partial class frmGSTReport : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmGSTReport()
        {
            InitializeComponent();
        }

        private void frmManagePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);
        }

        private void frmManagePurchase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                    this.Close();
            }
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdatePurchase fmAdd = new frmAddUpdatePurchase();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_GET_STOCK_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dt.Rows.Count > 0)
            {
                DataView db = dt.DefaultView;
                db.Sort = "T007_DATE ASC";
                DataTable sort = db.ToTable();

                ReportDocument RptDoc = new ReportDocument();
                RptDoc.Load(Application.StartupPath + @"\Report\rptStockReportListing.rpt");
                RptDoc.SetDataSource(sort);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "GST Report";

                Report.frmReportViwer fmReport = new Report.frmReportViwer();
                fmReport.Show();
            }
            else
            {
                Datalayer.InformationMessageBox("No Record..");
            }

        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
