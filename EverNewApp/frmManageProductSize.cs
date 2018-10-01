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
    public partial class frmManageProductSize : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmManageProductSize()
        {
            InitializeComponent();
        }

        private void frmManageJobWorkInvoice_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillItemName(cmbName);
            //if (cmbName.Items.Count > 0)
            //    cmbName.SelectedIndex = -1;

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnAdd, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnEdit, "ctrl + E");
            t1.SetToolTip(btnDelete, "ctrl + D");
        }

        private void frmManageJobWorkInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (dgDisplayData.SelectedRows.Count > 0)
                    dgDisplayData.ClearSelection();
                else
                    this.Close();
            }
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
            if (e.Control && e.KeyCode == Keys.E)
                btnEdit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.N)
                btnAdd_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                btnExit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.D)
                btnDelete_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateProductSize fmAdd = new frmAddUpdateProductSize();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            int iPartyID = 0;
            string sPartyID = "";
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iPartyID);

            if (iPartyID > 0)
                sPartyID = iPartyID.ToString();

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_PRODUCT_SIZEResult> lstCategory = new List<USP_VP_GET_PRODUCT_SIZEResult>();
            lstCategory = MyDa.USP_VP_GET_PRODUCT_SIZE("", sPartyID, null, null, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lstCategory;

            dgDisplayData.Columns["TM01_PRODUCTID"].Visible = false;
            dgDisplayData.Columns["TM01_PRODUCTID1"].Visible = false;
            dgDisplayData.Columns["TM01_ISSET"].Visible = false;
            dgDisplayData.Columns["TM02_PRODUCTSIZEID"].Visible = false;
            dgDisplayData.Columns["TM01_PRODUCTID"].Visible = false;
            dgDisplayData.Columns["TM01_HSNCODE"].Visible = false;
            dgDisplayData.Columns["TM01_TAX_RATE"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID1"].Visible = false;

            dgDisplayData.Columns["TM01_NO"].HeaderText = "No";
            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["TM02_SIZE"].HeaderText = "Size";
            dgDisplayData.Columns["TM02_PRICE"].HeaderText = "Price";

            dgDisplayData.Columns["TM01_NO"].DisplayIndex = 0;
            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 1;
            dgDisplayData.Columns["TM02_SIZE"].DisplayIndex = 2;
            dgDisplayData.Columns["TM02_PRICE"].DisplayIndex = 3;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;
            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;


        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        void EditData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int ID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["TM02_PRODUCTSIZEID"].Value.ToString(), out ID);

                Datalayer.iTM02_PRODUCTSIZEID = ID;

                frmAddUpdateProductSize frmAddSTD = new frmAddUpdateProductSize();
                frmAddSTD.MdiParent = this.MdiParent;
                frmAddSTD.Show();

                this.Close();
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
                DelelteData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DelelteData();
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["TM02_PRODUCTSIZEID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_DELETE_PRODUCTSIZE(ID, ref Iout);

                        if (Iout > 0)
                        {
                            Datalayer.DeleteMessageBox("Product price Details");
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Product price Details");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();
        }


    }
}
