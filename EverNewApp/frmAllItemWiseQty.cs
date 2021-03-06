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
    public partial class frmAllItemWiseQty : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Stock In Details";


        public frmAllItemWiseQty()
        {
            InitializeComponent();
        }

        private void frmManageStockIn_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnExit);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            PopualteData();
            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnExit, "ctrl + X");
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
            if (e.Control && e.KeyCode == Keys.N)
                btnAdd_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.X)
                btnExit_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;

        }

        private void cmbItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_ALL_ITEM_ON_CHALLENResult> lst = new List<USP_VP_GET_ALL_ITEM_ON_CHALLENResult>();
            lst = MyDa.USP_VP_GET_ALL_ITEM_ON_CHALLEN(dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            //dgDisplayData.Columns["TM01_NO"].HeaderText = "No";
            dgDisplayData.Columns["TM01_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["ChallenQty"].HeaderText = "Qty";

         //   dgDisplayData.Columns["TM01_NO"].DisplayIndex = 0;
            dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 1;
            dgDisplayData.Columns["ChallenQty"].DisplayIndex = 2;
            
            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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

        private void dgDisplayData_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpTodate.Value = DateTime.Now;

            PopualteData();
        }
    }
}
