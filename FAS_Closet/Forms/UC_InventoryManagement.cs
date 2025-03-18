using System;
using System.Windows.Forms;
using FASCloset.Services;
using System.Drawing;
using System.Linq;
using System.ComponentModel; // Add this line

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        public required TextBox txtProductId;
        public required TextBox txtStockQuantity;
        public required DataGridView dataGridViewLowStock;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required TextBox TxtSearchProductId { get; set; }

        public UcInventoryManagement()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtProductId = new TextBox();
            txtStockQuantity = new TextBox();
            dataGridViewLowStock = new DataGridView();
            TxtSearchProductId = new TextBox();
            TxtSearchProductId.Location = new Point(10, 10); // Adjust location as needed
            TxtSearchProductId.Size = new Size(200, 20); // Adjust size as needed
            TxtSearchProductId.TextChanged += TxtSearchProductId_TextChanged;
            this.Controls.Add(TxtSearchProductId);
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

        private void TxtSearchProductId_TextChanged(object? sender, EventArgs e) // Update nullability
        {
            var searchText = TxtSearchProductId.Text;
            if (int.TryParse(searchText, out int productId))
            {
                var filteredProducts = ProductManager.GetProducts().Where(p => p.ProductID == productId).ToList();
                dataGridViewLowStock.DataSource = new BindingSource { DataSource = filteredProducts };
            }
        }
    }
}
