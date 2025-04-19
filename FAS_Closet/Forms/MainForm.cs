using System;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    /// <summary>
    /// Main application form that serves as the container for all user controls and provides the main UI framework.
    /// This form handles user authentication, navigation, and hosts all user control modules in the application.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Currently logged in user instance.
        /// </summary>
        private User CurrentUser { get; set; }

        /// <summary>
        /// Currently selected warehouse ID.
        /// </summary>
        private int CurrentWarehouseID { get; set; } = 1;

        /// <summary>
        /// Reference to the current active button in the navigation menu.
        /// </summary>
        private Button? currentActiveButton = null;

        /// <summary>
        /// Label used to display the current navigation section.
        /// </summary>
        private Label? navigationLabel = null;

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// Sets up the UI components and initializes the form with default values.
        /// </summary>
        /// <param name="user">The authenticated user object.</param>
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

        /// <summary>
        /// Initializes the warehouse selector dropdown.
        /// Configures the dropdown style and loads warehouses for the current user.
        /// </summary>
        private void InitializeWarehouseSelector()
        {
            cmbWarehouses.DropDownStyle = ComboBoxStyle.DropDownList;

            // Load warehouses for this user
        }

        /// <summary>
        /// Event handler for the Logout button click.
        /// Closes the application and logs out the current user.
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Sets the appearance of the active button in the navigation menu.
        /// Updates the navigation label to reflect the current section.
        /// </summary>
        /// <param name="button">The button to mark as active.</param>
        /// <param name="sectionName">The name of the section associated with the button.</param>
        private void SetActiveButton(Button button, string sectionName)
        {
            if (currentActiveButton != null)
            {
                currentActiveButton.BackColor = Color.FromArgb(248, 249, 250);
                currentActiveButton.ForeColor = Color.FromArgb(52, 58, 64);
                currentActiveButton.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            }

            currentActiveButton = button;
            currentActiveButton.BackColor = Color.FromArgb(0, 123, 255);
            currentActiveButton.ForeColor = Color.White;
            currentActiveButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            if (navigationLabel != null)
            {
                navigationLabel.Text = "Đang xem: " + sectionName;
            }
        }

        /// <summary>
        /// Event handler for the Product Management button click.
        /// Activates the product management user control and updates the feature toolbar.
        /// </summary>
        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnProductManagement, "Quản lý Sản phẩm");
            UpdateFeatureToolbar(new string[] { "Thêm", "Sửa", "Xóa", "Phân loại" });

            if (ucProductManagement == null)
            {
                ucProductManagement = new UcProductManagement();
            }

            ucProductManagement.LoadCategories();
            LoadUserControl(ucProductManagement);
        }

        /// <summary>
        /// Event handler for the Inventory Management button click.
        /// Activates the inventory management user control and updates the feature toolbar.
        /// </summary>
        private void btnInventoryManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnInventoryManagement, "Quản lý Kho hàng");
            UpdateFeatureToolbar(new string[] { "Cập nhật tồn kho", "Cảnh báo sắp hết" });

            if (ucInventoryManagement == null)
            {
                ucInventoryManagement = new UcInventoryManagement();
                ucInventoryManagement.dataGridViewLowStock = new DataGridView();
            }

            ucInventoryManagement.LoadCategories();
            LoadUserControl(ucInventoryManagement);
        }

        /// <summary>
        /// Event handler for the Order Management button click.
        /// Activates the order management user control and updates the feature toolbar.
        /// </summary>
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

        /// <summary>
        /// Event handler for the Customer Management button click.
        /// Activates the customer management user control and updates the feature toolbar.
        /// </summary>
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

        /// <summary>
        /// Event handler for the Revenue Report button click.
        /// Activates the revenue report user control and updates the feature toolbar.
        /// </summary>
        private void btnRevenueReport_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRevenueReport, "Báo cáo Doanh thu");
            UpdateFeatureToolbar(new string[] { });

            if (ucRevenueReport == null)
            {
                ucRevenueReport = new UcRevenueReport();
            }

            LoadUserControl(ucRevenueReport);
        }

        /// <summary>
        /// Event handler for the Dashboard button click.
        /// Activates the dashboard user control and updates the feature toolbar.
        /// </summary>
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard, "Bảng điều khiển");
            UpdateFeatureToolbar(new string[] { "Sản phẩm bán chạy" });

            if (ucDashboard == null)
            {
                ucDashboard = new UcDashboard();
            }

            LoadUserControl(ucDashboard);
            ucDashboard.LoadDashboardData();
        }

        /// <summary>
        /// Event handler for the Notification Settings button click.
        /// Activates the notification settings user control and updates the feature toolbar.
        /// </summary>
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

        /// <summary>
        /// Loads the specified user control into the content panel.
        /// Clears existing controls and docks the new user control.
        /// </summary>
        /// <param name="userControl">The user control to load.</param>
        private void LoadUserControl(UserControl userControl)
        {
            contentPanel.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(userControl);
        }

        /// <summary>
        /// Updates the feature toolbar with buttons for the specified features.
        /// Clears existing buttons and adds new ones based on the provided feature list.
        /// </summary>
        /// <param name="features">An array of feature names to display in the toolbar.</param>
        private void UpdateFeatureToolbar(string[] features)
        {
            featureToolbarPanel.Controls.Clear();

            foreach (var feature in features)
            {
                var btn = CreateFeatureButton(feature);
                featureToolbarPanel.Controls.Add(btn);
            }
        }

        /// <summary>
        /// Creates a button for the specified feature.
        /// Configures the button appearance and assigns an event handler based on the feature name.
        /// </summary>
        /// <param name="feature">The name of the feature associated with the button.</param>
        /// <returns>A configured Button control.</returns>
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

            AssignFeatureButtonHandler(btn, feature);
            return btn;
        }

        /// <summary>
        /// Assigns an event handler to the specified button based on the feature name.
        /// </summary>
        /// <param name="btn">The button to assign the event handler to.</param>
        /// <param name="feature">The name of the feature associated with the button.</param>
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
                    btn.Click += (s, e) =>
                    {
                        if (ucRevenueReport != null)
                            ucRevenueReport.btnRefresh.PerformClick();
                    };
                    break;
                case "Xuất báo cáo":
                    btn.Click += (s, e) =>
                    {
                        if (ucRevenueReport != null)
                            ucRevenueReport.btnExport.PerformClick();
                    };
                    break;
                default:
                    btn.Click += (s, e) => MessageBox.Show("Chức năng: " + feature);
                    break;
            }
        }

        /// <summary>
        /// Navigates to the Inventory Management section.
        /// Optionally sets a product ID in the inventory management UI.
        /// </summary>
        /// <param name="productId">The product ID to set in the inventory management UI (optional).</param>
        public void NavigateToInventoryManagement(int productId = 0)
        {
            btnInventoryManagement_Click(this, EventArgs.Empty);

            if (productId > 0 && ucInventoryManagement != null)
            {
                ucInventoryManagement.txtProductId.Text = productId.ToString();
                ucInventoryManagement.BringToFront();
            }
        }

        /// <summary>
        /// Displays detailed information about best-selling products in a separate form.
        /// Allows filtering by date range and exporting data to Excel.
        /// </summary>
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

                Form detailForm = new Form
                {
                    Text = "Chi tiết sản phẩm bán chạy",
                    Size = new Size(800, 600),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

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

                dgvDetailedProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
                dgvDetailedProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvDetailedProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvDetailedProducts.ColumnHeadersHeight = 40;
                dgvDetailedProducts.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                dgvDetailedProducts.RowTemplate.Height = 35;

                dgvDetailedProducts.Columns.Add("ProductID", "Mã SP");
                dgvDetailedProducts.Columns.Add("ProductName", "Tên Sản Phẩm");
                dgvDetailedProducts.Columns.Add("CategoryName", "Danh Mục");
                dgvDetailedProducts.Columns.Add("Price", "Giá Bán");
                dgvDetailedProducts.Columns.Add("TotalQuantity", "SL Đã Bán");
                dgvDetailedProducts.Columns.Add("Revenue", "Doanh Thu");
                dgvDetailedProducts.Columns.Add("CurrentStock", "Tồn Kho");

                dgvDetailedProducts.Columns["ProductID"].Width = 70;
                dgvDetailedProducts.Columns["Price"].DefaultCellStyle.Format = "N0";
                dgvDetailedProducts.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetailedProducts.Columns["TotalQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetailedProducts.Columns["Revenue"].DefaultCellStyle.Format = "N0";
                dgvDetailedProducts.Columns["Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetailedProducts.Columns["CurrentStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvDetailedProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);

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

                Label titleLabel = new Label
                {
                    Text = "Chi tiết sản phẩm bán chạy",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 123, 255),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 40
                };

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

                dtpStartDate.Value = DateTime.Now.AddDays(-30);
                dtpEndDate.Value = DateTime.Now;

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
                btnFilter.Click += (s, ev) =>
                {
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

                btnExport.Click += (s, ev) =>
                {
                    MessageBox.Show("Tính năng xuất Excel sẽ được phát triển trong phiên bản tới.");
                };

                detailForm.Controls.Add(dgvDetailedProducts);
                detailForm.Controls.Add(btnClose);
                detailForm.Controls.Add(controlsPanel);
                detailForm.Controls.Add(titleLabel);

                detailForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
