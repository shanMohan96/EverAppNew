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
    public partial class frmPrintBill : Form
    {
        public frmPrintBill()
        {
            InitializeComponent();
        }

        private void frmPrintBill_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(button2);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);
        }

        private void frmPrintBill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PrintReport(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintReport(2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintReport(3);
        }

        void PrintReport(int no)
        {
            if (Datalayer.iPrintableBillId > 0)
            {
                DAL dl = new DAL();
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("exec USP_VP_PRINT_SALE_BILL '" + Datalayer.iPrintableBillId + "'");
                if (dt.Rows.Count > 0)
                {

                    DataTable dtQTY = new DataTable();
                    dtQTY = dl.SelectMethod("SELECT TM01_HSNCODE,SUM(T008_QTY)AS QTY FROM T008_SALEITEM T008 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T008.TM01_PRODUCTID WHERE T007_SALEID=" + Datalayer.iPrintableBillId + " GROUP BY  TM01.TM01_HSNCODE ");
                    if (dtQTY.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dtQTY.Rows.Count == 1)
                            {
                                dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                                dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();
                            }
                            if (dtQTY.Rows.Count == 2)
                            {
                                dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                                dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();

                                dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[1][0].ToString();
                                dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[1][1].ToString();
                            }

                            if (dtQTY.Rows.Count == 3)
                            {
                                dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                                dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();

                                dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[1][0].ToString();
                                dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[1][1].ToString();

                                dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[2][0].ToString();
                                dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[2][1].ToString();
                            }
                        }
                    }

                    if (no == 1)
                        dt.Rows[0]["BILLTYPE"] = "ORIGNAL";
                    else if (no == 2)
                        dt.Rows[0]["BILLTYPE"] = "DUPLICATE";
                    else if (no == 3)
                        dt.Rows[0]["BILLTYPE"] = "TRIPLICATE";

                    ReportDocument RptDoc = new ReportDocument();
                    if (dt.Rows[0]["TM_STATEID"].ToString() != "24")
                        RptDoc.Load(Application.StartupPath + @"\Report\rptOutGujInvoice.rpt");
                    else
                        RptDoc.Load(Application.StartupPath + @"\Report\rptGujInvoice.rpt");


                    //RptDoc.Load(Application.StartupPath + @"\Report\rptInvoice.rpt");
                    RptDoc.SetDataSource(dt);

                    Datalayer.RptReport = RptDoc;
                    Datalayer.sReportName = "Tax Invoice Bill";

                    Report.frmReportViwer fmReport = new Report.frmReportViwer();
                    fmReport.Show();
                }
                else
                {
                    Datalayer.InformationMessageBox("No Record..");
                }
            }

            if (Datalayer.iPrintableChallenId > 0)
            {
                DAL dl = new DAL();
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("exec USP_VP_PRINT_CHALLEN_BILL '" + Datalayer.iPrintableChallenId + "'");
                if (dt.Rows.Count > 0)
                {

                    if (no == 1)
                        dt.Rows[0]["BILLTYPE"] = "Orignal";
                    else if (no == 2)
                        dt.Rows[0]["BILLTYPE"] = "Duplicate";
                    else if (no == 3)
                        dt.Rows[0]["BILLTYPE"] = "Triplicate";


                    ReportDocument RptDoc = new ReportDocument();
                    RptDoc.Load(Application.StartupPath + @"\Report\rptChallen.rpt");
                    RptDoc.SetDataSource(dt);

                    Datalayer.RptReport = RptDoc;
                    Datalayer.sReportName = "Challen Bill";

                    Report.frmReportViwer fmReport = new Report.frmReportViwer();
                    fmReport.Show();
                }
                else
                {
                    Datalayer.InformationMessageBox("No Record..");
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
