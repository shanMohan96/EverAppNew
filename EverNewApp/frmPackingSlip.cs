using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace EverNewApp
{
    public partial class frmPackingSlip : Form
    {
        DataTable dtProducts = new DataTable();
        MyDabaseDataContext MyDa;
        DAL dl = new DAL();
        DatabaseOperation dbo = new DatabaseOperation();
        public static string sPageName = "Packing Slip Details";

        public frmPackingSlip()
        {
            InitializeComponent();
        }

        private void frmPackingSlip_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSave);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(button1);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            PopualteLastBillNo();
            dbo.FillAccountList(cmbName, "c");

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();
        }

        private void frmPackingSlip_KeyDown(object sender, KeyEventArgs e)
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

        }

        void PopualteData()
        {
            if (Datalayer.T010_PACKINGSLIPID > 0)
            {
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("SELECT * FROM T010_PACKINGSLIP WHERE T010_PACKINGSLIPID=" + Datalayer.T010_PACKINGSLIPID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbName.SelectedValue = int.Parse(dt.Rows[0]["T001_ACCOUNTID"].ToString());
                    PopualtePackingDetails();
                    cmbName.Enabled = false;
                    dtpDate.Text = dt.Rows[0]["T010_DATE"].ToString();
                    txtBillNo.Text = dt.Rows[0]["T010_NO"].ToString();
                    txtTransport.Text = dt.Rows[0]["T010_TRANSPORT"].ToString();
                    txtLRNo.Text = dt.Rows[0]["T010_LRNO"].ToString();

                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    List<USP_VP_GET_ORDERLIST_FOR_PACKINGSLIP_ON_IDResult> lstCategory = new List<USP_VP_GET_ORDERLIST_FOR_PACKINGSLIP_ON_IDResult>();
                    lstCategory = MyDa.USP_VP_GET_ORDERLIST_FOR_PACKINGSLIP_ON_ID(Datalayer.T010_PACKINGSLIPID.ToString(), Datalayer.iT001_COMPANYID.ToString()).ToList();
                    dgDisplayData.DataSource = lstCategory;

                    dgDisplayData.Columns["T004_ORDERID"].Visible = false;

                    dgDisplayData.Columns["TM01_NAME"].HeaderText = "Name";
                    dgDisplayData.Columns["TM02_PRICE"].Visible = false;
                    dgDisplayData.Columns["TM02_SIZE"].HeaderText = "Size";
                    dgDisplayData.Columns["T004_DATE"].HeaderText = "Date";
                    dgDisplayData.Columns["T004_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
                    dgDisplayData.Columns["T004_QTY"].HeaderText = "Qty";
                    dgDisplayData.Columns["T004_PENDING_QTY"].HeaderText = "Packing Qty";
                    dgDisplayData.Columns["T004_IS_COMPLITE"].HeaderText = "Is Complite ?";
                    dgDisplayData.Columns["T004_IS_URGENT"].HeaderText = "Is Urgent ?";

                    dgDisplayData.Columns["T004_DATE"].DisplayIndex = 0;
                    dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 1;
                    dgDisplayData.Columns["TM02_SIZE"].DisplayIndex = 2;
                    dgDisplayData.Columns["T004_QTY"].DisplayIndex = 3;
                    dgDisplayData.Columns["T004_PENDING_QTY"].DisplayIndex = 4;
                    dgDisplayData.Columns["T004_IS_COMPLITE"].DisplayIndex = 5;
                    dgDisplayData.Columns["T004_IS_URGENT"].DisplayIndex = 6;

                    this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
                    dgDisplayData.EnableHeadersVisualStyles = false;

                    // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                    dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    dgDisplayData.ColumnHeadersHeight = 30;
                    dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                    dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                    dgDisplayData.Columns["T004_DATE"].Width = 100;
                    dgDisplayData.Columns["TM01_NAME"].Width = 350;

                    dgDisplayData.Columns["T004_DATE"].ReadOnly = true;
                    dgDisplayData.Columns["TM01_NAME"].ReadOnly = true;
                    dgDisplayData.Columns["TM02_SIZE"].ReadOnly = true;
                    dgDisplayData.Columns["T004_QTY"].ReadOnly = true;
                    dgDisplayData.Columns["T004_IS_COMPLITE"].ReadOnly = true;
                    dgDisplayData.Columns["T004_IS_URGENT"].ReadOnly = true;


                    cmbName.Focus();
                }
            }
        }

        void PopualtePackingDetails()
        {
            int iT001_ACCOUNTID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
            {
                int.TryParse(cmbName.SelectedValue.ToString(), out iT001_ACCOUNTID);
                if (iT001_ACCOUNTID > 0)
                {
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    List<USP_VP_GET_ORDERLIST_FOR_PACKINGSLIPResult> lstCategory = new List<USP_VP_GET_ORDERLIST_FOR_PACKINGSLIPResult>();
                    lstCategory = MyDa.USP_VP_GET_ORDERLIST_FOR_PACKINGSLIP(iT001_ACCOUNTID.ToString(), Datalayer.iT001_COMPANYID.ToString()).ToList();
                    dgDisplayData.DataSource = lstCategory;

                    dgDisplayData.Columns["T004_ORDERID"].Visible = false;

                    dgDisplayData.Columns["TM01_NAME"].HeaderText = "Name";
                    dgDisplayData.Columns["TM02_PRICE"].Visible = false;
                    dgDisplayData.Columns["TM02_SIZE"].HeaderText = "Size";
                    dgDisplayData.Columns["T004_DATE"].HeaderText = "Date";
                    dgDisplayData.Columns["T004_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
                    dgDisplayData.Columns["T004_QTY"].HeaderText = "Qty";
                    dgDisplayData.Columns["T004_PENDING_QTY"].HeaderText = "Pending Qty";
                    dgDisplayData.Columns["T004_IS_COMPLITE"].HeaderText = "Is Complite ?";
                    dgDisplayData.Columns["T004_IS_URGENT"].HeaderText = "Is Urgent ?";

                    dgDisplayData.Columns["T004_DATE"].DisplayIndex = 0;
                    dgDisplayData.Columns["TM01_NAME"].DisplayIndex = 1;
                    dgDisplayData.Columns["TM02_SIZE"].DisplayIndex = 2;
                    dgDisplayData.Columns["T004_QTY"].DisplayIndex = 3;
                    dgDisplayData.Columns["T004_PENDING_QTY"].DisplayIndex = 4;
                    dgDisplayData.Columns["T004_IS_COMPLITE"].DisplayIndex = 5;
                    dgDisplayData.Columns["T004_IS_URGENT"].DisplayIndex = 6;

                    this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
                    dgDisplayData.EnableHeadersVisualStyles = false;

                    // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                    dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    dgDisplayData.ColumnHeadersHeight = 30;
                    dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                    dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                    dgDisplayData.Columns["T004_DATE"].Width = 100;
                    dgDisplayData.Columns["TM01_NAME"].Width = 350;

                    dgDisplayData.Columns["T004_DATE"].ReadOnly = true;
                    dgDisplayData.Columns["TM01_NAME"].ReadOnly = true;
                    dgDisplayData.Columns["TM02_SIZE"].ReadOnly = true;
                    dgDisplayData.Columns["T004_QTY"].ReadOnly = true;
                    dgDisplayData.Columns["T004_IS_COMPLITE"].ReadOnly = true;
                    dgDisplayData.Columns["T004_IS_URGENT"].ReadOnly = true;
                }
                else
                    dgDisplayData.DataSource = null;
            }
            else
                dgDisplayData.DataSource = null;
        }

        void PopualteLastBillNo()
        {
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT T010_NO FROM T010_PACKINGSLIP WHERE TM_COMPAYID=" + Datalayer.iT001_COMPANYID + " ORDER BY T010_PACKINGSLIPID DESC");
            int iNo = 0;
            if (dtData != null && dtData.Rows.Count > 0)
                int.TryParse(dtData.Rows[0][0].ToString(), out iNo);


            txtBillNo.Text = (iNo + 1).ToString();
        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdatePackingSlip();
        }

        void AddUpdatePackingSlip()
        {

            try
            {
                ep1.Clear();
                if (string.IsNullOrEmpty(cmbName.Text.Trim()))
                {
                    ep1.SetError(cmbName, "This field is required.");
                    cmbName.Focus();
                    return;
                }
                if (dgDisplayData.Rows.Count == 0)
                {
                    Datalayer.InformationMessageBox("Record is required..");
                }

                int T010_PACKINGSLIPID = Datalayer.T010_PACKINGSLIPID;

                // int TM01_CUSTOMERID = 0;
                string T010_NO = "", T010_TRANSPORT = "", T010_LRNO = "";
                int T001_ACCOUNTID = 0;
                DateTime T003_DATE = dtpDate.Value;

                // int.TryParse(cmbCustomerName.SelectedValue.ToString(), out TM01_CUSTOMERID);
                T010_NO = txtBillNo.Text.Trim();
                T010_TRANSPORT = txtTransport.Text.Trim();
                T010_LRNO = txtLRNo.Text.Trim();
                int.TryParse(cmbName.SelectedValue.ToString(), out T001_ACCOUNTID);

                if (T010_PACKINGSLIPID > 0)
                {
                    int? iDeletePurchaseItem_out = 0;
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    MyDa.USP_VP_DELETE_ALL_PACKINGSLIPORDER(T010_PACKINGSLIPID, ref iDeletePurchaseItem_out);
                }

                int? T010_PACKINGSLIPID_out = 0;
                Cursor.Current = Cursors.WaitCursor;
                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                MyDa.USP_VP_ADDUPDATE_PACKING_SLIP(T010_PACKINGSLIPID, T001_ACCOUNTID, T010_NO, T003_DATE, T010_TRANSPORT, T010_LRNO, Datalayer.iT001_COMPANYID, ref T010_PACKINGSLIPID_out);
                if (T010_PACKINGSLIPID_out > 0)
                {
                    T010_PACKINGSLIPID = int.Parse(T010_PACKINGSLIPID_out.Value.ToString());
                    int? @T011_PACKINGSLIPORDERID_OUT = 0;
                    for (int i = 0; i < dgDisplayData.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dgDisplayData.Rows[i].Cells["T004_ORDERID"].Value)))
                        {
                            int T004_ORDERID = 0, T011_QTY = 0;
                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T004_ORDERID"].Value), out T004_ORDERID);
                            int.TryParse(Convert.ToString(dgDisplayData.Rows[i].Cells["T004_PENDING_QTY"].Value), out T011_QTY);

                            if (T011_QTY > 0)
                            {
                                int? T002_PURCHASEITEMID_Out = 0;
                                MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                                MyDa.USP_VP_ADDUPDATE_PACKINGSLIPORDER(0, T010_PACKINGSLIPID, T004_ORDERID, T011_QTY, Datalayer.iT001_COMPANYID, ref T002_PURCHASEITEMID_Out);
                            }
                        }
                    }

                    if (Datalayer.T010_PACKINGSLIPID == 0)
                    {
                        Datalayer.InsertMessageBox(sPageName);
                        ResteData();

                        if (Datalayer.ShowQuestMsg("are you sure do you want to print this bill ?"))
                        {
                            DAL dl = new DAL();
                            DataTable dt = new DataTable();
                            dt = dl.SelectMethod("exec USP_VP_GET_PRINT_PACKING_SLIP_ONID '" + T010_PACKINGSLIPID + "'");
                            if (dt.Rows.Count > 0)
                            {
                                ReportDocument RptDoc = new ReportDocument();
                                RptDoc.Load(Application.StartupPath + @"\Report\rptPackingSlip.rpt");
                                RptDoc.SetDataSource(dt);

                                Datalayer.RptReport = RptDoc;
                                Datalayer.sReportName = "Packing Slip Bill";

                                Report.frmReportViwer fmReport = new Report.frmReportViwer();
                                fmReport.Show();
                            }
                            else
                            {
                                Datalayer.InformationMessageBox("No Record..");
                            }
                        }

                        //DataTable dt = new DataTable();
                        //dt = dl.SelectMethod("exec USP_VP_GET_FULLSALEBILL '" + T003_SALEID + "'");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    ReportDocument RptDoc = new ReportDocument();

                        //    RptDoc.Load(Application.StartupPath + @"\Report\rptRetailInvoice.rpt");
                        //    RptDoc.SetDataSource(dt);

                        //    Datalayer.RptReport = RptDoc;
                        //    Datalayer.sReportName = "Invoice";

                        //    frmReportViewer fmReport = new frmReportViewer();
                        //    fmReport.Show();
                        //}
                    }
                    else
                    {
                        Datalayer.UpdateMessageBox(sPageName);
                        this.Close();
                    }
                }
                else
                {
                    if (Datalayer.T010_PACKINGSLIPID == 0)
                        Datalayer.DosenotInsertMessageBox(sPageName);
                    else
                        Datalayer.DosenotUpdateMessageBox(sPageName);
                    return;
                }
            }
            catch (Exception ex)
            {
                Datalayer.WorningMessageBox(ex.Message.ToString(), "Error");
            }
        }

        void ResteData()
        {
            cmbName.Enabled = true;
            Datalayer.T010_PACKINGSLIPID = 0;
            Datalayer.Reset(panel1.Controls);
            Cursor.Current = Cursors.Default;
            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualtePackingDetails();
            PopualteLastBillNo();
            cmbName.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResteData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PopualtePackingDetails();
        }

        private void frmPackingSlip_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datalayer.T010_PACKINGSLIPID = 0;
        }
    }
}
