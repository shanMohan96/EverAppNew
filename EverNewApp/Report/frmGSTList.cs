using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;


namespace EverNewApp
{
    public partial class frmGSTList : Form
    {
        MyDabaseDataContext MyDa;
        DatabaseOperation dbo = new DatabaseOperation();

        public frmGSTList()
        {
            InitializeComponent();
        }

        private void frmManagePurchase_Load(object sender, EventArgs e)
        {
            Datalayer.SetButtion(btnExportToJSON);
            Datalayer.SetButtion(btnExportToCSV);
            Datalayer.SetButtion(btnExportExcel);
            Datalayer.SetButtion(btnClear);
            Datalayer.SetSoftwareThems(pnlHeader, pnlFooter);

            if (Datalayer.sGSTReportName == "B2B")
                lblTitle.Text = "B2B GST REPORT";
            else if (Datalayer.sGSTReportName == "B2CL")
                lblTitle.Text = "B2CL GST REPORT";
            else if (Datalayer.sGSTReportName == "B2CS")
                lblTitle.Text = "B2CS GST REPORT";
            else if (Datalayer.sGSTReportName == "HSN")
                lblTitle.Text = "HSN GST REPORT";
            else if (Datalayer.sGSTReportName == "B2B2")
                lblTitle.Text = "B2B GST REPORT";
            else if (Datalayer.sGSTReportName == "HSNSUM")
                lblTitle.Text = "HSN SUM GST REPORT";
            else if (Datalayer.sGSTReportName == "DOC")
                lblTitle.Text = "DOC GST REPORT";
        }

        private void frmManagePurchase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdatePurchase fmAdd = new frmAddUpdatePurchase();
            fmAdd.MdiParent = this.MdiParent;
            fmAdd.Show();

            this.Close();
        }

