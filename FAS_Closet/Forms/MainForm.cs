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
        private UcWarehouseManagement? ucWarehouseManagement = null;

        public MainForm(User user)
        {
            InitializeComponent();
            
            // Store the current user
            CurrentUser = user;
            lblWelcome.Text = "Welcome, " + user.Name;
            
            // Add warehouse selector
            InitializeWarehouseSelector();
            
            // Add notification settings button
            Button btnNotificationSettings = new Button();
            btnNotificationSettings.Text = "Notification Settings";
            btnNotificationSettings.BackColor = Color.FromArgb(0, 123, 255);
            btnNotificationSettings.ForeColor = Color.White;
            btnNotificationSettings.FlatStyle = FlatStyle.Flat;
            btnNotificationSettings.Location = new Point(15, 315);
            btnNotificationSettings.Margin = new Padding(5);
            btnNotificationSettings.Name = "btnNotificationSettings";
            btnNotificationSettings.Size = new Size(220, 40);
            btnNotificationSettings.TabIndex = 6;
            btnNotificationSettings.UseVisualStyleBackColor = false;
            btnNotificationSettings.Click += btnNotificationSettings_Click;
            leftPanel.Controls.Add(btnNotificationSettings);
            
            // Add warehouse management button
            Button btnWarehouseManagement = new Button();
            btnWarehouseManagement.Text = "Warehouse Management";
            btnWarehouseManagement.BackColor = Color.FromArgb(0, 123, 255);
            btnWarehouseManagement.ForeColor = Color.White;
            btnWarehouseManagement.FlatStyle = FlatStyle.Flat;
            btnWarehouseManagement.Location = new Point(15, 365);
            btnWarehouseManagement.Margin = new Padding(5);
            btnWarehouseManagement.Name = "btnWarehouseManagement";
            btnWarehouseManagement.Size = new Size(220, 40);
            btnWarehouseManagement.TabIndex = 7;
            btnWarehouseManagement.UseVisualStyleBackColor = false;
            btnWarehouseManagement.Click += btnWarehouseManagement_Click;
            leftPanel.Controls.Add(btnWarehouseManagement);
            
            // Check for low stock items on startup
            NotificationManager.CheckAndSendLowStockNotifications();
        }

        private User CurrentUser { get; set; }
        private int CurrentWarehouseID { get; set; } = 1; // Default to main warehouse

        private ComboBox cmbWarehouses;

        private void InitializeWarehouseSelector()
        {
            // Create warehouse label
            Label lblWarehouse = new Label();
            lblWarehouse.Text = "Current Warehouse:";
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(10, 15);
            
            // Create warehouse dropdown
            cmbWarehouses = new ComboBox();
            cmbWarehouses.Width = 180;
            cmbWarehouses.Location = new Point(120, 12);
            cmbWarehouses.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouses.SelectedIndexChanged += CmbWarehouses_SelectedIndexChanged;
            
            // Add controls to header
            Panel warehousePanel = new Panel();
            warehousePanel.Width = 320;
            warehousePanel.Height = 40;
            warehousePanel.Location = new Point(headerPanel.Width / 2 - 160, 5);
            warehousePanel.Controls.Add(lblWarehouse);
            warehousePanel.Controls.Add(cmbWarehouses);
            
            headerPanel.Controls.Add(warehousePanel);
            
            // Load warehouses for this user
            LoadWarehouses();
        }

        private void LoadWarehouses()
        {
            try
            {
                // Get warehouses for current user
                var warehouses = WarehouseManager.GetWarehousesByUser(CurrentUser.UserID);
                
                // If user has no warehouses assigned, get all warehouses
                if (warehouses.Count == 0)
                {
                    warehouses = WarehouseManager.GetWarehouses();
                }
                
                // Setup data binding
                cmbWarehouses.DisplayMember = "Name";
                cmbWarehouses.ValueMember = "WarehouseID";
                cmbWarehouses.DataSource = warehouses;
                
                // Default to first warehouse
                if (warehouses.Count > 0)
                {
                    CurrentWarehouseID = warehouses[0].WarehouseID;
                    cmbWarehouses.SelectedValue = CurrentWarehouseID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading warehouses: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbWarehouses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWarehouses.SelectedItem is Warehouse selectedWarehouse)
            {
                // Update current warehouse
                CurrentWarehouseID = selectedWarehouse.WarehouseID;
                
                // Refresh the current view to show data for selected warehouse
                RefreshCurrentView();
            }
        }

        private void RefreshCurrentView()
        {
            // Determine which UserControl is active and reload its data
            if (contentPanel.Controls.Count > 0 && contentPanel.Controls[0] is UserControl activeControl)
            {
                if (activeControl is UcInventoryManagement invManagement)
                {
                    invManagement.LoadWarehouseInventory(CurrentWarehouseID);
                }
                else if (activeControl is UcProductManagement prodManagement)
                {
                    prodManagement.LoadProducts(CurrentWarehouseID);
                }
                // Add more controls as needed
            }
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
            }
            
            LoadUserControl(ucProductManagement);
        }

        private void btnInventoryManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Cập nhật tồn kho", "Cảnh báo sắp hết" });
            
            if (ucInventoryManagement == null)
            {
                ucInventoryManagement = new UcInventoryManagement();
                ucInventoryManagement.txtProductId = new TextBox();
                ucInventoryManagement.txtStockQuantity = new TextBox();
                ucInventoryManagement.dataGridViewLowStock = new DataGridView();
                ucInventoryManagement.TxtSearchProductId = new TextBox();
            }
            
            LoadUserControl(ucInventoryManagement);
        }

        private void btnOrderManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Tạo đơn hàng", "Xử lý thanh toán", "In hóa đơn" });
            
            if (ucOrderManagement == null)
            {
                ucOrderManagement = new UcOrderManagement();
                ucOrderManagement.txtCustomerId = new TextBox();
                ucOrderManagement.txtTotalAmount = new TextBox();
                ucOrderManagement.cmbPaymentMethod = new ComboBox();
            }
            
            LoadUserControl(ucOrderManagement);
        }

        private void btnCustomerManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Lưu thông tin", "Lịch sử mua hàng", "Tích điểm" });
            
            if (ucCustomerManagement == null)
            {
                ucCustomerManagement = new UcCustomerManagement();
            }
            
            LoadUserControl(ucCustomerManagement);
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

        private void btnWarehouseManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Add Warehouse", "Edit Warehouse", "Deactivate Warehouse" });
            
            if (ucWarehouseManagement == null)
            {
                ucWarehouseManagement = new UcWarehouseManagement(CurrentUser);
            }
            
            LoadUserControl(ucWarehouseManagement);
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
                default:
                    btn.Click += (s, e) => MessageBox.Show("Chức năng: " + feature);
                    break;
            }
        }

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
    }
}
