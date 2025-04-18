using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using FASCloset.Services;
using FASCloset.Models;

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
            
            // Wire up the button click events
            btnExport.Click += btnExportDetailedReport_Click;
            btnRefresh.Click += btnRefresh_Click;
            
            // Also add a handler for the generate report button if it exists
            if (Controls.Find("btnGenerateReport", true).Length > 0)
            {
                Button btnGenerateReport = (Button)Controls.Find("btnGenerateReport", true)[0];
                btnGenerateReport.Click += btnGenerateSalesReport_Click;
            }
            
            // Set up the report type dropdown
            cmbReportType.SelectedIndexChanged += CmbReportType_SelectedIndexChanged;
        }
        
        private void CmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Generate report when the report type changes
            if (!backgroundWorker.IsBusy)
            {
                ProgressBarReport.Visible = true;
                backgroundWorker.RunWorkerAsync("GenerateSalesReport");
            }
        }

        private void btnGenerateSalesReport_Click(object sender, EventArgs e)
        {
            if (DateTimePickerEndDate.Value < DateTimePickerStartDate.Value)
            {
                MessageBox.Show("Ngày kết thúc không thể trước ngày bắt đầu", "Khoảng thời gian không hợp lệ", 
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
                MessageBox.Show("Ngày kết thúc không thể trước ngày bắt đầu", "Khoảng thời gian không hợp lệ", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (!backgroundWorker.IsBusy)
            {
                ProgressBarReport.Visible = true;
                backgroundWorker.RunWorkerAsync("ExportDetailedReport");
            }
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Reset date pickers to default values
            DateTimePickerStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Local);
            DateTimePickerEndDate.Value = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            
            // Clear the data grid
            DataGridViewReport.DataSource = null;
            
            // Reset summary values
            lblTotalRevenue.Text = "0 đ";
            lblOrderCount.Text = "0";
            lblAverageOrder.Text = "0 đ";
            
            // Prompt the user
            MessageBox.Show("Đã làm mới dữ liệu báo cáo", "Làm mới", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            DataTable reportData = null;

            // Make sure this runs on the UI thread
            if (cmbReportType.InvokeRequired)
            {
                // Use Invoke to access the UI control from the UI thread
                cmbReportType.Invoke(new Action(() => {
                    // Now you can safely access the SelectedIndex
                    switch (cmbReportType.SelectedIndex)
                    {
                        case 1: // Báo cáo bán hàng theo sản phẩm
                            reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                            break;
                        case 2: // Báo cáo bán hàng theo khách hàng
                            reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                            break;
                        default: // Tổng quan doanh số
                            reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                            break;
                    }
                }));
            }
            else
            {
                // You are on the UI thread, so you can directly access the control
                switch (cmbReportType.SelectedIndex)
                {
                    case 1: // Báo cáo bán hàng theo sản phẩm
                        reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                        break;
                    case 2: // Báo cáo bán hàng theo khách hàng
                        reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                        break;
                    default: // Tổng quan doanh số
                        reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                        break;
                }
            }

            e.Result = new object[] { "Report", reportData };
        }

        // Static helper methods for CSV export
        public static void WriteReportHeader(StringWriter writer, DataTable reportData)
        {
            for (int i = 0; i < reportData.Columns.Count; i++)
            {
                writer.Write(reportData.Columns[i].ColumnName);
                if (i < reportData.Columns.Count - 1)
                    writer.Write(",");
            }
            writer.WriteLine();
        }

        public static void WriteReportData(StringWriter writer, DataTable reportData)
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

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBarReport.Visible = false;

            if (e.Error != null)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {e.Error.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (e.Result is object[] result && result[0] is string actionType)
            {
                if (actionType == "Report" && result[1] is DataTable reportData)
                {
                    // Use Invoke to update the DataGridView on the UI thread
                    if (DataGridViewReport.InvokeRequired)
                    {
                        DataGridViewReport.Invoke(new Action(() =>
                        {
                            DataGridViewReport.DataSource = reportData;
                            FormatDataGridView();
                            UpdateSummary(reportData);
                        }));
                    }
                    else
                    {
                        DataGridViewReport.DataSource = reportData;
                        FormatDataGridView();
                        UpdateSummary(reportData);
                    }
                }
                else if (actionType == "Export" && result.Length > 2 &&
                         result[1] is string fileName && result[2] is string fileContent)
                {
                    HandleExport(fileName, fileContent);
                }
            }
        }
        
        private void FormatDataGridView()
        {
            // Format the data grid view with proper column styles
            if (DataGridViewReport.Columns.Count > 0)
            {
                foreach (DataGridViewColumn column in DataGridViewReport.Columns)
                {
                    // Check if the column contains "tiền" or "thu" to format as currency
                    if (column.HeaderText.Contains("tiền") || column.HeaderText.Contains("thu") || 
                        column.HeaderText.Contains("giá") || column.HeaderText.Contains("chi"))
                    {
                        column.DefaultCellStyle.Format = "N0";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    
                    // Format date columns
                    if (column.HeaderText.Contains("Ngày"))
                    {
                        column.DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                }
                
                // Auto resize columns for better display
                DataGridViewReport.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }
        
        private void UpdateSummary(DataTable reportData)
        {
            try
            {
                decimal totalRevenue = 0;
                int orderCount = reportData.Rows.Count;
                
                // Find the column containing money information based on report type
                string moneyColumnName = cmbReportType.SelectedIndex switch
                {
                    1 => "Doanh thu", // Product sales report
                    2 => "Tổng chi tiêu", // Customer report
                    _ => "Tổng tiền" // Default sales report
                };
                
                // Find the column index
                int moneyColumnIndex = -1;
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    if (reportData.Columns[i].ColumnName == moneyColumnName)
                    {
                        moneyColumnIndex = i;
                        break;
                    }
                }
                
                if (moneyColumnIndex >= 0)
                {
                    foreach (DataRow row in reportData.Rows)
                    {
                        if (decimal.TryParse(row[moneyColumnIndex].ToString(), out decimal amount))
                        {
                            totalRevenue += amount;
                        }
                    }
                }
                
                decimal averageOrder = orderCount > 0 ? totalRevenue / orderCount : 0;
                
                lblTotalRevenue.Text = $"{totalRevenue:N0} đ";
                lblOrderCount.Text = orderCount.ToString();
                lblAverageOrder.Text = $"{averageOrder:N0} đ";
            }
            catch
            {
                lblTotalRevenue.Text = "0 đ";
                lblOrderCount.Text = "0";
                lblAverageOrder.Text = "0 đ";
            }
        }
        
        // Static helpers
        public static int FindTotalAmountColumnIndex(DataTable reportData)
        {
            foreach (DataColumn column in reportData.Columns)
            {
                if (column.ColumnName == "TotalAmount" || column.ColumnName == "Tổng tiền")
                {
                    return reportData.Columns.IndexOf(column);
                }
            }
            return -1;
        }

        public static void ProcessExportDetailedReport(string filePath, List<ReportData> reportData)
        {
            // Implementation would go here - this is a placeholder
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write headers
                writer.WriteLine("Ngày,Mã đơn hàng,Mã khách hàng,Tên khách hàng,Tổng tiền,Phương thức thanh toán");
                
                // Write data
                foreach (var item in reportData)
                {
                    writer.WriteLine(
                        $"{item.OrderDate:yyyy-MM-dd},{item.OrderID},{item.CustomerID}," +
                        $"{item.CustomerName},{item.TotalAmount},{item.PaymentMethod}");
                }
            }
        }

        private void ProcessExportDetailedReport(DoWorkEventArgs e, DateTime startDate, DateTime endDate)
        {
            DataTable reportData = null;
            
            // Make sure this runs on the UI thread to get the report type
            if (cmbReportType.InvokeRequired)
            {
                cmbReportType.Invoke(new Action(() => {
                    switch (cmbReportType.SelectedIndex)
                    {
                        case 1: // Báo cáo bán hàng theo sản phẩm
                            reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                            break;
                        case 2: // Báo cáo bán hàng theo khách hàng
                            reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                            break;
                        default: // Tổng quan doanh số
                            reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                            break;
                    }
                }));
            }
            else
            {
                switch (cmbReportType.SelectedIndex)
                {
                    case 1: // Báo cáo bán hàng theo sản phẩm
                        reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                        break;
                    case 2: // Báo cáo bán hàng theo khách hàng
                        reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                        break;
                    default: // Tổng quan doanh số
                        reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                        break;
                }
            }

            if (reportData == null)
            {
                e.Result = new object[] { "Error", "Không thể tạo báo cáo" };
                return;
            }

            // Define file name
            string reportTypeName = "BaoCao";
            if (cmbReportType.InvokeRequired)
            {
                cmbReportType.Invoke(new Action(() => {
                    switch (cmbReportType.SelectedIndex)
                    {
                        case 1:
                            reportTypeName = "BaoCaoSanPham";
                            break;
                        case 2:
                            reportTypeName = "BaoCaoKhachHang";
                            break;
                        default:
                            reportTypeName = "BaoCaoTongHop";
                            break;
                    }
                }));
            }
            else
            {
                switch (cmbReportType.SelectedIndex)
                {
                    case 1:
                        reportTypeName = "BaoCaoSanPham";
                        break;
                    case 2:
                        reportTypeName = "BaoCaoKhachHang";
                        break;
                    default:
                        reportTypeName = "BaoCaoTongHop";
                        break;
                }
            }

            string fileName = $"{reportTypeName}_{startDate:yyyyMMdd}_den_{endDate:yyyyMMdd}.csv";

            using (var writer = new StringWriter())
            {
                // Write header
                WriteCsvHeader(writer, reportData);

                // Write data rows
                WriteCsvData(writer, reportData);

                // Return the result
                e.Result = new object[] { "Export", fileName, writer.ToString() };
            }
        }

        public static void WriteCsvHeader(StringWriter writer, DataTable reportData)
        {
            // Write the headers (column names)
            for (int i = 0; i < reportData.Columns.Count; i++)
            {
                writer.Write(reportData.Columns[i].ColumnName);
                if (i < reportData.Columns.Count - 1)
                    writer.Write(",");
            }
            writer.WriteLine(); // New line after header
        }

        public static void WriteCsvData(StringWriter writer, DataTable reportData)
        {
            // Loop through each row and write its data
            foreach (DataRow row in reportData.Rows)
            {
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    var value = row[i].ToString();

                    // If the value contains commas or quotes, escape it
                    if (value.Contains(",") || value.Contains("\""))
                    {
                        value = "\"" + value.Replace("\"", "\"\"") + "\""; // Escape double quotes by doubling them
                    }

                    writer.Write(value);

                    // Separate with commas for each column except the last one
                    if (i < reportData.Columns.Count - 1)
                        writer.Write(",");
                }
                writer.WriteLine(); // New line after each row
            }
        }

        private void HandleExport(string fileName, string fileContent)
        {
            // Show SaveFileDialog for user to choose location to save the CSV file
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Lưu báo cáo",
                FileName = fileName
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Write the content to the selected file
                    File.WriteAllText(saveFileDialog.FileName, fileContent);

                    // Notify user of success
                    MessageBox.Show($"Xuất báo cáo thành công đến {saveFileDialog.FileName}",
                        "Xuất báo cáo thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu file: {ex.Message}", "Lỗi xuất báo cáo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
