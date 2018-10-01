using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EverNewApp.Report
{
    public partial class frmReportViwer : Form
    {
        public frmReportViwer()
        {
            InitializeComponent();
        }

        private void frmReportViwer_Load(object sender, EventArgs e)
        {
            lblTitle.Text = Datalayer.sReportName.ToString();
            this.Text = Datalayer.sReportName.ToString();
            PrintReport();
        }

        private void frmReportViwer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }


        void PrintReport()
        {
            try
            {
                crystalReportViewer1.ReportSource = Datalayer.RptReport;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
