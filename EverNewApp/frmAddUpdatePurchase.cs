using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EverNewApp
{
    public partial class frmAddUpdatePurchase : Form
    {
        DataTable dtProducts = new DataTable();
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Purchase Details";

        public frmAddUpdatePurchase()
        {
            InitializeComponent();
        }

        private void frmAddUpdatePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillStateList(cmbState);
            dbo.FillAccountList(cmbName, "p");
            dbo.FillItemName(cmbItemName);
            cmbUnitName.SelectedIndex = 0;

            dtProducts.Columns.Add("rkd", typeof(Int32));
            dtProducts.Columns.Add("TM01_PRODUCTID", typeof(int));
            dtProducts.Columns.Add("TM01_NAME", typeof(string));
            dtProducts.Columns.Add("T003_QTY", typeof(decimal));
            dtProducts.Columns.Add("T003_UNIT", typeof(string));
            dtProducts.Columns.Add("T003_RATE", typeof(decimal));
            dtProducts.Columns.Add("T003_AMOUNT", typeof(decimal));


            dgDisplayData.DataSource = dtProducts;

            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Item";
            dgDisplayData.Columns["T003_QTY"].HeaderText = "Qty";
            dgDisplayData.Columns["T003_UNIT"].HeaderText = "Unit";
            dgDisplayData.Columns["T003_RATE"].HeaderText = "Price";
            dgDisplayData.Columns["T003_AMOUNT"].HeaderText = "Amount";

            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T003_QTY"].DisplayIndex = 1;
            dgDisplayData.Columns["T003_UNIT"].DisplayIndex = 2;
            dgDisplayData.Columns["T003_RATE"].DisplayIndex = 3;
            dgDisplayData.Columns["T003_AMOUNT"].DisplayIndex = 4;

            dgDisplayData.Columns["rkd"].Visible = false;
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
            if (Datalayer.iT002_PURCHASEID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T002_PURCHASE WHERE T002_PURCHASEID=" + Datalayer.iT002_PURCHASEID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbName.SelectedValue = int.Parse(dt.Rows[0]["T001_ACCOUNTID"].ToString());
                    cmbState.SelectedValue = int.Parse(dt.Rows[0]["T007_PLACE_OF_SUPPLY"].ToString());
                    dtpDate.Text = dt.Rows[0]["T002_DATE"].ToString();
                    txtBillNo.Text = dt.Rows[0]["T002_NO"].ToString();
                    txtTotalAmt.Text = dt.Rows[0]["T002_TOTAL_AMT"].ToString();
                    txtFinalAmount.Text = dt.Rows[0]["T002_NETAMOUNT"].ToString();
                    txtDetails.Text = dt.Rows[0]["T002_DETAILS"].ToString();
                    txtCGST.Text = dt.Rows[0]["T002_TAX1RATE"].ToString();
                    txtSGST.Text = dt.Rows[0]["T002_TAX2RATE"].ToString();
                    txtIGST.Text = dt.Rows[0]["T002_TAX3RATE"].ToString();

                    DataTable dtItems = new DataTable();
                    dtItems = dl.SelectMethod("SELECT  ROW_NUMBER() OVER(ORDER BY T003_PURCHASEITEMID ASC) AS rkd,T003.TM01_PRODUCTID ,T003.T003_QTY,T003_UNIT,T003_RATE,T003_AMOUNT,TM01.TM01_NAME FROM T003_PURCHASEITEM T003 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T003.TM01_PRODUCTID WHERE T002_PURCHASEID=" + Datalayer.iT002_PURCHASEID);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdatePurchase();
        }

        void AddUpdatePurchase()
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
                if (string.IsNullOrEmpty(txtFinalAmount.Text.Trim()))
                {
                    ep1.SetError(txtFinalAmount, "This field is required.");
                    txtFinalAmount.Focus();
                    return;
                }

                int T002_PURCHASEID = Datalayer.iT002_PURCHASEID;

                // int TM01_CUSTOMERID = 0;
                string T002_NO = "", T002_DETAILS = "";
                int T001_ACCOUNTID = 0, T007_PLACE_OF_SUPPLY=0;
                decimal T002_TOTAL_AMT = 0, T002_DISCOUNT = 0, T002_TAX1RATE = 0, T002_TAX1AMOUNT = 0, T002_TAX2RATE = 0, T002_TAX2AMOUNT = 0, T002_TAX3RATE = 0, T002_TAX3AMOUNT = 0, T002_NETAMOUNT = 0;
                DateTime T003_DATE = dtpDate.Value;

                T002_NO = txtBillNo.Text.Trim();
                int.TryParse(cmbName.SelectedValue.ToString(), out T001_ACCOUNTID);
                int.TryParse(cmbState.SelectedValue.ToString(), out T007_PLACE_OF_SUPPLY);
                decimal.TryParse(txtTotalAmt.Text.Trim(), out T002_TOTAL_AMT);
                decimal.TryParse(txtCGST.Text.Trim(), out T002_TAX1RATE);
                decimal.TryParse(lblCGSTAmount.Text.Trim(), out T002_TAX1AMOUNT);
                decimal.TryParse(txtSGST.Text.Trim(), out T002_TAX2RATE);
                decimal.TryParse(lblSGSTAmount.Text.Trim(), out T002_TAX2AMOUNT);
                decimal.TryParse(txtIGST.Text.Trim(), out T002_TAX3RATE);
                decimal.TryParse(lblIGSTAmount.Text.Trim(), out T002_TAX3AMOUNT);
                decimal.TryParse(txtFinalAmount.Text.Trim(), out T002_NETAMOUNT);
                T002_DETAILS = txtDetails.Text.Trim();

                if (T002_PURCHASEID > 0)
                {
                    int? Iout = 0;
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    MyDa.USP_VP_DELETE_PURCHASE_ITEM(T002_PURCHASEID, ref Iout);

                }

                int? T002_PURCHASEID_out = 0;
                Cursor.Current = Cursors.WaitCursor;
                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                MyDa.USP_VP_ADDUPDATE_PURCHASE(T002_PURCHASEID, T001_ACCOUNTID, T002_NO, T003_DATE, T002_TOTAL_AMT, T002_DISCOUNT, T002_TAX1RATE, T002_TAX1AMOUNT, T002_TAX2RATE, T002_TAX2AMOUNT, T002_TAX3RATE, T002_TAX3AMOUNT, T002_NETAMOUNT, T002_DETAILS, Datalayer.iT001_COMPANYID,T007_PLACE_OF_SUPPLY, ref T002_PURCHASEID_out);
                if (T002_PURCHASEID_out > 0)
                {
                    T002_PURCHASEID = int.Parse(T002_PURCHASEID_out.Value.ToString());
                    for (int i = 0; i < dgDisplayData.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_PRODUCTID"].Value)))
                        {
                            int TM01_PRODUCTID = 0;
                            decimal T003_RATE = 0, T003_AMOUNT = 0, T003_QTY = 0;
                            string T003_UNIT = "";

                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_PRODUCTID"].Value), out TM01_PRODUCTID);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T003_QTY"].Value), out T003_QTY);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T003_RATE"].Value), out T003_RATE);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T003_AMOUNT"].Value), out T003_AMOUNT);
                            T003_UNIT = Convert.ToString(dgDisplayData.Rows[i].Cells["T003_UNIT"].Value);

                            int? T008_SALEITEMID_Out = 0;
                            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                            MyDa.USP_VP_ADDUPDATE_PURCHASEITEM(0, T002_PURCHASEID, TM01_PRODUCTID, T003_QTY, T003_UNIT, T003_RATE, T003_AMOUNT, 0, Datalayer.iT001_COMPANYID, ref T008_SALEITEMID_Out);
                        }
                    }


                    if (Datalayer.iT002_PURCHASEID == 0)
                    {
                        Datalayer.InsertMessageBox(sPageName);
                        ResteData();
                    }
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                        ResteData();
                    }
                }
                else
                {
                    if (Datalayer.iT002_PURCHASEID == 0)
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
            Datalayer.iT002_PURCHASEID = 0;
            Datalayer.Reset(panel1.Controls);
            Datalayer.Reset(panel5.Controls);
            Cursor.Current = Cursors.Default;

            dtProducts.Clear();
            dgDisplayData.DataSource = dtProducts;
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManagePurchase fm = new frmManagePurchase();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            PopulteNetAmount();
        }

        void PopulteNetAmount()
        {
            decimal dQty = 0, dPrice = 0, dNetAmount = 0;

            decimal.TryParse(txtQty.Text.Trim(), out dQty);
            decimal.TryParse(txtPrice.Text.Trim(), out dPrice);

            txtNetAmount.Text = (dQty * dPrice).ToString();
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            PopulteNetAmount();
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
            if (string.IsNullOrEmpty(cmbUnitName.Text.Trim()))
            {
                ep1.SetError(cmbUnitName, "This field is required.");
                cmbUnitName.Focus();
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
            if (string.IsNullOrEmpty(txtNetAmount.Text.Trim()))
            {
                ep1.SetError(txtNetAmount, "This field is required.");
                txtNetAmount.Focus();
                return;
            }



            decimal dQty = 0, dPrice = 0, dNetAmount = 0;

            decimal.TryParse(txtQty.Text.Trim(), out dQty);
            decimal.TryParse(txtPrice.Text.Trim(), out dPrice);
            decimal.TryParse(txtNetAmount.Text.Trim(), out dNetAmount);

            int iProductId = 0;
            int.TryParse(cmbItemName.SelectedValue.ToString(), out iProductId);

            DataRow newRows = dtProducts.NewRow();
            newRows["rkd"] = dtProducts.Rows.Count + 1;
            newRows["TM01_PRODUCTID"] = iProductId;
            newRows["TM01_NAME"] = cmbItemName.Text.ToString();
            newRows["T003_QTY"] = dQty;
            newRows["T003_UNIT"] = cmbUnitName.SelectedItem.ToString(); ;
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
            txtNetAmount.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
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

            PopulateNET();
        }

        private void txtTotalAmt_TextChanged(object sender, EventArgs e)
        {
            PopulateNET();
        }

        void PopulateNET()
        {
            decimal dTotalAmt = 0;
            decimal dTax1 = 0, dTax2 = 0, dTax3 = 0;

            decimal.TryParse(txtTotalAmt.Text.Trim(), out dTotalAmt);
            decimal.TryParse(txtCGST.Text.Trim(), out dTax1);
            decimal.TryParse(txtSGST.Text.Trim(), out dTax2);
            decimal.TryParse(txtIGST.Text.Trim(), out dTax3);

            decimal dTax1Amount = 0, dTax2Amount = 0, dTax3Amount = 0;

            dTax1Amount = decimal.Round(((dTotalAmt * dTax1) / 100), 2);
            dTax2Amount = decimal.Round(((dTotalAmt * dTax2) / 100), 2);
            dTax3Amount = decimal.Round(((dTotalAmt * dTax3) / 100), 2);

            lblCGSTAmount.Text = dTax1Amount.ToString();
            lblSGSTAmount.Text = dTax2Amount.ToString();
            lblIGSTAmount.Text = dTax3Amount.ToString();

            decimal dNetAmount = 0;
            dNetAmount = dTotalAmt + dTax1Amount + dTax2Amount + dTax3Amount;

            txtFinalAmount.Text = decimal.Round(dNetAmount, 2).ToString();
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

    }
}
