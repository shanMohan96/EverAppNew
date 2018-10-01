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
    public partial class frmAddUpdateSale : Form
    {

        DataTable dtProducts = new DataTable();
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Sale Details";

        public frmAddUpdateSale()
        {
            InitializeComponent();
        }

        private void frmAddUpdatePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnPrint);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            cmbInvoiceType.SelectedIndex = 0;
            cmbReverseCharges.SelectedIndex = 0;

            dbo.FillStateList(cmbState);
            dbo.FillAccountList(cmbName, "c");
            dbo.FillAccountList(cmbShippedTo, "c");
            dbo.FillItemSetName(cmbItemName);
            if (cmbState.Items.Count > 0)
                cmbState.SelectedIndex = -1;

            cmbUnitName.SelectedIndex = 0;
            PopualteLastBillNo();
            PopuatePrice();

            dtProducts.Columns.Add("rkd", typeof(Int32));
            dtProducts.Columns.Add("TM01_PRODUCTID", typeof(int));
            dtProducts.Columns.Add("TM01_NAME", typeof(string));
            dtProducts.Columns.Add("T003_QTY", typeof(decimal));
            dtProducts.Columns.Add("T003_UNIT", typeof(string));
            dtProducts.Columns.Add("T003_RATE", typeof(decimal));
            dtProducts.Columns.Add("T003_AMOUNT", typeof(decimal));

            dgDisplayData.DataSource = dtProducts;

            dgDisplayData.Columns["rkd"].HeaderText = "No";
            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Item";
            dgDisplayData.Columns["T003_QTY"].HeaderText = "Qty";
            dgDisplayData.Columns["T003_UNIT"].HeaderText = "Unit";
            dgDisplayData.Columns["T003_RATE"].HeaderText = "Price";
            dgDisplayData.Columns["T003_AMOUNT"].HeaderText = "Amount";

            dgDisplayData.Columns["rkd"].DisplayIndex = 0;
            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 1;
            dgDisplayData.Columns["T003_QTY"].DisplayIndex = 2;
            dgDisplayData.Columns["T003_UNIT"].DisplayIndex = 3;
            dgDisplayData.Columns["T003_RATE"].DisplayIndex = 4;
            dgDisplayData.Columns["T003_AMOUNT"].DisplayIndex = 5;

            dgDisplayData.Columns["TM01_PRODUCTID"].Visible = false;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;
            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns["TM01_NAME"].Width = 350;

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
            if (Datalayer.iT007_SALEID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T007_SALE WHERE T007_SALEID=" + Datalayer.iT007_SALEID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbInvoiceType.SelectedItem = Convert.ToString(dt.Rows[0]["T007_TYPE"]);
                    cmbName.SelectedValue = int.Parse(dt.Rows[0]["T001_ACCOUNTID"].ToString());
                    dtpDate.Text = dt.Rows[0]["T007_DATE"].ToString();
                    dtpLRDate.Text = dt.Rows[0]["T007_LR_DATE"].ToString();
                    txtBillPrefix.Text = dt.Rows[0]["T007_INVOICE_PREFIX"].ToString();
                    txtBillNo.Text = dt.Rows[0]["T007_NO"].ToString();
                    txtTotalAmt.Text = dt.Rows[0]["T007_TOTAL_AMT"].ToString();
                    txtNetAmount.Text = dt.Rows[0]["T007_NETAMOUNT"].ToString();
                    txtDetails.Text = dt.Rows[0]["T007_DETAILS"].ToString();
                    txtLR1.Text = dt.Rows[0]["T007_LR1"].ToString();
                    txtTransport1.Text = dt.Rows[0]["T007_TRANSPORT1"].ToString();
                    txtTransport2.Text = dt.Rows[0]["T007_TRANSPORT2"].ToString();
                    txtCGST.Text = dt.Rows[0]["T007_CGST"].ToString();
                    txtSGST.Text = dt.Rows[0]["T007_SGST"].ToString();
                    txtIGST.Text = dt.Rows[0]["T007_IGST"].ToString();
                    txtTotalKG.Text = dt.Rows[0]["T007_TOTAL_KG"].ToString();
                    txtVehicleNo.Text = dt.Rows[0]["T007_VEHICLE_NO"].ToString();
                    cmbReverseCharges.SelectedItem = dt.Rows[0]["T007_IS_REVERSE_CHARGES"].ToString();
                    txtReverseCharges.Text = dt.Rows[0]["T007_REVERSE_CHARGE"].ToString();
                    txtEWayBill.Text = dt.Rows[0]["T007_E_WAY_BILL"].ToString();
                    cmbShippedTo.SelectedValue = int.Parse(dt.Rows[0]["T007_SHIPPED_TO_ID"].ToString());
                    cmbState.SelectedValue = int.Parse(dt.Rows[0]["T007_PLACE_OF_SUPPLY"].ToString());

                    txtDiscount.Text = dt.Rows[0]["T007_DISCOUNT"].ToString();
                    txtPacking.Text = dt.Rows[0]["T007_PACKING"].ToString();
                    txtTaxableAMT.Text = dt.Rows[0]["T007_TAXABLE_AMT"].ToString();

                    DataTable dtItems = new DataTable();
                    dtItems = dl.SelectMethod("SELECT ROW_NUMBER() OVER(ORDER BY T008_SALEITEMID ASC) AS rkd,TM01_NAME,T008_UNIT as T003_UNIT, t008.TM01_PRODUCTID,T008_QTY as T003_QTY,T008_RATE as T003_RATE,T008_NET_AMOUNT as T003_AMOUNT FROM T008_SALEITEM T008 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T008.TM01_PRODUCTID  WHERE T008.T007_SALEID=" + Datalayer.iT007_SALEID);
                    if (dtItems != null && dtItems.Rows.Count > 0)
                    {
                        dtProducts = dtItems;
                        dgDisplayData.DataSource = dtProducts;
                    }

                    cmbName.Focus();
                }
            }
        }

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

        public int Round(double value)
        {
            double decimalpoints = Math.Abs(value - Math.Floor(value));
            if (decimalpoints >= 0.5)
                return (int)Math.Ceiling(value);
            else
                return (int)Math.Floor(value);
        }

        void PopautleGrandTotal()
        {
            decimal iTotal = 0, dTaxAmount = 0;

            for (int i = 0; i < dgDisplayData.Rows.Count; i++)
            {
                decimal dPrice = 0, dTax;
                decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T003_AMOUNT"].Value), out dPrice);
                iTotal = iTotal + dPrice;
                dTaxAmount = dTaxAmount;
            }

            txtTotalAmt.Text = decimal.Round(iTotal, 2).ToString();

            PopulteNetAmount();
        }


        void PopulateNET()
        {
            decimal dTotalAmt = 0, dPacking = 0, dDiscount = 0, dTaxableAMt = 0, dDiscountAMT = 0;
            decimal dTax1 = 0, dTax2 = 0, dTax3 = 0;

            decimal.TryParse(txtTotalAmt.Text.Trim(), out dTotalAmt);
            decimal.TryParse(txtPacking.Text.Trim(), out dPacking);
            decimal.TryParse(txtDiscount.Text.Trim(), out dDiscount);
            dDiscountAMT = ((dTotalAmt + dPacking) * dDiscount) / 100;

            dTaxableAMt = (dTotalAmt + dPacking) - dDiscountAMT;
            txtTaxableAMT.Text = dTaxableAMt.ToString();
            decimal.TryParse(txtCGST.Text.Trim(), out dTax1);
            decimal.TryParse(txtSGST.Text.Trim(), out dTax2);
            decimal.TryParse(txtIGST.Text.Trim(), out dTax3);

            decimal dTax1Amount = 0, dTax2Amount = 0, dTax3Amount = 0;

            dTax1Amount = decimal.Round(((dTaxableAMt * dTax1) / 100), 2);
            dTax2Amount = decimal.Round(((dTaxableAMt * dTax2) / 100), 2);
            dTax3Amount = decimal.Round(((dTaxableAMt * dTax3) / 100), 2);

            //lblCGSTAmount.Text = dTax1Amount.ToString();
            //lblSGSTAmount.Text = dTax2Amount.ToString();
            //lblIGSTAmount.Text = dTax3Amount.ToString();

            decimal dNetAmount = 0;
            dNetAmount = dTaxableAMt + dTax1Amount + dTax2Amount + dTax3Amount;

            // txtNetAmount.Text = Round(double.Parse(dNetAmount.ToString())).ToString();
            txtNetAmount.Text = Math.Round(dNetAmount,2).ToString();
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
            AddUpdatePurchase(false);
        }

        void AddUpdatePurchase(bool IsPrint)
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
                if (string.IsNullOrEmpty(txtBillNo.Text.Trim()))
                {
                    ep1.SetError(txtBillNo, "This field is required.");
                    txtBillNo.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbShippedTo.Text.Trim()))
                {
                    ep1.SetError(cmbShippedTo, "This field is required.");
                    cmbShippedTo.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbState.Text.Trim()))
                {
                    ep1.SetError(cmbState, "This field is required.");
                    cmbState.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbReverseCharges.Text.Trim()))
                {
                    ep1.SetError(cmbReverseCharges, "This field is required.");
                    cmbReverseCharges.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtBillNo.Text.Trim()))
                {
                    ep1.SetError(txtBillNo, "This field is required.");
                    txtBillNo.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtNetAmount.Text.Trim()))
                {
                    ep1.SetError(txtNetAmount, "This field is required.");
                    txtNetAmount.Focus();
                    return;
                }
                if (dgDisplayData.Rows.Count == 0)
                {
                    Datalayer.InformationMessageBox("Record is required.");
                    return;
                }

                int T007_SALEID = Datalayer.iT007_SALEID;
                int T001_ACCOUNTID = 0, T007_SHIPPED_TO_ID = 0, T007_PLACE_OF_SUPPLY = 0;
                string T007_TYPE = "", T007_NO = "", T007_TRANSPORT1 = "", T007_LR1 = "", T007_TRANSPORT2 = "", T007_LR2 = "", T007_DETAILS = "", T007_VEHICLE_NO = "", T007_IS_REVERSE_CHARGES = "", T007_E_WAY_BILL = "";
                decimal T007_TOTAL_AMT = 0, T007_PACKING = 0, T007_FREIGHT = 0, T007_OTHER_EXPENSE = 0, T007_DISCOUNT = 0, T007_CGST = 0, T007_SGST = 0, T007_IGST = 0, T007_NETAMOUNT = 0, T007_REVERSE_CHARGE = 0;
                decimal T007_TAXABLE_AMT = 0;

                int.TryParse(cmbName.SelectedValue.ToString(), out T001_ACCOUNTID);
                T007_TYPE = cmbInvoiceType.SelectedItem.ToString();
                T007_NO = txtBillNo.Text.Trim();
                T007_TRANSPORT1 = txtTransport1.Text.Trim();
                T007_LR1 = txtLR1.Text.Trim();
                T007_TRANSPORT2 = txtTransport2.Text.Trim();
                decimal.TryParse(txtTotalAmt.Text.Trim(), out T007_TOTAL_AMT);
                decimal.TryParse(txtCGST.Text.Trim(), out T007_CGST);
                decimal.TryParse(txtSGST.Text.Trim(), out T007_SGST);
                decimal.TryParse(txtIGST.Text.Trim(), out T007_IGST);
                decimal.TryParse(txtNetAmount.Text.Trim(), out T007_NETAMOUNT);
                decimal.TryParse(txtPacking.Text.Trim(), out T007_PACKING);
                decimal.TryParse(txtDiscount.Text.Trim(), out T007_DISCOUNT);
                decimal.TryParse(txtTaxableAMT.Text.Trim(), out T007_TAXABLE_AMT);

                T007_DETAILS = txtDetails.Text.Trim();
                T007_VEHICLE_NO = txtVehicleNo.Text.Trim();
                T007_IS_REVERSE_CHARGES = cmbReverseCharges.SelectedItem.ToString();
                decimal.TryParse(txtReverseCharges.Text.Trim(), out T007_REVERSE_CHARGE);
                T007_E_WAY_BILL = txtEWayBill.Text.Trim();
                int.TryParse(cmbShippedTo.SelectedValue.ToString(), out T007_SHIPPED_TO_ID);
                int.TryParse(cmbState.SelectedValue.ToString(), out T007_PLACE_OF_SUPPLY);


                if (T007_NETAMOUNT <= 250000 && T007_PLACE_OF_SUPPLY == 22 && T007_TYPE == "B2CL")
                {
                    Datalayer.InformationMessageBox("please choose correct place of supply and provide bill value more than 2.5 lakhs");
                    return;
                }
                if (T007_TYPE == "B2CS" && T007_PLACE_OF_SUPPLY != 33)
                {
                    Datalayer.InformationMessageBox("please choose correct place of supply.");
                    return;
                }


                if (T007_SALEID > 0)
                {
                    int? Iout = 0;
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    MyDa.USP_VP_DELETE_SALEITEM(T007_SALEID, ref Iout);

                }

                int? T007_SALEID_out = 0;
                Cursor.Current = Cursors.WaitCursor;
                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                MyDa.USP_VP_ADDUPDATE_SALE(T007_SALEID, T001_ACCOUNTID, T007_TYPE, T007_NO, dtpDate.Value, T007_TRANSPORT1, T007_LR1, T007_TRANSPORT2, T007_LR2, T007_TOTAL_AMT, T007_PACKING, T007_DISCOUNT, T007_TAXABLE_AMT, T007_CGST, T007_SGST, T007_IGST, T007_NETAMOUNT, T007_DETAILS, Datalayer.iT001_COMPANYID, T007_VEHICLE_NO, T007_IS_REVERSE_CHARGES, T007_REVERSE_CHARGE, T007_E_WAY_BILL, T007_SHIPPED_TO_ID, T007_PLACE_OF_SUPPLY, dtpLRDate.Value, txtBillPrefix.Text.Trim(), txtTotalKG.Text.Trim(), ref T007_SALEID_out);
                if (T007_SALEID_out > 0)
                {
                    T007_SALEID = int.Parse(T007_SALEID_out.Value.ToString());
                    int? T002_PURCHASEITEMID_OUT = 0;
                    for (int i = 0; i < dgDisplayData.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_PRODUCTID"].Value)))
                        {
                            int TM01_PRODUCTID = 0;
                            decimal T003_RATE = 0, T003_AMOUNT = 0;
                            int T003_QTY = 0;
                            string T003_UNIT = "";

                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_PRODUCTID"].Value), out TM01_PRODUCTID);
                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T003_QTY"].Value), out T003_QTY);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T003_RATE"].Value), out T003_RATE);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T003_AMOUNT"].Value), out T003_AMOUNT);
                            T003_UNIT = Convert.ToString(dgDisplayData.Rows[i].Cells["T003_UNIT"].Value);

                            int? T008_SALEITEMID_Out = 0;
                            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                            MyDa.USP_VP_ADDPDATE_SALEITEM(0, T007_SALEID, TM01_PRODUCTID, T003_QTY, T003_RATE, T003_UNIT, T003_AMOUNT, Datalayer.iT001_COMPANYID, ref T008_SALEITEMID_Out);
                        }
                    }

                    if (Datalayer.iT007_SALEID == 0)
                    {
                        Datalayer.InsertMessageBox(sPageName);
                    }
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                        Datalayer.iT007_SALEID = T007_SALEID;
                    }

                    if (IsPrint)
                    {
                        Datalayer.iT007_SALEID = T007_SALEID;

                        DAL dl = new DAL();
                        DataTable dt = new DataTable();
                        dt = dl.SelectMethod("exec USP_VP_PRINT_SALE_BILL '" + Datalayer.iT007_SALEID + "'");
                        if (dt.Rows.Count > 0)
                        {

                            //DataTable dtQTY = new DataTable();
                            //dtQTY = dl.SelectMethod("SELECT TM01_HSNCODE,SUM(T008_QTY)AS QTY FROM T008_SALEITEM T008 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T008.TM01_PRODUCTID WHERE T007_SALEID=" + Datalayer.iT007_SALEID + " GROUP BY  TM01.TM01_HSNCODE ");
                            //if (dtQTY.Rows.Count > 0)
                            //{
                            //    for (int i = 0; i < dt.Rows.Count; i++)
                            //    {
                            //        if (dtQTY.Rows.Count == 1)
                            //        {
                            //            dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                            //            dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();
                            //        }
                            //        if (dtQTY.Rows.Count == 2)
                            //        {
                            //            dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                            //            dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();

                            //            dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[1][0].ToString();
                            //            dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[1][1].ToString();
                            //        }

                            //        if (dtQTY.Rows.Count == 3)
                            //        {
                            //            dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                            //            dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();

                            //            dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[1][0].ToString();
                            //            dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[1][1].ToString();

                            //            dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[2][0].ToString();
                            //            dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[2][1].ToString();
                            //        }
                            //    }
                            //}


                            //dt.Rows[0]["BILLTYPE"] = cmbPrintMode.SelectedItem.ToString(); ;

                            ReportDocument RptDoc = new ReportDocument();

                            RptDoc.Load(Application.StartupPath + @"\Report\rptInvoice.rpt");

                            RptDoc.SetDataSource(dt);

                            crystalReportViewer1.ReportSource = RptDoc;
                            crystalReportViewer1.Refresh();
                            crystalReportViewer1.PrintReport();

                            //Datalayer.RptReport = RptDoc;
                            //Datalayer.sReportName = "Invoice Bill";

                            //Report.frmReportViwer fmReport = new Report.frmReportViwer();
                            //fmReport.Show();
                        }
                        else
                        {
                            Datalayer.InformationMessageBox("No Record..");
                        }
                        //if (Datalayer.ShowQuestMsg("are you sure do you want to print this bill ?"))
                        //{
                        //    Datalayer.iPrintableBillId = T007_SALEID;
                        //    frmPrintBill fmPrnt = new frmPrintBill();
                        //    fmPrnt.Show();
                        //}
                    }

                    this.Close();
                }
                else
                {
                    if (Datalayer.iT007_SALEID == 0)
                        Datalayer.DosenotInsertMessageBox(sPageName);
                    else
                        Datalayer.DosenotUpdateMessageBox(sPageName);
                    return;
                }
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void ResteData()
        {
            Datalayer.iT007_SALEID = 0;

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
                txtTotalAmt.Focus();
                dgDisplayData.ClearSelection();
            }
        }


        void PopualteLastBillNo()
        {
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT T007_NO FROM T007_SALE WHERE TM_COMPAYID=" + Datalayer.iT001_COMPANYID + " ORDER BY T007_SALEID DESC");
            int iNo = 0;
            if (dtData != null && dtData.Rows.Count > 0)
                int.TryParse(dtData.Rows[0][0].ToString(), out iNo);

            txtBillNo.Text = (iNo + 1).ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManageSale fm = new frmManageSale();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveProduct();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            AddUpdatePurchase(true);
        }

        private void frmAddUpdateSale_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.iT007_SALEID = 0;
        }

        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbState.Text.Trim()))
            {
                int iStateId = 0;
                int.TryParse(cmbState.SelectedValue.ToString(), out iStateId);
                if (iStateId > 0)
                {
                    if (iStateId == 33)
                    {
                        txtCGST.Text = "2.5";
                        txtSGST.Text = "2.5";
                        txtIGST.Text = "";
                    }
                    else
                    {
                        txtCGST.Text = "";
                        txtSGST.Text = "";
                        txtIGST.Text = "5";
                    }
                }
            }
        }

        private void lnkSave_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveProduct();
        }

        void SaveProduct()
        {
            ep1.Clear();
            if (string.IsNullOrEmpty(cmbItemName.Text.Trim()))
            {
                ep1.SetError(cmbItemName, "This field is required.");
                cmbItemName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtQty.Text.Trim()))
            {
                ep1.SetError(txtQty, "This field is required.");
                txtQty.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
            {
                ep1.SetError(txtPrice, "This field is required.");
                txtPrice.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtTotal.Text.Trim()))
            {
                ep1.SetError(txtTotal, "This field is required.");
                txtTotal.Focus();
                return;
            }



            decimal dQty = 0, dPrice = 0, dNetAmount = 0;

            decimal.TryParse(txtQty.Text.Trim(), out dQty);
            decimal.TryParse(txtPrice.Text.Trim(), out dPrice);
            decimal.TryParse(txtTotal.Text.Trim(), out dNetAmount);

            int iProductId = 0;
            int.TryParse(cmbItemName.SelectedValue.ToString(), out iProductId);

            DataRow newRows = dtProducts.NewRow();
            newRows["rkd"] = dtProducts.Rows.Count + 1;
            newRows["TM01_PRODUCTID"] = iProductId;
            newRows["TM01_NAME"] = cmbItemName.Text.ToString();
            newRows["T003_QTY"] = dQty;
            newRows["T003_UNIT"] = cmbUnitName.Text.ToString(); ;
            newRows["T003_RATE"] = dPrice;
            newRows["T003_AMOUNT"] = dNetAmount;
            dtProducts.Rows.Add(newRows);

            if (dgDisplayData.Rows.Count > 0)
                dgDisplayData.FirstDisplayedScrollingRowIndex = dgDisplayData.RowCount - 1;

            PopautleGrandTotal();
            ReseItemDetails();

            cmbItemName.Focus();
        }

        void ReseItemDetails()
        {
            cmbItemName.SelectedIndex = 0;
            cmbUnitName.SelectedIndex = 0;
            txtTotal.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            PopulteNetAmount();
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            PopulteNetAmount();
        }

        void PopulteNetAmount()
        {
            decimal dQty = 0, dPrice = 0, dNetAmount = 0;

            decimal.TryParse(txtQty.Text.Trim(), out dQty);
            decimal.TryParse(txtPrice.Text.Trim(), out dPrice);

            txtTotal.Text = (dQty * dPrice).ToString();
        }

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
            {
                int Id = 0;
                int.TryParse(cmbName.SelectedValue.ToString(), out Id);
                if (Id > 0)
                {
                    cmbShippedTo.SelectedValue = Id;
                }

            }
        }

        private void txtCGST_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtSGST_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtIGST_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void cmbReverseCharges_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReverseCharges.Text.Trim() == "y")
                txtReverseCharges.Enabled = true;
            else
                txtReverseCharges.Enabled = false;
        }

        private void cmbItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopuatePrice();
        }

        void PopuatePrice()
        {
            if (!string.IsNullOrEmpty(cmbItemName.Text.Trim()))
            {
                int iItemId = 0;
                int.TryParse(cmbItemName.SelectedValue.ToString(), out iItemId);
                if (iItemId > 0)
                {
                    string sSql = "select TM01_PRICE from TM01_PRODUCT where TM01_PRODUCTID=" + iItemId;
                    DataTable dtData = new DataTable();
                    dtData = dl.SelectMethod(sSql);
                    if (dtData != null && dtData.Rows.Count > 0)
                        txtPrice.Text = dtData.Rows[0][0].ToString();
                    else
                        txtPrice.Text = "";
                }
                else
                    txtPrice.Text = "";
            }
            else
                txtPrice.Text = "";
        }

        private void cmbInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBillPrefix();
        }

        void PopulateBillPrefix()
        {
            if (!string.IsNullOrEmpty(cmbInvoiceType.Text.Trim()))
            {
                string sSql = "select T007_INVOICE_PREFIX from T007_SALE where T007_TYPE='" + cmbInvoiceType.Text.Trim() + "' order by T007_SALEID desc";
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod(sSql);
                if (dtData != null && dtData.Rows.Count > 0)
                    txtBillPrefix.Text = dtData.Rows[0][0].ToString();
                else
                    txtBillPrefix.Text = "";
            }
        }
    }
}
