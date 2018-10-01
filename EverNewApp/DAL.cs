using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Media;
using System.Drawing;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace EverNewApp
{
    public class DAL
    {
        NotifyIcon nt = new NotifyIcon();
        public static ArrayList RemindersList = new ArrayList();
        public static DataTable Addmaster = new DataTable();
        DataTable SheduleMaster = new DataTable();
        MyDabaseDataContext MyDa;
        public static string sPath = "";

        SqlConnection MainConn = new SqlConnection(Properties.Settings.Default.Style_King_Dev);
        SqlTransaction st;

        public void OpenCon()
        {
            MainConn.Open();
            st = MainConn.BeginTransaction();
            st.Commit();// ("save");
        }

        public void exeMethod(string query)
        {
            SqlCommand scom = new SqlCommand(query, MainConn);
            //OleDbCommand scom = new OleDbCommand(query, MainConn);
            scom.Transaction = st;
            scom.ExecuteNonQuery();

        }

        public void ROLLB()
        {
            //st.Rollback("save");
            st.Rollback();//.Rollback("save");
            MainConn.Close();
            //scon.Close();
        }
        public void CloseM()
        {
            st.Commit();
            MainConn.Close();
        }

        public SqlConnection GetConnectionString()
        //public OleDbConnection GetConnectionString()
        {
            return MainConn;
        }


        //Database manipulation codes
        public Boolean ExecuteMethod(string qry)
        {
            try
            {
                SqlCommand scom = new SqlCommand(qry, MainConn);
                //OleDbCommand scom = new OleDbCommand(qry, MainConn);
                MainConn.Open();
                scom.ExecuteNonQuery();
                MainConn.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                MainConn.Close();
            }

        }


        public int GetLastID(string Tablename, string ColumnName)
        {

            int LastId = 1;
            DataTable temp = SelectMethod("select " + ColumnName + " from " + Tablename + " order by " + ColumnName + " desc");
            if (temp.Rows.Count > 0)
            {
                LastId = Convert.ToInt16(temp.Rows[0][0]) + 1;
            }
            return LastId;

        }

        public DataTable SelectDateOnTable(string sTableName)
        {
            DataTable d1 = new DataTable();
            try
            {
                string sQry = "SELECT * FROM " + sTableName + " where ACTIVE='true'";
                SqlDataAdapter sda = new SqlDataAdapter(sQry, MainConn);
                //OleDbDataAdapter sda = new OleDbDataAdapter(sQry, MainConn);
                sda.Fill(d1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");

            }
            finally
            {
                MainConn.Close();
            }
            return d1;
        }

        public DataTable SelectDataOnID(string sTableName, string sColumnName, int id)
        {
            DataTable d1 = new DataTable();
            try
            {
                string sQry = "SELECT * FROM " + sTableName + " where " + sColumnName + "=" + id;
                SqlDataAdapter sda = new SqlDataAdapter(sQry, MainConn);
                //OleDbDataAdapter sda = new OleDbDataAdapter(sQry, MainConn);
                sda.Fill(d1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");

            }
            finally
            {
                MainConn.Close();
            }
            return d1;
        }

        public DataTable SelectDataOnValues(string sTableName, string sColumnName, string value)
        {
            DataTable d1 = new DataTable();
            try
            {

                string sQry = "SELECT * FROM " + sTableName + " where " + sColumnName + "='" + value + "' and ACTIVE='true'";
                SqlDataAdapter sda = new SqlDataAdapter(sQry, MainConn);
                //OleDbDataAdapter sda = new OleDbDataAdapter(sQry, MainConn);
                sda.Fill(d1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                MainConn.Close();
            }
            return d1;
        }

        public DataTable SelectMethod(string qry)
        {
            DataTable d1 = new DataTable();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(qry, MainConn);
                //OleDbDataAdapter sda = new OleDbDataAdapter(qry, MainConn);
                sda.Fill(d1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                MainConn.Close();
            }
            return d1;
        }

        public Boolean InsetDataQuery(string sTableName, string[] sColumnsName, string sValues)
        {
            string[] Values = sValues.Split(',');

            string sQuery = "";
            sQuery = "INSERT INTO " + sTableName + " (";
            for (int i = 0; i < sColumnsName.Length; i++)
            {
                if (i == 0)
                    sQuery = sQuery + sColumnsName[i].ToString();
                else
                    sQuery = sQuery + "," + sColumnsName[i].ToString();
            }
            sQuery = sQuery + " ) VALUES ( ";
            for (int J = 0; J < Values.Length; J++)
            {
                if (J == 0)
                    sQuery = sQuery + Values[J].ToString();
                else
                    sQuery = sQuery + "," + Values[J].ToString();
            }
            sQuery = sQuery + " )";

            return ExecuteMethod(sQuery);
        }

        public Boolean UpdateDataQuery(string sTableName, string[] sColumnsName, string sValues, string sWhereColumns, int ID)
        {
            string[] Values = sValues.Split(',');

            string sQuery = "";
            sQuery = "UPDATE " + sTableName + " SET ";
            for (int i = 0; i < sColumnsName.Length; i++)
            {
                if (i == 0)
                    sQuery = sQuery + sColumnsName[i].ToString() + "=" + Values[i].ToString();
                else
                    sQuery = sQuery + "," + sColumnsName[i].ToString() + "=" + Values[i].ToString();
            }
            sQuery = sQuery + " WHERE " + sWhereColumns + "=" + ID;

            return ExecuteMethod(sQuery);
        }


       

    }
}
