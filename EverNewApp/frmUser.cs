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
    public partial class frmUser : Form
    {
        MyDabaseDataContext db;
        public string sPageName = "User Deails";

        public frmUser()
        {
            InitializeComponent();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            PopulateUser();
        }

        private void frmUser_KeyDown(object sender, KeyEventArgs e)
        {
            Datalayer.SetButtion(btnClear);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(button1);
            Datalayer.SetButtion(button2);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        void PopulateUser()
        {
            db = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            var lst = (from u in db.TL01_USERs select u).ToList();
            if (lst != null && lst.Count > 0)
            {
                cmbUserName.DataSource = lst;
                cmbUserName.DisplayMember = "TL01_USERNAME";
                cmbUserName.ValueMember = "TL01_USERID";
            }
            else
                cmbUserName.DataSource = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUser();
        }

        void AddUser()
        {
            try
            {
                ep1.Clear();
                if (string.IsNullOrEmpty(txtOldPassword.Text.Trim()))
                {
                    ep1.SetError(txtOldPassword, "Old Password is requied.");
                    txtOldPassword.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    ep1.SetError(txtPassword, "Password is requied.");
                    txtPassword.Focus();
                    return;
                }
                db = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                var lst = (from u in db.TL01_USERs select u).Where(s => s.TL01_USERID == Datalayer.iTL01_USERID).ToList();
                if (lst != null && lst.Count > 0)
                {
                    if (lst[0].TL01_PASSWORD.ToString() != txtOldPassword.Text.Trim())
                    {
                        Datalayer.InformationMessageBox("Old password is not correct..");
                        txtOldPassword.Focus();
                        return;
                    }
                }

                db = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                TL01_USER tlUser = new TL01_USER();
                tlUser.TL01_PASSWORD = txtPassword.Text.Trim();
                tlUser.TL01_ROLE = "User";
                db.TL01_USERs.InsertOnSubmit(tlUser);
                db.SubmitChanges();

                txtPassword.Text = "";
                txtPassword.Focus();

                Datalayer.InsertMessageBox(sPageName);
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
            //txtUserName.Text = "";

            txtPassword.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetPassword();
        }

        void ResetPassword()
        {
            try
            {
                ep1.Clear();
                if (string.IsNullOrEmpty(cmbUserName.Text.Trim()))
                {
                    ep1.SetError(cmbUserName, "User Name is required.");
                    cmbUserName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtResetPassword.Text.Trim()))
                {
                    ep1.SetError(txtResetPassword, "Reset password is required.");
                    txtResetPassword.Focus();
                    return;
                }

                db = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                TL01_USER tblUser = db.TL01_USERs.Single(TL01_USER => TL01_USER.TL01_USERID.ToString() == cmbUserName.SelectedValue.ToString());
                tblUser.TL01_PASSWORD = txtResetPassword.Text.Trim();
                db.SubmitChanges();

                cmbUserName.SelectedIndex = 0;
                txtResetPassword.Text = "";

                Datalayer.InformationMessageBox("Password reset sucesfully.");
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cmbUserName.SelectedIndex = 0;
            txtResetPassword.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
