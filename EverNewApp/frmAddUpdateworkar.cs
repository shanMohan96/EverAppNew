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
    public partial class frmAddUpdateworkar : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Party Details";

        public frmAddUpdateworkar()
        {
            InitializeComponent();
        }

        private void frmAddUpdateParty_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(button1, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnSave, "ctrl + S");
        }

        DAL dl = new DAL();
        private void frmAddUpdateParty_KeyDown(object sender, KeyEventArgs e)
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

        void PopualteData()
        {
            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_WORKARResult> lstCategory = new List<USP_VP_GET_WORKARResult>();
            lstCategory = MyDa.USP_VP_GET_WORKAR(Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lstCategory;

            dgDisplayData.Columns["T14_WORKERID"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;
            dgDisplayData.Columns["T14_DETAILS"].Visible = false;
            dgDisplayData.Columns["T14_HOURS_PRICE"].Visible = false;
            dgDisplayData.Columns["T14_DAY_PRICE"].Visible = false;
            dgDisplayData.Columns["T14_ADDRESS"].Visible = false;

            dgDisplayData.Columns["T14_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T14_PHONE_NO"].HeaderText = "Phone No";

            dgDisplayData.Columns["T14_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T14_PHONE_NO"].DisplayIndex = 1;


            dgDisplayData.AllowUserToResizeColumns = false;
            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;

            //this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            //dgDisplayData.EnableHeadersVisualStyles = false;

            //// dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            //dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //dgDisplayData.ColumnHeadersHeight = 30;
            //dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            //dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;


        }


        void PopauteEditData()
        {
            if (Datalayer.iT14_WORKERID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T14_WORKER WHERE T14_WORKERID=" + Datalayer.iT14_WORKERID);
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["T14_NAME"].ToString();
                    txtAddress1.Text = dt.Rows[0]["T14_ADDRESS"].ToString();
                    txtMobile1.Text = dt.Rows[0]["T14_PHONE_NO"].ToString();
                    txtDayPrice.Text = dt.Rows[0]["T14_DAY_PRICE"].ToString();
                    txtHoursPrice.Text = dt.Rows[0]["T14_HOURS_PRICE"].ToString();
                    txtDetails.Text = dt.Rows[0]["T14_DETAILS"].ToString();

                    txtName.Focus();
                }
            }
        }

        void AddUpdateParty()
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
                decimal T14_DAY_PRICE = 0, T14_HOURS_PRICE = 0;
                decimal.TryParse(txtDayPrice.Text.Trim(), out T14_DAY_PRICE);
                decimal.TryParse(txtHoursPrice.Text.Trim(), out T14_HOURS_PRICE);

                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATE_WORKER(Datalayer.iT14_WORKERID, txtName.Text.Trim(), txtAddress1.Text.Trim(), txtMobile1.Text.Trim(), T14_DAY_PRICE, T14_HOURS_PRICE, txtDetails.Text.Trim(), Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iT14_WORKERID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                        Datalayer.UpdateMessageBox(sPageName);
                }
                else
                {
                    if (Datalayer.iT14_WORKERID == 0)
                        Datalayer.DosenotInsertMessageBox(sPageName);
                    else
                        Datalayer.DosenotUpdateMessageBox(sPageName);
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
            Datalayer.iT14_WORKERID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdateParty();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetData();

        }

        private void frmAddUpdateParty_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.iT14_WORKERID = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmMangeAccount fm = new frmMangeAccount();
            fm.MdiParent = this.MdiParent;
            fm.Show();
            this.Close();
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
                int.TryParse(dgDisplayData.CurrentRow.Cells["T14_WORKERID"].Value.ToString(), out ID);

                Datalayer.iT14_WORKERID = ID;
                PopauteEditData();
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

        void DelelteData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {

                if (Datalayer.ShowQuestMsg(Datalayer.sMessageConfirmation))
                {
                    try
                    {


                        int ID = 0;
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T14_WORKERID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        string Sq = "DELETE FROM T14_WORKER WHERE T14_WORKERID=" + ID;
                        DAL dl = new DAL();
                        if (dl.ExecuteMethod(Sq))
                        {
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Workar Details");
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
            EditData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DelelteData();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
