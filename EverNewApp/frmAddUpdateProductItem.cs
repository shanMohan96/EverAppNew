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
    public partial class frmAddUpdateProductItem : Form
    {
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Item Set Details";

        public frmAddUpdateProductItem()
        {
            InitializeComponent();
        }

        private void frmAddUpdateProductItem_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);


            dbo.FillItemSetName(cmbItemSetName);
            dbo.FillOnlyItemName(cmbItemName);

            PopauteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(button1, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnSave, "ctrl + S");
        }

        private void frmAddUpdateProductItem_KeyDown(object sender, KeyEventArgs e)
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

        private void cmbItemSetName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void cmbItemName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void PopauteData()
        {
            if (Datalayer.iTM03_PRODUCTITEMID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM TM03_PRODUCTITEM WHERE TM03_PRODUCTITEMID=" + Datalayer.iTM03_PRODUCTITEMID);
                if (dt.Rows.Count > 0)
                {
                    cmbItemSetName.SelectedValue = dt.Rows[0]["TM01_MAIN_PRODUCTID"].ToString();
                    cmbItemName.SelectedValue = dt.Rows[0]["TM01_PRODUCTID"].ToString();
                    //cmbItemSize.SelectedValue = dt.Rows[0]["TM02_PRODUCTSIZEID"].ToString();
                    txtQTY.Text = dt.Rows[0]["TM03_QTY"].ToString();
                    cmbItemSetName.Focus();
                }
            }
        }

        void AddUpdateData()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(cmbItemSetName.Text.Trim()))
                {
                    ep1.SetError(cmbItemSetName, "This field is Required..");
                    cmbItemSetName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbItemName.Text.Trim()))
                {
                    ep1.SetError(cmbItemName, "This field is Required..");
                    cmbItemName.Focus();
                    return;
                }
                
                if (string.IsNullOrEmpty(txtQTY.Text.Trim()))
                {
                    ep1.SetError(txtQTY, "This field is Required..");
                    txtQTY.Focus();
                    return;
                }

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;

                int TM01_MAIN_PRODUCTID = 0, TM01_PRODUCTID = 0, TM02_PRODUCTSIZEID = 0, TM02_MAIN_PRODUCTSIZEID=0;
                int.TryParse(cmbItemSetName.SelectedValue.ToString(), out TM01_MAIN_PRODUCTID);
               // int.TryParse(cmbMainItemSize.SelectedValue.ToString(), out TM02_MAIN_PRODUCTSIZEID);
                int.TryParse(cmbItemName.SelectedValue.ToString(), out TM01_PRODUCTID);
               // int.TryParse(cmbItemSize.SelectedValue.ToString(), out TM02_PRODUCTSIZEID);

                int TM03_QTY = 0;
                int.TryParse(txtQTY.Text.Trim(), out TM03_QTY);

                MyDa.USP_VP_ADDUPDATE_PRODUCTITEM(Datalayer.iTM03_PRODUCTITEMID, TM01_MAIN_PRODUCTID, TM01_PRODUCTID, TM03_QTY,Datalayer.iT001_COMPANYID , ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iTM03_PRODUCTITEMID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.iTM03_PRODUCTITEMID == 0)
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
            Datalayer.iTM03_PRODUCTITEMID = 0;
            ep1.Clear();
            //Datalayer.Reset(panel1.Controls);
            txtQTY.Text = "";
            cmbItemSetName.Focus();
        }

        private void frmAddUpdateProductItem_FormClosed(object sender, FormClosedEventArgs e)
        {
            Datalayer.iTM03_PRODUCTITEMID = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdateData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        private void cmbItemSetName_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  dbo.FillItemSize(cmbItemSetName, cmbMainItemSize);
        }

        private void cmbItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  dbo.FillItemSize(cmbItemName, cmbItemSize);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            

            frmManageProductItem fm = new frmManageProductItem();
            fm.MdiParent = this.MdiParent;
            fm.Show();

            this.Close();
        }

    }
}
