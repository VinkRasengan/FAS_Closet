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
        
        private void LoadDashboardData()
        {
            try
            {
                // Load best selling products - explicitly use the overload with DateTime parameters
                var bestSellingProducts = ReportManager.GetBestSellingProducts(null, null);
                dgvBestSellers.DataSource = bestSellingProducts;
                
                // Load metrics
                lblTotalProducts.Text = ProductManager.GetProducts().Count.ToString();
                lblTotalCustomers.Text = CustomerManager.GetCustomers().Count.ToString();
                lblTotalOrders.Text = OrderManager.GetOrders().Count.ToString();
                
                // Use the default parameter (0) to get low stock across all warehouses
                var lowStockItems = InventoryManager.GetLowStockProducts(0);
                lblLowStockWarning.Text = lowStockItems.Count.ToString();
                
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
