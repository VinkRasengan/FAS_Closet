using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        private System.Windows.Forms.Timer refreshTimer;
        private const int REFRESH_INTERVAL = 30000; // 30 seconds
        private bool _isRefreshing = false; // Flag to prevent multiple simultaneous refreshes

        public UcInventoryManagement()
        {
            InitializeComponent();
            
            // Initialize refresh timer
            InitializeRefreshTimer();
            
            // Load data directly in the constructor or in a Load event handler instead of a background thread
            // This ensures the control's handle is created before we try to access it
            this.Load += UcInventoryManagement_Load;
        }

        private void UcInventoryManagement_Load(object sender, EventArgs e)
        {
            // Load initial data when the control is fully loaded (the handle is created)
            LoadCategories();
            // Use Task.Run to load products asynchronously
            _ = LoadProducts();
            LoadLowStockProducts();
        }

        private void InitializeRefreshTimer()
        {
            // Create and configure the timer for auto-refresh
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = REFRESH_INTERVAL;
            refreshTimer.Tick += (s, e) => RefreshLowStockData();
            refreshTimer.Start();
        }

        // Method to refresh low stock data
        private async void RefreshLowStockData()
        {
            // Only refresh if the control is visible to save resources
            // and if we're not already refreshing
            if (this.Visible && this.ParentForm != null && this.ParentForm.Visible && !_isRefreshing)
            {
                _isRefreshing = true;
                try
                {
                    await LoadLowStockProductsAsync();
                }
                finally
                {
                    _isRefreshing = false;
                }
            }
        }

        public void LoadCategories()
        {
            try
            {
                // Get all categories
                var categories = CategoryManager.GetAllCategories();
                
                // Set the data source
                dataGridViewCategories.DataSource = categories;
                
                // Apply styling
                FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(dataGridViewCategories);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        // Load products into ComboBox
        public async Task LoadProducts()
        {
            try
            {
                // Get all active products from the database
                var products = await InventoryManager.GetAllProductsAsync(onlyActive: true);
                
                if (products != null && products.Count > 0)
                {
                    // Save current selection if possible
                    object currentSelection = cmbProducts.SelectedItem;
                    int selectedProductId = currentSelection is Product p ? p.ProductID : -1;
                    
                    // Configure the ComboBox
                    cmbProducts.DataSource = null;
                    cmbProducts.DisplayMember = "ProductName";
                    cmbProducts.ValueMember = "ProductID";
                    cmbProducts.DataSource = products;

                    // Try to restore selection
                    if (selectedProductId != -1)
                    {
                        SelectProductInComboBox(selectedProductId);
                    }
                    // Or add placeholder text if needed
                    else if (cmbProducts.Items.Count > 0)
                    {
                        cmbProducts.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbProducts.SelectedIndex = -1;
                    }
                }
                else
                {
                    // If no products, show a message
                    MessageBox.Show("No products found. Please add products first.", "No Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        // Helper method to select a specific product in the combo box
        public void SelectProductInComboBox(int productId)
        {
            if (cmbProducts?.Items.Count == 0) 
                return;
            
            // Iterate through the items in the ComboBox to find the product with the specified ID
            for (int i = 0; i < cmbProducts.Items.Count; i++)
            {
                if (cmbProducts.Items[i] is Product product && product.ProductID == productId)
                {
                    cmbProducts.SelectedIndex = i;
                    return;
                }
            }
        }

        // Handle ComboBox selection change
        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem is Product selectedProduct)
            {
                // Optionally, you can display the current stock value when a product is selected
                txtStockQuantity.Text = selectedProduct.Stock.ToString();
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
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Please enter a category name.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newCategory = new Category
            {
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtCategoryDescription.Text.Trim(),
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
                // Check for products in this category
                var productsInCategory = ProductManager.GetProductsByCategory(selected.CategoryID);
                if (productsInCategory.Count > 0)
                {
                    MessageBox.Show("Cannot delete category with products. Please reassign or delete the products first.",
                                   "Cannot Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

        private async void UpdateProductStock()
        {
            try
            {
                // Get the selected product from the ComboBox
                if (cmbProducts.SelectedItem is not Product selectedProduct)
                {
                    MessageBox.Show("Please select a product.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtStockQuantity.Text.Trim(), out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Please enter a valid non-negative stock quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"Are you sure you want to update the stock of '{selectedProduct.ProductName}' to {quantity}?",
                    "Confirm Stock Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes) return;

                // Store values to use in background worker
                int productId = selectedProduct.ProductID;
                string productName = selectedProduct.ProductName;
                
                // Show progress cursor
                this.Cursor = Cursors.WaitCursor;
                
                // Disable the button to prevent multiple clicks
                var updateButton = btnUpdateStock;
                if (updateButton != null) 
                    updateButton.Enabled = false;
                
                try
                {
                    // Use the asynchronous method
                    await InventoryManager.UpdateStockAsync(productId, quantity);
                    
                    // Clear the stock quantity field
                    txtStockQuantity.Clear();
                    
                    // Force refresh of data
                    await RefreshAfterStockUpdate(productId);
                    
                    MessageBox.Show($"Stock for '{productName}' updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Reset cursor and re-enable button
                    this.Cursor = Cursors.Default;
                    if (updateButton != null) 
                        updateButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Error preparing stock update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Method to specifically refresh data after a stock update
        private async Task RefreshAfterStockUpdate(int updatedProductId)
        {
            try
            {
                // Refresh product list
                var currentSelectedIndex = cmbProducts.SelectedIndex;
                await LoadProducts();
                
                // Try to reselect the same product
                if (currentSelectedIndex >= 0 && currentSelectedIndex < cmbProducts.Items.Count)
                    cmbProducts.SelectedIndex = currentSelectedIndex;
                else
                    SelectProductInComboBox(updatedProductId);
                
                // Force refresh of low stock products
                await LoadLowStockProductsAsync();
                
                // Update the main dashboard if available
                var mainForm = this.ParentForm as MainForm;
                mainForm?.RefreshDashboard();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing data after stock update: {ex.Message}");
            }
        }

        // Mở form cập nhật số lượng riêng
        private async void btnQuickUpdateStock_Click(object? sender, EventArgs e)
        {
            try
            {
                // Lấy sản phẩm đã chọn từ ComboBox
                if (cmbProducts.SelectedItem is not Product selectedProduct)
                {
                    MessageBox.Show("Vui lòng chọn một sản phẩm.", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form cập nhật stock riêng
                using (var stockUpdateForm = new StockUpdateForm(selectedProduct))
                {
                    if (stockUpdateForm.ShowDialog() == DialogResult.OK)
                    {
                        // Sau khi cập nhật thành công, làm mới dữ liệu
                        await RefreshAfterStockUpdate(selectedProduct.ProductID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Override the OnVisibleChanged method to start/stop the timer
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            
            // When the control becomes visible, refresh data and ensure timer is running
            if (this.Visible)
            {
                RefreshLowStockData();
                if (!refreshTimer.Enabled)
                    refreshTimer.Start();
            }
            else
            {
                // When hidden, stop the timer to save resources
                if (refreshTimer.Enabled)
                    refreshTimer.Stop();
            }
        }

        // Async version of LoadLowStockProducts        
        private async Task LoadLowStockProductsAsync()
        {
            try
            {
                // Get low stock products asynchronously
                var lowStockItems = await InventoryManager.GetLowStockProductsAsync();
                
                // Update UI on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateLowStockGridUI(lowStockItems)));
                }
                else
                {
                    UpdateLowStockGridUI(lowStockItems);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading low stock products: {ex.Message}");
                MessageBox.Show("Error loading low stock products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // UI updating part of low stock loading
        private void UpdateLowStockGridUI(List<LowStockProductView> lowStockItems)
        {
            // Capture selected row index if any for restoring selection after refresh
            int selectedRowIndex = -1;
            if (dataGridViewLowStock.CurrentRow != null)
                selectedRowIndex = dataGridViewLowStock.CurrentRow.Index;

            // Clear existing columns and set up new ones for better display
            dataGridViewLowStock.DataSource = null;
            dataGridViewLowStock.Columns.Clear();
            dataGridViewLowStock.AutoGenerateColumns = false;
                
            // Add custom columns for better display
            dataGridViewLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductID",
                HeaderText = "Mã SP",
                Width = 50
            });
                
            dataGridViewLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                Width = 200
            });
                
            dataGridViewLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockQuantity",
                HeaderText = "Tồn Kho",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
                
            dataGridViewLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MinimumStockThreshold",
                HeaderText = "Ngưỡng",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
                
            dataGridViewLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Giá",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
                
            dataGridViewLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Danh Mục",
                Width = 100
            });
                
            // Set the data source after column configuration
            dataGridViewLowStock.DataSource = lowStockItems;
                
            // Apply styling using the helper
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(dataGridViewLowStock);
                
            // Apply conditional formatting to highlight very low stock items
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyConditionalFormatting(dataGridViewLowStock, row =>
            {
                if (row?.DataBoundItem == null) return null;
                
                if (row.DataBoundItem is LowStockProductView product)
                {
                    // Out of stock (red)
                    if (product.StockQuantity <= 0)
                    {
                        return FASCloset.Extensions.DataGridViewStyleHelper.CreateHighlightStyle(
                            Color.FromArgb(255, 220, 220), // Stronger red background
                            Color.FromArgb(180, 0, 0),     // Dark red text
                            true                           // Bold text
                        );
                    }
                    // Very low stock (still < 25% of threshold) - orange
                    else if (product.StockQuantity <= product.MinimumStockThreshold * 0.25)
                    {
                        return FASCloset.Extensions.DataGridViewStyleHelper.CreateHighlightStyle(
                            Color.FromArgb(255, 240, 220), // Light orange background
                            Color.FromArgb(180, 80, 0)     // Dark orange text
                        );
                    }
                    // Low stock (< 50% of threshold) - yellow
                    else if (product.StockQuantity <= product.MinimumStockThreshold * 0.5)
                    {
                        return FASCloset.Extensions.DataGridViewStyleHelper.CreateHighlightStyle(
                            Color.FromArgb(255, 252, 220), // Light yellow background
                            Color.FromArgb(150, 150, 0)    // Dark yellow text
                        );
                    }
                    // At threshold level - light blue
                    else if (product.StockQuantity <= product.MinimumStockThreshold)
                    {
                        return FASCloset.Extensions.DataGridViewStyleHelper.CreateHighlightStyle(
                            Color.FromArgb(240, 248, 255), // Light blue background
                            Color.FromArgb(0, 90, 150)     // Dark blue text
                        );
                    }
                }
                
                return null;
            });

            // Try to restore selection
            if (selectedRowIndex >= 0 && selectedRowIndex < dataGridViewLowStock.Rows.Count)
            {
                dataGridViewLowStock.CurrentCell = dataGridViewLowStock.Rows[selectedRowIndex].Cells[0];
                dataGridViewLowStock.Rows[selectedRowIndex].Selected = true;
            }
            
            // Update the low stock count label if it exists
            if (lblLowStockCount != null)
            {
                lblLowStockCount.Text = $"Sản phẩm sắp hết hàng ({lowStockItems.Count})";
            }
        }
        
        private void LoadLowStockProducts()
        {
            // Start the async operation
            _ = LoadLowStockProductsAsync();
        }

        public void btnUpdateStock_Click(object? sender, EventArgs e) => UpdateProductStock();
        
        // Handle button click for manual refresh
        public void btnRefreshLowStock_Click(object? sender, EventArgs e) => RefreshLowStockData();
    }
}
