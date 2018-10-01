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
    public partial class frmUserLogin : Form
    {
        MyDabaseDataContext MyDa;
        public static string sPageName = "User Details";

        public frmUserLogin()
        {
            InitializeComponent();
            Datalayer.MakeConnections();
        }

        private void frmUserLogin_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);
        }

        private void frmUserLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        void CheckUser()
        {
            try
            {
                ep1.Clear();

                if (string.IsNullOrEmpty(txtUserPassword.Text.Trim()))
                {
                    ep1.SetError(txtUserPassword, "Password is required..");
                    txtUserPassword.Focus();
                    return;
                }

                DateTime myDateTime = DateTime.Now;
                DateTime checkDate1 = DateTime.Parse("2018-05-05");
                DateTime checkDate2 = DateTime.Parse("2018-08-25");

                //if ((myDateTime >= checkDate1 && myDateTime <= checkDate2) == false)
                //{
                //    Datalayer.InformationMessageBox("Demo is expried.");
                //    return;
                //}

                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                var list = (from TL01_USER in MyDa.GetTable<TL01_USER>() where TL01_USER.TL01_PASSWORD == txtUserPassword.Text.Trim() select TL01_USER).ToList();
                if (list.Count > 0 && list != null)
                {

                    //if (DateTime.Now.Month >= 3)
                    //{
                    //    Datalayer.InformationMessageBox("Your Demo is Expire. Please contact to admin.");
                    //    this.Close();
                    //}
                    //if (DateTime.Now.Month <= 2)
                    //{
                    //    Datalayer.InformationMessageBox("Your Demo is Expire. Please contact to admin.");
                    //    this.Close();
                    //}


                    Datalayer.iTL01_USERID = list[0].TL01_USERID;
                    Datalayer.UserType = list[0].TL01_ROLE.ToString();

                    frmCompanySelection fmMain = new frmCompanySelection();
                    fmMain.Show();

                    this.Hide();
                    //   }
                }
                else
                {
                    Datalayer.InformationMessageBox("User name or Password is wrong. please try agin..");
                    txtUserPassword.Text = "";
                    txtUserPassword.Focus();
                }

            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //int fallTime = 0;
            //if (fallTime == 0)
            //    Console.Beep(5000, 1000);

            CheckUser();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();

            Application.Exit();
        }
    }
}
