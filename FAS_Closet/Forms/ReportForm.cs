using System;
using System.Windows.Forms;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startDate = dateTimePickerStart.Value;
            DateTime endDate = dateTimePickerEnd.Value;

            var reportData = ReportManager.GenerateSalesReport(startDate, endDate);
            dataGridViewReport.DataSource = reportData;
        }
    }
}
