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
    public partial class frmAddFullProduct : Form
    {
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        public static string sPageName = "Item Details";

        public frmAddFullProduct()
        {
            InitializeComponent();
        }

        private void frmAddUpdateProduct_Load(object sender, EventArgs e)
        {
            AutoCompliteName();
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);
            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(button1, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnSave, "ctrl + S");
        }

        private void frmAddUpdateProduct_KeyDown(object sender, KeyEventArgs e)
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

        void AutoCompliteName()
        {
            //AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
            //DataTable dtData = new DataTable();
            //dtData = dl.SelectMethod("SELECT TM01_PRODUCTID,TM01_NO FROM TM01_PRODUCT WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM01_NO");
            //if (dtData != null && dtData.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dtData.Rows.Count; i++)
            //    {
            //        MyCollection.Add(dtData.Rows[i]["TM01_NO"].ToString());
            //    }
            //}

            //txtSearchNo.AutoCompleteMode = AutoCompleteMode.Suggest;
            //txtSearchNo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //txtSearchNo.AutoCompleteCustomSource = MyCollection;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void PopualteData()
        {
            string sNO = "";// txtSearchNo.Text.Trim();

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_PRODUCTResult> lstCategory = new List<USP_VP_GET_PRODUCTResult>();
            lstCategory = MyDa.USP_VP_GET_PRODUCT("",  null, null, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lstCategory;

            dgDisplayData.Columns["TM01_PRODUCTID"].Visible = false;
            dgDisplayData.Columns["TM01_ISSET"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;

            //dgDisplayData.Columns["TM01_NO"].HeaderText = "No";
            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["TM01_HSNCODE"].Visible = false;
            dgDisplayData.Columns["TM01_PRICE"].Visible = false;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;
            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            //dgDisplayData.Columns["TM01_NO"].Width = 100;

        }

        void AddUpdateData()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    ep1.SetError(txtName, "Name is Required..");
                    txtName.Focus();
                    return;
                }

           
                decimal dTM02_PRICE1 = 0, dTM02_PRICE2 = 0;
                decimal.TryParse(txtPrice.Text.Trim(), out dTM02_PRICE1);
              

                int iTM02_PRODUCTSIZEID1 = 0, iTM02_PRODUCTSIZEID2 = 0;
                int.TryParse(lblSizeId1.Text.Trim(), out iTM02_PRODUCTSIZEID1);
                int.TryParse(lblSizeId2.Text.Trim(), out iTM02_PRODUCTSIZEID2);

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;
                MyDa.USP_VP_ADD_PRODUCT(Datalayer.iTM01_PRODUCTID,txtName.Text.Trim(), chkIsSet.Checked, txtHSNCode.Text.Trim(), dTM02_PRICE1,  Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    Datalayer.InsertMessageBox(sPageName);
                }
                else
                {
                    Datalayer.DosenotInsertMessageBox(sPageName);
                }

                PopualteData();
                ResetData();
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void ResetData()
        {
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            txtName.Focus();
        }

        private void frmAddUpdateProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.iTM01_PRODUCTID = 0;
        }

        void PopauteEditData()
        {
            if (Datalayer.iTM01_PRODUCTID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM TM01_PRODUCT WHERE TM01_PRODUCTID=" + Datalayer.iTM01_PRODUCTID);
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["TM01_NAME"].ToString();
                    txtHSNCode.Text = dt.Rows[0]["TM01_HSNCODE"].ToString();
                    txtPrice.Text = dt.Rows[0]["TM01_PRICE"].ToString();

                    bool bIsSet = false;
                    bool.TryParse(Convert.ToString(dt.Rows[0]["TM01_ISSET"]), out bIsSet);
                    chkIsSet.Checked = bIsSet;
                    txtName.Focus();
                }

              
            }
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
            //frmManageItem fm = new frmManageItem();
            //fm.MdiParent = this.MdiParent;
            //fm.Show();

            this.Close();

        }

        private void dgDisplayData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                EditData();
            if (e.KeyCode == Keys.Delete)
                DelelteData();
        }

        void EditData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int ID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["TM01_PRODUCTID"].Value.ToString(), out ID);

                Datalayer.iTM01_PRODUCTID = ID;
                PopauteEditData();

            }
            else
            {
                Datalayer.InformationMessageBox(Datalayer.sMeessgeSelection);
            }
        }

        void DelelteData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {

                if (Datalayer.ShowQuestMsg(Datalayer.sMessageConfirmation))
                {
                    try
                    {
                        int ID = 0;
                        int.TryParse(dgDisplayData.CurrentRow.Cells["TM01_PRODUCTID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        string Sq = "DELETE FROM TM01_PRODUCT WHERE TM01_PRODUCTID=" + ID;
                        DAL dl = new DAL();
                        if (dl.ExecuteMethod(Sq))
                        {
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Product Details");
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ResetData();
            EditData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DelelteData();
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void dgDisplayData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditData();
        }
    }
}
