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
    public partial class frmProductionPLWiseReport : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmProductionPLWiseReport()
        {
            InitializeComponent();
        }

        private void frmManagePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbName, "pl");

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;
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
            string TM01_PRODUCTID = "", T001_ACCOUNTID = "";
            int iTM01_PRODUCTID = 0, iTM02_PRODUCTSIZEID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iTM02_PRODUCTSIZEID);

            if (iTM02_PRODUCTSIZEID > 0)
                T001_ACCOUNTID = iTM02_PRODUCTSIZEID.ToString();

            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_RP_PRODUCTION_REPORT '" + T001_ACCOUNTID + "', '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dt.Rows.Count > 0)
            {
                ReportDocument RptDoc = new ReportDocument();

                RptDoc.Load(Application.StartupPath + @"\Report\rptplProduction.rpt");
                RptDoc.SetDataSource(dt);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Production Report";

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
