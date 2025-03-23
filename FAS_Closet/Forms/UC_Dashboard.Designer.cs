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
            
            // Refresh button
            Button btnRefresh = new Button();
            btnRefresh.Text = "Refresh Data";
            btnRefresh.Location = new Point(500, 185);
            btnRefresh.Size = new Size(120, 30);
            btnRefresh.Click += btnRefresh_Click;
            
            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(metricsPanel);
            this.Controls.Add(lblBestSellersTitle);
            this.Controls.Add(dgvBestSellers);
            this.Controls.Add(btnRefresh);
        }

        private DataGridView dgvBestSellers;
        private Label lblTotalProducts;
        private Label lblTotalCustomers;
        private Label lblTotalOrders;
        private Label lblLowStockWarning;
    }
}
