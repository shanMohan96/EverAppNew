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
    public partial class frmManagePurchasePayment : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmManagePurchasePayment()
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



            dbo.FillAccountList(cmbName, Datalayer.sPaymentMode);

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnAdd, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnEdit, "ctrl + E");
            t1.SetToolTip(btnDelete, "ctrl + D");

            if (Datalayer.sPaymentMode == "R")
            {
                dbo.FillAccountList(cmbName, "c");
                lblTitle.Text = "Bill Receive Details";
            }
            else
            {
                dbo.FillAccountList(cmbName, "p");
                lblTitle.Text = "Bill Payment Details";
            }
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
            frmAddUpdatePurchasePayment fmAdd = new frmAddUpdatePurchasePayment();
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

            string sBillType = "";
            if (Datalayer.sPaymentMode == "R")
                sBillType = "Receive";
            else
                sBillType = "Payment";

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_PAYMENTResult> lst = new List<USP_VP_GET_PAYMENTResult>();
            lst = MyDa.USP_VP_GET_PAYMENT(TM02_PARTYID, null, sBillType, null, dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            dgDisplayData.Columns["T009_PAYMENTID"].Visible = false;
            dgDisplayData.Columns["T001_ACCOUNTID"].Visible = false;
            dgDisplayData.Columns["TM04_BANKID"].Visible = false;
            dgDisplayData.Columns["TM04_DETAILS"].Visible = false;
            dgDisplayData.Columns["T001_CONTACT_PERSON"].Visible = false;
            dgDisplayData.Columns["CREDIT"].Visible = false;
            dgDisplayData.Columns["DEBIT"].Visible = false;
            dgDisplayData.Columns["T009_PAYMENT_TYPE"].Visible = false;

            dgDisplayData.Columns["T001_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T001_MOBILE1"].HeaderText = "Mobile No";
            dgDisplayData.Columns["T009_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T009_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["T009_TYPE"].HeaderText = "Type";
            dgDisplayData.Columns["T009_PAYMENT_TYPE"].HeaderText = "Amount Type";
            dgDisplayData.Columns["T009_AMOUNT"].HeaderText = "Amount";

            dgDisplayData.Columns["T001_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T001_MOBILE1"].DisplayIndex = 1;
            dgDisplayData.Columns["T009_DATE"].DisplayIndex = 2;
            dgDisplayData.Columns["T009_TYPE"].DisplayIndex = 3;
            dgDisplayData.Columns["T009_PAYMENT_TYPE"].DisplayIndex = 4;
            dgDisplayData.Columns["T009_AMOUNT"].DisplayIndex = 5;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;


            dgDisplayData.Columns["T001_NAME"].Width = 350;
            dgDisplayData.Columns["T001_MOBILE1"].Width = 120;
            dgDisplayData.Columns["T009_DATE"].Width = 120;
            dgDisplayData.Columns["T009_TYPE"].Width = 120;
            dgDisplayData.Columns["T009_PAYMENT_TYPE"].Width = 120;
            dgDisplayData.Columns["T009_AMOUNT"].Width = 120;
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
                int.TryParse(dgDisplayData.CurrentRow.Cells["T009_PAYMENTID"].Value.ToString(), out ID);

                Datalayer.iT009_PURCHASEPAYMENTID = ID;


                frmAddUpdatePurchasePayment frmAddSTD = new frmAddUpdatePurchasePayment();
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T009_PAYMENTID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        string Sq = "DELETE FROM T009_PAYMENT WHERE T009_PAYMENTID=" + ID;
                        DAL dl = new DAL();
                        if (dl.ExecuteMethod(Sq))
                        {
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Payment Details");
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
