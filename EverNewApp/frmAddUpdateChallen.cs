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
    public partial class frmAddUpdateChallen : Form
    {

        DataTable dtProducts = new DataTable();
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Challen Details";

        public frmAddUpdateChallen()
        {
            InitializeComponent();
        }

        private void frmAddUpdatePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnPrint);
            Datalayer.SetButtion(bntPackingSlip);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);


            dtProducts.Columns.Add("rkd", typeof(Int32));
            dtProducts.Columns.Add("TM01_PRODUCTID", typeof(int));
            dtProducts.Columns.Add("TM01_NAME", typeof(string));
            dtProducts.Columns.Add("TM02_PRODUCTSIZEID", typeof(int));
            dtProducts.Columns.Add("TM02_SIZE", typeof(string));
            dtProducts.Columns.Add("T008_QTY", typeof(int));
            dtProducts.Columns.Add("T008_RATE", typeof(decimal));
            dtProducts.Columns.Add("T008_TOTAL_AMT", typeof(decimal));
            dtProducts.Columns.Add("T008_DISCOUNT_PER", typeof(decimal));
            dtProducts.Columns.Add("T008_DISCOUNT_AMT", typeof(decimal));
            dtProducts.Columns.Add("T008_TAX_RATE", typeof(decimal));
            dtProducts.Columns.Add("T008_TAX_AMT", typeof(decimal));
            dtProducts.Columns.Add("T008_NET_AMOUNT", typeof(decimal));


            dgDisplayData.DataSource = dtProducts;

            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Item";
            dgDisplayData.Columns["TM02_SIZE"].HeaderText = "Finish";
            dgDisplayData.Columns["T008_QTY"].HeaderText = "Qty";
            dgDisplayData.Columns["T008_RATE"].HeaderText = "Rate";
            dgDisplayData.Columns["T008_NET_AMOUNT"].HeaderText = "Amount";


            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["TM02_SIZE"].DisplayIndex = 1;
            dgDisplayData.Columns["T008_QTY"].DisplayIndex = 2;
            dgDisplayData.Columns["T008_RATE"].DisplayIndex = 3;
            dgDisplayData.Columns["T008_NET_AMOUNT"].DisplayIndex = 4;

            dgDisplayData.Columns["rkd"].Visible = false;
            dgDisplayData.Columns["TM01_PRODUCTID"].Visible = false;
            dgDisplayData.Columns["TM02_PRODUCTSIZEID"].Visible = false;
            dgDisplayData.Columns["T008_TOTAL_AMT"].Visible = false;
            dgDisplayData.Columns["T008_DISCOUNT_PER"].Visible = false;
            dgDisplayData.Columns["T008_DISCOUNT_AMT"].Visible = false;
            dgDisplayData.Columns["T008_TAX_RATE"].Visible = false;
            dgDisplayData.Columns["T008_TAX_AMT"].Visible = false;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;
            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns["TM01_NAME"].Width = 350;
            dgDisplayData.Columns["TM02_SIZE"].Width = 120;
            dgDisplayData.Columns["T008_QTY"].Width = 120;
            dgDisplayData.Columns["T008_RATE"].Width = 120;
            dgDisplayData.Columns["T008_NET_AMOUNT"].Width = 120;

            dbo.FillAccountList(cmbName, "c");
            PopualteDiscount();

            PopualteLastBillNo();

            PopualteData();
        }

        private void frmAddUpdatePurchase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");

            if (e.Control && e.KeyCode == Keys.S)
                btnSave_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.N)
                btnSave_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                btnExit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
        }

        void PopualteData()
        {
            if (Datalayer.iT012_CHALLENID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T012_CHALLEN WHERE T012_CHALLENID=" + Datalayer.iT012_CHALLENID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtItems = new DataTable();
                    dtItems = dl.SelectMethod("SELECT ROW_NUMBER() OVER(ORDER BY T012_CHALLENITEMID ASC) AS rkd,TM01_NAME,TM02_SIZE ,T008.TM01_PRODUCTID,T008.TM02_PRODUCTSIZEID ,T012_QTY AS T008_QTY,T012_RATE AS T008_RATE,T012_NET_AMOUNT AS T008_NET_AMOUNT,T012_TOTAL_AMT AS T008_TOTAL_AMT,T012_DISCOUNT_PER AS T008_DISCOUNT_PER,T012_DISCOUNT_AMT AS T008_DISCOUNT_AMT,T012_TAX_RATE AS T008_TAX_RATE,T012_TAX_AMT AS T008_TAX_AMT FROM T012_CHALLENITEM T008 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T008.TM01_PRODUCTID INNER JOIN TM02_PRODUCTSIZE TM02 ON TM02.TM02_PRODUCTSIZEID = T008.TM02_PRODUCTSIZEID WHERE T008.T012_CHALLENID=" + Datalayer.iT012_CHALLENID);
                    if (dtItems != null && dtItems.Rows.Count > 0)
                    {
                        dtProducts = dtItems;
                        dgDisplayData.DataSource = dtProducts;
                    }

                    PopautleGrandTotal();

                    cmbName.SelectedValue = int.Parse(dt.Rows[0]["T001_ACCOUNTID"].ToString());
                    dtpDate.Text = dt.Rows[0]["T012_DATE"].ToString();
                    txtBillNo.Text = dt.Rows[0]["T012_NO"].ToString();
                    txtTotalAmt.Text = dt.Rows[0]["T012_TOTAL_AMT"].ToString();
                    txtDiscount.Text = dt.Rows[0]["T012_DISCOUNT"].ToString();
                    txtTotalTaxAmount.Text = dt.Rows[0]["T012_TAX"].ToString();
                    txtNetAmount.Text = dt.Rows[0]["T012_NETAMOUNT"].ToString();
                    txtDetails.Text = dt.Rows[0]["T012_DETAILS"].ToString();
                    txtLR1.Text = dt.Rows[0]["T012_LR1"].ToString();
                    txtLR2.Text = dt.Rows[0]["T012_LR2"].ToString();
                    txtTransport1.Text = dt.Rows[0]["T012_TRANSPORT1"].ToString();
                    txtTransport2.Text = dt.Rows[0]["T012_TRANSPORT2"].ToString();
                    txtPacking.Text = dt.Rows[0]["T012_PACKING"].ToString();
                    txtFreight.Text = dt.Rows[0]["T012_FREIGHT"].ToString();
                    txtExpense.Text = dt.Rows[0]["T012_OTHER_EXPENSE"].ToString();

                    cmbName.Focus();
                }
            }
        }


        //void PopulateProductNetAmount()
        //{
        //    decimal dQty = 0, dPurchasePrice = 0, dTotal = 0;
        //    decimal.TryParse(txtProductQty.Text.Trim(), out dQty);
        //    decimal.TryParse(txtProductSalePrice.Text.Trim(), out dPurchasePrice);
        //    dTotal = decimal.Round(dQty * dPurchasePrice, 2);
        //    lblTotalAmount.Text = dTotal.ToString();
        //    decimal dDiscountRate = 0, dDiscounAmt = 0;
        //    decimal.TryParse(txtDiscountRate.Text.Trim(), out dDiscountRate);
        //    dDiscounAmt = (dTotal * dDiscountRate) / 100;
        //    lblDiscountAmount.Text = decimal.Round(dDiscounAmt, 2).ToString();

        //    decimal dTaxRate = 0, dTaxAmount = 0;
        //    decimal.TryParse(txtTaxRate.Text.Trim(), out dTaxRate);
        //    decimal dAmt = dTotal - dDiscounAmt;
        //    dTaxAmount = (dAmt * dTaxRate) / 100;
        //    lblTaxAmount.Text = decimal.Round(dTaxAmount, 2).ToString();

        //    decimal dNetAmt = (dAmt + dTaxAmount);
        //    txtProductNetAmount.Text = decimal.Round(dNetAmt, 2).ToString();
        //}


        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '£')
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        //void AddProduct()
        //{
        //    try
        //    {
        //        ep1.Clear();
        //        if (string.IsNullOrEmpty(cmbItemName.Text.Trim()))
        //        {
        //            ep1.SetError(cmbItemName, "This field is required.");
        //            cmbItemName.Focus();
        //            return;
        //        }
        //        if (string.IsNullOrEmpty(cmbMainItemSize.Text.Trim()))
        //        {
        //            ep1.SetError(cmbMainItemSize, "This field is required.");
        //            cmbMainItemSize.Focus();
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(txtProductQty.Text.Trim()))
        //        {
        //            ep1.SetError(txtProductQty, "This field is required.");
        //            txtProductQty.Focus();
        //            return;
        //        }
        //        if (string.IsNullOrEmpty(txtProductSalePrice.Text.Trim()))
        //        {
        //            ep1.SetError(txtProductSalePrice, "This field is required.");
        //            txtProductSalePrice.Focus();
        //            return;
        //        }
        //        if (string.IsNullOrEmpty(txtProductNetAmount.Text.Trim()))
        //        {
        //            ep1.SetError(txtProductNetAmount, "This field is required.");
        //            txtProductNetAmount.Focus();
        //            return;
        //        }

        //        int rkd = 0, TM01_PRODUCTID = 0, TM02_PRODUCTSIZEID = 0;
        //        decimal T008_QTY = 0, T008_RATE = 0, T008_NET_AMOUNT = 0;


        //        int.TryParse(lblProductId.Text.Trim(), out rkd);
        //        int.TryParse(cmbItemName.SelectedValue.ToString(), out TM01_PRODUCTID);
        //        int.TryParse(cmbMainItemSize.SelectedValue.ToString(), out TM02_PRODUCTSIZEID);

        //        decimal.TryParse(txtProductQty.Text.Trim(), out T008_QTY);
        //        decimal.TryParse(txtProductSalePrice.Text.Trim(), out T008_RATE);
        //        decimal.TryParse(txtProductNetAmount.Text.Trim(), out T008_NET_AMOUNT);

        //        decimal T008_TOTAL_AMT = 0, T008_DISCOUNT_PER = 0, T008_DISCOUNT_AMT = 0, T008_TAX_RATE = 0, T008_TAX_AMT = 0;
        //        decimal.TryParse(lblTotalAmount.Text.Trim(), out T008_TOTAL_AMT);
        //        decimal.TryParse(txtDiscountRate.Text.Trim(), out T008_DISCOUNT_PER);
        //        decimal.TryParse(lblDiscountAmount.Text.Trim(), out T008_DISCOUNT_AMT);
        //        decimal.TryParse(txtTaxRate.Text.Trim(), out T008_TAX_RATE);
        //        decimal.TryParse(lblTaxAmount.Text.Trim(), out T008_TAX_AMT);

        //        if (rkd == 0)
        //        {
        //            DataRow newRows = dtProducts.NewRow();
        //            newRows["rkd"] = dtProducts.Rows.Count + 1;
        //            newRows["T008_QTY"] = T008_QTY;
        //            newRows["T008_RATE"] = T008_RATE;
        //            newRows["T008_NET_AMOUNT"] = T008_NET_AMOUNT;
        //            newRows["TM01_PRODUCTID"] = TM01_PRODUCTID;
        //            newRows["TM01_NAME"] = cmbItemName.Text.Trim();
        //            newRows["TM02_PRODUCTSIZEID"] = TM02_PRODUCTSIZEID;
        //            newRows["TM02_SIZE"] = cmbMainItemSize.Text.Trim();
        //            newRows["T008_TOTAL_AMT"] = T008_TOTAL_AMT;
        //            newRows["T008_DISCOUNT_PER"] = T008_DISCOUNT_PER;
        //            newRows["T008_DISCOUNT_AMT"] = T008_DISCOUNT_AMT;
        //            newRows["T008_TAX_RATE"] = T008_TAX_RATE;
        //            newRows["T008_TAX_AMT"] = T008_TAX_AMT;

        //            dtProducts.Rows.Add(newRows);
        //        }
        //        else
        //        {
        //            DataRow newRows = dtProducts.Select("rkd=" + rkd + "").FirstOrDefault();
        //            if (newRows != null)
        //            {
        //                newRows["T008_QTY"] = T008_QTY;
        //                newRows["T008_QTY"] = T008_QTY;
        //                newRows["T008_RATE"] = T008_RATE;
        //                newRows["T008_NET_AMOUNT"] = T008_NET_AMOUNT;
        //                newRows["TM01_PRODUCTID"] = TM01_PRODUCTID;
        //                newRows["TM01_NAME"] = cmbItemName.Text.Trim();
        //                newRows["TM02_PRODUCTSIZEID"] = TM02_PRODUCTSIZEID;
        //                newRows["TM02_SIZE"] = cmbMainItemSize.Text.Trim();
        //                newRows["T008_TOTAL_AMT"] = T008_TOTAL_AMT;
        //                newRows["T008_DISCOUNT_PER"] = T008_DISCOUNT_PER;
        //                newRows["T008_DISCOUNT_AMT"] = T008_DISCOUNT_AMT;
        //                newRows["T008_TAX_RATE"] = T008_TAX_RATE;
        //                newRows["T008_TAX_AMT"] = T008_TAX_AMT;
        //            }
        //        }

        //        dgDisplayData.DataSource = dtProducts;
        //        PopautleGrandTotal();
        //        Datalayer.Reset(groupBox1.Controls);
        //        lblProductId.Text = "0";
        //        cmbItemName.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
        //    }
        //}

        void PopautleGrandTotal()
        {
            decimal iTotal = 0, dTaxAmount = 0;
            int iTotalItems = 0;

            for (int i = 0; i < dgDisplayData.Rows.Count; i++)
            {
                decimal dPrice = 0, dTax;
                int iItem = 0;
                int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_QTY"].Value), out iItem);

                decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_NET_AMOUNT"].Value), out dPrice);
                decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_TAX_AMT"].Value), out dTax);
                iTotal = iTotal + dPrice;
                dTaxAmount = dTaxAmount + dTax;
                iTotalItems = iTotalItems + iItem;
            }

            txtTotalAmt.Text = iTotal.ToString();
            lblTotalItem.Text = iTotalItems.ToString();
            txtTotalTaxAmount.Text = dTaxAmount.ToString();

            PopulateNET();
        }

        void PopulateNET()
        {
            decimal dTotalAmt = 0, dDiscount = 0;
            decimal dT007_PACKING = 0, dT007_FREIGHT = 0, dT007_OTHER_EXPENSE = 0;
            decimal.TryParse(txtPacking.Text.Trim(), out dT007_PACKING);
            decimal.TryParse(txtFreight.Text.Trim(), out dT007_FREIGHT);
            decimal.TryParse(txtExpense.Text.Trim(), out dT007_OTHER_EXPENSE);

            decimal.TryParse(txtTotalAmt.Text.Trim(), out dTotalAmt);
            decimal.TryParse(txtDiscount.Text.Trim(), out dDiscount);
            decimal dT007_TAX = 0;
            decimal.TryParse(txtTotalTaxAmount.Text.Trim(), out dT007_TAX);

            dTotalAmt = dTotalAmt + dT007_PACKING + dT007_FREIGHT + dT007_OTHER_EXPENSE + dT007_TAX;
            dTotalAmt = dTotalAmt - dDiscount;

            //decimal iTotal = 0, iTax = 0, iTax1 = 0, iTax2 = 0, iDis = 0;
            //decimal.TryParse(txtTotalCGST.Text.Trim(), out iTax);
            //decimal.TryParse(txtTotalSGST.Text.Trim(), out iTax1);

            //decimal dVat1Amt = 0, dVat2Amt = 0, dVat3Amt = 0;
            //dVat1Amt = decimal.Round((dTotalAmt * iTax) / 100, 2);
            //dVat2Amt = decimal.Round((dTotalAmt * iTax1) / 100, 2);
            //dVat3Amt = decimal.Round((dTotalAmt * iTax2) / 100, 2);

            //lblCGSTAMT.Text = dVat1Amt.ToString();
            //lblSGSTAMT.Text = dVat2Amt.ToString();

            txtNetAmount.Text = decimal.Round((dTotalAmt), 2).ToString();
        }

        private void txtTotalAmt_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtTotalCGST_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtTotalSGST_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdatePurchase(false, false);
        }

        void AddUpdatePurchase(bool IsPrint, bool IsPackingSlip)
        {

            try
            {
                ep1.Clear();
                if (string.IsNullOrEmpty(cmbName.Text.Trim()))
                {
                    ep1.SetError(cmbName, "This field is required.");
                    cmbName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtNetAmount.Text.Trim()))
                {
                    ep1.SetError(txtNetAmount, "This field is required.");
                    txtNetAmount.Focus();
                    return;
                }

                int T007_SALEID = Datalayer.iT012_CHALLENID;

                // int TM01_CUSTOMERID = 0;
                string T007_NO = "", T007_DETAILS = "";
                int T001_ACCOUNTID = 0;
                decimal T007_TOTAL_AMT = 0, T007_DISCOUNT = 0, T007_TAX1RATE = 0, T007_TAX1AMOUNT = 0, T007_TAX2RATE = 0, T007_TAX2AMOUNT = 0, T007_NETAMOUNT = 0, T007_TAX;
                DateTime T003_DATE = dtpDate.Value;

                // int.TryParse(cmbCustomerName.SelectedValue.ToString(), out TM01_CUSTOMERID);
                T007_NO = txtBillNo.Text.Trim();
                int.TryParse(cmbName.SelectedValue.ToString(), out T001_ACCOUNTID);
                decimal.TryParse(txtTotalAmt.Text.Trim(), out T007_TOTAL_AMT);
                decimal.TryParse(txtDiscount.Text.Trim(), out T007_DISCOUNT);
                decimal.TryParse(txtTotalTaxAmount.Text.Trim(), out T007_TAX);
                decimal.TryParse(txtNetAmount.Text.Trim(), out T007_NETAMOUNT);
                T007_DETAILS = txtDetails.Text.Trim();

                if (T007_SALEID > 0)
                {
                    int? Iout = 0;
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    MyDa.USP_VP_DELETE_CHALLENITEM(T007_SALEID, ref Iout);

                    //MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    //string sSql = "DELETE FROM T003_PURCHASEITEM WHERE T002_PURCHASEID =" + T007_SALEID;
                    //dl.ExecuteMethod(sSql);
                }

                decimal dT007_PACKING = 0, dT007_FREIGHT = 0, dT007_OTHER_EXPENSE = 0;
                decimal.TryParse(txtPacking.Text.Trim(), out dT007_PACKING);
                decimal.TryParse(txtFreight.Text.Trim(), out dT007_FREIGHT);
                decimal.TryParse(txtExpense.Text.Trim(), out dT007_OTHER_EXPENSE);

                int? T007_SALEID_out = 0;
                Cursor.Current = Cursors.WaitCursor;
                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                MyDa.USP_VP_ADDUPATE_CHALLEN(T007_SALEID, T001_ACCOUNTID, T007_NO, T003_DATE, txtTransport1.Text.Trim(), txtLR1.Text.Trim(), txtTransport2.Text.Trim(), txtLR2.Text.Trim(), T007_TOTAL_AMT, dT007_PACKING, dT007_FREIGHT, dT007_OTHER_EXPENSE, T007_DISCOUNT, T007_TAX, T007_NETAMOUNT, T007_DETAILS, Datalayer.iT001_COMPANYID, ref T007_SALEID_out);
                if (T007_SALEID_out > 0)
                {
                    T007_SALEID = int.Parse(T007_SALEID_out.Value.ToString());
                    int? T002_PURCHASEITEMID_OUT = 0;
                    for (int i = 0; i < dgDisplayData.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_NAME"].Value)))
                        {
                            int TM01_PRODUCTID = 0, TM02_PRODUCTSIZEID = 0, T008_QTY = 0;
                            decimal T008_RATE = 0, T008_NET_AMOUNT = 0;

                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_PRODUCTID"].Value), out TM01_PRODUCTID);
                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["TM02_PRODUCTSIZEID"].Value), out TM02_PRODUCTSIZEID);
                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_QTY"].Value), out T008_QTY);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_RATE"].Value), out T008_RATE);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_NET_AMOUNT"].Value), out T008_NET_AMOUNT);

                            decimal T008_TOTAL_AMT = 0, T008_DISCOUNT_PER = 0, T008_DISCOUNT_AMT = 0, T008_TAX_RATE = 0, T008_TAX_AMT = 0;
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_TOTAL_AMT"].Value), out T008_TOTAL_AMT);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_DISCOUNT_PER"].Value), out T008_DISCOUNT_PER);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_DISCOUNT_AMT"].Value), out T008_DISCOUNT_AMT);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_TAX_RATE"].Value), out T008_TAX_RATE);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T008_TAX_AMT"].Value), out T008_TAX_AMT);

                            int? T008_SALEITEMID_Out = 0;
                            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                            MyDa.USP_VP_ADDUPDATE_CHALLEN_ITEM(0, T007_SALEID, TM01_PRODUCTID, TM02_PRODUCTSIZEID, T008_QTY, T008_RATE, T008_TOTAL_AMT, T008_DISCOUNT_PER, T008_DISCOUNT_AMT, T008_TAX_RATE, T008_TAX_AMT, T008_NET_AMOUNT, Datalayer.iT001_COMPANYID, ref T008_SALEITEMID_Out);
                        }
                    }

                    if (Datalayer.iT012_CHALLENID == 0)
                    {
                        Datalayer.InsertMessageBox(sPageName);
                        ResteData();

                        if (IsPrint)
                        {
                            Datalayer.iPrintableChallenId = T007_SALEID;
                            frmPrintBill fmPrnt = new frmPrintBill();
                            fmPrnt.Show();
                        }
                        if (IsPackingSlip)
                        {
                            DAL dl = new DAL();
                            DataTable dt = new DataTable();
                            dt = dl.SelectMethod("exec USP_VP_PRINT_CHALLEN_BILL '" + T007_SALEID + "'");
                            if (dt.Rows.Count > 0)
                            {

                                ReportDocument RptDoc = new ReportDocument();
                                RptDoc.Load(Application.StartupPath + @"\Report\rptPackingSlipsOnInvoice.rpt");
                                RptDoc.SetDataSource(dt);

                                crystalReportViewer1.ReportSource = RptDoc;
                                crystalReportViewer1.Refresh();
                                crystalReportViewer1.PrintReport();

                                //Datalayer.RptReport = RptDoc;
                                //Datalayer.sReportName = "Packing Slip";

                                //Report.frmReportViwer fmReport = new Report.frmReportViwer();
                                //fmReport.Show();
                            }
                            else
                            {
                                Datalayer.InformationMessageBox("No Record..");
                            }
                        }
                        //DataTable dt = new DataTable();
                        //dt = dl.SelectMethod("exec USP_VP_GET_FULLSALEBILL '" + T003_SALEID + "'");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    ReportDocument RptDoc = new ReportDocument();

                        //    RptDoc.Load(Application.StartupPath + @"\Report\rptRetailInvoice.rpt");
                        //    RptDoc.SetDataSource(dt);

                        //    Datalayer.RptReport = RptDoc;
                        //    Datalayer.sReportName = "Invoice";

                        //    frmReportViewer fmReport = new frmReportViewer();
                        //    fmReport.Show();
                        //}
                    }
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);

                        if (IsPrint)
                        {
                            Datalayer.iPrintableChallenId = T007_SALEID;
                            frmPrintBill fmPrnt = new frmPrintBill();
                            fmPrnt.Show();
                        }
                        if (IsPackingSlip)
                        {
                            DAL dl = new DAL();
                            DataTable dt = new DataTable();
                            dt = dl.SelectMethod("exec USP_VP_PRINT_CHALLEN_BILL '" + T007_SALEID + "'");
                            if (dt.Rows.Count > 0)
                            {

                                ReportDocument RptDoc = new ReportDocument();
                                RptDoc.Load(Application.StartupPath + @"\Report\rptPackingSlipsOnInvoice.rpt");
                                RptDoc.SetDataSource(dt);

                                crystalReportViewer1.ReportSource = RptDoc;
                                crystalReportViewer1.Refresh();
                                crystalReportViewer1.PrintReport();

                                //Datalayer.RptReport = RptDoc;
                                //Datalayer.sReportName = "Packing Slip";

                                //Report.frmReportViwer fmReport = new Report.frmReportViwer();
                                //fmReport.Show();
                            }
                            else
                            {
                                Datalayer.InformationMessageBox("No Record..");
                            }
                        }
                    }
                }
                else
                {
                    if (Datalayer.iT012_CHALLENID == 0)
                        Datalayer.DosenotInsertMessageBox(sPageName);
                    else
                        Datalayer.DosenotUpdateMessageBox(sPageName);
                    return;
                }

                ResteData();
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void ResteData()
        {
            Datalayer.iT012_CHALLENID = 0;

            Datalayer.Reset(panel2.Controls);
            Datalayer.Reset(panel3.Controls);
            Datalayer.Reset(panel4.Controls);
            dtProducts.Clear();
            Cursor.Current = Cursors.Default;
            dgDisplayData.DataSource = dtProducts;
            PopualteLastBillNo();
            cmbName.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResteData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        private void dgDisplayData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (Datalayer.ShowQuestMsg(Datalayer.sMessageConfirmation))
                {
                    dgDisplayData.Rows.Remove(dgDisplayData.CurrentRow);
                    PopautleGrandTotal();
                }
            }
            if (e.KeyCode == Keys.Tab)
            {
                txtBillNo.Focus();
                dgDisplayData.ClearSelection();
            }
        }

        private void cmbMainItemSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopuatlePrise();
        }

        private void cmbItemName_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }

        private void cmbMainItemSize_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        void PopualteLastBillNo()
        {
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT T012_NO FROM T012_CHALLEN WHERE TM_COMPAYID=" + Datalayer.iT001_COMPANYID + " ORDER BY T012_CHALLENID DESC");
            int iNo = 0;
            if (dtData != null && dtData.Rows.Count > 0)
                int.TryParse(dtData.Rows[0][0].ToString(), out iNo);

            txtBillNo.Text = (iNo + 1).ToString();
        }

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopualteDiscount();
        }

        void PopualteDiscount()
        {
            int iT001_ACCOUNTID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
            {
                int.TryParse(cmbName.SelectedValue.ToString(), out iT001_ACCOUNTID);

                if (iT001_ACCOUNTID > 0)
                {
                    DataTable dtData = new DataTable();
                    dtData = dl.SelectMethod("SELECT T001_DISCOUNT FROM T001_ACCOUNT WHERE T001_ACCOUNTID=" + iT001_ACCOUNTID);
                    int iNo = 0;
                    if (dtData != null && dtData.Rows.Count > 0)
                        txtDiscountRate.Text = dtData.Rows[0][0].ToString();
                    else
                        txtDiscountRate.Text = "";
                }
            }
        }

        void PopulateItems()
        {
            string sItemNo = txtItemNo.Text.Trim();
            int iTM01_PRODUCTID = 0;
            if (!string.IsNullOrEmpty(cmbItemName.Text.Trim()))
            {
                int.TryParse(cmbItemName.SelectedValue.ToString(), out iTM01_PRODUCTID);
                if (iTM01_PRODUCTID > 0)
                {
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    List<USP_VP_GET_PRODUCT_FOR_CHALLEN_ON_IDResult> lst = new List<USP_VP_GET_PRODUCT_FOR_CHALLEN_ON_IDResult>();
                    lst = MyDa.USP_VP_GET_PRODUCT_FOR_CHALLEN_ON_ID(iTM01_PRODUCTID.ToString()).ToList();
                    if (lst != null && lst.Count > 0)
                    {
                        dgItems.DataSource = lst;
                        //lblItemName.Text = lst[0].TM01_NAME.ToString();

                        dgItems.Columns["TM01_NAME"].Visible = false;

                        dgItems.Columns["TM01_TAX_RATE"].Visible = false;

                        dgItems.Columns["T006_STOCK"].HeaderText = "Stock";
                        dgItems.Columns["TM01_NAME"].HeaderText = "Item";
                        dgItems.Columns["TM02_SIZE"].HeaderText = "Finish";
                        dgItems.Columns["TM02_PRICE"].HeaderText = "Price";
                        dgItems.Columns["QTY"].HeaderText = "Qty";


                        dgItems.Columns["TM02_SIZE"].DisplayIndex = 1;
                        dgItems.Columns["T006_STOCK"].DisplayIndex = 2;
                        dgItems.Columns["TM02_PRICE"].DisplayIndex = 3;
                        dgItems.Columns["QTY"].DisplayIndex = 4;

                        dgItems.Columns["TM02_SIZE"].ReadOnly = true;
                        dgItems.Columns["T006_STOCK"].ReadOnly = true;

                        dgItems.Columns["TM01_PRODUCTID"].Visible = false;
                        dgItems.Columns["TM02_PRODUCTSIZEID"].Visible = false;

                        this.dgItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
                        dgItems.EnableHeadersVisualStyles = false;
                        this.dgItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        dgItems.ColumnHeadersHeight = 30;
                        dgItems.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                        dgItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                        dgItems.Columns["TM01_NAME"].Width = 350;
                        dgItems.Columns["TM02_SIZE"].Width = 120;
                        dgItems.Columns["T006_STOCK"].Width = 120;
                        dgItems.Columns["QTY"].Width = 150;
                    }
                    else
                    {
                        cmbItemName.Text = "";
                        Datalayer.InformationMessageBox(sItemNo + " not exist");
                    }
                }
                else
                {
                    dgItems.DataSource = null;
                }
            }
            else
            {
                dgItems.DataSource = null;
            }
        }

        private void txtItemNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dbo.FillItemOnNo(txtItemNo.Text.Trim(), cmbItemName);
                PopulateItems();
            }
        }

        private void dgItems_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgItems.ClearSelection();
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveItems();
        }

        void SaveItems()
        {
            if (dgItems.Rows.Count > 0)
            {
                for (int i = 0; i < dgItems.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dgItems.Rows[i].Cells["QTY"].Value)))
                    {
                        int rkd = 0, TM01_PRODUCTID = 0, TM02_PRODUCTSIZEID = 0;
                        decimal T008_QTY = 0, T008_RATE = 0, T008_NET_AMOUNT = 0, TotalAmt = 0;

                        decimal.TryParse((Convert.ToString(dgItems.Rows[i].Cells["QTY"].Value)), out T008_QTY);
                        if (T008_QTY > 0)
                        {
                            string TM02_SIZE = (Convert.ToString(dgItems.Rows[i].Cells["TM02_SIZE"].Value));
                            int.TryParse((Convert.ToString(dgItems.Rows[i].Cells["TM01_PRODUCTID"].Value)), out TM01_PRODUCTID);
                            int.TryParse((Convert.ToString(dgItems.Rows[i].Cells["TM02_PRODUCTSIZEID"].Value)), out TM02_PRODUCTSIZEID);
                            decimal.TryParse((Convert.ToString(dgItems.Rows[i].Cells["TM02_PRICE"].Value)), out T008_RATE);
                            TotalAmt = T008_RATE * T008_QTY;

                            decimal dDiscountRate = 0, dDiscounAmt = 0;
                            decimal.TryParse(txtDiscountRate.Text.Trim(), out dDiscountRate);
                            dDiscounAmt = (TotalAmt * dDiscountRate) / 100;
                            lblDiscountAmount.Text = decimal.Round(dDiscounAmt, 2).ToString();

                            decimal T008_TOTAL_AMT = 0, T008_DISCOUNT_PER = 0, T008_DISCOUNT_AMT = 0, T008_TAX_RATE = 0, T008_TAX_AMT = 0;
                            T008_TOTAL_AMT = TotalAmt;
                            decimal.TryParse(txtDiscountRate.Text.Trim(), out T008_DISCOUNT_PER);
                            decimal.TryParse(lblDiscountAmount.Text.Trim(), out T008_DISCOUNT_AMT);
                            decimal.TryParse(txtTaxRate.Text.Trim(), out T008_TAX_RATE);
                            decimal.TryParse(lblTaxAmount.Text.Trim(), out T008_TAX_AMT);

                            T008_NET_AMOUNT = TotalAmt - T008_DISCOUNT_AMT;



                            if (rkd == 0)
                            {
                                DataRow newRows = dtProducts.NewRow();
                                newRows["rkd"] = dtProducts.Rows.Count + 1;
                                newRows["T008_QTY"] = T008_QTY;
                                newRows["T008_RATE"] = T008_RATE;
                                newRows["T008_NET_AMOUNT"] = T008_NET_AMOUNT;
                                newRows["TM01_PRODUCTID"] = TM01_PRODUCTID;
                                newRows["TM01_NAME"] = cmbItemName.Text.ToString();
                                newRows["TM02_PRODUCTSIZEID"] = TM02_PRODUCTSIZEID;
                                newRows["TM02_SIZE"] = TM02_SIZE;
                                newRows["T008_TOTAL_AMT"] = T008_TOTAL_AMT;
                                newRows["T008_DISCOUNT_PER"] = T008_DISCOUNT_PER;
                                newRows["T008_DISCOUNT_AMT"] = T008_DISCOUNT_AMT;
                                newRows["T008_TAX_RATE"] = T008_TAX_RATE;
                                newRows["T008_TAX_AMT"] = T008_TAX_AMT;

                                dtProducts.Rows.Add(newRows);
                            }
                            //else
                            //{
                            //    DataRow newRows = dtProducts.Select("rkd=" + rkd + "").FirstOrDefault();
                            //    if (newRows != null)
                            //    {
                            //        newRows["T008_QTY"] = T008_QTY;
                            //        newRows["T008_QTY"] = T008_QTY;
                            //        newRows["T008_RATE"] = T008_RATE;
                            //        newRows["T008_NET_AMOUNT"] = T008_NET_AMOUNT;
                            //        newRows["TM01_PRODUCTID"] = TM01_PRODUCTID;
                            //        newRows["TM01_NAME"] = cmbItemName.Text.Trim();
                            //        newRows["TM02_PRODUCTSIZEID"] = TM02_PRODUCTSIZEID;
                            //        newRows["TM02_SIZE"] = cmbMainItemSize.Text.Trim();
                            //        newRows["T008_TOTAL_AMT"] = T008_TOTAL_AMT;
                            //        newRows["T008_DISCOUNT_PER"] = T008_DISCOUNT_PER;
                            //        newRows["T008_DISCOUNT_AMT"] = T008_DISCOUNT_AMT;
                            //        newRows["T008_TAX_RATE"] = T008_TAX_RATE;
                            //        newRows["T008_TAX_AMT"] = T008_TAX_AMT;
                            //    }
                            //}
                        }
                    }
                }

                dgDisplayData.DataSource = dtProducts;
                PopautleGrandTotal();
                txtItemNo.Text = "";

                
                cmbItemName.DataSource = null;
                dgItems.DataSource = null;
                txtDiscountRate.Text = "";

                txtItemNo.Focus();
            }
        }

        private void lnkSave_Enter(object sender, EventArgs e)
        {
            // SaveItems();
        }

        private void frmAddUpdateChallen_FormClosed(object sender, FormClosedEventArgs e)
        {
            Datalayer.iT012_CHALLENID = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {


            frmManageChallen fm = new frmManageChallen();
            fm.MdiParent = this.MdiParent;
            fm.Show();
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            AddUpdatePurchase(true, false);
        }

        private void bntPackingSlip_Click(object sender, EventArgs e)
        {
            AddUpdatePurchase(false, true);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateItems();
        }

        private void txtItemNo_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
