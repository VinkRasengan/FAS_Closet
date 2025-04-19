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
            
            // Thiết lập khoảng thời gian mặc định là từ 2 năm trước đến hiện tại để đảm bảo bao gồm tất cả đơn hàng
            DateTimePickerStartDate.Value = new DateTime(DateTime.Now.Year - 2, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTimePickerEndDate.Value = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            
            // Wire up the button click events
            btnExport.Click += btnExportDetailedReport_Click;
            btnRefresh.Click += btnRefresh_Click;
            
            // Set up the report type dropdown
            cmbReportType.SelectedIndexChanged += CmbReportType_SelectedIndexChanged;
            
            // Cài đặt style ban đầu cho DataGridView
            SetupDataGridViewStyle();
            
            // Tự động tải dữ liệu khi form được khởi tạo (sau khi tất cả control đã load xong)
            this.Load += (s, e) => {
                // Hiển thị dữ liệu mẫu ngay để DataGridView luôn có nội dung
                ShowSampleData();
                
                // Tải dữ liệu thực từ cơ sở dữ liệu
                if (!backgroundWorker.IsBusy)
                {
                    ProgressBarReport.Visible = true;
                    backgroundWorker.RunWorkerAsync("GenerateSalesReport");
                }
            };
        }
        
        private void SetupDataGridViewStyle()
        {
            // Thiết lập style cơ bản cho DataGridView trước khi có dữ liệu
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
        
        private void CmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hiển thị dữ liệu mẫu ngay lập tức khi thay đổi loại báo cáo
            ShowSampleData();
            
            // Generate report when the report type changes
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
            // Reset date pickers để mở rộng khoảng thời gian
            DateTimePickerStartDate.Value = new DateTime(DateTime.Now.Year - 2, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTimePickerEndDate.Value = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            
            // Hiện thanh tiến trình
            ProgressBarReport.Visible = true;
            
            // Đảm bảo DataGridView luôn hiển thị bằng cách hiển thị dữ liệu mẫu ngay lập tức
            ShowSampleData();
            
            // Tải dữ liệu thực
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync("GenerateSalesReport");
            }
            else
            {
                MessageBox.Show("Hệ thống đang xử lý yêu cầu trước đó, vui lòng đợi trong giây lát",
                    "Đang xử lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ProgressBarReport.Visible = false;
            }
        }

        // Thêm phương thức mới để thiết lập các cột mặc định trong trường hợp không có dữ liệu
        private void SetupDefaultColumns()
        {
            DataGridViewReport.Columns.Clear();
            
            // Tạo cấu trúc cột cơ bản dựa trên loại báo cáo hiện tại
            switch (cmbReportType.SelectedIndex)
            {
                case 1: // Báo cáo theo sản phẩm
                    DataGridViewReport.Columns.Add("ProductID", "Mã sản phẩm");
                    DataGridViewReport.Columns.Add("ProductName", "Tên sản phẩm");
                    DataGridViewReport.Columns.Add("Quantity", "Số lượng");
                    DataGridViewReport.Columns.Add("Revenue", "Doanh thu");
                    DataGridViewReport.Columns.Add("Profit", "Lợi nhuận");
                    break;
                    
                case 2: // Báo cáo theo khách hàng
                    DataGridViewReport.Columns.Add("CustomerID", "Mã khách hàng");
                    DataGridViewReport.Columns.Add("CustomerName", "Tên khách hàng");
                    DataGridViewReport.Columns.Add("OrderCount", "Số đơn hàng");
                    DataGridViewReport.Columns.Add("TotalSpend", "Tổng chi tiêu");
                    break;
                    
                default: // Báo cáo tổng quan
                    DataGridViewReport.Columns.Add("OrderDate", "Ngày");
                    DataGridViewReport.Columns.Add("OrderID", "Mã đơn hàng");
                    DataGridViewReport.Columns.Add("CustomerName", "Khách hàng");
                    DataGridViewReport.Columns.Add("TotalAmount", "Tổng tiền");
                    DataGridViewReport.Columns.Add("PaymentMethod", "Thanh toán");
                    break;
            }
            
            // Áp dụng định dạng cho các cột
            FormatDataGridView();
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
        
        private void UpdateDataGridViewWithReport(DataTable reportData)
        {
            if (reportData == null || reportData.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu báo cáo trong khoảng thời gian đã chọn", 
                    "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Hiển thị dữ liệu mẫu để không để trống bảng
                ShowSampleData();
                return;
            }
            
            // Cập nhật DataSource với dữ liệu thực từ báo cáo
            DataGridViewReport.DataSource = null; // Clear trước để tránh lỗi khi thay đổi cấu trúc DataTable
            DataGridViewReport.DataSource = reportData;
            
            // Áp dụng định dạng và tính tổng
            FormatDataGridView();
            UpdateSummary(reportData);
            
            // Cập nhật trạng thái để hiển thị rằng đây là dữ liệu thực
            lblReportStatus.Text = $"✅ Đang hiển thị dữ liệu thực từ {DateTimePickerStartDate.Value:dd/MM/yyyy} đến {DateTimePickerEndDate.Value:dd/MM/yyyy}";
            lblReportStatus.ForeColor = Color.FromArgb(40, 167, 69); // Màu xanh lá
            lblReportStatus.Visible = true;
        }
        
        private void FormatDataGridView()
        {
            // Format the data grid view with proper column styles
            if (DataGridViewReport.Columns.Count > 0)
            {
                // Thiết lập thuộc tính cơ bản cho DataGridView
                DataGridViewReport.EnableHeadersVisualStyles = false;
                DataGridViewReport.BorderStyle = BorderStyle.None;
                DataGridViewReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
                DataGridViewReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                DataGridViewReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 215, 229);
                DataGridViewReport.DefaultCellStyle.SelectionForeColor = Color.Black;
                DataGridViewReport.BackgroundColor = Color.White;
                DataGridViewReport.RowHeadersVisible = false;
                
                // Định dạng tiêu đề cột
                DataGridViewReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 150, 190);
                DataGridViewReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DataGridViewReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
                DataGridViewReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DataGridViewReport.ColumnHeadersHeight = 40;
                
                // Font chữ cơ bản cho nội dung
                DataGridViewReport.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                
                foreach (DataGridViewColumn column in DataGridViewReport.Columns)
                {
                    // Định dạng cột tiền tệ
                    if (column.HeaderText.Contains("tiền") || column.HeaderText.Contains("thu") || 
                        column.HeaderText.Contains("giá") || column.HeaderText.Contains("chi"))
                    {
                        column.DefaultCellStyle.Format = "N0";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        column.DefaultCellStyle.ForeColor = Color.FromArgb(31, 111, 139);
                        column.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
                    }
                    
                    // Định dạng cột ngày tháng
                    if (column.HeaderText.Contains("Ngày"))
                    {
                        column.DefaultCellStyle.Format = "dd/MM/yyyy";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    
                    // Định dạng cột mã (ID)
                    if (column.HeaderText.Contains("Mã"))
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    
                    // Giới hạn chiều rộng cột và thêm tooltip khi nội dung bị cắt
                    if (column.HeaderText.Contains("Tên") || column.HeaderText.Contains("Email") || 
                        column.HeaderText.Contains("Địa chỉ") || column.HeaderText.Contains("sản phẩm"))
                    {
                        column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        column.MinimumWidth = 150;
                    }
                    
                    // Cột thanh toán
                    if (column.HeaderText.Contains("Thanh toán") || column.HeaderText.Contains("Payment"))
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                
                // Auto resize các cột theo nội dung tốt nhất
                DataGridViewReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                DataGridViewReport.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                
                // Giới hạn chiều rộng tối đa của cột
                foreach (DataGridViewColumn column in DataGridViewReport.Columns)
                {
                    if (column.Width > 250)
                        column.Width = 250;
                }
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
                    // Write the content to the selected file with UTF-8 encoding with BOM
                    // This ensures that Excel and other applications correctly recognize Vietnamese characters
                    File.WriteAllText(saveFileDialog.FileName, fileContent, new System.Text.UTF8Encoding(true));

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

        // Hiển thị dữ liệu mẫu để bảng luôn xuất hiện ngay cả khi không có dữ liệu thực
        private void ShowSampleData()
        {
            try
            {
                // Xóa dữ liệu nguồn hiện tại (nếu có)
                DataGridViewReport.DataSource = null;
                DataGridViewReport.Rows.Clear();
                DataGridViewReport.Columns.Clear();
                
                // Đảm bảo DataGridView được hiển thị
                DataGridViewReport.Visible = true;
                DataGridViewReport.BringToFront();
                
                // Tạo dữ liệu mẫu dựa trên loại báo cáo được chọn
                DataTable sampleData = new DataTable();
                
                switch (cmbReportType.SelectedIndex)
                {
                    case 1: // Báo cáo theo sản phẩm
                        // Thêm cột vào DataTable
                        sampleData.Columns.Add("Mã SP", typeof(string));
                        sampleData.Columns.Add("Tên sản phẩm", typeof(string));
                        sampleData.Columns.Add("Danh mục", typeof(string));
                        sampleData.Columns.Add("Số lượng bán", typeof(int));
                        sampleData.Columns.Add("Số đơn hàng", typeof(int));
                        sampleData.Columns.Add("Doanh thu", typeof(decimal));
                        
                        // Thêm dữ liệu mẫu
                        sampleData.Rows.Add("SP001", "Áo thun nam basic", "Áo", 15, 10, 1500000);
                        sampleData.Rows.Add("SP002", "Quần jeans nữ", "Quần", 8, 7, 2400000);
                        sampleData.Rows.Add("SP003", "Váy đầm dự tiệc", "Váy", 5, 5, 3250000);
                        sampleData.Rows.Add("SP004", "Áo khoác denim", "Áo", 7, 6, 2100000);
                        break;
                        
                    case 2: // Báo cáo theo khách hàng
                        sampleData.Columns.Add("Mã KH", typeof(string));
                        sampleData.Columns.Add("Tên khách hàng", typeof(string));
                        sampleData.Columns.Add("Email", typeof(string));
                        sampleData.Columns.Add("Điện thoại", typeof(string));
                        sampleData.Columns.Add("Tổng chi tiêu", typeof(decimal));
                        
                        // Thêm dữ liệu mẫu
                        sampleData.Rows.Add("KH001", "Nguyễn Văn An", "an@example.com", "0901234567", 2500000);
                        sampleData.Rows.Add("KH002", "Trần Thị Bình", "binh@example.com", "0912345678", 4300000);
                        sampleData.Rows.Add("KH003", "Lê Hoàng Cường", "cuong@example.com", "0923456789", 1800000);
                        sampleData.Rows.Add("KH004", "Phạm Minh Dương", "duong@example.com", "0934567890", 3600000);
                        break;
                        
                    default: // Báo cáo tổng quan
                        sampleData.Columns.Add("Ngày", typeof(DateTime));
                        sampleData.Columns.Add("Mã đơn hàng", typeof(string));
                        sampleData.Columns.Add("Khách hàng", typeof(string));
                        sampleData.Columns.Add("Tổng tiền", typeof(decimal));
                        sampleData.Columns.Add("Thanh toán", typeof(string));
                        
                        // Thêm dữ liệu mẫu
                        sampleData.Rows.Add(DateTime.Now.AddDays(-1), "DH001", "Nguyễn Văn An", 850000, "Tiền mặt");
                        sampleData.Rows.Add(DateTime.Now.AddDays(-2), "DH002", "Trần Thị Bình", 1200000, "Chuyển khoản");
                        sampleData.Rows.Add(DateTime.Now.AddDays(-3), "DH003", "Lê Hoàng Cường", 650000, "Tiền mặt");
                        sampleData.Rows.Add(DateTime.Now.AddDays(-4), "DH004", "Phạm Minh Dương", 950000, "Momo");
                        break;
                }
                
                // Cập nhật DataSource với dữ liệu mẫu
                DataGridViewReport.DataSource = sampleData;
                
                // Định dạng bảng và cập nhật tóm tắt
                FormatDataGridView();
                UpdateSummary(sampleData);
                
                // Hiển thị dialog để debug
                MessageBox.Show($"Đã tạo dữ liệu mẫu với {sampleData.Rows.Count} dòng và {sampleData.Columns.Count} cột", "Debug");
                
                // In console để debug
                Console.WriteLine($"Hiển thị dữ liệu mẫu với {sampleData.Rows.Count} dòng và {sampleData.Columns.Count} cột");
                Console.WriteLine($"DataGridView có {DataGridViewReport.RowCount} dòng và {DataGridViewReport.ColumnCount} cột");
                
                // Thêm nhãn để nhận biết đây là dữ liệu mẫu
                lblReportStatus.Text = "⚠️ Đang hiển thị dữ liệu mẫu. Vui lòng chọn khoảng thời gian và nhấn 'Làm mới' để xem dữ liệu thực.";
                lblReportStatus.Visible = true;
                lblReportStatus.ForeColor = Color.FromArgb(255, 128, 0);
                
                // Đảm bảo DataGridView được refresh
                DataGridViewReport.Refresh();
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị dữ liệu mẫu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi hiển thị dữ liệu mẫu: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
