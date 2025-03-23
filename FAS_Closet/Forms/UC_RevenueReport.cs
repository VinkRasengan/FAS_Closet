using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FASCloset.Services;
using System.ComponentModel;

namespace FASCloset.Forms
{
    public partial class UcRevenueReport : UserControl
    {
        private readonly BackgroundWorker backgroundWorker;

        public UcRevenueReport()
        {
            InitializeComponent();
            
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            
            // Set default date range to current month with explicit DateTimeKind
            DateTimePickerStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Local);
            DateTimePickerEndDate.Value = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
        }

        private void btnGenerateSalesReport_Click(object sender, EventArgs e)
        {
            if (DateTimePickerEndDate.Value < DateTimePickerStartDate.Value)
            {
                MessageBox.Show("End date cannot be before start date", "Invalid Date Range", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (!backgroundWorker.IsBusy)
            {
                ProgressBarReport.Visible = true;
                backgroundWorker.RunWorkerAsync("GenerateSalesReport");
            }
        }

        private void btnExportDetailedReport_Click(object sender, EventArgs e)
        {
            if (DateTimePickerEndDate.Value < DateTimePickerStartDate.Value)
            {
                MessageBox.Show("End date cannot be before start date", "Invalid Date Range", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (!backgroundWorker.IsBusy)
            {
                ProgressBarReport.Visible = true;
                backgroundWorker.RunWorkerAsync("ExportDetailedReport");
            }
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            ProcessBackgroundTask(e);
        }

        // Refactored to reduce cognitive complexity
        private void ProcessBackgroundTask(DoWorkEventArgs e)
        {
            string? task = e.Argument as string;
            DateTime startDate = DateTimePickerStartDate.Value;
            DateTime endDate = DateTimePickerEndDate.Value;
            
            if (task == "GenerateSalesReport")
            {
                ProcessGenerateSalesReport(e, startDate, endDate);
            }
            else if (task == "ExportDetailedReport")
            {
                ProcessExportDetailedReport(e, startDate, endDate);
            }
        }

        private void ProcessGenerateSalesReport(DoWorkEventArgs e, DateTime startDate, DateTime endDate)
        {
            DataTable reportData;
            
            // Get report based on selected type
            switch (cmbReportType.SelectedIndex)
            {
                case 1: // Product Sales
                case 2: // Customer Orders
                    reportData = ReportManager.GenerateDetailedSalesReport(startDate, endDate);
                    break;
                default: // Sales Summary or fallback
                    reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                    break;
            }
            
            e.Result = new object[] { "Report", reportData };
        }

        private void ProcessExportDetailedReport(DoWorkEventArgs e, DateTime startDate, DateTime endDate)
        {
            var reportData = ReportManager.GenerateDetailedSalesReport(startDate, endDate);
            
            string fileName = $"SalesReport_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}.csv";
            
            using (var writer = new StringWriter())
            {
                WriteReportHeader(writer, reportData);
                WriteReportData(writer, reportData);
                
                e.Result = new object[] { "Export", fileName, writer.ToString() };
            }
        }

        private void WriteReportHeader(StringWriter writer, DataTable reportData)
        {
            for (int i = 0; i < reportData.Columns.Count; i++)
            {
                writer.Write(reportData.Columns[i].ColumnName);
                if (i < reportData.Columns.Count - 1)
                    writer.Write(",");
            }
            writer.WriteLine();
        }

        private void WriteReportData(StringWriter writer, DataTable reportData)
        {
            foreach (DataRow row in reportData.Rows)
            {
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    if (row[i] != null)
                    {
                        string value = row[i].ToString() ?? "";
                        // Escape quotes and wrap in quotes if it contains comma
                        if (value.Contains(","))
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        writer.Write(value);
                    }
                    
                    if (i < reportData.Columns.Count - 1)
                        writer.Write(",");
                }
                writer.WriteLine();
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBarReport.Visible = false;
            
            if (e.Error != null)
            {
                MessageBox.Show($"An error occurred: {e.Error.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (e.Result is object[] result && result[0] is string actionType)
            {
                if (actionType == "Report" && result[1] is DataTable reportData)
                {
                    DataGridViewReport.DataSource = reportData;
                    UpdateSummary(reportData);
                }
                else if (actionType == "Export" && result.Length > 2 && 
                         result[1] is string fileName && result[2] is string fileContent)
                {
                    HandleExport(fileName, fileContent);
                }
            }
        }

        private void HandleExport(string fileName, string fileContent)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Save Report",
                FileName = fileName
            };
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, fileContent);
                    MessageBox.Show($"Report successfully exported to {saveFileDialog.FileName}", 
                        "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Export Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void UpdateSummary(DataTable reportData)
        {
            try
            {
                decimal totalRevenue = 0;
                int orderCount = reportData.Rows.Count;
                
                // Find the column containing total amount information
                int totalAmountColumnIndex = FindTotalAmountColumnIndex(reportData);
                
                if (totalAmountColumnIndex >= 0)
                {
                    foreach (DataRow row in reportData.Rows)
                    {
                        if (decimal.TryParse(row[totalAmountColumnIndex].ToString(), out decimal amount))
                        {
                            totalRevenue += amount;
                        }
                    }
                }
                
                decimal averageOrder = orderCount > 0 ? totalRevenue / orderCount : 0;
                
                lblTotalRevenue.Text = $"${totalRevenue:N2}";
                lblOrderCount.Text = orderCount.ToString();
                lblAverageOrder.Text = $"${averageOrder:N2}";
            }
            catch
            {
                lblTotalRevenue.Text = "$0.00";
                lblOrderCount.Text = "0";
                lblAverageOrder.Text = "$0.00";
            }
        }
        
        private int FindTotalAmountColumnIndex(DataTable reportData)
        {
            foreach (DataColumn column in reportData.Columns)
            {
                if (column.ColumnName == "TotalAmount")
                {
                    return reportData.Columns.IndexOf(column);
                }
            }
            return -1;
        }
    }
}
