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
    public partial class frmManageExpenseMaster : Form
    {
        MyDabaseDataContext MyDa;
        public frmManageExpenseMaster()
        {
            InitializeComponent();
        }

        private void frmManageItem_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnAdd, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnEdit, "ctrl + E");
            t1.SetToolTip(btnDelete, "ctrl + D");
        }

        private void frmManageItem_KeyDown(object sender, KeyEventArgs e)
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateExpenseMaster fmAdd = new frmAddUpdateExpenseMaster();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void PopualteData()
        {
            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_EXPENSEResult> lstCategory = new List<USP_VP_GET_EXPENSEResult>();
            lstCategory = MyDa.USP_VP_GET_EXPENSE("", "", Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lstCategory;

            dgDisplayData.Columns["TM05_EXPENSEID"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;

            dgDisplayData.Columns["TM05_EXPENSE"].HeaderText = "Name";

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
                int.TryParse(dgDisplayData.CurrentRow.Cells["TM05_EXPENSEID"].Value.ToString(), out ID);

                Datalayer.iTM05_EXPENSEID = ID;

                frmAddUpdateExpenseMaster frmAddSTD = new frmAddUpdateExpenseMaster();
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

        private void dgDisplayData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["TM05_EXPENSEID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        string Sq = "DELETE FROM TM05_EXPENSE WHERE TM05_EXPENSEID=" + ID;
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
    }
}
