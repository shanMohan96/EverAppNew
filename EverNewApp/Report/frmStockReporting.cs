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
    public partial class frmStockReporting : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmStockReporting()
        {
            InitializeComponent();
        }

        private void frmManageJobWorkInvoice_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillItemName(cmbName);
          //  dbo.FillItemSize(cmbName, cmbMainItemSize);

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;
            //if (cmbMainItemSize.Items.Count > 0)
            //    cmbMainItemSize.SelectedIndex = -1;
        }

        private void frmManageJobWorkInvoice_KeyDown(object sender, KeyEventArgs e)
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

        void PopualteData()
        {
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

            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_GET_STOCK '" + sPartyID + "','" + sTM02_MAIN_PRODUCTSIZEID + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dt.Rows.Count > 0)
            {
                ReportDocument RptDoc = new ReportDocument();

                RptDoc.Load(Application.StartupPath + @"\Report\rptStock.rpt");
                RptDoc.SetDataSource(dt);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Stock Listing Report";

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
