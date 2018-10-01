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
    public partial class frmManageChallen : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmManageChallen()
        {
            InitializeComponent();
        }

        private void frmManagePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnSearch);
            Datalayer.SetButtion(btnClear);

            Datalayer.SetButtion(btnPrint);
            Datalayer.SetButtion(btnAdd);
            Datalayer.SetButtion(btnExit);
            Datalayer.SetButtion(btnEdit);
            Datalayer.SetButtion(btnDelete);
            Datalayer.SetButtion(btnPackingSlip);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            dbo.FillAccountList(cmbName, "c");

            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(btnAdd, "ctrl + N");
            t1.SetToolTip(btnExit, "ctrl + X");
            t1.SetToolTip(btnEdit, "ctrl + E");
            t1.SetToolTip(btnDelete, "ctrl + D");
        }

        private void frmManagePurchase_KeyDown(object sender, KeyEventArgs e)
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmChallen fmChallen = new frmChallen();
            fmChallen.MdiParent = this.MdiParent;
            fmChallen.Show();

            //frmAddUpdateChallen fmAdd = new frmAddUpdateChallen();
            //fmAdd.MdiParent = this.MdiParent;
            //fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            string TM02_PARTYID = "";
            int iTM02_PARTYID = 0;
            if (!string.IsNullOrEmpty(cmbName.Text.Trim()))
                int.TryParse(cmbName.SelectedValue.ToString(), out iTM02_PARTYID);

            if (iTM02_PARTYID > 0)
                TM02_PARTYID = iTM02_PARTYID.ToString();

            decimal dTotal = 0;
            MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            List<USP_VP_GETCHALLENLISTResult> lst = new List<USP_VP_GETCHALLENLISTResult>();
            lst = MyDa.USP_VP_GETCHALLENLIST(null, TM02_PARTYID, null, dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;

            for (int i = 0; i < lst.Count; i++)
            {
                dTotal = dTotal + lst[i].T012_NETAMOUNT.Value;
            }

            lblNetTotal.Text = dTotal.ToString();
            dgDisplayData.Columns["T012_CHALLENID"].Visible = false;
            dgDisplayData.Columns["T001_ACCOUNTID"].Visible = false;
            dgDisplayData.Columns["T012_DETAILS"].Visible = false;
            dgDisplayData.Columns["T012_TOTAL_AMT"].Visible = false;
            dgDisplayData.Columns["T012_DISCOUNT"].Visible = false;
            dgDisplayData.Columns["T012_TAX"].Visible = false;
            dgDisplayData.Columns["T001_CONTACT_PERSON"].Visible = false;
            dgDisplayData.Columns["T001_MOBILE1"].Visible = false;



            dgDisplayData.Columns["T001_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T012_TRANSPORT1"].HeaderText = "Transport";
            dgDisplayData.Columns["T012_LR1"].HeaderText = "L.R.No";
            dgDisplayData.Columns["T012_TRANSPORT2"].HeaderText = "Parcel";

            dgDisplayData.Columns["T012_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T012_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["T012_NO"].HeaderText = "Invoice No";
            dgDisplayData.Columns["T012_NETAMOUNT"].HeaderText = "Net Amount";

            dgDisplayData.Columns["T001_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T012_DATE"].DisplayIndex = 1;
            dgDisplayData.Columns["T012_NO"].DisplayIndex = 2;
            dgDisplayData.Columns["T012_NETAMOUNT"].DisplayIndex = 3;
            dgDisplayData.Columns["T012_TRANSPORT1"].DisplayIndex = 4;
            dgDisplayData.Columns["T012_LR1"].DisplayIndex = 5;
            dgDisplayData.Columns["T012_TRANSPORT2"].DisplayIndex = 6;


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

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.ClearSelection();
        }

        void EditData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int ID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["T012_CHALLENID"].Value.ToString(), out ID);

                Datalayer.iT012_CHALLENID = ID;

                frmChallen frmAddSTD = new frmChallen();
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T012_CHALLENID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_DELETE_CHALLENITEM(ID, ref Iout);
                        MyDa.USP_VP_DELETE_CHALLEN(ID, ref Iout);

                        if (Iout > 0)
                        {
                            Datalayer.DeleteMessageBox("Challen Details");
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Challen Details");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopualteData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cmbName.Items.Count > 0)
                cmbName.SelectedIndex = -1;

            PopualteData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintData();
        }

        void PrintData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int ID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["T012_CHALLENID"].Value.ToString(), out ID);

                Datalayer.iPrintableChallenId = ID;

                DAL dl = new DAL();
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("exec USP_VP_PRINT_CHALLEN_BILL '" + Datalayer.iPrintableChallenId + "'");
                if (dt.Rows.Count > 0)
                {

                    dt.Rows[0]["BILLTYPE"] = "Orignal";

                    ReportDocument RptDoc = new ReportDocument();
                    RptDoc.Load(Application.StartupPath + @"\Report\rptChallen.rpt");
                    RptDoc.SetDataSource(dt);

                    Datalayer.RptReport = RptDoc;
                    Datalayer.sReportName = "Challen Bill";

                    Report.frmReportViwer fmReport = new Report.frmReportViwer();
                    fmReport.Show();
                }
                else
                {
                    Datalayer.InformationMessageBox("No Record..");
                }

                //frmPrintBill fmPrnt = new frmPrintBill();
                //fmPrnt.Show();
            }
        }

        private void btnPackingSlip_Click(object sender, EventArgs e)
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int ID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["T012_CHALLENID"].Value.ToString(), out ID);

                DAL dl = new DAL();
                DataTable dt = new DataTable();
                dt = dl.SelectMethod("exec USP_VP_PRINT_CHALLEN_BILL '" + ID + "'");
                if (dt.Rows.Count > 0)
                {

                    ReportDocument RptDoc = new ReportDocument();
                    RptDoc.Load(Application.StartupPath + @"\Report\rptPackingSlipsOnInvoice.rpt");
                    RptDoc.SetDataSource(dt);

                    Datalayer.RptReport = RptDoc;
                    Datalayer.sReportName = "Packing Slip";

                    Report.frmReportViwer fmReport = new Report.frmReportViwer();
                    fmReport.Show();
                }
                else
                {
                    Datalayer.InformationMessageBox("No Record..");
                }
            }
        }

        private void dgDisplayData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditData();
        }

    }
}
