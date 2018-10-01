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
    public partial class frmAddUpdateAccount : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Party Details";

        public frmAddUpdateAccount()
        {
            InitializeComponent();
        }

        private void frmAddUpdateParty_Load(object sender, EventArgs e)
        {
            dbo.FillStateList(cmbState);
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);


            cmbType.SelectedIndex = 0;
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
            string sType = "";
            if (!string.IsNullOrEmpty(Convert.ToString(cmbType.Text.Trim())))
                sType = cmbType.SelectedItem.ToString();

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_ACCOUNTResult> lstCategory = new List<USP_VP_GET_ACCOUNTResult>();
            //lstCategory = MyDa.USP_VP_GET_ACCOUNT("", txtName.Text.Trim(), "", sType, "", txtMobileNo.Text.Trim(), Datalayer.iT001_COMPANYID.ToString()).ToList();
            lstCategory = MyDa.USP_VP_GET_ACCOUNT("", "", "", "", "", "", Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lstCategory;

            dgDisplayData.Columns["T001_ACCOUNTID"].Visible = false;
            dgDisplayData.Columns["T001_CONTACT_PERSON"].Visible = false;
            dgDisplayData.Columns["T001_ADDRESS1"].Visible = false;
            dgDisplayData.Columns["T001_ADDRESS2"].Visible = false;
            dgDisplayData.Columns["TM_STATEID"].Visible = false;
            dgDisplayData.Columns["TM_STATEID1"].Visible = false;
            dgDisplayData.Columns["TM_STATE_NAME"].Visible = false;
            dgDisplayData.Columns["TM_STATE_CODE"].Visible = false;
            dgDisplayData.Columns["T001_MOBILE2"].Visible = false;
            dgDisplayData.Columns["T001_PHONE"].Visible = false;
            dgDisplayData.Columns["T001_EMAIL"].Visible = false;
            dgDisplayData.Columns["T001_OPENINGBAL"].Visible = false;
            dgDisplayData.Columns["T001_GSTTIN"].Visible = false;
            dgDisplayData.Columns["T001_TIN"].Visible = false;
            dgDisplayData.Columns["T001_DISCOUNT"].Visible = false;
            dgDisplayData.Columns["TM_COMPAYID"].Visible = false;

            dgDisplayData.Columns["T001_TYPE"].Visible = false;
            dgDisplayData.Columns["T001_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T001_MOBILE1"].HeaderText = "Mobile No";
            dgDisplayData.Columns["T001_CITY"].Visible = false;

            dgDisplayData.Columns["T001_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T001_MOBILE1"].DisplayIndex = 1;


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
            if (Datalayer.iT001_ACCOUNTID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T001_ACCOUNT WHERE T001_ACCOUNTID=" + Datalayer.iT001_ACCOUNTID);
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["T001_NAME"].ToString();
                    txtContactPerson.Text = dt.Rows[0]["T001_CONTACT_PERSON"].ToString();
                    txtAddress1.Text = dt.Rows[0]["T001_ADDRESS1"].ToString();
                    txtAddress2.Text = dt.Rows[0]["T001_ADDRESS2"].ToString();
                    txtCity.Text = dt.Rows[0]["T001_CITY"].ToString();
                    cmbState.SelectedValue = Convert.ToSingle(dt.Rows[0]["TM_STATEID"]);
                    txtMobile1.Text = dt.Rows[0]["T001_MOBILE1"].ToString();
                    txtMobile2.Text = dt.Rows[0]["T001_MOBILE2"].ToString();
                    txtPhone.Text = dt.Rows[0]["T001_PHONE"].ToString();
                    txtEmailID.Text = dt.Rows[0]["T001_EMAIL"].ToString();
                    txtGSTNo.Text = dt.Rows[0]["T001_GSTTIN"].ToString();
                    txtTinNo.Text = dt.Rows[0]["T001_TIN"].ToString();
                    txtOpeningBal.Text = dt.Rows[0]["T001_OPENINGBAL"].ToString();
                    txtDiscount.Text = dt.Rows[0]["T001_DISCOUNT"].ToString();
                    cmbType.SelectedItem = dt.Rows[0]["T001_TYPE"].ToString();

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
                decimal T001_OPENINGBAL = 0, T001_DISCOUNT = 0;
                decimal.TryParse(txtOpeningBal.Text.Trim(), out T001_OPENINGBAL);
                decimal.TryParse(txtDiscount.Text.Trim(), out T001_DISCOUNT);

                int TM_STATEID = 0;
                int.TryParse(cmbState.SelectedValue.ToString(), out TM_STATEID);

                int? Iout = 0;
                MyDa.USP_VP_ADDUPDATE_ACCOUNT(Datalayer.iT001_ACCOUNTID, txtName.Text.Trim(), txtContactPerson.Text.Trim(), txtAddress1.Text.Trim(), txtAddress2.Text.Trim(), cmbType.SelectedItem.ToString(), txtCity.Text.Trim(), TM_STATEID, txtMobile1.Text.Trim(), txtMobile2.Text.Trim(), txtPhone.Text.Trim(), txtEmailID.Text.Trim(), T001_OPENINGBAL, txtGSTNo.Text.Trim(), txtTinNo.Text.Trim(), T001_DISCOUNT, Datalayer.iT001_COMPANYID, ref Iout);
                if (Iout > 0)
                {
                    if (Datalayer.iT001_ACCOUNTID == 0)
                        Datalayer.InsertMessageBox(sPageName);
                    else
                        Datalayer.UpdateMessageBox(sPageName);
                }
                else
                {
                    if (Datalayer.iT001_ACCOUNTID == 0)
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
            Datalayer.iT001_ACCOUNTID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            cmbType.SelectedIndex = 0;
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
            Datalayer.iT001_ACCOUNTID = 0;
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
                int.TryParse(dgDisplayData.CurrentRow.Cells["T001_ACCOUNTID"].Value.ToString(), out ID);

                Datalayer.iT001_ACCOUNTID = ID;
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T001_ACCOUNTID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        string Sq = "DELETE FROM T001_ACCOUNT WHERE T001_ACCOUNTID=" + ID;
                        DAL dl = new DAL();
                        if (dl.ExecuteMethod(Sq))
                        {
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Account Details");
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
    }
}
