namespace FASCloset.Forms
{
    partial class UcDashboard
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
            
            // Title label
            Label lblTitle = new Label();
            lblTitle.Text = "Dashboard Overview";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(300, 30);
            
            // Metrics card container panel
            Panel metricsPanel = new Panel();
            metricsPanel.Location = new Point(20, 70);
            metricsPanel.Size = new Size(600, 100);
            metricsPanel.BackColor = Color.WhiteSmoke;
            
            // Total Products
            Label lblProductsTitle = new Label();
            lblProductsTitle.Text = "Total Products";
            lblProductsTitle.Location = new Point(20, 15);
            lblProductsTitle.Size = new Size(100, 20);
            lblProductsTitle.Font = new Font("Segoe UI", 10);
            
            lblTotalProducts = new Label();
            lblTotalProducts.Text = "Loading...";
            lblTotalProducts.Location = new Point(20, 45);
            lblTotalProducts.Size = new Size(100, 30);
            lblTotalProducts.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            
            // Total Customers
            Label lblCustomersTitle = new Label();
            lblCustomersTitle.Text = "Total Customers";
            lblCustomersTitle.Location = new Point(170, 15);
            lblCustomersTitle.Size = new Size(120, 20);
            lblCustomersTitle.Font = new Font("Segoe UI", 10);
            
            lblTotalCustomers = new Label();
            lblTotalCustomers.Text = "Loading...";
            lblTotalCustomers.Location = new Point(170, 45);
            lblTotalCustomers.Size = new Size(100, 30);
            lblTotalCustomers.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            
            // Total Orders
            Label lblOrdersTitle = new Label();
            lblOrdersTitle.Text = "Total Orders";
            lblOrdersTitle.Location = new Point(320, 15);
            lblOrdersTitle.Size = new Size(100, 20);
            lblOrdersTitle.Font = new Font("Segoe UI", 10);
            
            lblTotalOrders = new Label();
            lblTotalOrders.Text = "Loading...";
            lblTotalOrders.Location = new Point(320, 45);
            lblTotalOrders.Size = new Size(100, 30);
            lblTotalOrders.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            
            // Low Stock Warning
            Label lblWarningTitle = new Label();
            lblWarningTitle.Text = "Low Stock Items";
            lblWarningTitle.Location = new Point(470, 15);
            lblWarningTitle.Size = new Size(120, 20);
            lblWarningTitle.Font = new Font("Segoe UI", 10);
            
            lblLowStockWarning = new Label();
            lblLowStockWarning.Text = "Loading...";
            lblLowStockWarning.Location = new Point(470, 45);
            lblLowStockWarning.Size = new Size(100, 30);
            lblLowStockWarning.ForeColor = Color.OrangeRed;
            lblLowStockWarning.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            
            // Add metrics to panel
            metricsPanel.Controls.Add(lblProductsTitle);
            metricsPanel.Controls.Add(lblTotalProducts);
            metricsPanel.Controls.Add(lblCustomersTitle);
            metricsPanel.Controls.Add(lblTotalCustomers);
            metricsPanel.Controls.Add(lblOrdersTitle);
            metricsPanel.Controls.Add(lblTotalOrders);
            metricsPanel.Controls.Add(lblWarningTitle);
            metricsPanel.Controls.Add(lblLowStockWarning);
            
            // Best sellers section
            Label lblBestSellersTitle = new Label();
            lblBestSellersTitle.Text = "Best Selling Products";
            lblBestSellersTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblBestSellersTitle.Location = new Point(20, 190);
            lblBestSellersTitle.Size = new Size(200, 20);
            
            dgvBestSellers = new DataGridView();
            dgvBestSellers.Location = new Point(20, 220);
            dgvBestSellers.Size = new Size(600, 250);
            dgvBestSellers.ReadOnly = true;
            dgvBestSellers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBestSellers.AllowUserToAddRows = false;

