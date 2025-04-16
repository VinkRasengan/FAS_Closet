using System;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcDashboard : UserControl
    {
        public UcDashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        public void LoadDashboardData()
        {
            try
            {
                // Load best selling products
                var bestSellingProducts = ReportManager.GetBestSellingProducts(null, null);
                dgvBestSellers.DataSource = bestSellingProducts;

                // Load metrics
                lblTotalProducts.Text = ProductManager.GetProducts().Count.ToString();
                lblTotalCustomers.Text = CustomerManager.GetCustomers().Count.ToString();
                lblTotalOrders.Text = OrderManager.GetOrders().Count.ToString();

                // Get low stock products (no warehouse filtering)
                var lowStockItems = InventoryManager.GetLowStockProducts();
                lblLowStockWarning.Text = lowStockItems.Count.ToString();

                // Color based on quantity
                if (lowStockItems.Count > 5)
                {
                    lblLowStockWarning.ForeColor = Color.Red;
                }
                else if (lowStockItems.Count > 0)
                {
                    lblLowStockWarning.ForeColor = Color.OrangeRed;
                }
                else
                {
                    lblLowStockWarning.ForeColor = Color.Green;
                    lblLowStockWarning.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}");
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
    }
}
