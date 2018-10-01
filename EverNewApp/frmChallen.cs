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
    public partial class frmChallen : Form
    {
        DataTable dtProducts = new DataTable();
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Challen Details";

        public frmChallen()
        {
            InitializeComponent();
        }

        private void frmChallen_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnPrint);
            Datalayer.SetButtion(btnView);
            Datalayer.SetButtion(bntPackingSlip);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);


            dtProducts.Columns.Add("rkd", typeof(Int32));
            dtProducts.Columns.Add("TM01_PRODUCTID", typeof(int));
            dtProducts.Columns.Add("TM01_NO", typeof(string));
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

            AutoCompliteName();

            dgDisplayData.DataSource = dtProducts;


            dbo.FillAccountList(cmbName, "c");
            PopualteDiscount();

            PopualteLastBillNo();

            PopualteData();
        }

        private void frmChallen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");

            //if (e.Control && e.KeyCode == Keys.S)
            //    btnSave_Click(sender, e);
            //if (e.Control && e.KeyCode == Keys.N)
            //    btnSave_Click(sender, e);
            //if (e.Control && e.KeyCode == Keys.X)
            //    btnExit_Click(sender, e);
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
                    dtItems = dl.SelectMethod("SELECT ROW_NUMBER() OVER(ORDER BY T012_CHALLENITEMID ASC) AS rkd,TM01_NAME,TM01_NO,TM02_SIZE ,T008.TM01_PRODUCTID,T008.TM02_PRODUCTSIZEID ,T012_QTY AS T008_QTY,T012_RATE AS T008_RATE,T012_NET_AMOUNT AS T008_NET_AMOUNT,T012_TOTAL_AMT AS T008_TOTAL_AMT,T012_DISCOUNT_PER AS T008_DISCOUNT_PER,T012_DISCOUNT_AMT AS T008_DISCOUNT_AMT,T012_TAX_RATE AS T008_TAX_RATE,T012_TAX_AMT AS T008_TAX_AMT FROM T012_CHALLENITEM T008 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T008.TM01_PRODUCTID INNER JOIN TM02_PRODUCTSIZE TM02 ON TM02.TM02_PRODUCTSIZEID = T008.TM02_PRODUCTSIZEID WHERE T008.T012_CHALLENID=" + Datalayer.iT012_CHALLENID);
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
                    //txtDiscount.Text = dt.Rows[0]["T012_DISCOUNT"].ToString();
                    txtTotalTaxAmount.Text = dt.Rows[0]["T012_TAX"].ToString();
                    txtNetAmount.Text = dt.Rows[0]["T012_NETAMOUNT"].ToString();
                    txtDetails.Text = dt.Rows[0]["T012_DETAILS"].ToString();
                    txtLR1.Text = dt.Rows[0]["T012_LR1"].ToString();
                    txtLR2.Text = dt.Rows[0]["T012_LR2"].ToString();
                    txtTransport1.Text = dt.Rows[0]["T012_TRANSPORT1"].ToString();
                    txtTransport1.Text = dt.Rows[0]["T012_TRANSPORT2"].ToString();
                    txtPacking.Text = dt.Rows[0]["T012_PACKING"].ToString();
                    txtFreight.Text = dt.Rows[0]["T012_FREIGHT"].ToString();
                    txtExpense.Text = dt.Rows[0]["T012_OTHER_EXPENSE"].ToString();


                    cmbName.Focus();
                }
            }
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

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopualteDiscount();
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

        void PopulateItems()
        {
            string sItemNo = txtItemNo.Text.Trim();
            int iTM01_PRODUCTID = 0;
            if (!string.IsNullOrEmpty(txtItemNo.Text.Trim()))
            {
                string sItemNumber = txtItemNo.Text.Trim();
                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                List<USP_VP_GET_PRODUCT_FOR_CHALLEN_ON_NOResult> lst = new List<USP_VP_GET_PRODUCT_FOR_CHALLEN_ON_NOResult>();
                //lst = MyDa.USP_VP_GET_PRODUCT_FOR_CHALLEN_ON_NO(sItemNumber).ToList();
                //if (lst != null && lst.Count > 0)
                //{
                //    txtItemName.Text = lst[0].TM01_NAME.ToString();
                //    lblProductId.Text = lst[0].TM01_PRODUCTID.ToString();

                //    try
                //    {
                //        lblFinish1.Text = lst[0].TM02_SIZE.ToString();
                //        txtRate1.Text = lst[0].TM02_PRICE.ToString();
                //        txtStock1.Text = lst[0].T006_STOCK.ToString();
                //        lblFinishId1.Text = lst[0].TM02_PRODUCTSIZEID.ToString();
                //    }
                //    catch
                //    {
                //    }

                //    try
                //    {
                //        lblFinish2.Text = lst[1].TM02_SIZE.ToString();
                //        txtRate2.Text = lst[1].TM02_PRICE.ToString();
                //        txtStock2.Text = lst[1].T006_STOCK.ToString();
                //        lblFinishId2.Text = lst[1].TM02_PRODUCTSIZEID.ToString();
                //    }
                //    catch
                //    {
                //    }

                //}
                //else
                //{
                //    ReseItemDetails();
                //    Datalayer.InformationMessageBox(sItemNo + " not exist");
                //}
            }
            else
            {
                errorProvider1.SetError(txtItemNo, "Item number is required..");
                txtItemNo.Focus();
                return;
            }
        }

        void ReseItemDetails()
        {
            txtItemNo.Text = "";
            txtItemName.Text = "";
            lblProductId.Text = "";

            lblFinish1.Text = "";
            txtRate1.Text = "";
            txtStock1.Text = "";
            lblFinishId1.Text = "";

            lblFinish2.Text = "";
            txtRate2.Text = "";
            txtStock2.Text = "";
            lblFinishId2.Text = "";

            txtQty1.Text = "";
            txtQty2.Text = "";
        }

        private void txtItemNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtItemNo.Text.Trim()))
                PopulateItems();
        }

        private void txtItemNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                PopulateItems();
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveItems();
        }

        void SaveItems()
        {
            if (!string.IsNullOrEmpty(txtQty1.Text.Trim()))
            {
                int TM02_PRODUCTSIZEID = 0, TM01_PRODUCTID = 0;
                int.TryParse(lblFinishId1.Text.Trim(), out TM02_PRODUCTSIZEID);
                int.TryParse(lblProductId.Text.Trim(), out TM01_PRODUCTID);

                if (TM02_PRODUCTSIZEID > 0)
                {
                    string TM02_SIZE = (Convert.ToString(lblFinish1.Text.Trim()));
                    decimal T008_RATE = 0, T008_QTY = 0;

                    decimal.TryParse(txtQty1.Text.Trim(), out T008_QTY);
                    if (T008_QTY > 0)
                    {
                        decimal.TryParse(txtRate1.Text.Trim(), out T008_RATE);
                        decimal TotalAmt = T008_RATE * T008_QTY;

                        decimal dDiscountRate = 0, dDiscounAmt = 0;
                        decimal.TryParse(txtDiscountRate.Text.Trim(), out dDiscountRate);
                        dDiscounAmt = (TotalAmt * dDiscountRate) / 100;

                        decimal T008_NET_AMOUNT = Math.Round(TotalAmt - dDiscounAmt);

                        DataRow newRows = dtProducts.NewRow();
                        newRows["rkd"] = dtProducts.Rows.Count + 1;
                        newRows["T008_QTY"] = T008_QTY;
                        newRows["TM01_NO"] = txtItemNo.Text.ToString();
                        newRows["T008_RATE"] = T008_RATE;
                        newRows["T008_NET_AMOUNT"] = T008_NET_AMOUNT;
                        newRows["TM01_PRODUCTID"] = TM01_PRODUCTID;
                        newRows["TM01_NAME"] = txtItemName.Text.Trim();
                        newRows["TM02_PRODUCTSIZEID"] = TM02_PRODUCTSIZEID;
                        newRows["TM02_SIZE"] = TM02_SIZE;
                        newRows["T008_TOTAL_AMT"] = TotalAmt;
                        newRows["T008_DISCOUNT_PER"] = dDiscountRate;
                        newRows["T008_DISCOUNT_AMT"] = dDiscounAmt;
                        newRows["T008_TAX_RATE"] = 0;
                        newRows["T008_TAX_AMT"] = 0;

                        dtProducts.Rows.Add(newRows);
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtQty2.Text.Trim()))
            {
                int TM02_PRODUCTSIZEID1 = 0, TM01_PRODUCTID1 = 0;
                int.TryParse(lblFinishId2.Text.Trim(), out TM02_PRODUCTSIZEID1);
                int.TryParse(lblProductId.Text.Trim(), out TM01_PRODUCTID1);

                if (TM02_PRODUCTSIZEID1 > 0)
                {
                    string TM02_SIZE1 = (Convert.ToString(lblFinish2.Text.Trim()));
                    decimal T008_RATE1 = 0, T008_QTY1 = 0;

                    decimal.TryParse(txtRate2.Text.Trim(), out T008_RATE1);
                    decimal.TryParse(txtQty2.Text.Trim(), out T008_QTY1);
                    if (T008_QTY1 > 0)
                    {
                        decimal TotalAmt = T008_RATE1 * T008_QTY1;

                        decimal dDiscountRate = 0, dDiscounAmt = 0;
                        decimal.TryParse(txtDiscountRate.Text.Trim(), out dDiscountRate);
                        dDiscounAmt = (TotalAmt * dDiscountRate) / 100;

                        decimal T008_NET_AMOUNT = Math.Round(TotalAmt - dDiscounAmt);

                        DataRow newRows = dtProducts.NewRow();
                        newRows["rkd"] = dtProducts.Rows.Count + 1;
                        newRows["T008_QTY"] = T008_QTY1;
                        newRows["TM01_NO"] = txtItemNo.Text.ToString();
                        newRows["T008_RATE"] = T008_RATE1;
                        newRows["T008_NET_AMOUNT"] = T008_NET_AMOUNT;
                        newRows["TM01_PRODUCTID"] = TM01_PRODUCTID1;
                        newRows["TM01_NAME"] = txtItemName.Text.Trim();
                        newRows["TM02_PRODUCTSIZEID"] = TM02_PRODUCTSIZEID1;
                        newRows["TM02_SIZE"] = TM02_SIZE1;
                        newRows["T008_TOTAL_AMT"] = TotalAmt;
                        newRows["T008_DISCOUNT_PER"] = dDiscountRate;
                        newRows["T008_DISCOUNT_AMT"] = dDiscounAmt;
                        newRows["T008_TAX_RATE"] = 0;
                        newRows["T008_TAX_AMT"] = 0;

                        dtProducts.Rows.Add(newRows);
                    }
                }
            }

            dgDisplayData.FirstDisplayedScrollingRowIndex = dgDisplayData.RowCount - 1;
            PopautleGrandTotal();
            ReseItemDetails();

            txtItemNo.Focus();
        }

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

            PopulateNET();
        }

        void PopulateNET()
        {
            decimal dTotalAmt = 0, dDiscount = 0;
            decimal dT007_PACKING = 0, dT007_FREIGHT = 0, dT007_OTHER_EXPENSE = 0, dTax = 0;

            decimal.TryParse(txtPacking.Text.Trim(), out dT007_PACKING);
            decimal.TryParse(txtFreight.Text.Trim(), out dT007_FREIGHT);
            decimal.TryParse(txtExpense.Text.Trim(), out dT007_OTHER_EXPENSE);
            decimal.TryParse(txtTotalTaxAmount.Text.Trim(), out dTax);

            decimal.TryParse(txtTotalAmt.Text.Trim(), out dTotalAmt);

            dTotalAmt = dTotalAmt + dT007_PACKING + dT007_FREIGHT + dT007_OTHER_EXPENSE + dTax;
            txtNetAmount.Text = decimal.Round((dTotalAmt), 2).ToString();
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.Columns["TM01_NO"].HeaderText = "No";
            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Item";
            dgDisplayData.Columns["TM02_SIZE"].HeaderText = "Finish";
            dgDisplayData.Columns["T008_QTY"].HeaderText = "Qty";
            dgDisplayData.Columns["T008_RATE"].HeaderText = "Rate";
            dgDisplayData.Columns["T008_NET_AMOUNT"].HeaderText = "Amount";


            dgDisplayData.Columns["TM01_NO"].DisplayIndex = 0;
            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 1;
            dgDisplayData.Columns["TM02_SIZE"].DisplayIndex = 2;
            dgDisplayData.Columns["T008_QTY"].DisplayIndex = 3;
            dgDisplayData.Columns["T008_RATE"].DisplayIndex = 4;
            dgDisplayData.Columns["T008_NET_AMOUNT"].DisplayIndex = 5;

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

            dgDisplayData.Columns["TM01_NO"].Width = 70;
            dgDisplayData.Columns["TM01_NAME"].Width = 350;
            dgDisplayData.Columns["TM02_SIZE"].Width = 120;
            dgDisplayData.Columns["T008_QTY"].Width = 120;
            dgDisplayData.Columns["T008_RATE"].Width = 120;
            dgDisplayData.Columns["T008_NET_AMOUNT"].Width = 120;


            dgDisplayData.ClearSelection();
        }

        private void txtPacking_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtFreight_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtExpense_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        private void txtTotalTaxAmount_TextChanged(object sender, EventArgs e)
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
                //decimal.TryParse(txtDiscount.Text.Trim(), out T007_DISCOUNT);
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
                        //ResteData();
                        Datalayer.iT012_CHALLENID = T007_SALEID;
                        if (IsPrint)
                        {
                            Datalayer.iPrintableChallenId = T007_SALEID;

                            DAL dl = new DAL();
                            DataTable dt = new DataTable();
                            dt = dl.SelectMethod("exec USP_VP_PRINT_CHALLEN_BILL '" + Datalayer.iPrintableChallenId + "'");
                            if (dt.Rows.Count > 0)
                            {

                                ReportDocument RptDoc = new ReportDocument();
                                RptDoc.Load(Application.StartupPath + @"\Report\rptChallen.rpt");
                                RptDoc.SetDataSource(dt);

                                crystalReportViewer1.ReportSource = RptDoc;
                                crystalReportViewer1.Refresh();

                                crystalReportViewer1.PrintReport();
                                //Datalayer.RptReport = RptDoc;
                                //Datalayer.sReportName = "Challen Bill";

                                //Report.frmReportViwer fmReport = new Report.frmReportViwer();
                                //fmReport.Show();
                            }
                            else
                            {
                                Datalayer.InformationMessageBox("No Record..");
                            }


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
                        Datalayer.iT012_CHALLENID = T007_SALEID;

                        if (IsPrint)
                        {
                            Datalayer.iPrintableChallenId = T007_SALEID;

                            DAL dl = new DAL();
                            DataTable dt = new DataTable();
                            dt = dl.SelectMethod("exec USP_VP_PRINT_CHALLEN_BILL '" + Datalayer.iPrintableChallenId + "'");
                            if (dt.Rows.Count > 0)
                            {

                                ReportDocument RptDoc = new ReportDocument();
                                RptDoc.Load(Application.StartupPath + @"\Report\rptChallen.rpt");
                                RptDoc.SetDataSource(dt);

                                crystalReportViewer1.ReportSource = RptDoc;
                                crystalReportViewer1.Refresh();

                                crystalReportViewer1.PrintReport();
                            }
                            else
                            {
                                Datalayer.InformationMessageBox("No Record..");
                            }

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

                // ResteData();
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
            Datalayer.Reset(panel1.Controls);
            dtProducts.Clear();
            Cursor.Current = Cursors.Default;
            dgDisplayData.DataSource = dtProducts;
            PopualteLastBillNo();
            cmbName.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            AddUpdatePurchase(true, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResteData();
        }

        private void bntPackingSlip_Click(object sender, EventArgs e)
        {
            AddUpdatePurchase(false, true);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManageChallen fm = new frmManageChallen();
            fm.MdiParent = this.MdiParent;
            fm.Show();
            this.Close();
        }

        private void dgDisplayData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Datalayer.ShowQuestMsg(Datalayer.sMessageConfirmation))
            {
                dgDisplayData.Rows.Remove(dgDisplayData.CurrentRow);
                PopautleGrandTotal();
            }
        }

        void AutoCompliteName()
        {
            AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM01_PRODUCTID,TM01_NO FROM TM01_PRODUCT WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM01_NO");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    MyCollection.Add(dtData.Rows[i]["TM01_NO"].ToString());
                }
            }

            txtItemNo.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtItemNo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtItemNo.AutoCompleteCustomSource = MyCollection;
        }

        private void frmChallen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.iT012_CHALLENID = 0;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmManageChallen fm = new frmManageChallen();
            fm.MdiParent = this.MdiParent;
            fm.Show();
            this.Close();
        }

    }
}

