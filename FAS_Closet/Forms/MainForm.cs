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

        public MainForm(User user)
        {
            InitializeComponent();

            // Store the current user
            CurrentUser = user;
            lblWelcome.Text = "Welcome, " + user.Name;

            // Add warehouse selector
            InitializeWarehouseSelector();

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

        // Các sự kiện điều hướng load các UserControl và cập nhật thanh tính năng
        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Thêm", "Sửa", "Xóa", "Phân loại" });

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
            UpdateFeatureToolbar(new string[] { "Cập nhật tồn kho", "Cảnh báo sắp hết" });

            if (ucInventoryManagement == null)
            {
                ucInventoryManagement = new UcInventoryManagement();
                ucInventoryManagement.dataGridViewLowStock = new DataGridView();
            }

            ucInventoryManagement.LoadCategories();

            LoadUserControl(ucInventoryManagement);
        }

        private void btnOrderManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Tạo đơn hàng", "Xử lý thanh toán", "In hóa đơn" });

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
            UpdateFeatureToolbar(new string[] { "Thống kê doanh số", "Xuất báo cáo chi tiết" });

            if (ucRevenueReport == null)
            {
                ucRevenueReport = new UcRevenueReport();
                ucRevenueReport.DateTimePickerStartDate = new DateTimePicker();
                ucRevenueReport.DateTimePickerEndDate = new DateTimePicker();
                ucRevenueReport.DataGridViewReport = new DataGridView();
                ucRevenueReport.ProgressBarReport = new ProgressBar();
            }

            LoadUserControl(ucRevenueReport);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Sản phẩm bán chạy" });

            if (ucDashboard == null)
            {
                ucDashboard = new UcDashboard();
            }

            ucDashboard.LoadDashboardData();

            LoadUserControl(ucDashboard);
        }

        private void btnNotificationSettings_Click(object sender, EventArgs e)
        {
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
                case "Cập nhật tồn kho":
                    btn.Click += (s, e) => HandleInventoryUpdate(s, e);
                    break;
                case "Thêm khách hàng":
                    btn.Click += (s, e) => HandleCustomerAdd(s, e);
                    break;
                case "Xóa khách hàng":
                    btn.Click += (s, e) => HandleCustomerDelete(s, e);
                    break;
                case "Làm mới":
                    btn.Click += (s, e) => HandleCustomerRefresh(s, e);
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
                ucInventoryManagement.txtProductId.Text = productId.ToString();
                ucInventoryManagement.BringToFront();
            }
        }
    }
}
