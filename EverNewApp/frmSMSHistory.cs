using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;

namespace EverNewApp
{
    public partial class frmSMSHistory : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Order Details";

        public frmSMSHistory()
        {
            InitializeComponent();
        }

        private void frmOrder_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            PopualteData();


            // customize dataviewgrid, add checkbox column
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 30;
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgDisplayData.Columns.Insert(0, checkboxColumn);

            // add checkbox header
            Rectangle rect = dgDisplayData.GetCellDisplayRectangle(0, -1, true);
            // set checkbox header to center of header cell. +1 pixel to position correctly.
            //rect.X = rect.Location.X + (rect.Width / 4);
            rect.X = 10;

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

        }


        private void frmOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");

            if (e.Control && e.KeyCode == Keys.X)
                this.Close();
            if (e.Control && e.KeyCode == Keys.M)
                this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        void PopualteData()
        {
            Boolean bStatus = false;
            if (!string.IsNullOrEmpty(comboBox1.Text.Trim()))
            {
                if (comboBox1.SelectedItem.ToString() == "Send")
                    bStatus = true;
            }

            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GET_SMSResult> lst = new List<USP_VP_GET_SMSResult>();
            lst = MyDa.USP_VP_GET_SMS(dtpDate.Value, bStatus.ToString(), Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;


        }

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.Columns["SMSID"].Visible = false;
            dgDisplayData.Columns["IsSend"].Visible = false;
            dgDisplayData.Columns["SMS_Date"].Visible = false;

            dgDisplayData.Columns["MobileNo"].HeaderText = "Mobile No";
            dgDisplayData.Columns["Msg"].HeaderText = "Msg";

            //dgDisplayData.Columns["MobileNo"].DisplayIndex = 0;
            //dgDisplayData.Columns["Msg"].DisplayIndex = 1;

            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns[0].Width = 50;
            dgDisplayData.Columns[1].Width = 75;
            dgDisplayData.Columns[2].Width = 250;

            dgDisplayData.ClearSelection();
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Datalayer.ShowQuestMsg("Are you sure? Do you want to Send SMS? "))
            {

                int iSend = 0, iNotSend = 0;

                for (int i = 0; i < dgDisplayData.Rows.Count; i++)
                {
                    if (Convert.ToString(dgDisplayData.Rows[i].Cells[0].Value) == "True")
                    {
                        string sMobileNo = dgDisplayData.Rows[i].Cells["MobileNo"].Value.ToString();
                        string sMSG = dgDisplayData.Rows[i].Cells["Msg"].Value.ToString();
                        string SMSID = dgDisplayData.Rows[i].Cells["SMSID"].Value.ToString();

                        int iD = 0;
                        int.TryParse(SMSID, out iD);
                        bool IsSend = SendSMS(sMobileNo, sMSG);
                        if (IsSend)
                            iSend = iSend + 1;
                        else
                            iNotSend = iNotSend + 1;

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_ADDSMS(iD, DateTime.Now, sMobileNo, sMSG, IsSend, Datalayer.iT001_COMPANYID, ref Iout);
                    }
                }

                Datalayer.InformationMessageBox("SMS Send successfully.");
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            PopualteData();
        }


    }
}
