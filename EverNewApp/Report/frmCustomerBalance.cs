using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace EverNewApp.Report
{
    public partial class frmCustomerBalance : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmCustomerBalance()
        {
            InitializeComponent();
        }

        private void frmCustomerBalance_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbName, "c");

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;
        }

        private void frmCustomerBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
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

            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_GET_CUSTOMER_BAL_SUMMARY '" + TM02_PARTYID + "','" + Datalayer.iT001_COMPANYID + "'");
            if (dt.Rows.Count > 0)
            {
                DataTable dt1 = new DataTable();
                DataRow[] dr = dt.Select("TOTALSALE >0 ");
                if (dr.Length > 0)
                    dt1 = dr.CopyToDataTable();

                ReportDocument RptDoc = new ReportDocument();

                RptDoc.Load(Application.StartupPath + @"\Report\rptCustomerBalSummary.rpt");
                RptDoc.SetDataSource(dt1);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Customer Balance Report";

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

    }
}
