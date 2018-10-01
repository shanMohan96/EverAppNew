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
    public partial class frmOrder : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Order Details";

        public frmOrder()
        {
            InitializeComponent();
        }

        private void frmOrder_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbAccount, "c");
            dbo.FillItemName(cmbItemName);
            dbo.FillItemSize(cmbItemName, cmbMainItemSize);

            PopualteData();
        }

        private void frmOrder_KeyDown(object sender, KeyEventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbo.FillItemSize(cmbItemName, cmbMainItemSize);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        void Save()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(cmbAccount.Text.Trim()))
                {
                    ep1.SetError(cmbAccount, "This field is required.");
                    cmbAccount.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbItemName.Text.Trim()))
                {
                    ep1.SetError(cmbItemName, "This field is required.");
                    cmbItemName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbMainItemSize.Text.Trim()))
                {
                    ep1.SetError(cmbMainItemSize, "This field is required.");
                    cmbMainItemSize.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtQty.Text.Trim()))
                {
                    ep1.SetError(txtQty, "This field is required.");
                    txtQty.Focus();
                    return;
                }


                int T004_ORDERID = 0, T001_ACCOUNTID = 0, TM01_PRODUCTID = 0, TM02_PRODUCTSIZEID = 0, T004_QTY = 0;
                string T004_DETAILS = "";
                bool T004_IS_URGENT = false;
                if (chkIsUrgent.Checked)
                    T004_IS_URGENT = true;

                int.TryParse(lblId.Text.Trim(), out T004_ORDERID);
                int.TryParse(cmbAccount.SelectedValue.ToString(), out T001_ACCOUNTID);
                int.TryParse(cmbItemName.SelectedValue.ToString(), out TM01_PRODUCTID);
                int.TryParse(cmbMainItemSize.SelectedValue.ToString(), out TM02_PRODUCTSIZEID);
                int.TryParse(txtQty.Text.Trim(), out T004_QTY);
                T004_DETAILS = txtDetails.Text.Trim();

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? T004_ORDERID_out = 0;
                MyDa.USP_VP_ADDUPDATE_ORDER(T004_ORDERID, T001_ACCOUNTID, dtpDate.Value, TM01_PRODUCTID, TM02_PRODUCTSIZEID, T004_QTY, T004_IS_URGENT, T004_DETAILS, Datalayer.iT001_COMPANYID, ref T004_ORDERID_out);
                if (T004_ORDERID_out > 0)
                {
                    if (T004_ORDERID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                        Datalayer.UpdateMessageBox(sPageName);

                    cmbItemName.SelectedIndex = 0;
                    txtQty.Text = "";
                    txtDetails.Text = "";
                    chkIsUrgent.Checked = false;
                    cmbItemName.Focus();
                    
                    cmbAccount.SelectedValue = T001_ACCOUNTID;
                    PopualteData();
                }
                else
                {
                    if (T004_ORDERID == 0)
                        Datalayer.DosenotInsertMessageBox(sPageName);
                    else
                        Datalayer.DosenotUpdateMessageBox(sPageName);
                }

            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void ResetData()
        {
            lblId.Text = "";
            Datalayer.Reset(panel3.Controls);
            cmbAccount.Focus();
        }

        void PopualteData()
        {
            string T001_ACCOUNTID = "";
            int iT001_ACCOUNTID = 0;
            if (!string.IsNullOrEmpty(cmbAccount.Text.Trim()))
                int.TryParse(cmbAccount.SelectedValue.ToString(), out iT001_ACCOUNTID);

            if (iT001_ACCOUNTID > 0)
                T001_ACCOUNTID = iT001_ACCOUNTID.ToString();

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_ORDERResult> lst = new List<USP_VP_GET_ORDERResult>();
            lst = MyDa.USP_VP_GET_ORDER(null, T001_ACCOUNTID, null, null, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            dgDisplayData.Columns["T004_ORDERID"].Visible = false;
            dgDisplayData.Columns["T001_ACCOUNTID"].Visible = false;
            dgDisplayData.Columns["TM01_PRODUCTID"].Visible = false;
            dgDisplayData.Columns["TM02_PRODUCTSIZEID"].Visible = false;
            
            dgDisplayData.Columns["T001_NAME"].Visible = false;
            dgDisplayData.Columns["T001_MOBILE1"].Visible = false;

            dgDisplayData.Columns["T004_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T004_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["TM01_NAME"].HeaderText = "ITEM";
            dgDisplayData.Columns["TM02_SIZE"].HeaderText = "Finish";
            dgDisplayData.Columns["T004_QTY"].HeaderText = "Qty";
            dgDisplayData.Columns["T004_IS_URGENT"].HeaderText = "Urgent";
            dgDisplayData.Columns["T004_DETAILS"].HeaderText = "Details";
            dgDisplayData.Columns["TM01_NO"].HeaderText = "No";

            dgDisplayData.Columns["T004_DATE"].DisplayIndex = 1;
            dgDisplayData.Columns["TM01_NO"].DisplayIndex = 2;
            dgDisplayData.Columns["TM01_NAME"].DisplayIndex =3;
            dgDisplayData.Columns["TM02_SIZE"].DisplayIndex = 4;
            dgDisplayData.Columns["T004_QTY"].DisplayIndex = 5;
            dgDisplayData.Columns["T004_IS_URGENT"].DisplayIndex = 6;
            dgDisplayData.Columns["T004_DETAILS"].DisplayIndex = 7;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns["T004_DATE"].Width = 70;
            dgDisplayData.Columns["TM01_NO"].Width = 70;
            dgDisplayData.Columns["TM01_NAME"].Width = 120;
            dgDisplayData.Columns["TM02_SIZE"].Width = 50;
            dgDisplayData.Columns["T004_QTY"].Width = 50;
            dgDisplayData.Columns["T004_IS_URGENT"].Width = 50;
            dgDisplayData.Columns["T004_DETAILS"].Width = 350;
        }

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        void EditData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                lblId.Text = Convert.ToString(dgDisplayData.CurrentRow.Cells["T004_ORDERID"].Value);
                cmbAccount.SelectedValue = Convert.ToString(dgDisplayData.CurrentRow.Cells["T001_ACCOUNTID"].Value);
                dtpDate.Text = Convert.ToString(dgDisplayData.CurrentRow.Cells["T004_DATE"].Value);
                cmbItemName.SelectedValue = Convert.ToString(dgDisplayData.CurrentRow.Cells["TM01_PRODUCTID"].Value);
                cmbMainItemSize.SelectedValue = Convert.ToString(dgDisplayData.CurrentRow.Cells["TM02_PRODUCTSIZEID"].Value);
                txtQty.Text = Convert.ToString(dgDisplayData.CurrentRow.Cells["T004_QTY"].Value);
                if (Convert.ToString(dgDisplayData.CurrentRow.Cells["T004_IS_URGENT"].Value) == "YES")
                    chkIsUrgent.Checked = true;
                else
                    chkIsUrgent.Checked = false;

                txtDetails.Text = Convert.ToString(dgDisplayData.CurrentRow.Cells["T004_DETAILS"].Value);
            }
        }

        void DeleteData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {

                if (Datalayer.ShowQuestMsg(Datalayer.sMessageConfirmation))
                {
                    try
                    {
                        int ID = 0;
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T004_ORDERID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_DELETE_ORDER(ID, ref Iout);

                        if (Iout > 0)
                        {
                            Datalayer.DeleteMessageBox("Order Details");
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Order Details");
                    }
                    catch (Exception)
                    {
                        Datalayer.InformationMessageBox(Datalayer.sMessageForainKey);
                        return;
                    }
                }
            }
            else
            {
                Datalayer.InformationMessageBox(Datalayer.sMeessgeSelection);
            }
        }

        private void dgDisplayData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                EditData();
            if (e.KeyCode == Keys.Delete)
                DeleteData();
        }

        private void dgDisplayData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
