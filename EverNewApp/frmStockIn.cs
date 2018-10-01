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
    public partial class frmStockIn : Form
    {
        DataTable dtProducts = new DataTable();
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Stock In Details";


        public frmStockIn()
        {
            InitializeComponent();
        }

        private void frmStockIn_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(button1);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            cmbUnitName.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            dbo.FillAccountList(cmbName, "pl");
            dbo.FillItemSetName(cmbItemName);

            dtProducts.Columns.Add("rkd", typeof(Int32));
            dtProducts.Columns.Add("TM01_PRODUCTID", typeof(int));
            dtProducts.Columns.Add("TM01_NAME", typeof(string));
            dtProducts.Columns.Add("T005_QTY", typeof(decimal));
            dtProducts.Columns.Add("T005_UNIT", typeof(string));
            dtProducts.Columns.Add("T005_TYPE", typeof(string));
            dtProducts.Columns.Add("T005_WEIGHT", typeof(decimal));


            dgDisplayData.DataSource = dtProducts;

            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Item";
            dgDisplayData.Columns["T005_QTY"].HeaderText = "Qty";
            dgDisplayData.Columns["T005_UNIT"].HeaderText = "Unit";
            dgDisplayData.Columns["T005_TYPE"].HeaderText = "Type";
            dgDisplayData.Columns["T005_WEIGHT"].HeaderText = "Weight";

            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T005_QTY"].DisplayIndex = 1;
            dgDisplayData.Columns["T005_UNIT"].DisplayIndex = 2;
            dgDisplayData.Columns["T005_TYPE"].DisplayIndex = 3;
            dgDisplayData.Columns["T005_WEIGHT"].DisplayIndex = 4;

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

            PopauteData();
        }

        private void frmStockIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");

            if (e.Control && e.KeyCode == Keys.S)
                btnSave_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                this.Close();
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Save()
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

                if (dgDisplayData.Rows.Count == 0)
                {
                    Datalayer.InformationMessageBox("Record is required.");
                    return;
                }

                int T005_STOCKINID = 0, T001_ACCOUNTID = 0;
                string T004_DETAILS = "";

                T005_STOCKINID = Datalayer.iT005_STOCKINID;

                int.TryParse(cmbName.SelectedValue.ToString(), out T001_ACCOUNTID);
                T004_DETAILS = txtDetails.Text.Trim();

                if (T005_STOCKINID > 0)
                {
                    int? Iout = 0;
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    MyDa.USP_VP_DELETE_STOCK_IN(T005_STOCKINID, ref Iout);
                }

                int.TryParse(cmbName.SelectedValue.ToString(), out T001_ACCOUNTID);
                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? T004_ORDERID_out = 0;
                Cursor.Current = Cursors.WaitCursor;
                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                MyDa.USP_VP_ADDUPDATE_STOCK_MASTER(T005_STOCKINID, dtpDate.Value, T001_ACCOUNTID, txtDetails.Text.Trim(), Datalayer.iT001_COMPANYID, ref T004_ORDERID_out);
                if (T004_ORDERID_out > 0)
                {
                    T005_STOCKINID = int.Parse(T004_ORDERID_out.Value.ToString());
                    for (int i = 0; i < dgDisplayData.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_PRODUCTID"].Value)))
                        {
                            int TM01_PRODUCTID = 0;
                            decimal T005_WEIGHT = 0, T005_QTY = 0;
                            string T005_UNIT = "", T005_TYPE = "";

                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["TM01_PRODUCTID"].Value), out TM01_PRODUCTID);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T005_QTY"].Value), out T005_QTY);
                            decimal.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T005_WEIGHT"].Value), out T005_WEIGHT);
                            T005_UNIT = Convert.ToString(dgDisplayData.Rows[i].Cells["T005_UNIT"].Value);
                            T005_TYPE = Convert.ToString(dgDisplayData.Rows[i].Cells["T005_TYPE"].Value);

                            int? T008_SALEITEMID_Out = 0;
                            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                            MyDa.USP_VP_ADDUPDATE_STOCK_IN(0, T005_STOCKINID, TM01_PRODUCTID, T005_QTY, T005_UNIT, T005_TYPE, T005_WEIGHT, Datalayer.iT001_COMPANYID, ref T004_ORDERID_out);
                        }
                    }

                    if (T004_ORDERID_out > 0)
                    {
                        if (Datalayer.iT005_STOCKINID == 0)
                            Datalayer.InsertMessageBox(sPageName);
                        else
                            Datalayer.UpdateMessageBox(sPageName);

                        ResetData();
                    }
                    else
                    {
                        if (Datalayer.iT005_STOCKINID == 0)
                            Datalayer.DosenotInsertMessageBox(sPageName);
                        else
                            Datalayer.DosenotUpdateMessageBox(sPageName);
                    }
                }
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void PopauteData()
        {
            if (Datalayer.iT005_STOCKINID > 0)
            {
                DataTable dt = new DataTable();
                DAL dl = new DAL();
                dt = dl.SelectMethod("SELECT * FROM T007_STOCKINMASTER WHERE T007_STOCKINMASTERID=" + Datalayer.iT005_STOCKINID);
                if (dt.Rows.Count > 0)
                {
                    cmbName.SelectedValue = dt.Rows[0]["T001_ACCOUNTID"].ToString();
                    txtDetails.Text = dt.Rows[0]["T007_DETAILS"].ToString();
                    dtpDate.Text = dt.Rows[0]["T007_DATE"].ToString();

                    DataTable dtItems = new DataTable();
                    dtItems = dl.SelectMethod("SELECT ROW_NUMBER() OVER(ORDER BY T005_STOCKINID ASC) AS rkd ,T005.TM01_PRODUCTID ,T005_QTY ,T005_UNIT ,T005_TYPE ,T005_WEIGHT ,TM01.TM01_NAME FROM T005_STOCKIN T005 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T005.TM01_PRODUCTID WHERE T005.T007_STOCKINMASTERID=" + Datalayer.iT005_STOCKINID);
                    if (dtItems != null && dtItems.Rows.Count > 0)
                    {
                        dtProducts = dtItems;
                        dgDisplayData.DataSource = dtProducts;
                    }

                    cmbItemName.Focus();
                }
            }
        }

        void ResetData()
        {
            Datalayer.iT005_STOCKINID = 0;
            txtQty.Text = "";
            txtDetails.Text = "";

            Datalayer.Reset(panel1.Controls);
            dtProducts.Clear();
            dgDisplayData.DataSource = dtProducts;
            cmbName.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmManageStockIn fm = new frmManageStockIn();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
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
            if (string.IsNullOrEmpty(cmbUnitName.Text.Trim()))
            {
                ep1.SetError(cmbUnitName, "This field is required.");
                cmbUnitName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(cmbType.Text.Trim()))
            {
                ep1.SetError(cmbType, "This field is required.");
                cmbType.Focus();
                return;
            }

            decimal dQty = 0, T005_WEIGHT = 0;

            decimal.TryParse(txtQty.Text.Trim(), out dQty);
            decimal.TryParse(txtWeight.Text.Trim(), out T005_WEIGHT);

            int iProductId = 0;
            int.TryParse(cmbItemName.SelectedValue.ToString(), out iProductId);

            DataRow newRows = dtProducts.NewRow();
            newRows["rkd"] = dtProducts.Rows.Count + 1;
            newRows["TM01_PRODUCTID"] = iProductId;
            newRows["TM01_NAME"] = cmbItemName.Text.ToString();
            newRows["T005_QTY"] = dQty;
            newRows["T005_UNIT"] = cmbUnitName.SelectedItem.ToString(); ;
            newRows["T005_TYPE"] = cmbType.SelectedItem.ToString(); ;
            newRows["T005_WEIGHT"] = T005_WEIGHT;
            dtProducts.Rows.Add(newRows);

            if (dgDisplayData.Rows.Count > 0)
                dgDisplayData.FirstDisplayedScrollingRowIndex = dgDisplayData.RowCount - 1;
            ReseItemDetails();

            cmbItemName.Focus();
        }

        void ReseItemDetails()
        {
            cmbItemName.SelectedIndex = 0;
            cmbUnitName.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            txtWeight.Text = "";
            txtQty.Text = "";
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveProduct();
        }

        private void dgDisplayData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (Datalayer.ShowQuestMsg(Datalayer.sMessageConfirmation))
                {
                    dgDisplayData.Rows.Remove(dgDisplayData.CurrentRow);
                }
            }
        }

    }
}
