using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;

namespace EverNewApp
{
    public partial class MDIMaster : Form
    {
        private int childFormNumber = 0;

        public MDIMaster()
        {
            InitializeComponent();

            if (File.Exists(Application.StartupPath + "\\BG.jpg"))
                this.BackgroundImage = new Bitmap(Application.StartupPath + "\\BG.jpg");
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MDIMaster_Load(object sender, EventArgs e)
        {
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            Datalayer.MakeConnections();
            PopualteData();

            //frmDashBoard fmDashboard = new frmDashBoard();
            //fmDashboard.MdiParent = this;
            //fmDashboard.Show();
        }

        DAL dl = new DAL();
        void PopualteData()
        {
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("SELECT * FROM TM_COMPANY WHERE TM_COMPAYID= " + Datalayer.iT001_COMPANYID);
            if (dt.Rows.Count > 0)
            {
                lblName.Text = dt.Rows[0]["TM_NAME"].ToString();
                this.Text = dt.Rows[0]["TM_NAME"].ToString();
                lblAddress.Text = dt.Rows[0]["TM_ADDRESS1"].ToString();
                Datalayer.iT001_COMPANYID = int.Parse(dt.Rows[0]["TM_COMPAYID"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["TM_IMAGE"])))
                {
                    MyDabaseDataContext MyDa;
                    MyDa = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                    var r = (from a in MyDa.TM_COMPANies where a.TM_COMPAYID == Datalayer.iT001_COMPANYID select a).First();
                    MemoryStream ms = new MemoryStream(r.TM_IMAGE.ToArray());
                    ms.Seek(0, SeekOrigin.Begin);
                    pictureBox2.Image = Image.FromStream(ms);
                }

            }
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MDIMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MDIMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void databaseBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            Datalayer.DatabaseBackup(fd);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
        }

        private void itemDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();
            //frmManageItem fmItm = new frmManageItem();
            //fmItm.MdiParent = this;
            //fmItm.Show();
        }

