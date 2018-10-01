using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace EverNewApp
{
    public class DatabaseOperation
    {
        MyDabaseDataContext Myda;
        DAL dl = new DAL();

        public void FillStateList(ComboBox cmbstate)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM_STATEID,TM_STATE_CODE +' - '+ TM_STATE_NAME AS ProductName FROM TM_STATE ORDER BY TM_STATEID");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbstate.DataSource = dtData;
                cmbstate.DisplayMember = "ProductName";
                cmbstate.ValueMember = "TM_STATEID";
            }
            else
                cmbstate.DataSource = null;
        }

        public void FillItemName(ComboBox cmbItemName)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM01_PRODUCTID, TM01_NAME AS ProductName FROM TM01_PRODUCT WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM01_NAME");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbItemName.DataSource = dtData;
                cmbItemName.DisplayMember = "ProductName";
                cmbItemName.ValueMember = "TM01_PRODUCTID";
            }
            else
                cmbItemName.DataSource = null;
        }

        public void FillItemOnNo(string TM01_NO, ComboBox cmbItemName)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM01_PRODUCTID,TM01_NAME AS ProductName FROM TM01_PRODUCT WHERE TM01_NO='" + TM01_NO + "' and TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM01_PRODUCTID DESC");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbItemName.DataSource = dtData;
                cmbItemName.DisplayMember = "ProductName";
                cmbItemName.ValueMember = "TM01_PRODUCTID";
            }
            else
                cmbItemName.DataSource = null;
        }

        public void FillOnlyItemName(ComboBox cmbItemName)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM01_PRODUCTID,TM01_NAME AS ProductName FROM TM01_PRODUCT WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM01_NAME");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbItemName.DataSource = dtData;
                cmbItemName.DisplayMember = "ProductName";
                cmbItemName.ValueMember = "TM01_PRODUCTID";
            }
            else
                cmbItemName.DataSource = null;
        }

        public void FillItemSetName(ComboBox cmbItemSetName)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM01_PRODUCTID,TM01_NAME AS ProductName FROM TM01_PRODUCT WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM01_NAME");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbItemSetName.DataSource = dtData;
                cmbItemSetName.DisplayMember = "ProductName";
                cmbItemSetName.ValueMember = "TM01_PRODUCTID";
            }
            else
                cmbItemSetName.DataSource = null;
        }

        public void FillItemSize(ComboBox cmbItemName, ComboBox cmbItemSize)
        {
            int TM01_PRODUCTID = 0;
            if (!string.IsNullOrEmpty(cmbItemName.Text.Trim()))
            {
                int.TryParse(cmbItemName.SelectedValue.ToString(), out TM01_PRODUCTID);

                Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("SELECT TM02_PRODUCTSIZEID,TM02_SIZE FROM TM02_PRODUCTSIZE WHERE TM01_PRODUCTID=" + TM01_PRODUCTID + "AND TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM02_SIZE ASC");
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    cmbItemSize.DataSource = dtData;
                    cmbItemSize.DisplayMember = "TM02_SIZE";
                    cmbItemSize.ValueMember = "TM02_PRODUCTSIZEID";
                }
                else
                    cmbItemSize.DataSource = null;
            }
        }

        public decimal GetItemPrice(ComboBox cmbItemSize)
        {
            int TM02_PRODUCTSIZEID = 0;
            decimal dTM02_PRICE = 0;
            if (!string.IsNullOrEmpty(cmbItemSize.Text.Trim()))
            {
                int.TryParse(cmbItemSize.SelectedValue.ToString(), out TM02_PRODUCTSIZEID);

                Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
                DataTable dtData = new DataTable();
                dtData = dl.SelectMethod("SELECT TM02_PRICE FROM TM02_PRODUCTSIZE WHERE TM02_PRODUCTSIZEID=" + TM02_PRODUCTSIZEID + "AND TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM01_NO");
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    decimal.TryParse(dtData.Rows[0][0].ToString(), out dTM02_PRICE);
                }
            }

            return dTM02_PRICE;
        }

        public void FillBankList(ComboBox cmbBank)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM04_BANKID,TM04_NAME FROM TM04_BANK WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM04_NAME");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbBank.DataSource = dtData;
                cmbBank.DisplayMember = "TM04_NAME";
                cmbBank.ValueMember = "TM04_BANKID";
            }
            else
                cmbBank.DataSource = null;
        }

        public void FillExpenseMaster(ComboBox cmbExpense)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT TM05_EXPENSEID,TM05_EXPENSE FROM TM05_EXPENSE WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY TM05_EXPENSE");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbExpense.DataSource = dtData;
                cmbExpense.DisplayMember = "TM05_EXPENSE";
                cmbExpense.ValueMember = "TM05_EXPENSEID";
            }
            else
                cmbExpense.DataSource = null;
        }

        public void FillAccountList(ComboBox cmbAccount, string sType)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            if (string.IsNullOrEmpty(sType))
                dtData = dl.SelectMethod("SELECT T001_ACCOUNTID,T001_NAME FROM T001_ACCOUNT WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY T001_NAME");
            else if (sType == "p")
                dtData = dl.SelectMethod("SELECT T001_ACCOUNTID,T001_NAME FROM T001_ACCOUNT WHERE T001_TYPE='PARTY' AND TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY T001_NAME");
            else if (sType == "c")
                dtData = dl.SelectMethod("SELECT T001_ACCOUNTID,T001_NAME FROM T001_ACCOUNT WHERE T001_TYPE='CUSTOMER' AND TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY T001_NAME");
            else if (sType == "pl")
                dtData = dl.SelectMethod("SELECT T001_ACCOUNTID,T001_NAME FROM T001_ACCOUNT WHERE T001_TYPE='POWERLOOM' AND TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY T001_NAME");

            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbAccount.DataSource = dtData;
                cmbAccount.DisplayMember = "T001_NAME";
                cmbAccount.ValueMember = "T001_ACCOUNTID";
            }
            else
                cmbAccount.DataSource = null;
        }

        public void FillEmployeeList(ComboBox cmbEmployee)
        {
            Myda = new MyDabaseDataContext(Properties.Settings.Default.Style_King_Dev);
            DataTable dtData = new DataTable();
            dtData = dl.SelectMethod("SELECT T14_WORKERID,T14_NAME FROM T14_WORKER WHERE TM_COMPAYID='" + Datalayer.iT001_COMPANYID.ToString() + "' ORDER BY T14_NAME");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                cmbEmployee.DataSource = dtData;
                cmbEmployee.DisplayMember = "T14_NAME";
                cmbEmployee.ValueMember = "T14_WORKERID";
            }
            else
                cmbEmployee.DataSource = null;
        }

    }
}
