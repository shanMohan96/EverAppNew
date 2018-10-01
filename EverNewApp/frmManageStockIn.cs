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
    public partial class frmManageStockIn : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Manage Material Process Details";


        public frmManageStockIn()
        {
            InitializeComponent();
        }

        private void frmManageStockIn_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbName, "pl");

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();
            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnAdd, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnEdit, "ctrl + E");
            t1.SetToolTip(btnDelete, "ctrl + D");
        }

        private void frmManageStockIn_KeyDown(object sender, KeyEventArgs e)
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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmStockIn fmAdd = new frmStockIn();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            string TM01_PRODUCTID = "", T001_ACCOUNTID = "";
            int iTM01_PRODUCTID = 0, iTM02_PRODUCTSIZEID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iTM02_PRODUCTSIZEID);

            if (iTM01_PRODUCTID > 0)
                TM01_PRODUCTID = iTM01_PRODUCTID.ToString();
            if (iTM02_PRODUCTSIZEID > 0)
                T001_ACCOUNTID = iTM02_PRODUCTSIZEID.ToString();

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_STOCK_MASTERResult> lst = new List<USP_VP_GET_STOCK_MASTERResult>();
            lst = MyDa.USP_VP_GET_STOCK_MASTER(T001_ACCOUNTID, dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            dgDisplayData.Columns["T007_STOCKINMASTERID"].Visible = false;
            dgDisplayData.Columns["T001_ACCOUNTID"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;
            dgDisplayData.Columns["T007_DETAILS"].Visible = false;

            //dgDisplayData.Columns["TM01_NO"].HeaderText = "NO";
            dgDisplayData.Columns["T001_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T007_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T007_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["List_Output"].HeaderText = "Item";
            //dgDisplayData.Columns["TM01_NAME"].HeaderText = "Item";
            //dgDisplayData.Columns["IN"].HeaderText = "IN";
            //dgDisplayData.Columns["OUT"].HeaderText = "OUT";
            //dgDisplayData.Columns["T005_WEIGHT"].HeaderText = "Weight";
            //dgDisplayData.Columns["T005_UNIT"].HeaderText = "Unit";


            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

            dgDisplayData.Columns["List_Output"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgDisplayData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns["T001_NAME"].Width = 150;
            dgDisplayData.Columns["T007_DATE"].Width = 100;
            dgDisplayData.Columns["List_Output"].Width = 600;
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
                int.TryParse(dgDisplayData.CurrentRow.Cells["T007_STOCKINMASTERID"].Value.ToString(), out ID);

                Datalayer.iT005_STOCKINID = ID;

                frmStockIn frmAddSTD = new frmStockIn();
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T007_STOCKINMASTERID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_DELETE_STOCK_IN(ID, ref Iout);
                        MyDa.USP_VP_DELETE_STOCK_MASTER(ID, ref Iout);

                        if (Iout > 0)
                        {
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Stock In Details");
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();
        }
    }
}