            // Initialize Low Stock Alert Panel with header elements
            lowStockAlertPanel = new Panel
            {
                Name = "lowStockAlertPanel",
                Size = new Size(300, 250),
                Location = new Point(650, 220),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(255, 248, 230),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            
            // Add title to the low stock panel
            Label lowStockTitle = new Label
            {
                Text = "⚠️ Sản Phẩm Sắp Hết Hàng",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(204, 102, 0),
                Location = new Point(10, 10),
                Size = new Size(280, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lowStockAlertPanel.Controls.Add(lowStockTitle);
            
            // Add a line separator
            Panel lowStockSeparator = new Panel
            {
                Location = new Point(10, 40),
                Size = new Size(280, 1),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            lowStockAlertPanel.Controls.Add(lowStockSeparator);
            
            // Create column headers for low stock panel
            Label productNameHeader = new Label
            {
                Text = "Tên Sản Phẩm",
                Location = new Point(10, 45),
                Size = new Size(180, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 102, 102)
            };
            
            Label stockHeader = new Label
            {
                Text = "SL",
                Location = new Point(215, 45),
                Size = new Size(30, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 102, 102),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Label priceHeader = new Label
            {
                Text = "Giá",
                Location = new Point(245, 45),
                Size = new Size(45, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 102, 102),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            lowStockAlertPanel.Controls.Add(productNameHeader);
            lowStockAlertPanel.Controls.Add(stockHeader);
            lowStockAlertPanel.Controls.Add(priceHeader);
            
            // Add second separator below headers
            Panel headerSeparator = new Panel
            {
                Location = new Point(10, 68),
                Size = new Size(280, 1),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            lowStockAlertPanel.Controls.Add(headerSeparator);
            
            // Add "Manage Inventory" button to low stock panel
            Button manageInventoryButton = new Button
            {
                Text = "Quản Lý Kho Hàng",
                Location = new Point(75, 210),
                Size = new Size(150, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 153, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            
            manageInventoryButton.FlatAppearance.BorderSize = 0;
            
            manageInventoryButton.Click += (s, e) => {
                if (ParentForm is MainForm mainForm)
                {
                    mainForm.NavigateToInventoryManagement();
                }
            };
            
            lowStockAlertPanel.Controls.Add(manageInventoryButton);

            Label lblVIPCustomersTitle = new Label();
            lblVIPCustomersTitle.Text = "Top VIP Customers";
            lblVIPCustomersTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblVIPCustomersTitle.Location = new Point(20, 490); 
            lblVIPCustomersTitle.Size = new Size(200, 20);
            // Initialize and style the DataGridView
            dgvVIPCustomers = new DataGridView
            {
                Location = new Point(20, 520),  // Position below the title label
                Size = new Size(600, 200),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(230, 230, 230),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };

            // Add custom columns
            dgvVIPCustomers.Columns.Add("CustomerName", "Customer Name");
            dgvVIPCustomers.Columns.Add("LoyaltyPoints", "Loyalty Points");

            // Apply styling to column headers
            dgvVIPCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 118, 210);
            dgvVIPCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVIPCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvVIPCustomers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Apply styling to rows
            dgvVIPCustomers.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvVIPCustomers.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
            dgvVIPCustomers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dgvVIPCustomers.DefaultCellStyle.SelectionForeColor = Color.White;

            // Add alternating row color for better readability
            dgvVIPCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252);

            // Apply row height for better presentation
            dgvVIPCustomers.RowTemplate.Height = 35;

            // Add a separator line between rows and header
            dgvVIPCustomers.RowTemplate.DefaultCellStyle.Padding = new Padding(5);

            // Add buttons
            Button btnRefresh = new Button();
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Text = "Làm mới";
            btnRefresh.Location = new Point(500, 185);
            btnRefresh.Size = new Size(120, 30);
            btnRefresh.Click += btnRefresh_Click;

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(metricsPanel);
            this.Controls.Add(lblBestSellersTitle);
            this.Controls.Add(dgvBestSellers);
            this.Controls.Add(lblVIPCustomersTitle);
            this.Controls.Add(dgvVIPCustomers);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(lowStockAlertPanel);
        }

        private DataGridView dgvBestSellers;
        private Label lblTotalProducts;
        private Label lblTotalCustomers;
        private Label lblTotalOrders;
        private Label lblLowStockWarning;
        private DataGridView dgvVIPCustomers;
        private Panel lowStockAlertPanel; // Add this field for the low stock panel
    }
}
