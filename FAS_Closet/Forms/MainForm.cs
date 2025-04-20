using System;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class MainForm : Form
    {
        private UcProductManagement? ucProductManagement = null;
        private UcInventoryManagement? ucInventoryManagement = null;
        private UcOrderManagement? ucOrderManagement = null;
        private UcCustomerManagement? ucCustomerManagement = null;
        private UcRevenueReport? ucRevenueReport = null;
        private UcDashboard? ucDashboard = null;
        private UcNotificationSettings? ucNotificationSettings = null;
        
        // Add reference to current active button
        private Button? currentActiveButton = null;
        private Label? navigationLabel = null;

        public MainForm(User user)
        {
            InitializeComponent();

            // Store the current user
            CurrentUser = user;
            lblWelcome.Text = "Welcome, " + user.Name;

            // Add warehouse selector
            InitializeWarehouseSelector();
            
            // Create navigation label to show current section
            navigationLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(50, 50, 50),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25
            };
            contentPanel.Controls.Add(navigationLabel);

            // Check for low stock items on startup (but don't show toast notifications)
            NotificationManager.CheckAndSendLowStockNotifications();

            btnDashboard_Click(this, EventArgs.Empty);
        }

        private User CurrentUser { get; set; }
        private int CurrentWarehouseID { get; set; } = 1;

        private void InitializeWarehouseSelector()
        {
            // Cấu hình warehouse dropdown
            cmbWarehouses.DropDownStyle = ComboBoxStyle.DropDownList;

            // Load warehouses for this user
        }

        // Sự kiện cho nút Logout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Method to set active button appearance
        private void SetActiveButton(Button button, string sectionName)
        {
            // Reset current active button if exists
            if (currentActiveButton != null)
            {
                // Reset về màu mặc định của menu
                currentActiveButton.BackColor = Color.FromArgb(248, 249, 250);
                currentActiveButton.ForeColor = Color.FromArgb(52, 58, 64);
                currentActiveButton.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            }
            
            // Set new active button
            currentActiveButton = button;
            currentActiveButton.BackColor = Color.FromArgb(0, 123, 255);
            currentActiveButton.ForeColor = Color.White;
            currentActiveButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            
            // Update navigation label
            if (navigationLabel != null)
            {
                navigationLabel.Text = "Đang xem: " + sectionName;
            }
        }
        
        // Các sự kiện điều hướng load các UserControl và cập nhật thanh tính năng
        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnProductManagement, "Quản lý Sản phẩm");
            UpdateFeatureToolbar(new string[] { "Thêm", "Sửa", "Xóa" });

            if (ucProductManagement == null)
            {
                ucProductManagement = new UcProductManagement();
                // KHÔNG gán lại btnAdd, btnEdit, btnDelete ở đây nữa
            }

            ucProductManagement.LoadCategories();

            LoadUserControl(ucProductManagement);
        }

        // Make sure these handlers correctly call the product management methods
        private void HandleProductAdd(object? sender, EventArgs e)
        {
            if (ucProductManagement != null)
                ucProductManagement.btnAdd_Click(sender ?? this, e);
            else
                MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
        }

        private void HandleProductEdit(object? sender, EventArgs e)
        {
            if (ucProductManagement != null)
                ucProductManagement.btnEdit_Click(sender ?? this, e);
            else
                MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
        }

        private void HandleProductDelete(object? sender, EventArgs e)
        {
            if (ucProductManagement != null)
                ucProductManagement.btnDelete_Click(sender ?? this, e);
            else
                MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
        }

        private void btnInventoryManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnInventoryManagement, "Quản lý Kho hàng");
            UpdateFeatureToolbar(new string[] { "Cập nhật tồn kho" });

            if (ucInventoryManagement == null)
            {
                ucInventoryManagement = new UcInventoryManagement();
            }

            ucInventoryManagement.LoadCategories();

            LoadUserControl(ucInventoryManagement);
        }

        private void btnOrderManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnOrderManagement, "Quản lý Đơn hàng");
            UpdateFeatureToolbar(new string[] { "Tạo đơn hàng", "In hóa đơn" });

            if (ucOrderManagement == null)
            {
                ucOrderManagement = new UcOrderManagement();
            }
            ucOrderManagement.LoadProducts();
            ucOrderManagement.LoadCustomers();

            LoadUserControl(ucOrderManagement);
        }

        private void btnCustomerManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnCustomerManagement, "Quản lý Khách hàng");
            UpdateFeatureToolbar(new string[] { "Thêm khách hàng", "Chỉnh sửa thông tin khách hàng", "Xóa khách hàng", "Làm mới" });

            if (ucCustomerManagement == null)
            {
                ucCustomerManagement = new UcCustomerManagement();

                ucCustomerManagement.btnAdd = new Button();
                ucCustomerManagement.btnAdd.Text = "Thêm khách hàng";
                ucCustomerManagement.btnDelete = new Button();
                ucCustomerManagement.btnDelete.Text = "Xóa khách hàng";
                ucCustomerManagement.btnRefresh = new Button();
                ucCustomerManagement.btnRefresh.Text = "Làm mới";
            }

            LoadUserControl(ucCustomerManagement);
        }

        private void HandleCustomerAdd(object? sender, EventArgs e)
        {
            if (ucCustomerManagement != null)
                ucCustomerManagement.btnAdd.PerformClick();
            else
                MessageBox.Show("Vui lòng chọn Quản lý khách hàng trước.");
        }

        private void HandleCustomerEdit(object? sender, EventArgs e)
        {
            if (ucCustomerManagement != null)
                ucCustomerManagement.btnEdit.PerformClick();
            else
                MessageBox.Show("Vui lòng chọn khách hàng cần chỉnh sửa.");
        }

        private void HandleCustomerDelete(object? sender, EventArgs e)
        {
            if (ucCustomerManagement != null)
                ucCustomerManagement.btnDelete.PerformClick();
            else
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.");
        }

        private void HandleCustomerRefresh(object? sender, EventArgs e)
        {
            if (ucCustomerManagement != null)
                ucCustomerManagement.btnRefresh.PerformClick();
            else
                MessageBox.Show("Vui lòng chọn Quản lý khách hàng trước.");
        }

        private void btnRevenueReport_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRevenueReport, "Báo cáo Doanh thu");
            // Removed duplicated toolbar buttons since UC_RevenueReport already has these buttons
            UpdateFeatureToolbar(new string[] { });

            if (ucRevenueReport == null)
            {
                ucRevenueReport = new UcRevenueReport();
                // Gỡ bỏ việc tạo lại các control vì đã được khởi tạo trong InitializeComponent
                // của UC_RevenueReport.Designer.cs
            }

            LoadUserControl(ucRevenueReport);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard, "Bảng điều khiển");
            UpdateFeatureToolbar(new string[] { "Sản phẩm bán chạy" });

            if (ucDashboard == null)
            {
                ucDashboard = new UcDashboard();
            }

            // First add the dashboard to the form 
            LoadUserControl(ucDashboard);
            
            // Then load the data after it's visible in the UI
            ucDashboard.LoadDashboardData();
        }

        private void btnNotificationSettings_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNotificationSettings, "Cài đặt Thông báo");
            UpdateFeatureToolbar(new string[] { "Configure Notifications", "View Notification Logs" });

            if (ucNotificationSettings == null)
            {
                ucNotificationSettings = new UcNotificationSettings();
            }

            LoadUserControl(ucNotificationSettings);
        }

        // Hàm phụ trợ để load một UserControl vào contentPanel
        private void LoadUserControl(UserControl userControl)
        {
            contentPanel.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(userControl);
        }

        // Refactored method to reduce cognitive complexity
        private void UpdateFeatureToolbar(string[] features)
        {
            featureToolbarPanel.Controls.Clear();

            foreach (var feature in features)
            {
                var btn = CreateFeatureButton(feature);
                featureToolbarPanel.Controls.Add(btn);
            }
        }

        private Button CreateFeatureButton(string feature)
        {
            var btn = new Button
            {
                Text = feature,
                AutoSize = true,
                Margin = new Padding(5),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Assign event handler based on feature
            AssignFeatureButtonHandler(btn, feature);
            return btn;
        }

        private void AssignFeatureButtonHandler(Button btn, string feature)
        {
            switch (feature)
            {
                case "Thêm":
                    btn.Click += (s, e) => HandleProductAdd(s, e);
                    break;
                case "Sửa":
                    btn.Click += (s, e) => HandleProductEdit(s, e);
                    break;
                case "Xóa":
                    btn.Click += (s, e) => HandleProductDelete(s, e);
                    break;
                case "Tạo đơn hàng":
                    btn.Click += (s, e) => HandleOrderCreate(s, e);
                    break;
                case "In hóa đơn":
                    btn.Click += (s, e) => HandlePrintInvoice(s, e);
                    break;
                case "Cập nhật tồn kho":
                    btn.Click += (s, e) => HandleInventoryUpdate(s, e);
                    break;
                case "Thêm khách hàng":
                    btn.Click += (s, e) => HandleCustomerAdd(s, e);
                    break;
                case "Chỉnh sửa thông tin khách hàng":
                    btn.Click += (s, e) => HandleCustomerEdit(s, e);
                    break;
                case "Xóa khách hàng":
                    btn.Click += (s, e) => HandleCustomerDelete(s, e);
                    break;
                case "Làm mới":
                    btn.Click += (s, e) => HandleCustomerRefresh(s, e);
                    break;
                case "Sản phẩm bán chạy":
                    btn.Click += (s, e) => ShowBestSellingProductsDetail(s, e);
                    break;
                case "Làm mới báo cáo":
                    btn.Click += (s, e) => {
                        if (ucRevenueReport != null)
                            ucRevenueReport.btnRefresh.PerformClick();
                    };
                    break;
                case "Xuất báo cáo":
                    btn.Click += (s, e) => {
                        if (ucRevenueReport != null)
                            ucRevenueReport.btnExport.PerformClick();
                    };
                    break;
                default:
                    btn.Click += (s, e) => MessageBox.Show("Chức năng: " + feature);
                    break;
            }
        }

        private void HandleOrderCreate(object? sender, EventArgs e)
        {
            if (ucOrderManagement != null)
                ucOrderManagement.btnCreateOrder_Click(sender ?? this, e);
            else
                MessageBox.Show("Vui lòng chọn Quản lý đơn hàng trước.");
        }        
        
        private void HandlePrintInvoice(object? sender, EventArgs e)
        {
            if (ucOrderManagement != null)
                ucOrderManagement.btnPrintInvoice_Click(sender ?? this, e);
            else
                MessageBox.Show("Vui lòng chọn Quản lý đơn hàng trước.");
        }

        private void HandleInventoryUpdate(object? sender, EventArgs e)
        {
            if (ucInventoryManagement != null)
                ucInventoryManagement.btnUpdateStock_Click(sender ?? this, e);
            else
                MessageBox.Show("Vui lòng chọn Quản lý kho hàng trước.");
        }

        // Add this public navigation method for low stock items that can be accessed from other controls
        public void NavigateToInventoryManagement(int productId = 0)
        {
            btnInventoryManagement_Click(this, EventArgs.Empty);

            // If a product ID was provided, set it in the inventory management UI
            if (productId > 0 && ucInventoryManagement != null)
            {
                ucInventoryManagement.SelectProductInComboBox(productId);
                ucInventoryManagement.BringToFront();
            }
        }
        
        // Method to refresh the dashboard when needed from other forms
        public void RefreshDashboard()
        {
            // Only refresh if dashboard exists and is visible
            if (ucDashboard != null && ucDashboard.Visible)
            {
                ucDashboard.LoadDashboardData();
            }
        }

        private void ShowBestSellingProductsDetail(object? sender, EventArgs e)
        {
            if (ucDashboard == null)
            {
                MessageBox.Show("Chưa sẵn sàng hiển thị dữ liệu sản phẩm bán chạy.");
                return;
            }

            try
            {
                var bestSellingProducts = ReportManager.GetBestSellingProducts(null, null);
                if (bestSellingProducts == null || bestSellingProducts.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu sản phẩm bán chạy.");
                    return;
                }

                // Create a Form to display detailed information
                Form detailForm = new Form
                {
                    Text = "Chi tiết sản phẩm bán chạy",
                    Size = new Size(800, 600),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                // Create DataGridView with detailed information
                DataGridView dgvDetailedProducts = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    MultiSelect = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    RowHeadersVisible = false
                };

                // Style the DataGridView
                dgvDetailedProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
                dgvDetailedProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvDetailedProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvDetailedProducts.ColumnHeadersHeight = 40;
                dgvDetailedProducts.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                dgvDetailedProducts.RowTemplate.Height = 35;

                // Add columns
                dgvDetailedProducts.Columns.Add("ProductID", "Mã SP");
                dgvDetailedProducts.Columns.Add("ProductName", "Tên Sản Phẩm");
                dgvDetailedProducts.Columns.Add("CategoryName", "Danh Mục");
                dgvDetailedProducts.Columns.Add("Price", "Giá Bán");
                dgvDetailedProducts.Columns.Add("TotalQuantity", "SL Đã Bán");
                dgvDetailedProducts.Columns.Add("Revenue", "Doanh Thu");
                dgvDetailedProducts.Columns.Add("CurrentStock", "Tồn Kho");

                // Configure column formatting
                dgvDetailedProducts.Columns["ProductID"].Width = 70;
                dgvDetailedProducts.Columns["Price"].DefaultCellStyle.Format = "N0";
                dgvDetailedProducts.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetailedProducts.Columns["TotalQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetailedProducts.Columns["Revenue"].DefaultCellStyle.Format = "N0";
                dgvDetailedProducts.Columns["Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetailedProducts.Columns["CurrentStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Add alternating row color
                dgvDetailedProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);

                // Add the data
                foreach (var product in bestSellingProducts)
                {
                    var currentStock = ProductManager.GetProductById(product.ProductID)?.Stock ?? 0;
                    dgvDetailedProducts.Rows.Add(
                        product.ProductID,
                        product.ProductName,
                        product.CategoryName,
                        product.Price,
                        product.TotalQuantity,
                        product.Revenue,
                        currentStock
                    );
                }

                // Create a title label
                Label titleLabel = new Label
                {
                    Text = "Chi tiết sản phẩm bán chạy",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 123, 255),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 40
                };

                // Create DateTimePicker controls for filtering by date range
                Label lblDateRange = new Label
                {
                    Text = "Chọn khoảng thời gian:",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(10, 50),
                    Size = new Size(150, 25)
                };

                DateTimePicker dtpStartDate = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(160, 50),
                    Size = new Size(120, 25)
                };

                Label lblTo = new Label
                {
                    Text = "đến",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(290, 50),
                    Size = new Size(40, 25),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                DateTimePicker dtpEndDate = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(340, 50),
                    Size = new Size(120, 25)
                };

                // Set default date range (last 30 days)
                dtpStartDate.Value = DateTime.Now.AddDays(-30);
                dtpEndDate.Value = DateTime.Now;

                // Create filter button
                Button btnFilter = new Button
                {
                    Text = "Lọc",
                    Font = new Font("Segoe UI", 9),
                    Location = new Point(470, 50),
                    Size = new Size(80, 25),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(0, 123, 255),
                    ForeColor = Color.White
                };

                // Create export button
                Button btnExport = new Button
                {
                    Text = "Xuất Excel",
                    Font = new Font("Segoe UI", 9),
                    Location = new Point(560, 50),
                    Size = new Size(100, 25),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White
                };

                // Create Panel for controls
                Panel controlsPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 85
                };

                controlsPanel.Controls.Add(lblDateRange);
                controlsPanel.Controls.Add(dtpStartDate);
                controlsPanel.Controls.Add(lblTo);
                controlsPanel.Controls.Add(dtpEndDate);
                controlsPanel.Controls.Add(btnFilter);
                controlsPanel.Controls.Add(btnExport);

                // Create close button
                Button btnClose = new Button
                {
                    Text = "Đóng",
                    Font = new Font("Segoe UI", 10),
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White
                };

                btnClose.Click += (s, ev) => detailForm.Close();
                btnFilter.Click += (s, ev) => {
                    try
                    {
                        var filteredProducts = ReportManager.GetBestSellingProducts(dtpStartDate.Value, dtpEndDate.Value);
                        dgvDetailedProducts.Rows.Clear();
                        
                        if (filteredProducts == null || filteredProducts.Count == 0)
                        {
                            MessageBox.Show("Không có dữ liệu sản phẩm bán chạy trong khoảng thời gian này.");
                            return;
                        }
                        
                        foreach (var product in filteredProducts)
                        {
                            var currentStock = ProductManager.GetProductById(product.ProductID)?.Stock ?? 0;
                            dgvDetailedProducts.Rows.Add(
                                product.ProductID,
                                product.ProductName,
                                product.CategoryName,
                                product.Price,
                                product.TotalQuantity,
                                product.Revenue,
                                currentStock
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}");
                    }
                };

                btnExport.Click += (s, ev) => {
                    MessageBox.Show("Tính năng xuất Excel sẽ được phát triển trong phiên bản tới.");
                };

                // Add all controls to the form
                detailForm.Controls.Add(dgvDetailedProducts);
                detailForm.Controls.Add(btnClose);
                detailForm.Controls.Add(controlsPanel);
                detailForm.Controls.Add(titleLabel);

                // Show the form
                detailForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
