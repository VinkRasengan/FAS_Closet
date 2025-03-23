using System;
using System.Windows.Forms;
using FASCloset.Services;
using System.Drawing;
using System.Linq;
using System.ComponentModel;

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        public UcInventoryManagement()
        {
            InitializeComponent();
        }

        public void btnUpdateStock_Click(object? sender, EventArgs e)
        {
            try
            {
                if (txtProductId == null || txtStockQuantity == null)
                {
                    MessageBox.Show("Product ID and stock quantity fields are required.");
                    return;
                }
                
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

        private void btnLowStockWarning_Click(object? sender, EventArgs e)
        {
            if (dataGridViewLowStock == null)
            {
                MessageBox.Show("Data grid view not initialized.");
                return;
            }
            
            var lowStockProducts = InventoryManager.GetLowStockProducts();
            dataGridViewLowStock.DataSource = lowStockProducts;
        }

        private void TxtSearchProductId_TextChanged(object? sender, EventArgs e)
        {
            if (TxtSearchProductId == null || dataGridViewLowStock == null)
                return;
                
            var searchText = TxtSearchProductId.Text;
            if (int.TryParse(searchText, out int productId))
            {
                var filteredProducts = ProductManager.GetProducts().Where(p => p.ProductID == productId).ToList();
                dataGridViewLowStock.DataSource = new BindingSource { DataSource = filteredProducts };
            }
        }
    }
}