        private void itemPriceSIzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageProductSize fmItm = new frmManageProductSize();
            fmItm.MdiParent = this;
            fmItm.Show();
        }

        private void iTEMSETITEMDETAILSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageProductItem fmItm = new frmManageProductItem();
            fmItm.MdiParent = this;
            fmItm.Show();
        }

        private void accountDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdateAccount fmItm = new frmAddUpdateAccount();
            fmItm.MdiParent = this;
            fmItm.Show();
        }

        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmUser fmUser = new frmUser();
            fmUser.MdiParent = this;
            fmUser.Show();
        }

        void CloseAll()
        {
            //foreach (Form childForm in MdiChildren)
            //{
            //    childForm.Close();
            //}
        }

        private void managePurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManagePurchase fmUser = new frmManagePurchase();
            fmUser.MdiParent = this;
            fmUser.Show();

        }

        private void manageOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmOrder fmOrder = new frmOrder();
            fmOrder.MdiParent = this;
            fmOrder.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmDashBoard fmDash = new frmDashBoard();
            fmDash.MdiParent = this;
            fmDash.Show();
        }

        private void manageStockInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageStockIn fmDash = new frmManageStockIn();
            fmDash.MdiParent = this;
            fmDash.Show();
        }

        private void manageSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageSale fmSale = new frmManageSale();
            fmSale.MdiParent = this;
            fmSale.Show();
        }

        private void stockDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmStock fmstk = new frmStock();
            fmstk.MdiParent = this;
            fmstk.Show();
        }

        private void manageBankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageBank fmBank = new frmManageBank();
            fmBank.MdiParent = this;
            fmBank.Show();
        }

        private void managePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManagePurchasePayment fmPayment = new frmManagePurchasePayment();
            fmPayment.MdiParent = this;
            fmPayment.Show();
        }

        private void managePackingSlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            //frmManagePackingSlip fmPack = new frmManagePackingSlip();
            //fmPack.MdiParent = this;
            //fmPack.Show();
        }

        private void productListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DAL dl = new DAL();
            DataTable dt = new DataTable();
            dt = dl.SelectMethod("exec USP_VP_RPT_GETPRODUCT '" + Datalayer.iT001_COMPANYID.ToString() + "' ");
            if (dt.Rows.Count > 0)
            {
                ReportDocument RptDoc = new ReportDocument();

                RptDoc.Load(Application.StartupPath + @"\Report\rptProduct.rpt");
                RptDoc.SetDataSource(dt);

                Datalayer.RptReport = RptDoc;
                Datalayer.sReportName = "Product Listing Report";

                Report.frmReportViwer fmReport = new Report.frmReportViwer();
                fmReport.Show();
            }
            else
            {
                Datalayer.InformationMessageBox("No Record..");
            }
        }

        private void accountListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAccount fmAccount = new frmAccount();
            fmAccount.MdiParent = this;
            fmAccount.Show();
        }

        private void purchaseListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmPurchaseList fmPurchaselst = new frmPurchaseList();
            fmPurchaselst.MdiParent = this;
            fmPurchaselst.Show();

        }

        private void saleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmSaleList fmPurchaselst = new frmSaleList();
            fmPurchaselst.MdiParent = this;
            fmPurchaselst.Show();
        }

        private void productStockListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmStockReporting fmstk = new frmStockReporting();
            fmstk.MdiParent = this;
            fmstk.Show();
        }

        private void customerBalanceSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            Report.frmCustomerBalance fmCustomerBal = new Report.frmCustomerBalance();
            fmCustomerBal.MdiParent = this;
            fmCustomerBal.Show();
        }

        private void partyBalanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            Report.frmPartyBalance fmParty = new Report.frmPartyBalance();
            fmParty.MdiParent = this;
            fmParty.Show();
        }

        private void companyDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageCompany fmAddCmp = new frmManageCompany();
            fmAddCmp.MdiParent = this;
            fmAddCmp.Show();
        }

        private void paymentListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmPaymentList fmPayment = new frmPaymentList();
            fmPayment.MdiParent = this;
            fmPayment.Show();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddFullProduct fmProduct = new frmAddFullProduct();
            fmProduct.MdiParent = this;
            fmProduct.Show();
        }

        private void manageChallenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();



            frmManageChallen fmSale = new frmManageChallen();
            fmSale.MdiParent = this;
            fmSale.Show();
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddFullProduct fmAddProduct = new frmAddFullProduct();
            fmAddProduct.MdiParent = this;
            fmAddProduct.Show();
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdateAccount fmAccount = new frmAddUpdateAccount();
            fmAccount.MdiParent = this;
            fmAccount.Show();
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdatePurchase fmPurchase = new frmAddUpdatePurchase();
            fmPurchase.MdiParent = this;
            fmPurchase.Show();
        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            CloseAll();
            frmStockIn fmstkin = new frmStockIn();
            fmstkin.MdiParent = this;
            fmstkin.Show();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdateSale fmSale = new frmAddUpdateSale();
            fmSale.MdiParent = this;
            fmSale.Show();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            CloseAll();
            Datalayer.sPaymentMode = "R";

            frmAddUpdatePurchasePayment fmPayment = new frmAddUpdatePurchasePayment();
            fmPayment.MdiParent = this;
            fmPayment.Show();
        }

        private void btnAboutUS_Click(object sender, EventArgs e)
        {
            CloseAll();

            FolderBrowserDialog fm = new FolderBrowserDialog();
            Datalayer.DatabaseBackup(fm);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CloseAll();

            this.Close();
        }

        private void orderListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmOrderList fmOrderlst = new frmOrderList();
            fmOrderlst.MdiParent = this;
            fmOrderlst.Show();
        }

        private void challenListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmChallenList fmChallen = new frmChallenList();
            fmChallen.MdiParent = this;
            fmChallen.Show();
        }

        private void manageBillReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();
            Datalayer.sPaymentMode = "R";

            frmManagePurchasePayment fmPayment = new frmManagePurchasePayment();
            fmPayment.MdiParent = this;
            fmPayment.Show();
        }

        private void managePaymentReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();
            Datalayer.sPaymentMode = "P";

            frmManagePurchasePayment fmPayment = new frmManagePurchasePayment();
            fmPayment.MdiParent = this;
            fmPayment.Show();
        }

        private void manageExpenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageExpenseMaster fmExp = new frmManageExpenseMaster();
            fmExp.MdiParent = this;
            fmExp.Show();
        }

        private void manageExpenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageExpenseTransaction fmExp = new frmManageExpenseTransaction();
            fmExp.MdiParent = this;
            fmExp.Show();
        }

        private void accountStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAccountStatement fmstatment = new frmAccountStatement();
            fmstatment.MdiParent = this;
            fmstatment.Show();
        }

        private void pendingOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmPendingOrder fmPending = new frmPendingOrder();
            fmPending.MdiParent = this;
            fmPending.Show();
        }

        private void partyWiseQtyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPartyWiseQty fmqty = new frmPartyWiseQty();
            fmqty.MdiParent = this;
            fmqty.Show();
        }

        private void allItemWiseQtyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAllItemWiseQty fmAll = new frmAllItemWiseQty();
            fmAll.MdiParent = this;
            fmAll.Show();
        }

        private void imgLogo_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void workarDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmAddUpdateworkar fmWorkar = new frmAddUpdateworkar();
            fmWorkar.MdiParent = this;
            fmWorkar.Show();
        }

        private void manageWorkarLoanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageLoan fmLoan = new frmManageLoan();
            fmLoan.MdiParent = this;
            fmLoan.Show();
        }

        private void manageSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageSalary fmSalary = new frmManageSalary();
            fmSalary.MdiParent = this;
            fmSalary.Show();
        }

        private void manageLoanPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmManageLoanPayment fmSalary = new frmManageLoanPayment();
            fmSalary.MdiParent = this;
            fmSalary.Show();
        }

        private void salaryListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmSalaryList fmSalary = new frmSalaryList();
            fmSalary.MdiParent = this;
            fmSalary.Show();
        }

        private void gSTReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void b2BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            Datalayer.sGSTReportName = "B2B";
            frmGSTList fmGST = new frmGSTList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void b2CLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            Datalayer.sGSTReportName = "B2CL";
            frmGSTList fmGST = new frmGSTList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void b2CSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            Datalayer.sGSTReportName = "B2CS";
            frmGSTList fmGST = new frmGSTList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void hSNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            Datalayer.sGSTReportName = "HSN";
            frmGSTList fmGST = new frmGSTList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void b2BToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CloseAll();

            Datalayer.sGSTReportName = "B2B2";
            frmGSTList fmGST = new frmGSTList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void hSNToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CloseAll();

            Datalayer.sGSTReportName = "HSNSUM";
            frmGSTList fmGST = new frmGSTList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void dOCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            Datalayer.sGSTReportName = "DOC";
            frmGSTList fmGST = new frmGSTList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void stockInListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmStockInList fmGST = new frmStockInList();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void partyStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmPartyStatement fmGST = new frmPartyStatement();
            fmGST.MdiParent = this;
            fmGST.Show();
        }

        private void gSTSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmGSTReport fmgst = new frmGSTReport();
            fmgst.MdiParent = this;
            fmgst.Show();
        }

        private void productionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmProductionReport fmProduction = new frmProductionReport();
            fmProduction.MdiParent = this;
            fmProduction.Show();
        }

        private void saleItemListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmSaleItemList fmSaleItem = new frmSaleItemList();
            fmSaleItem.MdiParent = this;
            fmSaleItem.Show();
        }

        private void purchaseItemLIstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmPurchaseItemList fmPurchase = new frmPurchaseItemList();
            fmPurchase.MdiParent = this;
            fmPurchase.Show();
        }

        private void productionPLWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmProductionPLWiseReport fmPurchase = new frmProductionPLWiseReport();
            fmPurchase.MdiParent = this;
            fmPurchase.Show();
        }

        private void paymentReminderSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmPaymentReminder fmReminder = new frmPaymentReminder();
            fmReminder.MdiParent = this;
            fmReminder.Show();
        }

        private void sMSHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();

            frmSMSHistory fmSMS = new frmSMSHistory();
            fmSMS.MdiParent = this;
            fmSMS.Show();
        }
    }
}
