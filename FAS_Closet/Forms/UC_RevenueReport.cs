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
        public DateTimePicker DateTimePickerStartDate = new DateTimePicker();
        public DateTimePicker DateTimePickerEndDate = new DateTimePicker();
        public DataGridView DataGridViewReport = new DataGridView();
        public ProgressBar ProgressBarReport = new ProgressBar();
        
        private Button btnGenerateSalesReport;
        private Button btnExportDetailedReport;
        private ComboBox cmbReportType;
        private Label lblTotalRevenue;
        private Label lblOrderCount;
        private Label lblAverageOrder;
        private readonly BackgroundWorker backgroundWorker;

        public UcRevenueReport()
        {
            InitializeComponent();
            
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            
            // Set default date range to current month
            DateTimePickerStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTimePickerEndDate.Value = DateTime.Now;
        }

        private void InitializeComponent()
        {
            // Date range selection
            Label lblDateRange = new Label();
            lblDateRange.Text = "Select Date Range:";
            lblDateRange.Location = new Point(20, 20);
            lblDateRange.Size = new Size(120, 25);
            lblDateRange.TextAlign = ContentAlignment.MiddleLeft;
            
            DateTimePickerStartDate = new DateTimePicker();
            DateTimePickerStartDate.Location = new Point(150, 20);
            DateTimePickerStartDate.Size = new Size(150, 25);
            DateTimePickerStartDate.Format = DateTimePickerFormat.Short;
            
            Label lblTo = new Label();
            lblTo.Text = "to";
            lblTo.Location = new Point(310, 20);
            lblTo.Size = new Size(30, 25);
            lblTo.TextAlign = ContentAlignment.MiddleCenter;
            
            DateTimePickerEndDate = new DateTimePicker();
            DateTimePickerEndDate.Location = new Point(350, 20);
            DateTimePickerEndDate.Size = new Size(150, 25);
            DateTimePickerEndDate.Format = DateTimePickerFormat.Short;
            
            // Report type selection
            Label lblReportType = new Label();
            lblReportType.Text = "Report Type:";
            lblReportType.Location = new Point(20, 60);
            lblReportType.Size = new Size(120, 25);
            lblReportType.TextAlign = ContentAlignment.MiddleLeft;
            
            cmbReportType = new ComboBox();
            cmbReportType.Location = new Point(150, 60);
            cmbReportType.Size = new Size(200, 25);
            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportType.Items.AddRange(new object[] { "Sales Summary", "Product Sales", "Customer Orders" });
            cmbReportType.SelectedIndex = 0;
            
            // Action buttons
            btnGenerateSalesReport = new Button();
            btnGenerateSalesReport.Text = "Generate Report";
            btnGenerateSalesReport.Location = new Point(20, 100);
            btnGenerateSalesReport.Size = new Size(150, 30);
            btnGenerateSalesReport.Click += btnGenerateSalesReport_Click;
            
            btnExportDetailedReport = new Button();
            btnExportDetailedReport.Text = "Export to CSV";
            btnExportDetailedReport.Location = new Point(180, 100);
            btnExportDetailedReport.Size = new Size(150, 30);
            btnExportDetailedReport.Click += btnExportDetailedReport_Click;
            
            // Progress bar
            ProgressBarReport = new ProgressBar();
            ProgressBarReport.Location = new Point(350, 100);
            ProgressBarReport.Size = new Size(200, 30);
            ProgressBarReport.Style = ProgressBarStyle.Marquee;
            ProgressBarReport.Visible = false;
            
            // Summary panel
            Panel summaryPanel = new Panel();
            summaryPanel.Location = new Point(20, 150);
            summaryPanel.Size = new Size(600, 60);
            summaryPanel.BackColor = Color.AliceBlue;
            summaryPanel.BorderStyle = BorderStyle.FixedSingle;
            
            Label lblTotalRevenueTitle = new Label();
            lblTotalRevenueTitle.Text = "Total Revenue:";
            lblTotalRevenueTitle.Location = new Point(20, 10);
            lblTotalRevenueTitle.Size = new Size(100, 20);
            lblTotalRevenueTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            
            lblTotalRevenue = new Label();
            lblTotalRevenue.Text = "$0.00";
            lblTotalRevenue.Location = new Point(20, 30);
            lblTotalRevenue.Size = new Size(150, 20);
            lblTotalRevenue.Font = new Font("Segoe UI", 10);
            
            Label lblOrderCountTitle = new Label();
            lblOrderCountTitle.Text = "Order Count:";
            lblOrderCountTitle.Location = new Point(200, 10);
            lblOrderCountTitle.Size = new Size(100, 20);
            lblOrderCountTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            
            lblOrderCount = new Label();
            lblOrderCount.Text = "0";
            lblOrderCount.Location = new Point(200, 30);
            lblOrderCount.Size = new Size(150, 20);
            lblOrderCount.Font = new Font("Segoe UI", 10);
            
            Label lblAverageOrderTitle = new Label();
            lblAverageOrderTitle.Text = "Average Order:";
            lblAverageOrderTitle.Location = new Point(380, 10);
            lblAverageOrderTitle.Size = new Size(100, 20);
            lblAverageOrderTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            
            lblAverageOrder = new Label();
            lblAverageOrder.Text = "$0.00";
            lblAverageOrder.Location = new Point(380, 30);
            lblAverageOrder.Size = new Size(150, 20);
            lblAverageOrder.Font = new Font("Segoe UI", 10);
            
            summaryPanel.Controls.Add(lblTotalRevenueTitle);
            summaryPanel.Controls.Add(lblTotalRevenue);
            summaryPanel.Controls.Add(lblOrderCountTitle);
            summaryPanel.Controls.Add(lblOrderCount);
            summaryPanel.Controls.Add(lblAverageOrderTitle);
            summaryPanel.Controls.Add(lblAverageOrder);
            
            // Report grid
            DataGridViewReport = new DataGridView();
            DataGridViewReport.Location = new Point(20, 230);
            DataGridViewReport.Size = new Size(600, 300);
            DataGridViewReport.AllowUserToAddRows = false;
            DataGridViewReport.ReadOnly = true;
            DataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // Add all controls to the form
            this.Controls.Add(lblDateRange);
            this.Controls.Add(DateTimePickerStartDate);
            this.Controls.Add(lblTo);
            this.Controls.Add(DateTimePickerEndDate);
            this.Controls.Add(lblReportType);
            this.Controls.Add(cmbReportType);
            this.Controls.Add(btnGenerateSalesReport);
            this.Controls.Add(btnExportDetailedReport);
            this.Controls.Add(ProgressBarReport);
            this.Controls.Add(summaryPanel);
            this.Controls.Add(DataGridViewReport);
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
            string? task = e.Argument as string;
            DateTime startDate = DateTimePickerStartDate.Value;
            DateTime endDate = DateTimePickerEndDate.Value;
            
            if (task == "GenerateSalesReport")
            {
                DataTable reportData;
                
                // Get report based on selected type
                switch (cmbReportType.SelectedIndex)
                {
                    case 0: // Sales Summary
                    default:
                        reportData = ReportManager.GenerateSalesReport(startDate, endDate);
                        break;
                    case 1: // Product Sales
                    case 2: // Customer Orders
                        reportData = ReportManager.GenerateDetailedSalesReport(startDate, endDate);
                        break;
                }
                
                e.Result = new object[] { "Report", reportData };
            }
            else if (task == "ExportDetailedReport")
            {
                var reportData = ReportManager.GenerateDetailedSalesReport(startDate, endDate);
                
                string fileName = $"SalesReport_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}.csv";
                
                using (var writer = new StringWriter())
                {
                    // Write header
                    for (int i = 0; i < reportData.Columns.Count; i++)
                    {
                        writer.Write(reportData.Columns[i].ColumnName);
                        if (i < reportData.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();
                    
                    // Write data rows
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
                    
                    e.Result = new object[] { "Export", fileName, writer.ToString() };
                }
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
            
            if (e.Result is object[] result)
            {
                if (result[0] is string actionType)
                {
                    if (actionType == "Report" && result[1] is DataTable reportData)
                    {
                        DataGridViewReport.DataSource = reportData;
                        UpdateSummary(reportData);
                    }
                    else if (actionType == "Export" && result[1] is string fileName && result[2] is string fileContent)
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
                int totalAmountColumnIndex = -1;
                foreach (DataColumn column in reportData.Columns)
                {
                    if (column.ColumnName == "TotalAmount")
                    {
                        totalAmountColumnIndex = reportData.Columns.IndexOf(column);
                        break;
                    }
                }
                
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
    }
}