        void PopualteData()
        {
            string TM02_PARTYID = "";
            int iTM02_PARTYID = 0;


            if (iTM02_PARTYID > 0)
                TM02_PARTYID = iTM02_PARTYID.ToString();




            string sql = null;
            string data = null;
            int i = 0;
            int j = 0;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;

            Excel.Worksheet xlWorkSheet;
            Excel.Worksheet xlWorkSheet2;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();


            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = "b2b";

            DAL dl = new DAL();
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("exec USP_VP_GET_GST_REPORT 'B2B','" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dtData.Rows.Count > 0)
            {
                try
                {
                    xlWorkSheet.Cells[1, 1] = "No. of Recipients";
                    xlWorkSheet.Cells[1, 2] = "";
                    xlWorkSheet.Cells[1, 3] = "No. of Invoices";
                    xlWorkSheet.Cells[1, 4] = "";
                    xlWorkSheet.Cells[1, 5] = "Total Invoice Value";
                    xlWorkSheet.Cells[1, 6] = "";
                    xlWorkSheet.Cells[1, 7] = "";
                    xlWorkSheet.Cells[1, 8] = "";
                    xlWorkSheet.Cells[1, 9] = "";
                    xlWorkSheet.Cells[1, 10] = "";
                    xlWorkSheet.Cells[1, 11] = "Total Taxable Value";
                    xlWorkSheet.Cells[1, 12] = "Total Cess";

                    xlWorkSheet.Cells[2, 1] = "1";
                    xlWorkSheet.Cells[2, 2] = "";
                    xlWorkSheet.Cells[2, 3] = dtData.Rows.Count.ToString();
                    xlWorkSheet.Cells[2, 4] = "";
                    xlWorkSheet.Cells[2, 5] = Convert.ToDecimal(dtData.Compute("SUM(T007_NETAMOUNT)", string.Empty));
                    xlWorkSheet.Cells[2, 6] = "";
                    xlWorkSheet.Cells[2, 7] = "";
                    xlWorkSheet.Cells[2, 8] = "";
                    xlWorkSheet.Cells[2, 9] = "";
                    xlWorkSheet.Cells[2, 10] = "";
                    xlWorkSheet.Cells[2, 11] = Convert.ToDecimal(dtData.Compute("SUM(T007_TOTAL_AMT)", string.Empty));
                    xlWorkSheet.Cells[2, 12] = "0";

                    //add title
                    for (j = 0; j <= dtData.Columns.Count - 1; j++)
                    {
                        data = dtData.Columns[j].ColumnName.ToString();
                        string sColumnName = "";
                        if (data == "T001_GSTTIN")
                            sColumnName = "GSTIN/UIN of Recipient";
                        else if (data == "T001_NAME")
                            sColumnName = "Receiver Name";
                        else if (data == "T007_NO")
                            sColumnName = "Invoice Number";
                        else if (data == "T007_DATE")
                            sColumnName = "Invoice date";
                        else if (data == "T007_NETAMOUNT")
                            sColumnName = "Invoice Value";
                        else if (data == "PlaceOfSupply")
                            sColumnName = "Place Of Supply";
                        else if (data == "T007_IS_REVERSE_CHARGES")
                            sColumnName = "Reverse Charge";
                        else if (data == "Invoice_Type")
                            sColumnName = "Invoice Type";
                        else if (data == "E_Commerce_GSTIN")
                            sColumnName = "E-Commerce GSTIN";
                        else if (data == "Rate")
                            sColumnName = "Rate";
                        else if (data == "T007_TOTAL_AMT")
                            sColumnName = "Taxable Value";
                        else if (data == "Cess_Amount")
                            sColumnName = "Cess Amount";

                        xlWorkSheet.Cells[3, j + 1] = sColumnName;
                    }

                    for (i = 0; i <= dtData.Rows.Count - 1; i++)
                    {
                        for (j = 0; j <= dtData.Columns.Count - 1; j++)
                        {
                            data = dtData.Rows[i].ItemArray[j].ToString();
                            Microsoft.Office.Interop.Excel.Range rng = xlWorkSheet.Cells[i + 4, j + 1] as Microsoft.Office.Interop.Excel.Range;
                            System.Text.RegularExpressions.Regex Emailexpr = new System.Text.RegularExpressions.Regex(@"^0");
                            if (Emailexpr.IsMatch(data))
                                data = "'" + data;
                            rng.Value2 = data;

                            try
                            {
                                string sType = dtData.Columns[j].DataType.ToString();
                                if (sType.Contains("DateTime"))
                                    rng.NumberFormat = "dd-MMM-yyyy";
                                else if (sType.Contains("String"))
                                    rng.NumberFormat = "@";
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            Excel.Worksheet oSheet2 = xlApp.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            oSheet2.Name = "BSCL";

            DataTable dtData_bscl = new DataTable();
            dtData_bscl = dl.SelectMethod("exec USP_VP_GET_GST_REPORT 'B2B','" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dtData_bscl.Rows.Count > 0)
            {
                dtData_bscl.Columns.Remove("T001_GSTTIN");
                dtData_bscl.Columns.Remove("T001_NAME");
                dtData_bscl.Columns.Remove("T007_IS_REVERSE_CHARGES");
                dtData_bscl.Columns.Remove("Invoice_Type");
                dtData_bscl.Columns.Remove("Cess_Amount");

                oSheet2.Cells[1, 1] = "Summary For B2CL(5)";
                oSheet2.Cells[1, 2] = "";
                oSheet2.Cells[1, 3] = "";
                oSheet2.Cells[1, 4] = "";
                oSheet2.Cells[1, 5] = "";
                oSheet2.Cells[1, 6] = "";
                oSheet2.Cells[1, 7] = "";
                oSheet2.Cells[1, 8] = "";

                oSheet2.Cells[2, 1] = "No. of Invoices";
                oSheet2.Cells[2, 2] = "";
                oSheet2.Cells[2, 3] = "Total Inv Value";
                oSheet2.Cells[2, 4] = "";
                oSheet2.Cells[2, 5] = "";
                oSheet2.Cells[2, 6] = "Total Taxable Value";
                oSheet2.Cells[2, 7] = "Total Cess";
                oSheet2.Cells[2, 8] = "";

                oSheet2.Cells[3, 1] = dtData_bscl.Rows.Count.ToString();
                oSheet2.Cells[3, 2] = "";
                oSheet2.Cells[3, 3] = Convert.ToDecimal(dtData.Compute("SUM(T007_NETAMOUNT)", string.Empty));
                oSheet2.Cells[3, 4] = "";
                oSheet2.Cells[3, 5] = "";
                oSheet2.Cells[3, 6] = Convert.ToDecimal(dtData.Compute("SUM(T007_TOTAL_AMT)", string.Empty));
                oSheet2.Cells[3, 7] = "";
                oSheet2.Cells[3, 8] = "";

                for (j = 0; j <= dtData_bscl.Columns.Count - 1; j++)
                {
                    data = dtData_bscl.Columns[j].ColumnName.ToString();
                    string sColumnName = "";
                    if (data == "T007_NO")
                        sColumnName = "Invoice Number";
                    else if (data == "T007_DATE")
                        sColumnName = "Invoice date";
                    else if (data == "T007_NETAMOUNT")
                        sColumnName = "Invoice Value";
                    else if (data == "PlaceOfSupply")
                        sColumnName = "Place Of Supply";
                    else if (data == "Rate")
                        sColumnName = "Rate";
                    else if (data == "T007_TOTAL_AMT")
                        sColumnName = "Taxable Value";
                    else if (data == "Cess_Amount")
                        sColumnName = "Cess Amount";
                    else if (data == "E_Commerce_GSTIN")
                        sColumnName = "E-Commerce GSTIN";


                    oSheet2.Cells[4, j + 1] = sColumnName;
                }

                for (i = 0; i <= dtData_bscl.Rows.Count - 1; i++)
                {
                    for (j = 0; j <= dtData_bscl.Columns.Count - 1; j++)
                    {
                        data = dtData_bscl.Rows[i].ItemArray[j].ToString();
                        Microsoft.Office.Interop.Excel.Range rng = oSheet2.Cells[i + 5, j + 1] as Microsoft.Office.Interop.Excel.Range;
                        System.Text.RegularExpressions.Regex Emailexpr = new System.Text.RegularExpressions.Regex(@"^0");
                        if (Emailexpr.IsMatch(data))
                            data = "'" + data;
                        rng.Value2 = data;

                        try
                        {
                            string sType = dtData_bscl.Columns[j].DataType.ToString();
                            if (sType.Contains("DateTime"))
                                rng.NumberFormat = "dd-MMM-yyyy";
                            else if (sType.Contains("String"))
                                rng.NumberFormat = "@";
                        }
                        catch
                        {
                        }
                    }
                }
            }

            Excel.Worksheet oSheet3 = xlApp.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            oSheet3.Name = "B2CS";

            DataTable dtData_b2cs = new DataTable();
            dtData_b2cs = dl.SelectMethod("exec USP_VP_GET_GST_REPORT 'B2CS','" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
            if (dtData_b2cs.Rows.Count > 0)
            {
                oSheet3.Cells[1, 1] = "Summary For B2CS(7)";
                oSheet3.Cells[1, 2] = "";
                oSheet3.Cells[1, 3] = "";
                oSheet3.Cells[1, 4] = "";
                oSheet3.Cells[1, 5] = "";
                oSheet3.Cells[1, 6] = "";

                oSheet3.Cells[2, 1] = "";
                oSheet3.Cells[2, 2] = "";
                oSheet3.Cells[2, 3] = "";
                oSheet3.Cells[2, 4] = "Total Taxable  Value";
                oSheet3.Cells[2, 5] = "Total Cess";
                oSheet3.Cells[2, 6] = "";

                oSheet3.Cells[3, 1] = "";
                oSheet3.Cells[3, 2] = "";
                oSheet3.Cells[3, 3] = "";
                oSheet3.Cells[3, 4] = Convert.ToDecimal(dtData.Compute("SUM(T007_TOTAL_AMT)", string.Empty));
                oSheet3.Cells[3, 5] = "";
                oSheet3.Cells[3, 6] = "";

                for (j = 0; j <= dtData_b2cs.Columns.Count - 1; j++)
                {
                    data = dtData_b2cs.Columns[j].ColumnName.ToString();
                    string sColumnName = "";
                    if (data == "PlaceOfSupply")
                        sColumnName = "Place Of Supply";
                    else if (data == "Invoice_Type")
                        sColumnName = "Type";
                    else if (data == "E_Commerce_GSTIN")
                        sColumnName = "E-Commerce GSTIN";
                    else if (data == "Rate")
                        sColumnName = "Rate";
                    else if (data == "T007_TOTAL_AMT")
                        sColumnName = "Taxable Value";
                    else if (data == "Cess_Amount")
                        sColumnName = "Cess Amount";

                    oSheet3.Cells[4, j + 1] = sColumnName;
                }

                for (i = 0; i <= dtData_b2cs.Rows.Count - 1; i++)
                {
                    for (j = 0; j <= dtData_b2cs.Columns.Count - 1; j++)
                    {
                        data = dtData_bscl.Rows[i].ItemArray[j].ToString();
                        Microsoft.Office.Interop.Excel.Range rng = oSheet3.Cells[i + 5, j + 1] as Microsoft.Office.Interop.Excel.Range;
                        System.Text.RegularExpressions.Regex Emailexpr = new System.Text.RegularExpressions.Regex(@"^0");
                        if (Emailexpr.IsMatch(data))
                            data = "'" + data;
                        rng.Value2 = data;

                        try
                        {
                            string sType = dtData_bscl.Columns[j].DataType.ToString();
                            if (sType.Contains("DateTime"))
                                rng.NumberFormat = "dd-MMM-yyyy";
                            else if (sType.Contains("String"))
                                rng.NumberFormat = "@";
                        }
                        catch
                        {
                        }
                    }
                }
            }



            string sPath = Datalayer.GenerateFilePath + "demo.xls";
            if (File.Exists(sPath))
                File.Delete(sPath);
            xlWorkBook.SaveAs(sPath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            string sMSg = "Excel File Generated \n" + sPath + "\n view now ?";
            DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(sPath);
            }


        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ExportToExcel();
            Cursor.Current = Cursors.Default;
        }

        void ExportToExcel()
        {
            if (Datalayer.sGSTReportName == "B2B")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2B_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                string sFileName = "b2b.xls";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.ExportToExce(dtData, sFileName, "b2b");
                if (_Response.Result)
                {
                    string sMSg = "Excel File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }

            }
            else if (Datalayer.sGSTReportName == "B2CL")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2CL_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "B2CL.xls";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.ExportToExce(dtData, sFileName, "B2CL");
                if (_Response.Result)
                {
                    string sMSg = "Excel File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }

            else if (Datalayer.sGSTReportName == "B2CS")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2CS_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "B2CS.xls";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.ExportToExce(dtData, sFileName, "B2CS");
                if (_Response.Result)
                {
                    string sMSg = "Excel File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "HSN")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_HSN_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "HSN.xls";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.ExportToExce(dtData, sFileName, "HSN");
                if (_Response.Result)
                {
                    string sMSg = "Excel File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "B2B2")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2B_GSTR2_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "b2b2.xls";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.ExportToExce(dtData, sFileName, "b2b2");
                if (_Response.Result)
                {
                    string sMSg = "Excel File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "HSNSUM")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_HSN_GSTR2_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "HSNSUM.xls";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.ExportToExce(dtData, sFileName, "HSNSUM");
                if (_Response.Result)
                {
                    string sMSg = "Excel File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }

            else if (Datalayer.sGSTReportName == "DOC")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_DOC '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "DOC.xls";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.ExportToExce(dtData, sFileName, "DOC");
                if (_Response.Result)
                {
                    string sMSg = "Excel File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
            }
            //}
        }

        void ExportToCSV()
        {
            if (Datalayer.sGSTReportName == "B2B")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2B_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "b2b.csv";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToCSV(dtData, ',', sFileName);
                if (_Response.Result)
                {
                    string sMSg = "CSV File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "B2CL")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2CL_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "B2CL.csv";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToCSV(dtData, ',', sFileName);
                if (_Response.Result)
                {
                    string sMSg = "CSV File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "B2CS")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2CS_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "B2CS.csv";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToCSV(dtData, ',', sFileName);
                if (_Response.Result)
                {
                    string sMSg = "CSV File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "HSN")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_HSN_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "HSN.csv";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToCSV(dtData, ',', sFileName);
                if (_Response.Result)
                {
                    string sMSg = "CSV File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "B2B2")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2B_GSTR2_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "B2B2.csv";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToCSV(dtData, ',', sFileName);
                if (_Response.Result)
                {
                    string sMSg = "CSV File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            if (Datalayer.sGSTReportName == "HSNSUM")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_HSN_GSTR2_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "HSNSUM.csv";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToCSV(dtData, ',', sFileName);
                if (_Response.Result)
                {
                    string sMSg = "CSV File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            if (Datalayer.sGSTReportName == "DOC")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_DOC '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "DOC.csv";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToCSV(dtData, ',', sFileName);
                if (_Response.Result)
                {
                    string sMSg = "CSV File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
            }
            //}
        }

        void ExportToJSON()
        {
            if (Datalayer.sGSTReportName == "B2B")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2B_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "b2b.json";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToJSONWithStringBuilder(dtData, sFileName);
                if (_Response.Result)
                {
                    string sMSg = "JSON File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "B2CL")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2CL_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "B2CL.json";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToJSONWithStringBuilder(dtData, sFileName);
                if (_Response.Result)
                {
                    string sMSg = "JSON File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "B2CS")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2CS_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "B2CS.json";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToJSONWithStringBuilder(dtData, sFileName);
                if (_Response.Result)
                {
                    string sMSg = "JSON File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "HSN")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_HSN_GSTR1_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "HSN.json";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToJSONWithStringBuilder(dtData, sFileName);
                if (_Response.Result)
                {
                    string sMSg = "JSON File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            else if (Datalayer.sGSTReportName == "B2B2")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_B2B_GSTR2_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "b2b2.json";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToJSONWithStringBuilder(dtData, sFileName);
                if (_Response.Result)
                {
                    string sMSg = "JSON File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            if (Datalayer.sGSTReportName == "HSNSUM")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_HSN_GSTR2_REPORT '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "HSNSUM.json";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToJSONWithStringBuilder(dtData, sFileName);
                if (_Response.Result)
                {
                    string sMSg = "JSON File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
                //}
            }
            if (Datalayer.sGSTReportName == "DOC")
            {
                DAL dl = new DAL();
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("exec USP_VP_GET_DOC '" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "','" + dtpTodate.Value.ToString("yyyy-MM-dd") + "','" + Datalayer.iT001_COMPANYID + "' ");
                //if (dtData.Rows.Count > 0)
                //{
                string sFileName = "DOC.json";
                string sFullPath = Datalayer.GenerateFilePath + sFileName;

                if (File.Exists(sFullPath))
                    File.Delete(sFullPath);

                Response _Response = new Response();
                Datalayer _Datalayer = new Datalayer();
                _Response = _Datalayer.DataTableToJSONWithStringBuilder(dtData, sFileName);
                if (_Response.Result)
                {
                    string sMSg = "JSON File Generated \n" + sFullPath + "\n view now ?";
                    DialogResult dr = MessageBox.Show(sMSg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sFullPath);
                    }
                }
                else
                {
                    Datalayer.WorningMessageBox(_Response.ErrorLog.ToString(), "Error");
                }
            }
            //}
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ExportToCSV();
            Cursor.Current = Cursors.Default;
        }

        private void btnExportToJSON_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ExportToJSON();
            Cursor.Current = Cursors.Default;
        }

    }
}
