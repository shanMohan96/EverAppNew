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
    public partial class frmAddUpdateProductSize : Form
    {
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Item Size and Price Details";

        public frmAddUpdateProductSize()
        {
            InitializeComponent();
        }

        private void frmAddUpdateProductSize_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);


            dbo.FillItemName(cmbName);

            PopauteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(button1, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnSave, "ctrl + S");
        }

        private void frmAddUpdateProductSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");

            if (e.Control && e.KeyCode == Keys.S)
                btnSave_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.N)
                button1_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                btnExit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void PopauteData()
        {
            if (Datalayer.iTM02_PRODUCTSIZEID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM TM02_PRODUCTSIZE WHERE TM02_PRODUCTSIZEID=" + Datalayer.iTM02_PRODUCTSIZEID);
                if (dt.Rows.Count > 0)
                {
                    cmbName.SelectedValue = dt.Rows[0]["TM01_PRODUCTID"].ToString();
                    txtSize.Text = dt.Rows[0]["TM02_SIZE"].ToString();
                    txtPrice.Text = dt.Rows[0]["TM02_PRICE"].ToString();
                    cmbName.Focus();
                }
            }
        }

        void AddUpdateData()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(cmbName.Text.Trim()))
                {
                    ep1.SetError(cmbName, "Name is Required..");
                    cmbName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
                {
                    ep1.SetError(txtPrice, "Price is Required..");
                    txtPrice.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtSize.Text.Trim()))
                {
                    ep1.SetError(txtSize, "Size is Required..");
                    txtSize.Focus();
                    return;
                }

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;

                int TM01_PRODUCTID = 0;
                int.TryParse(cmbName.SelectedValue.ToString(), out TM01_PRODUCTID);

                decimal TM02_PRICE = 0;
                decimal.TryParse(txtPrice.Text.Trim(), out TM02_PRICE);

                MyDa.USP_VP_ADDUPDATE_PRODUCTSIZE(Datalayer.iTM02_PRODUCTSIZEID, TM01_PRODUCTID, txtSize.Text.Trim(), TM02_PRICE,Datalayer.iT001_COMPANYID , ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iTM02_PRODUCTSIZEID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.iTM02_PRODUCTSIZEID == 0)
                        Datalayer.DosenotInsertMessageBox(sPageName);
                    else
                        Datalayer.DosenotUpdateMessageBox(sPageName);
                }

                ResetData();
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void ResetData()
        {
            Datalayer.iTM02_PRODUCTSIZEID = 0;
            ep1.Clear();
            //Datalayer.Reset(panel1.Controls);
            txtPrice.Text = "";
            txtSize.Text = "";
            cmbName.Focus();
        }

        private void frmAddUpdateProductSize_FormClosed(object sender, FormClosedEventArgs e)
        {
            Datalayer.iTM02_PRODUCTSIZEID = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdateData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            

            frmManageProductSize fm = new frmManageProductSize();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }

    }
}
