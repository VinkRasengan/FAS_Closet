using System;
using System.Windows.Forms;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        public required TextBox txtProductId;
        public required TextBox txtStockQuantity;
        public required DataGridView dataGridViewLowStock;

        public UcInventoryManagement()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtProductId = new TextBox();
            txtStockQuantity = new TextBox();
            dataGridViewLowStock = new DataGridView();
            // Initialize other components and set properties
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = int.Parse(txtProductId.Text);
                int stockQuantity = int.Parse(txtStockQuantity.Text);
                InventoryManager.UpdateStock(productId, stockQuantity);
                MessageBox.Show("Stock updated successfully.");
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnLowStockWarning_Click(object sender, EventArgs e)
        {
            var lowStockProducts = InventoryManager.GetLowStockProducts();
            dataGridViewLowStock.DataSource = lowStockProducts;
        }
    }
}
