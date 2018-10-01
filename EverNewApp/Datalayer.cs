using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using EverNewApp;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace EverNewApp
{
    public class Datalayer
    {
        //GLOBAL DATA DECLARATION

        // CHEKING ENG AND GUJ

        public static string sMessageInserted = " inserted successfully.";
        public static string sMessageNotInserted = " dose not inserted successfully, please try again.";
        public static string sMessageUpdated = " updated successfully.";
        public static string sMessageNotUpdated = " dose not updated successfully, please try again.";
        public static string sMessageDeleted = " deleted successfully.";
        public static string sMessageNotDeleted = " deleted successfully, please try again.";
        public static string sMessageSaved = " saved successfully.";
        public static string sMessageBackupApp = " Are you sure? Do you want to Save Database Backup ? ";
        public static string sMessageNotUser = " You have not permision to access this function please contact to admin ";

        public static string sCompanyName = "hp infosys - Rajkot";

        public static string sMessageDuplicate = " already exist with same name.";

        public static string sMessageConfirmation = " Are you sure? Do you want to Delete this Record? ";
        public static string sCaptionConfirmation = " Confirmation ";

        public static string sMessageExitApp = " Are you sure? Do you want to Exit Application? ";
        public static string sCaptionExitApp = " Exit Application ";

        public static string sMessageReturnItem = " Are you sure? Do you want to Return This Iteam? ";
        public static string sCaptionReturnItem = " Return Iteam ";

        public static string sMessageForainKey = "Dose not remove this record becouse this record is exist in Another.";
        public static string sMeessgeSelection = "Record selection is required to Edit/Delete data.";


        public static string sMessageRestoreConformation = "Are you sure? Do you want to Restore this Database ?";
        public static string sMeessgeNotDeleted = "You Have Not Access This Operation..";
        public static string sUserRole = "Admin";

        public static string sSaveToltipMsg = "Control+S";
        public static string sPrintToltipMsg = "Control+P";
        public static string sRestToltipMsg = "Control+N";
        public static string sExitToltipMsg = "Control+X";
        public static string sAddToltipMsg = "Control+N";
        public static string sEditToltipMsg = "Control+E";
        public static string sDeleteToltipMsg = "Control+D";


        public static int iT001_COMPANYID = 0;
        public static int iTM05_EXPENSEID = 0;
        public static int iT013_EXPENSETRANSACATIONID = 0;
        public static int iTM01_PRODUCTID = 0;
        public static int iTM02_PRODUCTSIZEID = 0;
        public static int iTM03_PRODUCTITEMID = 0;
        public static int iT001_ACCOUNTID = 0;
        public static int iT14_WORKERID = 0;
        public static int iT002_PURCHASEID = 0;
        public static int iT007_SALEID = 0;
        public static int iT012_CHALLENID = 0;
        public static int iT005_STOCKINID = 0;
        public static int iTM04_BANKID = 0;
        public static int iT009_PURCHASEPAYMENTID = 0;
        public static int T010_PACKINGSLIPID = 0;
        public static int iPrintableBillId = 0;
        public static int iPrintableChallenId = 0;
        public static string sPaymentMode = "";
        public static int iT15_LOANID = 0;
        public static int iT017_WORKAR_PAYMENTID = 0;
        public static int iT16_SALARYID = 0;
        public static int iEditT001_COMPANYID = 0;


        public static string sGSTReportName = "";
        public static int iT2_YEARID = 1;
        public static string sUserName = "";
        public static string sYEARNAME = "";
        public static string UserType = "";
        public static int iTL01_USERID = 1;


        public static string sReportName = "";
        public static ReportDocument RptReport;
        public static string GenerateFilePath = Application.StartupPath + "\\Data\\";

        public static string sRoleType = "";
        public static string sLanguage = "";
        public static string sUerName = "";
        public static string sDATABASENAME = "Style_King_DB";

        //GRID BACKGROUD COLOR 0, 47, 93  ,0, 174, 237   0, 175, 242
        public static int sGridHeaderColor1 = 0;
        public static int sGridHeaderColor2 = 175;
        public static int sGridHeaderColor3 = 242;

        public static string sConnStringDLL = File.ReadAllText(Application.StartupPath + "/connstr.dll");
        public static void MakeConnections()
        {
            Properties.Settings.Default.Style_King_Dev = sConnStringDLL;
            Properties.Settings.Default.Save();

            MyDabaseDataContext myDA;
            myDA = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
        }

        public static void SetSoftwareThems(Panel pnlHeader, Panel pnlFooter)
        {
            pnlHeader.BackColor = Color.FromArgb(1, 115, 177);
            pnlFooter.BackColor = Color.FromArgb(1, 115, 177);

            //pnlHeader.ForeColor = Color.Black;  0, 85, 158
            //pnlFooter.ForeColor = Color.Black;
        }

        public static void SetButtion(Button btn)
        {
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(48, 120, 208);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Image = null;
            //btn.ForeColor = Color.Black;
        }
        public static void SetExitButtion(Button btn)
        {
            btn.BackColor = Color.FromArgb(43, 41, 43);
        }

        public static void PopulateAdminRole(Button btnName)
        {
            if (Datalayer.sRoleType == "Admin")
                btnName.Enabled = true;
            else
                btnName.Enabled = false;
        }

        public static void SetError(ErrorProvider ep1, Control control, string sError)
        {

            ep1.SetError(control, sError);
            control.Focus();
            return;
        }

        public static void PopulateButtonVisibilities(bool bDelete, Button btnDelete)
        {
            btnDelete.Enabled = bDelete;
        }

        public static void InformationMessageBox(string sMesage)
        {
            MessageBox.Show(sMesage, "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void InsertMessageBox(string sPageName)
        {
            MessageBox.Show(sPageName + sMessageInserted, "Inserted successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void DosenotInsertMessageBox(string sPageName)
        {
            MessageBox.Show(sPageName + sMessageNotInserted, "Dose not inserted successfully", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void DuplicateMessageBox(string sPageName, string sCaption)
        {
            MessageBox.Show(sPageName + sMessageDuplicate, sCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void WorningMessageBox(string sMessage, string sCaption)
        {
            MessageBox.Show(sMessage, sCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void UpdateMessageBox(string sPageName)
        {
            MessageBox.Show(sPageName + sMessageUpdated, "Updated successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void DosenotUpdateMessageBox(string sPageName)
        {
            MessageBox.Show(sPageName + sMessageNotUpdated, "Dose not updated successfully", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void DeleteMessageBox(string sPageName)
        {
            MessageBox.Show(sPageName + sMessageDeleted, "Deleted successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void DosenotDeleteMessageBox(string sPageName)
        {
            MessageBox.Show(sPageName + sMessageNotDeleted, "Dose not deleted successfully", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static bool ShowQuestMsg(string msg)
        {
            DialogResult dr = MessageBox.Show(msg, "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                return true;
            }
            else { return false; }
        }

        public static void Reset(Control.ControlCollection col)
        {
            try
            {
                foreach (Control c in col)
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Windows.Forms.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "NumericTextBox.NumTextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Windows.Forms.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Windows.Forms.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Windows.Forms.DateTimePicker":
                            ((DateTimePicker)c).Value = DateTime.Now;
                            break;
                        case "System.Windows.Forms.ComboBox":
                            if (((ComboBox)c).Items.Count > 0)
                            {
                                ((ComboBox)c).SelectedIndex = 0;
                            }
                            else
                            {
                                ((ComboBox)c).SelectedIndex = -1;
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public static bool TimeMask(string MskTime)
        {
            string formate = @"^([0]?[1-9]|[1][0-2]):([0-5][0-9]|[1-9]) [AP]M$";
            if (System.Text.RegularExpressions.Regex.Match(MskTime, formate).Success)
                return true;
            else
                return false;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        public static void capitalizedlatter(TextBox _TextBox)
        {
            char[] v = _TextBox.Text.ToCharArray();
            if (v.Length > 0)
            {
                string s = v[0].ToString().ToUpper();
                for (int i = 1; i < v.Length; i++)
                {
                    if (v[i - 1].ToString().Equals(" "))
                        s += v[i].ToString().ToUpper();
                    else
                        s += v[i].ToString().ToLower();

                }
                _TextBox.Text = s;
                _TextBox.Select(_TextBox.Text.Length, 0);
            }
        }

        public static void DatabaseBackup(FolderBrowserDialog fd1)
        {
            SqlConnection sCon = new SqlConnection(Properties.Settings.Default.Style_King_Dev);
            try
            {

                fd1.Reset();
                fd1.ShowDialog();
                if (fd1.SelectedPath.Trim().Length > 0)
                {

                    SqlCommand scom = new SqlCommand();
                    string path = @"'" + fd1.SelectedPath + "\\" + Datalayer.sDATABASENAME + "_" + DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_') + ".bak'";
                    path = path.Replace("\\\\", "\\");
                    scom.CommandText = "backup database [" + Datalayer.sDATABASENAME + "] to disk = " + path;
                    scom.Connection = sCon;
                    try
                    {
                        if (sCon.State.ToString().ToLower() != "open")
                            sCon.Open();
                        scom.ExecuteNonQuery();
                        sCon.Close();
                        MessageBox.Show("Backup completed successfully!", "Database Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        WorningMessageBox(ex.Message.ToString(), "Error");
                    }
                    finally
                    {
                        if (sCon.State.ToString().ToLower() == "open")
                        {
                            sCon.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WorningMessageBox(ex.Message.ToString(), "Failed");
            }
            finally
            {
                if (sCon.State.ToString().ToLower() == "open")
                {
                    sCon.Close();
                }
            }
        }

        public static void DatabaseBackup(string fd1)
        {
            SqlConnection sCon = new SqlConnection(Properties.Settings.Default.Style_King_Dev);
            try
            {

                SqlCommand scom = new SqlCommand();
                string path = @"'" + fd1.ToString() + "\\" + Datalayer.sDATABASENAME + "_" + DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_') + ".bak'";
                path = path.Replace("\\\\", "\\");
                scom.CommandText = "backup database [" + Datalayer.sDATABASENAME + "] to disk = " + path;
                scom.Connection = sCon;
                try
                {
                    if (sCon.State.ToString().ToLower() != "open")
                        sCon.Open();
                    scom.ExecuteNonQuery();
                    sCon.Close();
                    // MessageBox.Show("Backup completed successfully!", "Database Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    WorningMessageBox(ex.Message.ToString(), "Error");
                }
                finally
                {
                    if (sCon.State.ToString().ToLower() == "open")
                    {
                        sCon.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                WorningMessageBox(ex.Message.ToString(), "Failed");
            }
            finally
            {
                if (sCon.State.ToString().ToLower() == "open")
                {
                    sCon.Close();
                }
            }
        }

        public static void RestoreDatabase(OpenFileDialog opnDilog)
        {
            opnDilog.ShowDialog();
            if (opnDilog.FileName.Trim().Length > 0)
            {
                string sBackupPath = File.ReadAllText(Application.StartupPath + "/backup_path.dll");
                DatabaseBackup(sBackupPath);

                string path = @"" + opnDilog.FileName;// +"\\" + Datalayer.sDATABASENAME + "_" + DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_') + ".bak'";
                path = path.Replace("\\\\", "\\");

                string st1Query = "USE master";
                st1Query = st1Query + " ALTER DATABASE " + Datalayer.sDATABASENAME + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE ";
                st1Query = st1Query + " ALTER DATABASE " + Datalayer.sDATABASENAME + " SET READ_ONLY";
                st1Query = st1Query + " ALTER DATABASE " + Datalayer.sDATABASENAME + " SET MULTI_USER";
                st1Query = st1Query + " RESTORE DATABASE " + Datalayer.sDATABASENAME + " FROM DISK  = '" + path + "'";
                st1Query = st1Query + " ";


                //DatabaseBackup( Application.StartupPath + "\\SCHOOL.jpg"
                SqlConnection scon = new SqlConnection(Properties.Settings.Default.Style_King_Dev);
                SqlCommand scom = new SqlCommand(st1Query, scon);
                try
                {
                    scon.Open();
                    scom.ExecuteNonQuery();
                    scon.Close();
                    //scon.Open();
                    //scom.CommandText = "restore database " + Datalayer.sDATABASENAME + " from disk = '" + opnDilog.FileName + "'";
                    //scom.ExecuteNonQuery();
                    //scon.Close();
                    MessageBox.Show("Database Restored successfully!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    scon.Close();
                }
            }
        }

        //public static void DatabaseBackup(FolderBrowserDialog fd1)
        //{
        //    SqlConnection sCon = new SqlConnection(Properties.Settings.Default.Rudra_Dev);
        //    try
        //    {

        //        fd1.Reset();
        //        fd1.ShowDialog();
        //        if (fd1.SelectedPath.Trim().Length > 0)
        //        {

        //            SqlCommand scom = new SqlCommand();
        //            string path = @"'" + fd1.SelectedPath + "\\Rudra_DB.bak'";
        //            path = path.Replace("\\\\", "\\");
        //            scom.CommandText = "backup database Rudra_DB to disk = " + path;
        //            scom.Connection = sCon;
        //            try
        //            {
        //                if (sCon.State.ToString().ToLower() != "open")
        //                    sCon.Open();
        //                scom.ExecuteNonQuery();
        //                sCon.Close();
        //                MessageBox.Show("Backup completed successfully!", "Database Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            catch (Exception ex)
        //            {
        //                WorningMessageBox(ex.Message.ToString(), "Error");
        //            }
        //            finally
        //            {
        //                if (sCon.State.ToString().ToLower() == "open")
        //                {
        //                    sCon.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WorningMessageBox(ex.Message.ToString(), "Failed");
        //    }
        //    finally
        //    {
        //        if (sCon.State.ToString().ToLower() == "open")
        //        {
        //            sCon.Close();
        //        }
        //    }
        //}

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
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public string SendSMS(string sPhoneNo, string sMessage)
        {
            string sMSG = "";
            sMSG = "NO";

            if (!CheckForInternetConnection())
            {
                sMSG = "NONET";
            }
            else
            {
                string sURL;
                StreamReader objReader;

                sURL = "http://180.149.240.33/vendorsms/pushsms.aspx?user=pithiyavipul&password=vipul@123&msisdn=91" + sPhoneNo.Trim() + "&sid=MODYUN&msg=" + sMessage.Trim() + "&fl=0&gwid=2";
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(sURL);
                try
                {
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                    objReader = new StreamReader(objStream);
                    objReader.Close();

                    sMSG = "YES";
                }
                catch (Exception ex)
                {
                    sMSG = "NO";
                }
            }

            return sMSG;
        }

        public Response DataTableToJSONWithStringBuilder(DataTable table, string sFileName)
        {
            Response _Response = new Response();

            try
            {
                var JSONString = new StringBuilder();
                if (table.Rows.Count > 0)
                {
                    JSONString.Append("[");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        JSONString.Append("{");
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            if (j < table.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                            }
                            else if (j == table.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                            }
                        }
                        if (i == table.Rows.Count - 1)
                        {
                            JSONString.Append("}");
                        }
                        else
                        {
                            JSONString.Append("},");
                        }
                    }
                    JSONString.Append("]");
                }

                //return JSONString.ToString();
                string sPath = Datalayer.GenerateFilePath + sFileName;
                if (File.Exists(sPath))
                    File.Delete(sPath);

                File.WriteAllText(sPath, JSONString.ToString());
                //return sb.ToString();
                //
                _Response.Result = true;
            }
            catch (Exception ex)
            {
                _Response.Result = false;
                _Response.ErrorLog = ex.Message.ToString(); ;
            }

            return _Response;
        }

        public Response DataTableToCSV(DataTable dt, char seperator, string sFileName)
        {
            Response _Response = new Response();

            try
            {
                StringBuilder sb = new StringBuilder();

                string[] columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                    ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }

                string sPath = Datalayer.GenerateFilePath + sFileName;
                if (File.Exists(sPath))
                    File.Delete(sPath);

                File.WriteAllText(sPath, sb.ToString());
                //return sb.ToString();
                //
                _Response.Result = true;
            }
            catch (Exception ex)
            {
                _Response.Result = false;
                _Response.ErrorLog = ex.Message.ToString(); ;
            }

            return _Response;
        }

        public Response ExportToExce(DataTable dtData, string sFileName, string sSheetName)
        {
            Response _Response = new Response();

            try
            {
                string sql = null;
                string data = null;
                int i = 0;
                int j = 0;

                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Name = sSheetName;

                //add title
                for (j = 0; j <= dtData.Columns.Count - 1; j++)
                {
                    data = dtData.Columns[j].ColumnName.ToString();
                    xlWorkSheet.Cells[1, j + 1] = data;
                }

                for (i = 0; i <= dtData.Rows.Count - 1; i++)
                {
                    for (j = 0; j <= dtData.Columns.Count - 1; j++)
                    {
                        data = dtData.Rows[i].ItemArray[j].ToString();
                        Microsoft.Office.Interop.Excel.Range rng = xlWorkSheet.Cells[i + 2, j + 1] as Microsoft.Office.Interop.Excel.Range;
                        System.Text.RegularExpressions.Regex Emailexpr = new System.Text.RegularExpressions.Regex(@"^0");
                        if (Emailexpr.IsMatch(data))
                            data = "'" + data;
                        
                        rng.Value2 = data;

                        try
                        {
                            string sType = dtData.Columns[j].DataType.ToString();
                            //if (sType.Contains("DateTime"))
                            //    rng.NumberFormat = "dd-MMM-yyyy";
                            //if (sType.Contains("String"))
                            //    rng.NumberFormat = "@";
                        }
                        catch
                        {
                        }
                    }
                }

                string sPath = Datalayer.GenerateFilePath + sFileName;
                if (File.Exists(sPath))
                    File.Delete(sPath);
                xlWorkBook.SaveAs(sPath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                _Response.Result = true;
            }
            catch (Exception ex)
            {
                _Response.Result = false;
                _Response.ErrorLog = ex.Message.ToString(); ;
            }

            return _Response;
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

    }

    public class Response
    {
        public bool Result { get; set; }
        public string ErrorLog { get; set; }
    }
}
