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
    public partial class frmDashBoard : Form
    {
        public frmDashBoard()
        {
            InitializeComponent();
        }

        int iTik = 0;
        private void frmDashBoard_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void frmAccount_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmMangeAccount fmItm = new frmMangeAccount();
            fmItm.MdiParent = this.MdiParent;
            fmItm.Show();
        }

        void CloseAll()
        {
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                childForm.Close();
            }

            timer1.Enabled = false;
        }

        private void frmPurchase_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdatePurchase fmUser = new frmAddUpdatePurchase();
            fmUser.MdiParent = this.MdiParent;
            fmUser.Show();
        }

        void PopuateReprot()
        {
            try
            {
                DAL dl = new DAL();
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("exec USP_RPT_MONTH_SUMMARY '" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Year.ToString() + "','" + Datalayer.iT001_COMPANYID + "'");
                if (dt.Rows.Count > 0)
                {
                    ReportDocument RptDoc = new ReportDocument();

                    RptDoc.Load(Application.StartupPath + @"\Report\rptSummary.rpt");
                    RptDoc.SetDataSource(dt);

                    crystalReportViewer1.ReportSource = RptDoc;
                    crystalReportViewer1.Refresh();
                }
                else
                {
                    crystalReportViewer1.ReportSource = null;
                    crystalReportViewer1.Refresh();
                }
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmOrder fmOrder = new frmOrder();
            fmOrder.MdiParent = this.MdiParent;
            fmOrder.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CloseAll();

            //frmPackingSlip fmpack = new frmPackingSlip();
            //fmpack.MdiParent = this.MdiParent;
            //fmpack.Show();
        }

        private void frmStockIn_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdatePurchase fmPurchase = new frmAddUpdatePurchase();
        }

        private void frmStockIn_Click_1(object sender, EventArgs e)
        {
            CloseAll();

            frmStockIn fmstkin = new frmStockIn();
            fmstkin.MdiParent = this.MdiParent; ;
            fmstkin.Show();
        }

        private void frmSale_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdateSale fmSale = new frmAddUpdateSale();
            fmSale.MdiParent = this.MdiParent; ;
            fmSale.Show();
        }

        private void frmStock_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmStock fmstk = new frmStock();
            fmstk.MdiParent = this.MdiParent; ;
            fmstk.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            iTik = iTik + 1;
            if (iTik == 5)
            {
                PopuateReprot();
                timer1.Enabled = false;
            }

        }
    }
}
