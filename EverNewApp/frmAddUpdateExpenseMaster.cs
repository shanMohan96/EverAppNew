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
    public partial class frmAddUpdateExpenseMaster : Form
    {
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        public static string sPageName = "Expense Details";

        public frmAddUpdateExpenseMaster()
        {
            InitializeComponent();
        }

        private void frmAddUpdateProduct_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            PopauteData();

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void PopauteData()
        {
            if (Datalayer.iTM05_EXPENSEID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM TM05_EXPENSE WHERE TM05_EXPENSEID=" + Datalayer.iTM05_EXPENSEID);
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["TM05_EXPENSE"].ToString();
                    txtName.Focus();
                }
            }
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

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATE_EXPENSE(Datalayer.iTM05_EXPENSEID,  txtName.Text.Trim(), Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iTM05_EXPENSEID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                    }
                }
                else
                {
                    if (Datalayer.iTM05_EXPENSEID == 0)
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
            Datalayer.iTM05_EXPENSEID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            txtName.Focus();
        }

        private void frmAddUpdateProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.iTM05_EXPENSEID = 0;
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
            frmManageExpenseMaster fm = new frmManageExpenseMaster();
            fm.MdiParent = this.MdiParent;
            fm.Show();
            this.Close();
        }

    }
}
