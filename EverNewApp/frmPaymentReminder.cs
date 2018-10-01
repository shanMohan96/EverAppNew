using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;

namespace EverNewApp
{
    public partial class frmPaymentReminder : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmPaymentReminder()
        {
            InitializeComponent();
        }

        private void frmPaymentReminder_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);
            PopualteData();

            // customize dataviewgrid, add checkbox column
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 30;
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgDisplayData.Columns.Insert(0, checkboxColumn);

            // add checkbox header
            Rectangle rect = dgDisplayData.GetCellDisplayRectangle(0, -1, true);
            // set checkbox header to center of header cell. +1 pixel to position correctly.
            rect.X = rect.Location.X + (rect.Width / 4);

            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            dgDisplayData.Controls.Add(checkboxHeader);

        }

        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgDisplayData.RowCount; i++)
            {
                dgDisplayData[0, i].Value = ((CheckBox)dgDisplayData.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            dgDisplayData.EndEdit();

            //foreach (DataGridViewRow row in dgFromYearCourse.Rows)
            //{
            //    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
            //    if (chk.Selected == true)
            //    {
            //        chk.Selected = false;
            //    }
            //    else
            //    {
            //        chk.Selected = true;
            //    }
            //}
        }


        private void frmPaymentReminder_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        void PopualteData()
        {
            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_GET_CUSTOMER_BAL_SUMMARY '','" + Datalayer.iT001_COMPANYID + "'");
            if (dt.Rows.Count > 0)
            {
                DataTable dt1 = new DataTable();
                DataRow[] dr = dt.Select("TOTALPENDING >0 ");
                if (dr.Length > 0)
                {
                    dt1 = dr.CopyToDataTable();

                    dgDisplayData.DataSource = dt1;
                    dgDisplayData.Columns["TOTALBANK"].Visible = false;
                    dgDisplayData.Columns["TOTALCASH"].Visible = false;
                    dgDisplayData.Columns["TM_NAME"].Visible = false;
                    dgDisplayData.Columns["TM_PHONENO"].Visible = false;

                    dgDisplayData.Columns["T001_NAME"].HeaderText = "Name";
                    dgDisplayData.Columns["T001_MOBILE1"].HeaderText = "Mobile No";
                    dgDisplayData.Columns["TOTALSALE"].HeaderText = "Total Sale";
                    dgDisplayData.Columns["TOTALPAID"].HeaderText = "Total Paid";
                    dgDisplayData.Columns["TOTALPENDING"].HeaderText = "Total Pending";

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
            }
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgDisplayData.Rows.Count == 0)
            {
                Datalayer.InformationMessageBox("Please select customer ");
                return;
            }
            if (Datalayer.ShowQuestMsg("Are you sure? Do you want to Send SMS? "))
            {

                int iSend = 0, iNotSend = 0;

                for (int i = 0; i < dgDisplayData.Rows.Count; i++)
                {
                    if (Convert.ToString(dgDisplayData.Rows[i].Cells[0].Value) == "True")
                    {
                        string sMobileNo = dgDisplayData.Rows[i].Cells["T001_MOBILE1"].Value.ToString();
                        string sName = dgDisplayData.Rows[i].Cells["T001_NAME"].Value.ToString();
                        string sAmount = dgDisplayData.Rows[i].Cells["TOTALPENDING"].Value.ToString();
                        string sCompanyName = dgDisplayData.Rows[i].Cells["TM_NAME"].Value.ToString();
                        string sPhoneNo = dgDisplayData.Rows[i].Cells["TM_PHONENO"].Value.ToString();

                        string sMSG = "Dear, " + sName + " your pending amount payable to " + sCompanyName + " is Rs." + sAmount + " and it is now overdue. Please Contact us on " + sPhoneNo + " urgently to discuss our policy.";
                        bool IsSend = SendSMS(sMobileNo, sMSG);
                        if (IsSend)
                            iSend = iSend + 1;
                        else
                            iNotSend = iNotSend + 1;

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_ADDSMS(0, DateTime.Now, sMobileNo, sMSG, IsSend, Datalayer.iT001_COMPANYID, ref Iout);
                    }
                }

                Datalayer.InformationMessageBox("SMS Send successfully.");
                lblTotalSend.Text = "Total Send SMS:" + iSend.ToString();
                lblNotSend.Text = "Total Not Send SMS:" + iNotSend.ToString();
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                    // presumably online
                }
                else
                {
                    return false;
                }

                //using (var client = new WebClient())
                //using (var stream = client.OpenRead("http://www.google.com"))
                //{
                //    return true;
                //}
            }
            catch
            {
                return false;
            }
        }


        public bool SendSMS(string sMobileNumber, string sMsg)
        {
            // sMobileNumber = "9374045413";
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            bool bSMSsent = false;
            try
            {
                if (CheckForInternetConnection())
                {

                    string url = "http://login.arihantsms.com/vendorsms/pushsms.aspx?user=PENAMT&password=PENAMT&msisdn=91" + sMobileNumber.Trim() + "&sid=PENAMT&msg=" + sMsg.Trim() + "&fl=0&gwid=2";
                    request = WebRequest.Create(url);

                    // Send the 'HttpWebRequest' and wait for response.
                    response = (HttpWebResponse)request.GetResponse();

                    Stream stream = response.GetResponseStream();
                    Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader reader = new System.IO.StreamReader(stream, ec);
                    result = reader.ReadToEnd();

                    int iT025_SMSLOGID_OUT = 0;
                    bSMSsent = (result.ToLower().IndexOf("success") >= 0);
                    //S  

                    reader.Close();
                    stream.Close();
                }
                else
                {
                    Datalayer.InformationMessageBox("Please check the internet connection..");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
            finally
            {
                if (response != null)
                    response.Close();

            }
            return bSMSsent;
        }

    }
}
