using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace FASCloset.Forms
{
    public partial class UcProductManagement : UserControl
    {
        private bool showInactiveProducts = false;
        private bool showOnlyLowStock = false;

        // Use a BindingSource to manage the data binding
        private BindingSource productsBindingSource = new BindingSource();

        // Định nghĩa hằng cho các literal lặp lại nhiều lần
        private const string DataLoadingError = "Data Loading Error";
        private const string ErrorLiteral = "Error";

        public UcProductManagement()
        {
            InitializeComponent();
            
            // Setup data grid view
            SetupDataGridView();
            
            // Setup data binding
            productsBindingSource.DataSource = typeof(List<Product>);
            ProductDisplay.DataSource = productsBindingSource;
            
            // Load data
            LoadProducts();
            LoadCategories();
        }

        #region DataGridView Setup and Formatting

        private void SetupDataGridView()
        {
            // Configure columns for better display
            ProductDisplay.AutoGenerateColumns = false;
            
            // Clear existing columns to avoid duplicates on reload
            ProductDisplay.Columns.Clear();
            
            // Add columns with meaningful headers and improved styling
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductID",
                HeaderText = "Mã SP",
                Width = 60,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                Width = 180,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Danh Mục",
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ManufacturerName",
                HeaderText = "Nhà Sản Xuất",
                Width = 140,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Giá Bán",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0", 
                    Alignment = DataGridViewContentAlignment.MiddleRight 
                },
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Tồn Kho",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleRight
                },
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            // Add a column for low stock indicator
            DataGridViewCheckBoxColumn lowStockColumn = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsLowStock",
                HeaderText = "Sắp Hết",
                Width = 80,
                ReadOnly = true,
                TrueValue = true,
                FalseValue = false,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
            ProductDisplay.Columns.Add(lowStockColumn);
            
            // Add a column for active status
            DataGridViewCheckBoxColumn isActiveColumn = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsActive",
                HeaderText = "Đang Kinh Doanh",
                Width = 80,
                ReadOnly = true,
                TrueValue = true,
                FalseValue = false,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
            ProductDisplay.Columns.Add(isActiveColumn);
            
            // Configure selection behavior
            ProductDisplay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProductDisplay.MultiSelect = false;
            ProductDisplay.ReadOnly = true;
            ProductDisplay.AllowUserToOrderColumns = true;
            
            // Aplicar estilo completo usando el helper (con color de encabezado personalizado para productos)
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(ProductDisplay, Color.FromArgb(0, 123, 255));
            
            // Aplicar formato específico para las columnas basado en su contenido
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyColumnFormatting(ProductDisplay);
            
            // Add DataError handler to avoid binding errors
            ProductDisplay.DataError += (s, e) => {
                // Suppress DataError dialog
                e.ThrowException = false;
            };

            // Add CellFormatting handler
            ProductDisplay.CellFormatting += ProductDisplay_CellFormatting;
        }

        private void ProductDisplay_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Color rows based on low stock and inactive status
            if (e.RowIndex >= 0 && e.RowIndex < ProductDisplay.Rows.Count)
            {
                var row = ProductDisplay.Rows[e.RowIndex];
                
                // Make sure DataBoundItem exists and is a Product
                if (row?.DataBoundItem is Product product)
                {
                    // Gray out inactive products
                    if (!product.IsActive)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        row.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                        return;
                    }

                    // Highlight low stock items in yellow
                    if (product.IsLowStock)
                    {
                        // Make even more obvious if stock is at 0
                        if (product.Stock == 0)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                        }
                    }
                }
            }
        }

        #endregion

        #region Data Loading Methods

        // Add a method to load products with a specific warehouse ID
        public void LoadProducts()
        {
            try
            {
                List<Product> resultProducts;

                if (showOnlyLowStock)
                {
                    // Lấy các sản phẩm tồn kho thấp (Stock < 5)
                    resultProducts = InventoryManager.GetLowStockProducts()
                        .Where(p => showInactiveProducts || p.IsActive)
                        .ToList();
                }
                else
                {
                    // Lấy tất cả sản phẩm, có thể bao gồm sản phẩm không hoạt động
                    resultProducts = InventoryManager.GetAllProducts(!showInactiveProducts);
                }

                Console.WriteLine($"Retrieved {resultProducts?.Count ?? 0} products");

                // Cập nhật dữ liệu hiển thị
                productsBindingSource.SuspendBinding();
                productsBindingSource.Clear();
                productsBindingSource.DataSource = null;
                productsBindingSource.DataSource = resultProducts;
                productsBindingSource.ResumeBinding();

                ProductDisplay.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadProducts: {ex.Message}");
                MessageBox.Show($"Error loading products: {ex.Message}", DataLoadingError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductsByCategory(int categoryId)
        {
            try
            {
                var categoryProducts = ProductManager.GetProductsByCategory(categoryId, showInactiveProducts);
                productsBindingSource.DataSource = categoryProducts;
                ProductDisplay.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products by category: {ex.Message}", DataLoadingError, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadCategories()
        {
            try
            {
                var categories = ProductManager.GetCategories();
                
                // Add "All Categories" option for filtering
                var allCategories = new List<Category>
                {
                    new Category { CategoryID = 0, CategoryName = "All Categories" }
                };
                allCategories.AddRange(categories);
                
                // Setup filter dropdown
                CmbFilterCategory.DisplayMember = "CategoryName";
                CmbFilterCategory.ValueMember = "CategoryID";
                CmbFilterCategory.DataSource = new BindingSource { DataSource = allCategories };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", DataLoadingError, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region UI Event Handlers

        public void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the ProductForm for adding a new product
                using (var productForm = new ProductForm())
                {
                    if (productForm.ShowDialog() == DialogResult.OK)
                    {
                        // Get the new product from the form
                        var newProduct = productForm.Product;
                        
                        // Save the new product to the database
                        ProductManager.AddProduct(newProduct);
                        
                        // Reload the product list
                        LoadProducts();
                        
                        // Select the newly added product
                        SelectProductInGridByName(newProduct.ProductName);
                        
                        MessageBox.Show("Product added successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", ErrorLiteral, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void btnEdit_Click(object sender, EventArgs e)
        {
            if (ProductDisplay.SelectedRows.Count > 0)
            {
                try
                {
                    // Get the selected product
                    var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                    
                    if (selectedProduct != null)
                    {
                        // Open the ProductForm for editing
                        using (var productForm = new ProductForm(selectedProduct))
                        {
                            if (productForm.ShowDialog() == DialogResult.OK)
                            {
                                // Get the updated product from the form
                                var updatedProduct = productForm.Product;
                                
                                // Save the updated product to the database
                                ProductManager.UpdateProduct(updatedProduct);
                                
                                // Reload the product list
                                LoadProducts();
                                
                                // Select the updated product
                                SelectProductInGrid(updatedProduct.ProductID);
                                
                                MessageBox.Show("Product updated successfully!", "Success", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating product: {ex.Message}", ErrorLiteral, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a product to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            if (ProductDisplay.SelectedRows.Count > 0)
            {
                var selectedToDelete = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedToDelete != null)
                {
                    if (MessageBox.Show("Are you sure you want to archive this product? It will be marked as inactive but not permanently deleted.", 
                        "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            ProductManager.DeleteProduct(selectedToDelete.ProductID);
                            LoadProducts();
                            MessageBox.Show("Product has been archived successfully.", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error archiving product: {ex.Message}", ErrorLiteral, 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to archive.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CmbFilterCategory.SelectedItem is Category selectedCategory)
                {
                    Console.WriteLine($"Category selected: {selectedCategory.CategoryName} (ID: {selectedCategory.CategoryID})");

                    if (selectedCategory.CategoryID == 0)
                    {
                        // "All Categories" selected — load all products
                        Console.WriteLine("Loading all products...");
                        LoadProducts();
                    }
                    else
                    {
                        // Load products for selected category
                        Console.WriteLine($"Loading products for category ID: {selectedCategory.CategoryID}");
                        LoadProductsByCategory(selectedCategory.CategoryID);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in category change: {ex.Message}");
                MessageBox.Show($"Error loading products by category: {ex.Message}", ErrorLiteral,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            showInactiveProducts = ChkShowInactive.Checked;
            LoadProducts();
        }

        private void btnShowLowStock_Click(object sender, EventArgs e)
        {
            showOnlyLowStock = !showOnlyLowStock;
            btnShowLowStock.Text = showOnlyLowStock ? "Show All Products" : "Show Low Stock";
            LoadProducts();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                PerformSearch(TxtSearch.Text);
            }
            else
            {
                LoadProducts(); // Reset to normal view when search box is empty
            }
        }

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            try
            {
                using (var categoryForm = new CategoryForm())
                {
                    if (categoryForm.ShowDialog() == DialogResult.OK)
                    {
                        // Reload categories
                        LoadCategories();
                        
                        MessageBox.Show("Category added successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error managing categories: {ex.Message}", ErrorLiteral, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnManageManufacturers_Click(object sender, EventArgs e)
        {
            try
            {
                using (var manufacturerForm = new ManufacturerForm())
                {
                    if (manufacturerForm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Manufacturer added successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error managing manufacturers: {ex.Message}", ErrorLiteral, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helper Methods

        private void PerformSearch(string searchText)
        {
            try
            {
                var searchResults = ProductManager.SearchProducts(searchText, showInactiveProducts);
                productsBindingSource.DataSource = searchResults;
                ProductDisplay.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error performing search: {ex.Message}", ErrorLiteral, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to select a specific product in the grid
        private void SelectProductInGrid(int productId)
        {
            foreach (DataGridViewRow row in ProductDisplay.Rows)
            {
                if (row.DataBoundItem is Product product && product.ProductID == productId)
                {
                    ProductDisplay.ClearSelection();
                    row.Selected = true;
                    ProductDisplay.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        // Helper method to select a specific product in the grid by name
        private void SelectProductInGridByName(string productName)
        {
            foreach (DataGridViewRow row in ProductDisplay.Rows)
            {
                if (row.DataBoundItem is Product product && 
                    product.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    ProductDisplay.ClearSelection();
                    row.Selected = true;
                    ProductDisplay.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        #endregion

        #region Validation Event Handlers

        private void TxtProductName_Validating(object? sender, CancelEventArgs e)
        {
            // Add validation logic for product name if needed
        }

        private void CmbCategory_Validating(object? sender, CancelEventArgs e)
        {
            // Add validation logic for category selection if needed
        }

        private void TxtPrice_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Allow only digits, control chars, and one decimal separator
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            // Only allow one decimal point
            TextBox? tb = sender as TextBox;
            if (e.KeyChar == '.' && tb != null && tb.Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void TxtPrice_Validating(object? sender, CancelEventArgs e)
        {
            // Add validation logic for price if needed
        }

        private void TxtStock_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Allow only digits and control chars
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtStock_Validating(object? sender, CancelEventArgs e)
        {
            // Add validation logic for stock if needed
        }

        private void TxtDescription_Validating(object? sender, CancelEventArgs e)
        {
            // Add validation logic for description if needed
        }

        #endregion
    }
}
