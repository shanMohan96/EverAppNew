﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EverNewApp
{
    public partial class frmManageExpenseTransaction : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmManageExpenseTransaction()
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

            dbo.FillExpenseMaster(cmbName);
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
            frmAddUpdateExpenseTransaction fmAdd = new frmAddUpdateExpenseTransaction();
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

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_EXPENSETRANSACTIONResult> lst = new List<USP_VP_GET_EXPENSETRANSACTIONResult>();
            lst = MyDa.USP_VP_GET_EXPENSETRANSACTION(TM02_PARTYID, null, dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            dgDisplayData.Columns["T013_EXPENSETRANSACATIONID"].Visible = false;
            dgDisplayData.Columns["TM05_EXPENSEID"].Visible = false;
            dgDisplayData.Columns["T013_DETAILS"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;

            dgDisplayData.Columns["TM05_EXPENSE"].HeaderText = "Name";
            dgDisplayData.Columns["T013_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T013_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["T013_AMOUNT"].HeaderText = "Amount";

            dgDisplayData.Columns["TM05_EXPENSE"].DisplayIndex = 0;
            dgDisplayData.Columns["T013_DATE"].DisplayIndex = 1;
            dgDisplayData.Columns["T013_AMOUNT"].DisplayIndex = 2;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns["TM05_EXPENSE"].Width = 350;
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
                int.TryParse(dgDisplayData.CurrentRow.Cells["T013_EXPENSETRANSACATIONID"].Value.ToString(), out ID);

                Datalayer.iT013_EXPENSETRANSACATIONID = ID;

                frmAddUpdateExpenseTransaction frmAddSTD = new frmAddUpdateExpenseTransaction();
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T013_EXPENSETRANSACATIONID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        string Sq = "DELETE FROM T013_EXPENSETRANSACATION WHERE T013_EXPENSETRANSACATIONID=" + ID;
                        DAL dl = new DAL();
                        if (dl.ExecuteMethod(Sq))
                        {
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Expense Details");
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
