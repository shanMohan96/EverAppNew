using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;

namespace EverNewApp
{
    public partial class frmAccount : Form
    {
        MyDabaseDataContext MyDa;
        public frmAccount()
        {
            InitializeComponent();


        }

        private void frmMangeParty_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            this.WindowState = FormWindowState.Normal;
            txtName.Focus();

        }

        private void frmMangeParty_KeyDown(object sender, KeyEventArgs e)
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
            string sType = "";
            if (!string.IsNullOrEmpty(cmbType.Text.Trim()))
                sType = Convert.ToString(cmbType.Text.Trim());

            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_GET_ACCOUNT '','" + txtName.Text.Trim() + "','','" + sType + "'  ,'" + txtCity.Text.Trim() + "','" + txtMobileNo.Text.Trim() + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dt.Rows.Count > 0)
            {
                ReportDocument RptDoc = new ReportDocument();

                RptDoc.Load(Application.StartupPath + @"\Report\rptAccount.rpt");
                RptDoc.SetDataSource(dt);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Account Listing Report";

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
