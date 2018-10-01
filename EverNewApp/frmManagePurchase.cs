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
    public partial class frmManagePurchase : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmManagePurchase()
        {
            InitializeComponent();
        }

        private void frmManagePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbName, "p");

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnAdd, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnEdit, "ctrl + E");
            t1.SetToolTip(btnDelete, "ctrl + D");
        }

        private void frmManagePurchase_KeyDown(object sender, KeyEventArgs e)
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
            frmAddUpdatePurchase fmAdd = new frmAddUpdatePurchase();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            string TM02_PARTYID = "";
            int iTM02_PARTYID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iTM02_PARTYID);

            if (iTM02_PARTYID > 0)
                TM02_PARTYID = iTM02_PARTYID.ToString();

            decimal dTotal = 0;

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_PURCHASELISTResult> lst = new List<USP_VP_GET_PURCHASELISTResult>();
            lst = MyDa.USP_VP_GET_PURCHASELIST(null, TM02_PARTYID, null, dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            for (int i = 0; i < lst.Count; i++)
            {
                dTotal = dTotal + lst[i].T002_NETAMOUNT.Value;
            }

            lblNetTotal.Text = dTotal.ToString();
            dgDisplayData.Columns["T002_PURCHASEID"].Visible = false;
            dgDisplayData.Columns["T002_DETAILS"].Visible = false;
            dgDisplayData.Columns["T002_TOTAL_AMT"].Visible = false;
            dgDisplayData.Columns["T002_DISCOUNT"].Visible = false;
            dgDisplayData.Columns["T002_TAX1RATE"].Visible = false;
            dgDisplayData.Columns["T002_TAX1AMOUNT"].Visible = false;
            dgDisplayData.Columns["T002_TAX2RATE"].Visible = false;
            dgDisplayData.Columns["T002_TAX2AMOUNT"].Visible = false;
            dgDisplayData.Columns["T002_TAX2RATE"].Visible = false;
            dgDisplayData.Columns["T002_TAX3AMOUNT"].Visible = false;
            dgDisplayData.Columns["T002_TAX3RATE"].Visible = false;
            dgDisplayData.Columns["T001_CONTACT_PERSON"].Visible = false;


            dgDisplayData.Columns["T001_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T001_MOBILE1"].HeaderText = "Mobile No";
            dgDisplayData.Columns["T002_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T002_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["T002_NO"].HeaderText = "Invoice No";
            dgDisplayData.Columns["T002_NETAMOUNT"].HeaderText = "Net Amount";
            dgDisplayData.Columns["List_Output"].HeaderText = "Item";

            dgDisplayData.Columns["T001_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T001_MOBILE1"].DisplayIndex = 1;
            dgDisplayData.Columns["T002_DATE"].DisplayIndex = 2;
            dgDisplayData.Columns["T002_NO"].DisplayIndex = 3;
            dgDisplayData.Columns["List_Output"].DisplayIndex = 4;
            dgDisplayData.Columns["T002_NETAMOUNT"].DisplayIndex = 5;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

           // dgDisplayData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
           // dgDisplayData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgDisplayData.Columns["List_Output"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgDisplayData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgDisplayData.Columns["List_Output"].Width = 450;

            dgDisplayData.Columns["T001_NAME"].Width = 250;
            dgDisplayData.Columns["T001_MOBILE1"].Width = 75;
            dgDisplayData.Columns["T002_DATE"].Width = 90;
            dgDisplayData.Columns["T002_NO"].Width = 75;
            dgDisplayData.Columns["T002_NETAMOUNT"].Width = 100;
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
                int.TryParse(dgDisplayData.CurrentRow.Cells["T002_PURCHASEID"].Value.ToString(), out ID);

                Datalayer.iT002_PURCHASEID = ID;

                frmAddUpdatePurchase frmAddSTD = new frmAddUpdatePurchase();
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T002_PURCHASEID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_DELETE_PURCHASE(ID, ref Iout);
                        MyDa.USP_VP_DELETE_PURCHASE_ITEM(ID, ref Iout);

                        if (Iout > 0)
                        {
                            Datalayer.DeleteMessageBox("Purchase Details");
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Purchase Details");
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
