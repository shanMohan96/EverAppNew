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
    public partial class frmManageSale : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmManageSale()
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
            frmAddUpdateSale fmAdd = new frmAddUpdateSale();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

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
            List<USP_VP_GETSALELISTResult> lst = new List<USP_VP_GETSALELISTResult>();
            lst = MyDa.USP_VP_GETSALELIST(null, TM02_PARTYID, null, dtpFromDate.Value, dtpTodate.Value, Datalayer.iT001_COMPANYID.ToString()).ToList();
            dgDisplayData.DataSource = lst;
            for (int i = 0; i < lst.Count; i++)
            {
                dTotal = dTotal + lst[i].T007_NETAMOUNT.Value;
            }

            lblNetTotal.Text = dTotal.ToString();
            dgDisplayData.Columns["T007_SALEID"].Visible = false;
            dgDisplayData.Columns["T001_ACCOUNTID"].Visible = false;
            dgDisplayData.Columns["T007_DETAILS"].Visible = false;
            dgDisplayData.Columns["T007_TOTAL_AMT"].Visible = false;
            dgDisplayData.Columns["T007_DISCOUNT"].Visible = false;
            // dgDisplayData.Columns["T007_TAX"].Visible = false;
            dgDisplayData.Columns["T001_CONTACT_PERSON"].Visible = false;
            dgDisplayData.Columns["T001_MOBILE1"].Visible = false;
            dgDisplayData.Columns["T007_CGST"].Visible = false;
            dgDisplayData.Columns["T007_SGST"].Visible = false;
            dgDisplayData.Columns["T007_IGST"].Visible = false;


            dgDisplayData.Columns["T001_NAME"].HeaderText = "Name";
            dgDisplayData.Columns["T001_MOBILE1"].HeaderText = "Mobile No";
            dgDisplayData.Columns["T007_DATE"].HeaderText = "Date";
            dgDisplayData.Columns["T007_DATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgDisplayData.Columns["T007_NO"].HeaderText = "Invoice No";
            dgDisplayData.Columns["T007_NETAMOUNT"].HeaderText = "Net Amount";
            dgDisplayData.Columns["T007_TRANSPORT1"].HeaderText = "Transport";
            dgDisplayData.Columns["T007_TRANSPORT2"].HeaderText = "Parcels";
            dgDisplayData.Columns["T007_LR1"].HeaderText = "L.R.No";
            dgDisplayData.Columns["List_Output"].HeaderText = "Items";


            this.dgDisplayData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgDisplayData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(Datalayer.sGridHeaderColor1, Datalayer.sGridHeaderColor2, Datalayer.sGridHeaderColor3);
            dgDisplayData.EnableHeadersVisualStyles = false;
            this.dgDisplayData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // dgCategory.CurrentRow.DefaultCellStyle.BackColor = Color.White;
            dgDisplayData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgDisplayData.ColumnHeadersHeight = 30;

            dgDisplayData.Columns["List_Output"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgDisplayData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgDisplayData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            dgDisplayData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgDisplayData.Columns["T001_NAME"].Width = 200;
            dgDisplayData.Columns["T001_MOBILE1"].Width = 75;
            dgDisplayData.Columns["T007_DATE"].Width = 90;
            dgDisplayData.Columns["T007_NO"].Width = 75;
            dgDisplayData.Columns["T007_NETAMOUNT"].Width = 100;
            dgDisplayData.Columns["T007_TRANSPORT1"].Width = 100;
            dgDisplayData.Columns["T007_TRANSPORT2"].Width = 100;
            dgDisplayData.Columns["T007_LR1"].Width = 100;
            dgDisplayData.Columns["List_Output"].Width = 300;

        }

        private void dgDisplayData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgDisplayData.Columns["T001_NAME"].DisplayIndex = 0;
            dgDisplayData.Columns["T007_DATE"].DisplayIndex = 1;
            dgDisplayData.Columns["T007_NO"].DisplayIndex = 2;
            dgDisplayData.Columns["List_Output"].DisplayIndex = 3;
            dgDisplayData.Columns["T007_NETAMOUNT"].DisplayIndex = 4;
            dgDisplayData.Columns["T007_TRANSPORT1"].DisplayIndex = 5;
            dgDisplayData.Columns["T007_TRANSPORT2"].DisplayIndex = 6;
            dgDisplayData.Columns["T007_LR1"].DisplayIndex = 7;


            dgDisplayData.ClearSelection();
        }

        void EditData()
        {
            if (dgDisplayData.SelectedRows.Count > 0)
            {
                int ID = 0;
                int.TryParse(dgDisplayData.CurrentRow.Cells["T007_SALEID"].Value.ToString(), out ID);

                Datalayer.iT007_SALEID = ID;

                frmAddUpdateSale frmAddSTD = new frmAddUpdateSale();
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
                        int.TryParse(dgDisplayData.CurrentRow.Cells["T007_SALEID"].Value.ToString(), out ID);

                        int? Iout = 0;
                        MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                        MyDa.USP_VP_DELETE_SALEITEM(ID, ref Iout);
                        MyDa.USP_VP_DELETE_SALE(ID, ref Iout);

                        if (Iout > 0)
                        {
                            Datalayer.DeleteMessageBox("Sale Details");
                            PopualteData();
                        }
                        else
                            Datalayer.DosenotDeleteMessageBox("Sale Details");
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
            int ID = 0;
            int.TryParse(dgDisplayData.CurrentRow.Cells["T007_SALEID"].Value.ToString(), out ID);

            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_PRINT_SALE_BILL '" + ID + "'");
            if (dt.Rows.Count > 0)
            {

                //DataTable dtQTY = new DataTable();
                //dtQTY = dl.SelectMethod("SELECT TM01_HSNCODE,SUM(T008_QTY)AS QTY FROM T008_SALEITEM T008 INNER JOIN TM01_PRODUCT TM01 ON TM01.TM01_PRODUCTID = T008.TM01_PRODUCTID WHERE T007_SALEID=" + Datalayer.iPrintableBillId + " GROUP BY  TM01.TM01_HSNCODE ");
                //if (dtQTY.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        if (dtQTY.Rows.Count == 1)
                //        {
                //            dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                //            dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();
                //        }
                //        if (dtQTY.Rows.Count == 2)
                //        {
                //            dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                //            dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();

                //            dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[1][0].ToString();
                //            dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[1][1].ToString();
                //        }

                //        if (dtQTY.Rows.Count == 3)
                //        {
                //            dt.Rows[i]["HSNCODE1"] = dtQTY.Rows[0][0].ToString();
                //            dt.Rows[i]["HSNCODEQTY1"] = dtQTY.Rows[0][1].ToString();

                //            dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[1][0].ToString();
                //            dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[1][1].ToString();

                //            dt.Rows[i]["HSNCODE2"] = dtQTY.Rows[2][0].ToString();
                //            dt.Rows[i]["HSNCODEQTY2"] = dtQTY.Rows[2][1].ToString();
                //        }
                //    }
                //}



                ReportDocument RptDoc = new ReportDocument();
                RptDoc.Load(Application.StartupPath + @"\Report\rptInvoice.rpt");

                //RptDoc.Load(Application.StartupPath + @"\Report\rptInvoice.rpt");
                RptDoc.SetDataSource(dt);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Invoice Bill";

                Report.frmReportViwer fmReport = new Report.frmReportViwer();
                fmReport.Show();
            }
            else
            {
                Datalayer.InformationMessageBox("No Record..");
            }

        }

    }
}
