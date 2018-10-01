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
    public partial class frmPartyStatement : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmPartyStatement()
        {
            InitializeComponent();
        }

        private void frmManagePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbName, "p");

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
            string TM02_PARTYID = "";
            int iTM02_PARTYID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iTM02_PARTYID);

            if (iTM02_PARTYID > 0)
                TM02_PARTYID = iTM02_PARTYID.ToString();

            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_GET_PARTY_STATEMENT '" + TM02_PARTYID + "','" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dt.Rows.Count > 0)
            {
                var orderedRows = from row in dt.AsEnumerable()
                                  orderby row.Field<DateTime?>("Invoice_Date")
                                  select row;
                DataTable tblOrdered = orderedRows.CopyToDataTable();
                decimal dBal = 0;
                decimal dTotalSale = 0, dTotalPayment = 0;

                for (int i = 0; i < tblOrdered.Rows.Count; i++)
                {
                    decimal dSale_Amount = 0, Recive_Amount = 0;
                    decimal.TryParse(tblOrdered.Rows[i]["Sale_Amount"].ToString(), out dSale_Amount);
                    decimal.TryParse(tblOrdered.Rows[i]["Recive_Amount"].ToString(), out Recive_Amount);

                    dSale_Amount = Math.Abs(dSale_Amount);
                    dTotalSale = dTotalSale + Math.Abs(dSale_Amount);
                    dTotalPayment = dTotalPayment + Math.Abs(Recive_Amount);
                    dBal = dBal + dSale_Amount;
                    dBal = dBal - Recive_Amount;

                    //if (dSale_Amount > 0)
                    //{
                    //    if (dBal > 0)
                    //        dBal = dBal + dSale_Amount;
                    //    else
                    //        dBal = dBal - dSale_Amount;
                    //}
                    //else
                    //{
                    //    if (dBal > 0)
                    //        dBal = dBal - dSale_Amount;
                    //    else
                    //        dBal = dBal + dSale_Amount;
                    //}

                    //if (Recive_Amount > 0)
                    //{
                    //    if (dBal > 0)
                    //        dBal = dBal - Recive_Amount;
                    //    else
                    //        dBal = dBal + Recive_Amount;
                    //}

                    //if (dBal > 0)
                    //{
                    //    dBal = dBal + dSale_Amount;
                    //    dBal = dBal - Recive_Amount;
                    //}
                    //else
                    //{
                    //    dBal = dBal + dSale_Amount;
                    //    dBal = dBal - Recive_Amount;

                    //    dBal = (dBal * -1);
                    //}

                    tblOrdered.Rows[i]["FromDate"] = dtpFromDate.Value.ToString("dd-MM-yyyy");
                    tblOrdered.Rows[i]["ToDate"] = dtpTodate.Value.ToString("dd-MM-yyyy");

                    if (i == 0)
                        tblOrdered.Rows[i]["Sale_Amount"] = 0;
                    else
                        tblOrdered.Rows[i]["Sale_Amount"] = System.Math.Abs(dSale_Amount);

                    tblOrdered.Rows[i]["Balance"] = decimal.Round(dBal, 2);
                    //tblOrdered.Rows[0]["T001_NAME"] = tblOrdered.Rows[1]["T001_NAME"].ToString();
                }

                tblOrdered.Rows[dt.Rows.Count - 1]["TOTALSALE"] = dTotalSale;
                tblOrdered.Rows[dt.Rows.Count - 1]["TOTALPAYMENT"] = dTotalPayment;

                ReportDocument RptDoc = new ReportDocument();

                RptDoc.Load(Application.StartupPath + @"\Report\rptPartyStatement.rpt");
                RptDoc.SetDataSource(tblOrdered);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Account Statement Report";

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
