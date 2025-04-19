using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;
using System.Linq;
using System.Drawing;

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        public UcInventoryManagement()
        {
            InitializeComponent();
            LoadCategories();
            LoadLowStockProducts();
        }

        public void LoadCategories()
        {
            try
            {
                // Get categories from the database
                var categories = CategoryManager.GetCategories();
                
                // Configure columns with better headers before setting data source
                dataGridViewCategories.AutoGenerateColumns = false;
                dataGridViewCategories.Columns.Clear();
                
                // Add custom columns with better headers and styling
                dataGridViewCategories.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CategoryID",
                    HeaderText = "Mã Danh Mục",
                    Width = 80
                });
                
                dataGridViewCategories.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CategoryName",
                    HeaderText = "Tên Danh Mục",
                    Width = 160
                });
                
                dataGridViewCategories.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Description",
                    HeaderText = "Mô Tả",
                    Width = 200
                });
                
                dataGridViewCategories.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "IsActive",
                    HeaderText = "Đang Hoạt Động",
                    Width = 80
                });
                
                dataGridViewCategories.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CreatedDate",
                    HeaderText = "Ngày Tạo",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "dd/MM/yyyy"
                    }
                });
                
                // Set the data source after column configuration
                dataGridViewCategories.DataSource = categories;
                
                // Apply styling using the helper
                FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(dataGridViewCategories);
                
                // Refresh the UI
                dataGridViewCategories.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void btnViewProductsByCategory_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            if (dataGridViewCategories.SelectedRows[0].DataBoundItem is Category selectedCategory)
            {
                var products = ProductManager.GetProductsByCategory(selectedCategory.CategoryID);

                var form = new FormViewCategoryProducts(selectedCategory.CategoryName, products);
                form.ShowDialog();
            }
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

        private void UpdateProductStock()
        {
            try
            {
                if (!int.TryParse(txtProductId.Text.Trim(), out int productId))
                {
                    MessageBox.Show("Please enter a valid numeric Product ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtStockQuantity.Text.Trim(), out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Please enter a valid non-negative stock quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var product = ProductManager.GetProductById(productId);
                if (product == null)
                {
                    MessageBox.Show("No product found with that ID.", "Product Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"Are you sure you want to update the stock of '{product.ProductName}' to {quantity}?",
                    "Confirm Stock Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes) return;

                InventoryManager.UpdateStock(productId, quantity);

                txtProductId.Clear();
                txtStockQuantity.Clear();
                
                // Refresh the low stock products list to reflect changes
                LoadLowStockProducts();

                MessageBox.Show("Stock updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void btnUpdateStock_Click(object? sender, EventArgs e) => UpdateProductStock();
        
        private void LoadLowStockProducts()
        {
            try
            {
                // Lấy danh sách sản phẩm sắp hết hàng và gán vào DataGridView
                var lowStockItems = InventoryManager.GetLowStockProducts();
                dataGridViewLowStock.DataSource = lowStockItems;
                
                // Apply styling using the helper
                FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(dataGridViewLowStock);
                
                // Apply special column formatting for monetary and quantity values
                FASCloset.Extensions.DataGridViewStyleHelper.ApplyColumnFormatting(dataGridViewLowStock);
                
                // Apply conditional formatting to highlight very low stock items (less than minimum threshold)
                FASCloset.Extensions.DataGridViewStyleHelper.ApplyConditionalFormatting(dataGridViewLowStock, row =>
                {
                    if (row?.DataBoundItem == null) return null;
                    
                    // Safely extract stock quantity and threshold using reflection
                    var item = row.DataBoundItem;
                    var stockProp = item.GetType().GetProperty("StockQuantity");
                    var thresholdProp = item.GetType().GetProperty("MinimumStockThreshold");
                    
                    if (stockProp != null && thresholdProp != null)
                    {
                        var stock = (int?)stockProp.GetValue(item) ?? 0;
                        var threshold = (int?)thresholdProp.GetValue(item) ?? 0;
                        
                        // Highlight rows where stock is below or equal to the threshold
                        if (stock <= threshold)
                        {
                            return FASCloset.Extensions.DataGridViewStyleHelper.CreateHighlightStyle(
                                Color.FromArgb(255, 240, 240), // Light red background
                                Color.FromArgb(180, 0, 0)      // Dark red text
                            );
                        }
                    }
                    
                    return null;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading low stock products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
