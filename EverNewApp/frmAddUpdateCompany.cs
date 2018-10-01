using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EverNewApp
{
    public partial class frmAddUpdateCompany : Form
    {
        MyDabaseDataContext db;
        public static string sPageName = "Company Details";
        byte[] bytStudentPhoto;
        byte[] bytStudentSign;

        public frmAddUpdateCompany()
        {
            InitializeComponent();
        }

        private void frmAddUpdateCompany_Load(object sender, EventArgs e)
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

        private void frmAddUpdateCompany_KeyDown(object sender, KeyEventArgs e)
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

        DAL dl = new DAL();

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void PopauteData()
        {
            if (Datalayer.iEditT001_COMPANYID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM TM_COMPANY WHERE TM_COMPAYID=" + Datalayer.iEditT001_COMPANYID);
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["TM_NAME"].ToString();
                    txtAddress1.Text = dt.Rows[0]["TM_ADDRESS1"].ToString();
                    txtPhoneNo.Text = dt.Rows[0]["TM_PHONENO"].ToString();
                    txtEmailID.Text = dt.Rows[0]["TM_EMAIL"].ToString();
                    txtGSTNo.Text = dt.Rows[0]["TM_TINNO"].ToString();
                    txtBankName.Text = dt.Rows[0]["TM_BANK"].ToString();
                    txtBranch.Text = dt.Rows[0]["TM_BRANCH"].ToString();
                    txtIFSCCOde.Text = dt.Rows[0]["TM_IFSC"].ToString();
                    txtAccountNo.Text = dt.Rows[0]["TM_ACCOUNTNO"].ToString();
                    chkIsActive.Checked = dt.Rows[0]["TM_ACTIVE"].ToString() == "True" ? true : false;
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["TM_IMAGE"])))
                    {
                        db = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        var r = (from a in db.TM_COMPANies where a.TM_COMPAYID == Datalayer.iT001_COMPANYID select a).First();
                        MemoryStream ms = new MemoryStream(r.TM_IMAGE.ToArray());
                        ms.Seek(0, SeekOrigin.Begin);

                        bytStudentPhoto = (byte[])r.TM_IMAGE.ToArray();
                    }
                    else
                        bytStudentPhoto = null;

                    txtName.Focus();
                }

            }
        }

        void AddUpdateCompany()
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

                FileStream fsStudentPhto;
                BinaryReader brStudentPhoto;
                if (!string.IsNullOrEmpty(txtPhoto.Text.Trim()))
                {
                    string FileName = txtPhoto.Text.Trim();

                    fsStudentPhto = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    brStudentPhoto = new BinaryReader(fsStudentPhto);
                    bytStudentPhoto = brStudentPhoto.ReadBytes((int)fsStudentPhto.Length);
                    brStudentPhoto.Close();
                    fsStudentPhto.Close();
                }


                int? Iout = 0;
                if (Datalayer.iEditT001_COMPANYID > 0)
                {
                    db = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    TM_COMPANY _TM_COMPANY = db.TM_COMPANies.Single(TM_COMPANY => TM_COMPANY.TM_COMPAYID.ToString() == Datalayer.iEditT001_COMPANYID.ToString());
                    _TM_COMPANY.TM_NAME = txtName.Text.Trim();
                    _TM_COMPANY.TM_ADDRESS1 = txtAddress1.Text.Trim();
                    _TM_COMPANY.TM_PHONENO = txtPhoneNo.Text.Trim();
                    _TM_COMPANY.TM_EMAIL = txtEmailID.Text.Trim();
                    _TM_COMPANY.TM_BANK = txtBankName.Text.Trim();
                    _TM_COMPANY.TM_BRANCH = txtBranch.Text.Trim();
                    _TM_COMPANY.TM_IFSC = txtIFSCCOde.Text.Trim();
                    _TM_COMPANY.TM_ACCOUNTNO = txtAccountNo.Text.Trim();
                    _TM_COMPANY.TM_ACTIVE = chkIsActive.Checked;
                    _TM_COMPANY.TM_TINNO = txtGSTNo.Text.Trim();
                    if (bytStudentPhoto != null)
                        _TM_COMPANY.TM_IMAGE = bytStudentPhoto;
                    db.SubmitChanges();

                    Datalayer.UpdateMessageBox(sPageName);

                   // Datalayer.InformationMessageBox("Please restart software.");
                    this.Close();
                }
                else
                {
                    TM_COMPANY _TM_COMPANY = new TM_COMPANY();
                    _TM_COMPANY.TM_NAME = txtName.Text.Trim();
                    _TM_COMPANY.TM_ADDRESS1 = txtAddress1.Text.Trim();
                    _TM_COMPANY.TM_PHONENO = txtPhoneNo.Text.Trim();
                    _TM_COMPANY.TM_EMAIL = txtEmailID.Text.Trim();
                    _TM_COMPANY.TM_BANK = txtBankName.Text.Trim();
                    _TM_COMPANY.TM_BRANCH = txtBranch.Text.Trim();
                    _TM_COMPANY.TM_IFSC = txtIFSCCOde.Text.Trim();
                    _TM_COMPANY.TM_ACCOUNTNO = txtAccountNo.Text.Trim();
                    _TM_COMPANY.TM_TINNO = txtGSTNo.Text.Trim();
                    _TM_COMPANY.TM_ACTIVE = chkIsActive.Checked;
                    if (!string.IsNullOrEmpty(txtPhoto.Text))
                        _TM_COMPANY.TM_IMAGE = bytStudentPhoto;

                    db = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    db.TM_COMPANies.InsertOnSubmit(_TM_COMPANY);
                    db.SubmitChanges();

                    Datalayer.InsertMessageBox(sPageName);
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
            Datalayer.iT001_COMPANYID = 0;
            ep1.Clear();
            Datalayer.Reset(panel1.Controls);
            txtName.Focus();
        }

        private void frmAddUpdateCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.iEditT001_COMPANYID = 0;
            if (Datalayer.iT001_COMPANYID == 0)
            {
                //frmCompanySelection fmSchool = new frmCompanySelection();
                //fmSchool.Show();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdateCompany();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetData();
        }


        private void txtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '£')
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void linkBrowse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                // openFileDialog1.Filter = "Image files | *.jpg";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtPhoto.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
