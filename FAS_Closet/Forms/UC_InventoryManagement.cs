using System;
using System.Windows.Forms;
using FASCloset.Models;  // Add this import for Product class
using FASCloset.Services;
using System.Drawing;
using System.Linq;
using System.ComponentModel;

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        // Add a field to track current warehouse
        private int currentWarehouseId = 1;

        public void LoadCategories()
        {
            dataGridViewCategories.DataSource = CategoryManager.GetCategories();
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text.Trim();
            string desc = txtCategoryDescription.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a category name.");
                return;
            }
            var newCategory = new Category
            {
                CategoryName = name,
                Description = desc,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            CategoryManager.AddCategory(newCategory);
            LoadCategories();
            txtCategoryName.Clear();
            txtCategoryDescription.Clear();
        }

        private void BtnUpdateCategory_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0 &&
                dataGridViewCategories.SelectedRows[0].DataBoundItem is Category selected)
            {
                selected.CategoryName = txtCategoryName.Text.Trim();
                selected.Description = txtCategoryDescription.Text.Trim();
                CategoryManager.UpdateCategory(selected);
                LoadCategories();
            }
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0 &&
                dataGridViewCategories.SelectedRows[0].DataBoundItem is Category selected)
            {
                if (CategoryManager.IsCategoryUsed(selected.CategoryID))
                {
                    MessageBox.Show("Cannot delete. This category is in use.");
                    return;
                }

                if (MessageBox.Show($"Delete category '{selected.CategoryName}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CategoryManager.DeleteCategory(selected.CategoryID);
                    LoadCategories();
                    txtCategoryName.Clear();
                    txtCategoryDescription.Clear();
                }
            }
        }

        private void DataGridViewCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0 &&
                dataGridViewCategories.SelectedRows[0].DataBoundItem is Category selected)
            {
                txtCategoryName.Text = selected.CategoryName;
                txtCategoryDescription.Text = selected.Description;
            }
        }

        public UcInventoryManagement()
        {
            InitializeComponent();
            LoadCategories();
        }

        public void btnUpdateStock_Click(object? sender, EventArgs e)
        {
            UpdateProductStock();
        }

        // Add a method to load warehouse-specific inventory
        public void LoadWarehouseInventory(int warehouseId)
        {
            // Store the current warehouse ID
            currentWarehouseId = warehouseId;
            
            try
            {
                // Get low stock products for this warehouse
                var lowStockProducts = InventoryManager.GetLowStockProducts(warehouseId);
                dataGridViewLowStock.DataSource = lowStockProducts;
                
                // Update UI elements
                lblCurrentWarehouse.Text = $"Current Warehouse: {warehouseId}";
                
                // Refresh grid views
                dataGridViewLowStock.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading warehouse inventory: {ex.Message}", 
                    "Inventory Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update the UpdateStock method to include warehouse
        private void UpdateProductStock()
        {
            try
            {
                if (string.IsNullOrEmpty(txtProductId.Text))
                {
                    MessageBox.Show("Please enter a product ID.", "Missing Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrEmpty(txtStockQuantity.Text))
                {
                    MessageBox.Show("Please enter a stock quantity.", "Missing Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (!int.TryParse(txtProductId.Text, out int productId))
                {
                    MessageBox.Show("Invalid product ID. Please enter a number.", "Invalid Input", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (!int.TryParse(txtStockQuantity.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Invalid stock quantity. Please enter a non-negative number.", "Invalid Input", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Get product information to confirm
                var product = ProductManager.GetProductById(productId);
                if (product == null)
                {
                    MessageBox.Show("No product found with that ID.", "Product Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Confirm update
                if (MessageBox.Show($"Are you sure you want to update stock for {product.ProductName} to {quantity}?", 
                    "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Update stock with warehouse
                    InventoryManager.UpdateStock(productId, currentWarehouseId, quantity);
                    
                    // Show success message and refresh data
                    MessageBox.Show("Stock updated successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    // Refresh inventory display
                    LoadWarehouseInventory(currentWarehouseId);
                    
                    // Clear input fields
                    txtProductId.Text = "";
                    txtStockQuantity.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnLowStockWarning_Click(object? sender, EventArgs e)
        {
            if (dataGridViewLowStock == null)
            {
                MessageBox.Show("Data grid view not initialized.");
                return;
            }
            
            var lowStockProducts = InventoryManager.GetLowStockProducts(currentWarehouseId);
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
