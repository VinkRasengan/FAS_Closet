// This file contains the UcRevenueReport UserControl that handles revenue reporting functionality
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
    /// <summary>
    /// User control for generating and displaying revenue reports
    /// Provides functionality for viewing sales data by different criteria, exporting to CSV, and filtering by date range
    /// </summary>
    public partial class UcRevenueReport : UserControl
    {
        /// <summary>
        /// Background worker for handling report generation operations without freezing the UI
        /// </summary>
        private readonly BackgroundWorker backgroundWorker;

        /// <summary>
        /// Initializes a new instance of the UcRevenueReport user control
        /// Sets up UI components, event handlers, and default date ranges
        /// </summary>
        public UcRevenueReport()
        {
            InitializeComponent();
            
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            
            // Set default date range to include all orders from the past 2 years to present
            DateTimePickerStartDate.Value = new DateTime(DateTime.Now.Year - 2, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTimePickerEndDate.Value = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            
            // Wire up the button click events
            btnExport.Click += btnExportDetailedReport_Click;
            btnRefresh.Click += btnRefresh_Click;
            
            // Set up the report type dropdown
            cmbReportType.SelectedIndexChanged += CmbReportType_SelectedIndexChanged;
            
            // Set initial DataGridView styling
            SetupDataGridViewStyle();
            
            // Automatically load data when form is initialized
            this.Load += (s, e) => {
                // Show sample data first so DataGridView is never empty
                ShowSampleData();
                
                // Load actual data from database
                if (!backgroundWorker.IsBusy)
                {
                    ProgressBarReport.Visible = true;
                    backgroundWorker.RunWorkerAsync("GenerateSalesReport");
                }
            };
        }
        
        /// <summary>
        /// Applies consistent styling to the DataGridView to ensure a professional appearance
        /// </summary>
        private void SetupDataGridViewStyle()
        {
            DataGridViewReport.BorderStyle = BorderStyle.None;
            DataGridViewReport.BackgroundColor = Color.White;
            DataGridViewReport.GridColor = Color.FromArgb(230, 230, 230);
            DataGridViewReport.EnableHeadersVisualStyles = false;
            DataGridViewReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 150, 190);
            DataGridViewReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridViewReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            DataGridViewReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridViewReport.ColumnHeadersHeight = 40;
            DataGridViewReport.RowHeadersVisible = false;
            DataGridViewReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGridViewReport.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            DataGridViewReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            DataGridViewReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            DataGridViewReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 215, 229);
            DataGridViewReport.DefaultCellStyle.SelectionForeColor = Color.Black;
            DataGridViewReport.RowTemplate.Height = 35;
            DataGridViewReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        
        /// <summary>
        /// Event handler triggered when the user changes the report type
        /// Refreshes the display with the new report type and triggers a data reload
        /// </summary>
        /// <param name="sender">The control that triggered the event</param>
        /// <param name="e">Event arguments</param>
        private void CmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show sample data immediately when report type changes
            ShowSampleData();
            
            // Generate report when the report type changes
            if (!backgroundWorker.IsBusy)
            {
                ProgressBarReport.Visible = true;
                backgroundWorker.RunWorkerAsync("GenerateSalesReport");
            }
        }

        /// <summary>
        /// Event handler for export button click
        /// Validates date range and triggers the export operation
        /// </summary>
        /// <param name="sender">The control that triggered the event</param>
        /// <param name="e">Event arguments</param>
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
        
        /// <summary>
        /// Event handler for refresh button click
        /// Resets date pickers to default range (past 2 years) and reloads report data
        /// </summary>
        /// <param name="sender">The control that triggered the event</param>
        /// <param name="e">Event arguments</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Reset date range to expand time coverage
            DateTimePickerStartDate.Value = new DateTime(DateTime.Now.Year - 2, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTimePickerEndDate.Value = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            
            // Show progress bar
            ProgressBarReport.Visible = true;
            
            // Ensure DataGridView always has content by showing sample data immediately
            ShowSampleData();
            
            // Load actual data
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync("GenerateSalesReport");
            }
            else
            {
                MessageBox.Show("System is processing the previous request, please wait a moment",
                    "Processing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ProgressBarReport.Visible = false;
            }
        }

        /// <summary>
        /// Sets up default columns in the DataGridView when no data is available
        /// Creates appropriate columns based on the selected report type
        /// </summary>
        private void SetupDefaultColumns()
        {
            DataGridViewReport.Columns.Clear();
            
            // Create basic column structure based on the current report type
            switch (cmbReportType.SelectedIndex)
            {
                case 1: // Product report
                    DataGridViewReport.Columns.Add("ProductID", "Product ID");
                    DataGridViewReport.Columns.Add("ProductName", "Product Name");
                    DataGridViewReport.Columns.Add("Quantity", "Quantity");
                    DataGridViewReport.Columns.Add("Revenue", "Revenue");
                    DataGridViewReport.Columns.Add("Profit", "Profit");
                    break;
                    
                case 2: // Customer report
                    DataGridViewReport.Columns.Add("CustomerID", "Customer ID");
                    DataGridViewReport.Columns.Add("CustomerName", "Customer Name");
                    DataGridViewReport.Columns.Add("OrderCount", "Order Count");
                    DataGridViewReport.Columns.Add("TotalSpend", "Total Spend");
                    break;
                    
                default: // Overview report
                    DataGridViewReport.Columns.Add("OrderDate", "Date");
                    DataGridViewReport.Columns.Add("OrderID", "Order ID");
                    DataGridViewReport.Columns.Add("CustomerName", "Customer");
                    DataGridViewReport.Columns.Add("TotalAmount", "Total Amount");
                    DataGridViewReport.Columns.Add("PaymentMethod", "Payment Method");
                    break;
            }
            
            // Apply formatting to columns
            FormatDataGridView();
        }

        /// <summary>
        /// Background worker DoWork event handler
        /// Entry point for background processing tasks
        /// </summary>
        /// <param name="sender">The background worker object</param>
        /// <param name="e">Event arguments containing the task to be processed</param>
        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            ProcessBackgroundTask(e);
        }

        /// <summary>
        /// Processes background tasks based on the task type
        /// Routes to appropriate processing method based on the task argument
        /// </summary>
        /// <param name="e">Event arguments containing the task to be processed</param>
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

        /// <summary>
        /// Generates sales report data based on selected report type and date range
        /// Handles thread-safe access to UI controls from background thread
        /// </summary>
        /// <param name="e">Event arguments for storing the result</param>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
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
                        case 1: // Product sales report
                            reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                            break;
                        case 2: // Customer sales report
                            reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                            break;
                        default: // Sales overview
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
                    case 1: // Product sales report
                        reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                        break;
                    case 2: // Customer sales report
                        reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                        break;
                    default: // Sales overview
                        reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                        break;
                }
            }

            e.Result = new object[] { "Report", reportData };
        }

        /// <summary>
        /// Writes CSV header row with column names
        /// </summary>
        /// <param name="writer">StringWriter for output</param>
        /// <param name="reportData">DataTable containing the report data</param>
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

        /// <summary>
        /// Writes CSV data rows with proper escaping for special characters
        /// </summary>
        /// <param name="writer">StringWriter for output</param>
        /// <param name="reportData">DataTable containing the report data</param>
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

        /// <summary>
        /// Background worker completion event handler
        /// Processes the results from background tasks and updates the UI accordingly
        /// </summary>
        /// <param name="sender">The background worker object</param>
        /// <param name="e">Event arguments containing the task results</param>
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                    // Use Invoke to update the DataGridView on the UI thread
                    if (DataGridViewReport.InvokeRequired)
                    {
                        DataGridViewReport.Invoke(new Action(() =>
                        {
                            UpdateDataGridViewWithReport(reportData);
                        }));
                    }
                    else
                    {
                        UpdateDataGridViewWithReport(reportData);
                    }
                }
                else if (actionType == "Export" && result.Length > 2 &&
                         result[1] is string fileName && result[2] is string fileContent)
                {
                    HandleExport(fileName, fileContent);
                }
            }
        }
        
        /// <summary>
        /// Updates the DataGridView with report data and applies formatting
        /// Shows a message if no data is available for the selected date range
        /// </summary>
        /// <param name="reportData">DataTable containing the report data</param>
        private void UpdateDataGridViewWithReport(DataTable reportData)
        {
            if (reportData == null || reportData.Rows.Count == 0)
            {
                MessageBox.Show("No report data available for the selected date range", 
                    "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Show sample data to ensure the grid is not empty
                ShowSampleData();
                return;
            }
            
            // Update DataSource with actual report data
            DataGridViewReport.DataSource = null; // Clear first to avoid errors when changing DataTable structure
            DataGridViewReport.DataSource = reportData;
            
            // Apply formatting and update summary
            FormatDataGridView();
            UpdateSummary(reportData);
            
            // Update status to indicate that this is actual data
            lblReportStatus.Text = $"✅ Displaying actual data from {DateTimePickerStartDate.Value:dd/MM/yyyy} to {DateTimePickerEndDate.Value:dd/MM/yyyy}";
            lblReportStatus.ForeColor = Color.FromArgb(40, 167, 69); // Green color
            lblReportStatus.Visible = true;
        }
        
        /// <summary>
        /// Applies consistent formatting to the DataGridView columns
        /// Sets appropriate formatting for currency, date, and ID columns
        /// </summary>
        private void FormatDataGridView()
        {
            if (DataGridViewReport.Columns.Count > 0)
            {
                DataGridViewReport.EnableHeadersVisualStyles = false;
                DataGridViewReport.BorderStyle = BorderStyle.None;
                DataGridViewReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
                DataGridViewReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                DataGridViewReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 215, 229);
                DataGridViewReport.DefaultCellStyle.SelectionForeColor = Color.Black;
                DataGridViewReport.BackgroundColor = Color.White;
                DataGridViewReport.RowHeadersVisible = false;
                
                DataGridViewReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 150, 190);
                DataGridViewReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DataGridViewReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
                DataGridViewReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DataGridViewReport.ColumnHeadersHeight = 40;
                
                DataGridViewReport.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                
                foreach (DataGridViewColumn column in DataGridViewReport.Columns)
                {
                    if (column.HeaderText.Contains("money") || column.HeaderText.Contains("revenue") || 
                        column.HeaderText.Contains("price") || column.HeaderText.Contains("spend"))
                    {
                        column.DefaultCellStyle.Format = "N0";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        column.DefaultCellStyle.ForeColor = Color.FromArgb(31, 111, 139);
                        column.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
                    }
                    
                    if (column.HeaderText.Contains("Date"))
                    {
                        column.DefaultCellStyle.Format = "dd/MM/yyyy";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    
                    if (column.HeaderText.Contains("ID"))
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    
                    if (column.HeaderText.Contains("Name") || column.HeaderText.Contains("Email") || 
                        column.HeaderText.Contains("Address") || column.HeaderText.Contains("product"))
                    {
                        column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        column.MinimumWidth = 150;
                    }
                    
                    if (column.HeaderText.Contains("Payment"))
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                
                DataGridViewReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                DataGridViewReport.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                
                foreach (DataGridViewColumn column in DataGridViewReport.Columns)
                {
                    if (column.Width > 250)
                        column.Width = 250;
                }
            }
        }
        
        /// <summary>
        /// Updates the summary statistics displayed on the report
        /// Calculates total revenue, order count, and average order value
        /// </summary>
        /// <param name="reportData">DataTable containing the report data</param>
        private void UpdateSummary(DataTable reportData)
        {
            try
            {
                decimal totalRevenue = 0;
                int orderCount = reportData.Rows.Count;
                
                string moneyColumnName = cmbReportType.SelectedIndex switch
                {
                    1 => "Revenue", // Product sales report
                    2 => "Total Spend", // Customer report
                    _ => "Total Amount" // Default sales report
                };
                
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
        
        /// <summary>
        /// Finds the column index containing total amount information in a report DataTable
        /// </summary>
        /// <param name="reportData">DataTable containing the report data</param>
        /// <returns>Index of the total amount column, or -1 if not found</returns>
        public static int FindTotalAmountColumnIndex(DataTable reportData)
        {
            foreach (DataColumn column in reportData.Columns)
            {
                if (column.ColumnName == "TotalAmount" || column.ColumnName == "Total Amount")
                {
                    return reportData.Columns.IndexOf(column);
                }
            }
            return -1;
        }

        /// <summary>
        /// Processes the detailed report export request
        /// Generates report data and converts it to CSV format
        /// </summary>
        /// <param name="e">Event arguments for storing the result</param>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        private void ProcessExportDetailedReport(DoWorkEventArgs e, DateTime startDate, DateTime endDate)
        {
            DataTable reportData = null;
            
            if (cmbReportType.InvokeRequired)
            {
                cmbReportType.Invoke(new Action(() => {
                    switch (cmbReportType.SelectedIndex)
                    {
                        case 1: // Product sales report
                            reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                            break;
                        case 2: // Customer sales report
                            reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                            break;
                        default: // Sales overview
                            reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                            break;
                    }
                }));
            }
            else
            {
                switch (cmbReportType.SelectedIndex)
                {
                    case 1: // Product sales report
                        reportData = ReportManager.GenerateProductSalesReport(startDate, endDate);
                        break;
                    case 2: // Customer sales report
                        reportData = ReportManager.GenerateCustomerSalesReport(startDate, endDate);
                        break;
                    default: // Sales overview
                        reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                        break;
                }
            }

            if (reportData == null)
            {
                e.Result = new object[] { "Error", "Unable to generate report" };
                return;
            }

            string reportTypeName = "Report";
            if (cmbReportType.InvokeRequired)
            {
                cmbReportType.Invoke(new Action(() => {
                    switch (cmbReportType.SelectedIndex)
                    {
                        case 1:
                            reportTypeName = "ProductReport";
                            break;
                        case 2:
                            reportTypeName = "CustomerReport";
                            break;
                        default:
                            reportTypeName = "OverviewReport";
                            break;
                    }
                }));
            }
            else
            {
                switch (cmbReportType.SelectedIndex)
                {
                    case 1:
                        reportTypeName = "ProductReport";
                        break;
                    case 2:
                        reportTypeName = "CustomerReport";
                        break;
                    default:
                        reportTypeName = "OverviewReport";
                        break;
                }
            }

            string fileName = $"{reportTypeName}_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}.csv";

            using (var writer = new StringWriter())
            {
                WriteCsvHeader(writer, reportData);
                WriteCsvData(writer, reportData);
                e.Result = new object[] { "Export", fileName, writer.ToString() };
            }
        }

        /// <summary>
        /// Writes CSV header row with column names
        /// </summary>
        /// <param name="writer">StringWriter for output</param>
        /// <param name="reportData">DataTable containing the report data</param>
        public static void WriteCsvHeader(StringWriter writer, DataTable reportData)
        {
            for (int i = 0; i < reportData.Columns.Count; i++)
            {
                writer.Write(reportData.Columns[i].ColumnName);
                if (i < reportData.Columns.Count - 1)
                    writer.Write(",");
            }
            writer.WriteLine();
        }

        /// <summary>
        /// Writes CSV data rows with proper escaping for special characters
        /// </summary>
        /// <param name="writer">StringWriter for output</param>
        /// <param name="reportData">DataTable containing the report data</param>
        public static void WriteCsvData(StringWriter writer, DataTable reportData)
        {
            foreach (DataRow row in reportData.Rows)
            {
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    var value = row[i].ToString();

                    if (value.Contains(",") || value.Contains("\""))
                    {
                        value = "\"" + value.Replace("\"", "\"\"") + "\"";
                    }

                    writer.Write(value);

                    if (i < reportData.Columns.Count - 1)
                        writer.Write(",");
                }
                writer.WriteLine();
            }
        }

        /// <summary>
        /// Handles the file export process
        /// Shows a save dialog and writes the CSV file to disk with proper encoding
        /// </summary>
        /// <param name="fileName">Suggested file name</param>
        /// <param name="fileContent">CSV content to write</param>
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
                    File.WriteAllText(saveFileDialog.FileName, fileContent, new System.Text.UTF8Encoding(true));
                    MessageBox.Show($"Report successfully exported to {saveFileDialog.FileName}",
                        "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Export Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Shows sample data in the DataGridView when no actual data is available
        /// Creates different sample data based on the selected report type
        /// </summary>
        private void ShowSampleData()
        {
            try
            {
                DataGridViewReport.DataSource = null;
                DataGridViewReport.Rows.Clear();
                DataGridViewReport.Columns.Clear();
                
                DataGridViewReport.Visible = true;
                DataGridViewReport.BringToFront();
                
                DataTable sampleData = new DataTable();
                
                switch (cmbReportType.SelectedIndex)
                {
                    case 1: // Product report
                        sampleData.Columns.Add("Product ID", typeof(string));
                        sampleData.Columns.Add("Product Name", typeof(string));
                        sampleData.Columns.Add("Category", typeof(string));
                        sampleData.Columns.Add("Quantity Sold", typeof(int));
                        sampleData.Columns.Add("Order Count", typeof(int));
                        sampleData.Columns.Add("Revenue", typeof(decimal));
                        
                        sampleData.Rows.Add("P001", "Basic T-Shirt", "Shirts", 15, 10, 1500000);
                        sampleData.Rows.Add("P002", "Women's Jeans", "Pants", 8, 7, 2400000);
                        sampleData.Rows.Add("P003", "Party Dress", "Dresses", 5, 5, 3250000);
                        sampleData.Rows.Add("P004", "Denim Jacket", "Shirts", 7, 6, 2100000);
                        break;
                        
                    case 2: // Customer report
                        sampleData.Columns.Add("Customer ID", typeof(string));
                        sampleData.Columns.Add("Customer Name", typeof(string));
                        sampleData.Columns.Add("Email", typeof(string));
                        sampleData.Columns.Add("Phone", typeof(string));
                        sampleData.Columns.Add("Total Spend", typeof(decimal));
                        
                        sampleData.Rows.Add("C001", "John Doe", "john@example.com", "0901234567", 2500000);
                        sampleData.Rows.Add("C002", "Jane Smith", "jane@example.com", "0912345678", 4300000);
                        sampleData.Rows.Add("C003", "Michael Brown", "michael@example.com", "0923456789", 1800000);
                        sampleData.Rows.Add("C004", "Emily Davis", "emily@example.com", "0934567890", 3600000);
                        break;
                        
                    default: // Overview report
                        sampleData.Columns.Add("Date", typeof(DateTime));
                        sampleData.Columns.Add("Order ID", typeof(string));
                        sampleData.Columns.Add("Customer", typeof(string));
                        sampleData.Columns.Add("Total Amount", typeof(decimal));
                        sampleData.Columns.Add("Payment Method", typeof(string));
                        
                        sampleData.Rows.Add(DateTime.Now.AddDays(-1), "O001", "John Doe", 850000, "Cash");
                        sampleData.Rows.Add(DateTime.Now.AddDays(-2), "O002", "Jane Smith", 1200000, "Bank Transfer");
                        sampleData.Rows.Add(DateTime.Now.AddDays(-3), "O003", "Michael Brown", 650000, "Cash");
                        sampleData.Rows.Add(DateTime.Now.AddDays(-4), "O004", "Emily Davis", 950000, "Momo");
                        break;
                }
                
                DataGridViewReport.DataSource = sampleData;
                
                FormatDataGridView();
                UpdateSummary(sampleData);
                
                lblReportStatus.Text = "⚠️ Displaying sample data. Please select a date range and click 'Refresh' to view actual data.";
                lblReportStatus.Visible = true;
                lblReportStatus.ForeColor = Color.FromArgb(255, 128, 0);
                
                DataGridViewReport.Refresh();
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying sample data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Error displaying sample data: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
