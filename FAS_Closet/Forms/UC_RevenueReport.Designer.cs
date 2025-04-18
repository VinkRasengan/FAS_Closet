namespace FASCloset.Forms
{
    partial class UcRevenueReport
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            
            // Date range selection
            Label lblDateRange = new Label();
            lblDateRange.Text = "Chọn khoảng thời gian:";
            lblDateRange.Location = new Point(20, 20);
            lblDateRange.Size = new Size(120, 25);
            lblDateRange.TextAlign = ContentAlignment.MiddleLeft;
            
            DateTimePickerStartDate = new DateTimePicker();
            DateTimePickerStartDate.Location = new Point(150, 20);
            DateTimePickerStartDate.Size = new Size(150, 25);
            DateTimePickerStartDate.Format = DateTimePickerFormat.Short;
            
            Label lblTo = new Label();
            lblTo.Text = "đến";
            lblTo.Location = new Point(310, 20);
            lblTo.Size = new Size(30, 25);
            lblTo.TextAlign = ContentAlignment.MiddleCenter;
            
            DateTimePickerEndDate = new DateTimePicker();
            DateTimePickerEndDate.Location = new Point(350, 20);
            DateTimePickerEndDate.Size = new Size(150, 25);
            DateTimePickerEndDate.Format = DateTimePickerFormat.Short;
            
            // Report type selection
            Label lblReportType = new Label();
            lblReportType.Text = "Loại báo cáo:";
            lblReportType.Location = new Point(20, 60);
            lblReportType.Size = new Size(120, 25);
            lblReportType.TextAlign = ContentAlignment.MiddleLeft;
            
            cmbReportType = new ComboBox();
            cmbReportType.Location = new Point(150, 60);
            cmbReportType.Size = new Size(250, 25);
            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportType.Items.AddRange(new object[] { "Tổng quan doanh số", "Báo cáo bán hàng theo sản phẩm", "Báo cáo bán hàng theo khách hàng" });
            cmbReportType.SelectedIndex = 0;
            
            // Chỉ giữ lại nút làm mới và xuất báo cáo
            btnRefresh = new Button();
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.BackColor = Color.FromArgb(40, 167, 69); // Màu xanh lá tương tự Bootstrap success
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Text = "🔄 Làm mới báo cáo";
            btnRefresh.Location = new Point(320, 100);
            btnRefresh.Size = new Size(180, 35);
            
            btnExport = new Button();
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.BackColor = Color.FromArgb(0, 123, 255); // Màu xanh tương tự Bootstrap primary
            btnExport.ForeColor = Color.White;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Cursor = Cursors.Hand;
            btnExport.Text = "📊 Xuất báo cáo"; // Đã sửa từ "Xuất báo" thành "Xuất báo cáo"
            btnExport.Location = new Point(150, 100);
            btnExport.Size = new Size(160, 35);
            
            // Progress bar
            ProgressBarReport = new ProgressBar();
            ProgressBarReport.Location = new Point(510, 100);
            ProgressBarReport.Size = new Size(180, 35);
            ProgressBarReport.Style = ProgressBarStyle.Marquee;
            ProgressBarReport.Visible = false;
            
            // Report status label (NEW)
            lblReportStatus = new Label();
            lblReportStatus.Location = new Point(20, 205);
            lblReportStatus.Size = new Size(700, 20);
            lblReportStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblReportStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblReportStatus.ForeColor = Color.FromArgb(255, 128, 0);
            lblReportStatus.Text = "";
            lblReportStatus.Visible = false;
            
            // Summary panel
            Panel summaryPanel = new Panel();
            summaryPanel.Location = new Point(20, 150);
            summaryPanel.Size = new Size(700, 60);
            summaryPanel.BackColor = Color.AliceBlue;
            summaryPanel.BorderStyle = BorderStyle.FixedSingle;
            
            Label lblTotalRevenueTitle = new Label();
            lblTotalRevenueTitle.Text = "Tổng doanh thu:";
            lblTotalRevenueTitle.Location = new Point(20, 10);
            lblTotalRevenueTitle.Size = new Size(120, 20);
            lblTotalRevenueTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            
            lblTotalRevenue = new Label();
            lblTotalRevenue.Text = "0 đ";
            lblTotalRevenue.Location = new Point(20, 30);
            lblTotalRevenue.Size = new Size(150, 20);
            lblTotalRevenue.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalRevenue.ForeColor = Color.FromArgb(0, 123, 255);
            
            Label lblOrderCountTitle = new Label();
            lblOrderCountTitle.Text = "Số đơn hàng:";
            lblOrderCountTitle.Location = new Point(200, 10);
            lblOrderCountTitle.Size = new Size(120, 20);
            lblOrderCountTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            
            lblOrderCount = new Label();
            lblOrderCount.Text = "0";
            lblOrderCount.Location = new Point(200, 30);
            lblOrderCount.Size = new Size(150, 20);
            lblOrderCount.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblOrderCount.ForeColor = Color.FromArgb(40, 167, 69);
            
            Label lblAverageOrderTitle = new Label();
            lblAverageOrderTitle.Text = "Trung bình đơn:";
            lblAverageOrderTitle.Location = new Point(380, 10);
            lblAverageOrderTitle.Size = new Size(120, 20);
            lblAverageOrderTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            
            lblAverageOrder = new Label();
            lblAverageOrder.Text = "0 đ";
            lblAverageOrder.Location = new Point(380, 30);
            lblAverageOrder.Size = new Size(150, 20);
            lblAverageOrder.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblAverageOrder.ForeColor = Color.FromArgb(108, 117, 125);
            
            summaryPanel.Controls.Add(lblTotalRevenueTitle);
            summaryPanel.Controls.Add(lblTotalRevenue);
            summaryPanel.Controls.Add(lblOrderCountTitle);
            summaryPanel.Controls.Add(lblOrderCount);
            summaryPanel.Controls.Add(lblAverageOrderTitle);
            summaryPanel.Controls.Add(lblAverageOrder);
            
            // Report grid với màu sắc mới và đảm bảo luôn hiển thị
            DataGridViewReport = new DataGridView();
            DataGridViewReport.Location = new Point(20, 230);
            DataGridViewReport.Size = new Size(700, 350);
            DataGridViewReport.AllowUserToAddRows = false;
            DataGridViewReport.ReadOnly = true;
            DataGridViewReport.Visible = true; // Đảm bảo luôn hiển thị
            DataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewReport.BorderStyle = BorderStyle.None;
            DataGridViewReport.BackgroundColor = Color.White;
            DataGridViewReport.GridColor = Color.FromArgb(230, 230, 230);
            DataGridViewReport.EnableHeadersVisualStyles = false;
            DataGridViewReport.RowHeadersVisible = false;
            
            // Định dạng tiêu đề cột
            DataGridViewReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 150, 190);
            DataGridViewReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridViewReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            DataGridViewReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridViewReport.ColumnHeadersHeight = 40;
            
            // Thiết lập dòng
            DataGridViewReport.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            DataGridViewReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            DataGridViewReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            DataGridViewReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 215, 229);
            DataGridViewReport.DefaultCellStyle.SelectionForeColor = Color.Black;
            DataGridViewReport.RowTemplate.Height = 35;
            DataGridViewReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Add all controls to the form
            this.Controls.Add(lblDateRange);
            this.Controls.Add(DateTimePickerStartDate);
            this.Controls.Add(lblTo);
            this.Controls.Add(DateTimePickerEndDate);
            this.Controls.Add(lblReportType);
            this.Controls.Add(cmbReportType);
            this.Controls.Add(btnExport);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(ProgressBarReport);
            this.Controls.Add(summaryPanel);
            this.Controls.Add(lblReportStatus);
            this.Controls.Add(DataGridViewReport);
            
            // Đặt kích thước cho user control
            this.Size = new Size(750, 600);
        }

        public DateTimePicker DateTimePickerStartDate;
        public DateTimePicker DateTimePickerEndDate;
        public DataGridView DataGridViewReport;
        public ProgressBar ProgressBarReport;
        public ComboBox cmbReportType;
        public Label lblTotalRevenue;
        public Label lblOrderCount;
        public Label lblAverageOrder;
        public Button btnExport;
        public Button btnRefresh;
        private Label lblReportStatus;
    }
}
