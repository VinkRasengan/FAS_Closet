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
        private enum Mode { View, Add, Edit, Duplicate }
        private Mode currentMode = Mode.View;
        private List<Product> products = new List<Product>();
        private bool showInactiveProducts = false;
        private bool showOnlyLowStock = false;

        // For keyboard shortcuts
        private Keys lastKeyPressed = Keys.None;
        
        // Use a BindingSource to manage the data binding
        private BindingSource productsBindingSource = new BindingSource();

        public UcProductManagement()
        {
            InitializeComponent();
            
            // Setup data binding and grid view
            SetupDataGridView();
            
            // Setup data binding
            productsBindingSource.DataSource = typeof(List<Product>);
            ProductDisplay.DataSource = productsBindingSource;
            
            // Load data
            LoadProducts();
            LoadCategories();
            LoadManufacturers();
            
            // Hide the AddEditPanel initially
            AddEditPanel.Visible = false;
        }

        #region DataGridView Setup and Formatting

        private void SetupDataGridView()
        {
            // Configure columns for better display
            ProductDisplay.AutoGenerateColumns = false;
            
            // Clear existing columns to avoid duplicates on reload
            ProductDisplay.Columns.Clear();
            
            // Add columns with meaningful headers
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductID",
                HeaderText = "ID",
                Width = 50,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Product Name",
                Width = 150,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Category",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ManufacturerName",
                HeaderText = "Manufacturer",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Price",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "C2", 
                    Alignment = DataGridViewContentAlignment.MiddleRight 
                },
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            
            ProductDisplay.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Stock",
                Width = 60,
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
                HeaderText = "Low Stock",
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
                HeaderText = "Active",
                Width = 60,
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
            
            // Add DataError handler to avoid binding errors
            ProductDisplay.DataError += (s, e) => {
                // Suppress DataError dialog
                e.ThrowException = false;
            };
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
        public void LoadProducts(int warehouseId = 1)
        {
            try
            {
                List<Product> resultProducts;
                
                // Load products with appropriate filters
                if (showOnlyLowStock)
                {
                    var lowStockProducts = InventoryManager.GetLowStockProducts(warehouseId);
                    
                    // Convert Inventory objects to Product objects, then filter
                    resultProducts = lowStockProducts
                        .Where(i => i.Product != null)
                        .Select(i => i.Product!)
                        .Where(p => showInactiveProducts || p.IsActive)
                        .ToList();
                }
                else
                {
                    // Get products filtered by warehouse
                    resultProducts = ProductManager.GetProductsForWarehouse(warehouseId, showInactiveProducts);
                }
                
                // Debug information
                Console.WriteLine($"Retrieved {resultProducts?.Count ?? 0} products for warehouse {warehouseId}");
                
                // Properly clear and rebind data
                productsBindingSource.SuspendBinding();
                productsBindingSource.Clear();
                productsBindingSource.DataSource = null;
                productsBindingSource.DataSource = resultProducts;
                productsBindingSource.ResumeBinding();
                
                // Make sure UI is updated
                ProductDisplay.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadProducts: {ex.Message}");
                MessageBox.Show($"Error loading products: {ex.Message}", "Data Loading Error", 
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
                MessageBox.Show($"Error loading products by category: {ex.Message}", "Data Loading Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
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
                
                // Setup add/edit dropdown (without All Categories)
                CmbCategory.DisplayMember = "CategoryName";
                CmbCategory.ValueMember = "CategoryID";
                CmbCategory.DataSource = new BindingSource { DataSource = categories };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Data Loading Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadManufacturers()
        {
            try
            {
                var manufacturers = ProductManager.GetManufacturers();
                CmbManufacturer.DisplayMember = "ManufacturerName";
                CmbManufacturer.ValueMember = "ManufacturerID";
                CmbManufacturer.DataSource = new BindingSource { DataSource = manufacturers };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading manufacturers: {ex.Message}", "Data Loading Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region UI Event Handlers

        public void btnAdd_Click(object sender, EventArgs e)
        {
            currentMode = Mode.Add;
            InitializeAddProductUI();
            ShowAddEditPanel();
            btnDuplicate.Visible = false;
        }

        public void btnEdit_Click(object sender, EventArgs e)
        {
            if (ProductDisplay.SelectedRows.Count > 0)
            {
                currentMode = Mode.Edit;
                var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    FillAddEditPanel(selectedProduct);
                    ShowAddEditPanel();
                    btnDuplicate.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Please select a product to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Edit && ProductDisplay.SelectedRows.Count > 0)
            {
                currentMode = Mode.Duplicate;
                
                // Update UI to indicate we're duplicating
                this.Text = "Duplicate Product";
                
                // Change visibility of controls if needed
                btnDuplicate.Visible = false;
            }
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            if (ProductDisplay.SelectedRows.Count > 0)
            {
                var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    if (MessageBox.Show("Are you sure you want to archive this product? It will be marked as inactive but not permanently deleted.", 
                        "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            ProductManager.DeleteProduct(selectedProduct.ProductID);
                            LoadProducts();
                            MessageBox.Show("Product has been archived successfully.", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error archiving product: {ex.Message}", "Error", 
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateAllInputs())
            {
                SaveProduct();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            HideAddEditPanel();
        }

        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbFilterCategory.SelectedItem is Category selectedCategory)
            {
                if (selectedCategory.CategoryID == 0) 
                {
                    // All Categories selected
                    LoadProducts(); 
                }
                else 
                {
                    LoadProductsByCategory(selectedCategory.CategoryID);
                }
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Handle keyboard shortcuts
            if (keyData == Keys.Escape && AddEditPanel.Visible)
            {
                btnCancel_Click(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.S) && AddEditPanel.Visible)
            {
                btnSave_Click(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.A) && !AddEditPanel.Visible)
            {
                btnAdd_Click(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.E) && !AddEditPanel.Visible && ProductDisplay.SelectedRows.Count > 0)
            {
                btnEdit_Click(this, EventArgs.Empty);
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Data Validation

        private bool ValidateAllInputs()
        {
            errorProvider.Clear();
            bool isValid = true;
            
            // Validate product name
            if (string.IsNullOrWhiteSpace(TxtProductName.Text))
            {
                errorProvider.SetError(TxtProductName, "Product name is required");
                isValid = false;
            }
            
            // Validate category selection
            if (CmbCategory.SelectedItem == null)
            {
                errorProvider.SetError(CmbCategory, "Please select a category");
                isValid = false;
            }
            
            // Validate price
            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price < 0)
            {
                errorProvider.SetError(TxtPrice, "Please enter a valid price");
                isValid = false;
            }
            
            // Validate stock
            if (!int.TryParse(TxtStock.Text, out int stock) || stock < 0)
            {
                errorProvider.SetError(TxtStock, "Please enter a valid stock quantity");
                isValid = false;
            }
            
            // Validate description
            if (string.IsNullOrWhiteSpace(TxtDescription.Text))
            {
                errorProvider.SetError(TxtDescription, "Description is required");
                isValid = false;
            }
            
            return isValid;
        }

        private void TxtProductName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtProductName.Text))
            {
                errorProvider.SetError(TxtProductName, "Product name is required");
            }
            else
            {
                errorProvider.SetError(TxtProductName, "");
            }
        }

        private void CmbCategory_Validating(object sender, CancelEventArgs e)
        {
            if (CmbCategory.SelectedItem == null)
            {
                errorProvider.SetError(CmbCategory, "Please select a category");
            }
            else
            {
                errorProvider.SetError(CmbCategory, "");
            }
        }

        private void TxtPrice_Validating(object sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price < 0)
            {
                errorProvider.SetError(TxtPrice, "Please enter a valid price");
            }
            else
            {
                errorProvider.SetError(TxtPrice, "");
            }
        }

        private void TxtStock_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(TxtStock.Text, out int stock) || stock < 0)
            {
                errorProvider.SetError(TxtStock, "Please enter a valid stock quantity");
            }
            else
            {
                errorProvider.SetError(TxtStock, "");
            }
        }

        private void TxtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDescription.Text))
            {
                errorProvider.SetError(TxtDescription, "Description is required");
            }
            else
            {
                errorProvider.SetError(TxtDescription, "");
            }
        }

        // Allow only numeric input for price field
        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, decimal point, and control characters (like Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        // Allow only numeric input for stock field
        private void TxtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Helper Methods

        private void InitializeAddProductUI()
        {
            TxtProductName.Text = "";
            if (CmbCategory.Items.Count > 0)
                CmbCategory.SelectedIndex = 0;
            else
                CmbCategory.SelectedIndex = -1;
                
            if (CmbManufacturer.Items.Count > 0)
                CmbManufacturer.SelectedIndex = 0;
            else
                CmbManufacturer.SelectedIndex = -1;
                
            TxtPrice.Text = "0.00";
            TxtStock.Text = "0";
            TxtDescription.Text = "";
            ChkIsActive.Checked = true;
            
            errorProvider.Clear();
        }

        private void FillAddEditPanel(Product product)
        {
            TxtProductName.Text = product.ProductName;
            
            // Find the category in the combo box
            for (int i = 0; i < CmbCategory.Items.Count; i++)
            {
                var category = (Category)CmbCategory.Items[i];
                if (category.CategoryID == product.CategoryID)
                {
                    CmbCategory.SelectedIndex = i;
                    break;
                }
            }
            
            // Find the manufacturer in the combo box
            if (product.ManufacturerID.HasValue)
            {
                for (int i = 0; i < CmbManufacturer.Items.Count; i++)
                {
                    var manufacturer = (Manufacturer)CmbManufacturer.Items[i];
                    if (manufacturer.ManufacturerID == product.ManufacturerID)
                    {
                        CmbManufacturer.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                CmbManufacturer.SelectedIndex = -1;
            }
            
            TxtPrice.Text = product.Price.ToString("0.00");
            TxtStock.Text = product.Stock.ToString();
            TxtDescription.Text = product.Description;
            ChkIsActive.Checked = product.IsActive;
            
            errorProvider.Clear();
        }

        private void ShowAddEditPanel()
        {
            AddEditPanel.Visible = true;
            AddEditPanel.BringToFront();
            FilterPanel.Visible = false; // Hide FilterPanel to avoid overlap
            TxtProductName.Focus();
        }

        private void HideAddEditPanel()
        {
            AddEditPanel.Visible = false;
            FilterPanel.Visible = true;
            currentMode = Mode.View;
            errorProvider.Clear();
        }

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
                MessageBox.Show($"Error performing search: {ex.Message}", "Search Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveProduct()
        {
            try
            {
                if (!decimal.TryParse(TxtPrice.Text, out decimal price))
                {
                    errorProvider.SetError(TxtPrice, "Invalid price format");
                    return;
                }
                
                if (!int.TryParse(TxtStock.Text, out int stock))
                {
                    errorProvider.SetError(TxtStock, "Invalid stock quantity");
                    return;
                }
                
                // Validate uniqueness for new products
                if (currentMode == Mode.Add || currentMode == Mode.Duplicate)
                {
                    var existingProduct = ProductManager.GetProductByName(TxtProductName.Text.Trim());
                    if (existingProduct != null)
                    {
                        errorProvider.SetError(TxtProductName, "A product with this name already exists");
                        MessageBox.Show("A product with this name already exists.", "Duplicate Name", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                // Safely get the category and manufacturer IDs
                int categoryId;
                if (CmbCategory.SelectedValue != null && int.TryParse(CmbCategory.SelectedValue.ToString(), out categoryId))
                {
                    // Good, we have a valid category ID
                }
                else
                {
                    errorProvider.SetError(CmbCategory, "Please select a valid category");
                    return;
                }
                
                // Manufacturer can be null
                int? manufacturerId = null;
                if (CmbManufacturer.SelectedValue != null)
                {
                    if (int.TryParse(CmbManufacturer.SelectedValue.ToString(), out int mId))
                        manufacturerId = mId;
                }
                
                if (currentMode == Mode.Add || currentMode == Mode.Duplicate)
                {
                    // Create a new product
                    var product = new Product
                    {
                        ProductName = TxtProductName.Text.Trim(),
                        CategoryID = categoryId,
                        ManufacturerID = manufacturerId,
                        Price = price,
                        Stock = stock,
                        Description = TxtDescription.Text.Trim(),
                        IsActive = ChkIsActive.Checked
                    };
                    
                    ProductManager.AddProduct(product);
                    MessageBox.Show("Product added successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (currentMode == Mode.Edit && ProductDisplay.SelectedRows.Count > 0)
                {
                    // Update existing product
                    var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                    if (selectedProduct != null)
                    {
                        selectedProduct.ProductName = TxtProductName.Text.Trim();
                        selectedProduct.CategoryID = categoryId;
                        selectedProduct.ManufacturerID = manufacturerId;
                        selectedProduct.Price = price;
                        selectedProduct.Stock = stock;
                        selectedProduct.Description = TxtDescription.Text.Trim();
                        selectedProduct.IsActive = ChkIsActive.Checked;
                        
                        ProductManager.UpdateProduct(selectedProduct);
                        MessageBox.Show("Product updated successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                // Refresh the data and hide the panel
                LoadProducts();
                HideAddEditPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            // Show an input dialog for new category name
            using (var form = new Form())
            {
                form.Text = "Add Category";
                form.Size = new Size(300, 150);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                
                var label = new Label() { Left = 20, Top = 20, Text = "Category Name:" };
                var textBox = new TextBox() { Left = 120, Top = 20, Width = 150 };
                var buttonOk = new Button() { Text = "OK", Left = 120, Top = 70, Width = 75, DialogResult = DialogResult.OK };
                var buttonCancel = new Button() { Text = "Cancel", Left = 195, Top = 70, Width = 75, DialogResult = DialogResult.Cancel };
                
                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(buttonOk);
                form.Controls.Add(buttonCancel);
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;
                
                var result = form.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(textBox.Text))
                {
                    try
                    {
                        // Check if category already exists
                        var categories = ProductManager.GetCategories();
                        if (categories.Any(c => c.CategoryName.Equals(textBox.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                        {
                            MessageBox.Show("This category already exists.", "Duplicate Category", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        // Add the new category
                        var category = new Category
                        {
                            CategoryName = textBox.Text.Trim(),
                            Description = "",
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        
                        ProductManager.AddCategory(category);
                        LoadCategories();
                        MessageBox.Show("Category added successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding category: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAddManufacturer_Click(object sender, EventArgs e)
        {
            // Show an input dialog for new manufacturer name
            using (var form = new Form())
            {
                form.Text = "Add Manufacturer";
                form.Size = new Size(300, 150);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                
                var label = new Label() { Left = 20, Top = 20, Text = "Manufacturer Name:" };
                var textBox = new TextBox() { Left = 120, Top = 20, Width = 150 };
                var buttonOk = new Button() { Text = "OK", Left = 120, Top = 70, Width = 75, DialogResult = DialogResult.OK };
                var buttonCancel = new Button() { Text = "Cancel", Left = 195, Top = 70, Width = 75, DialogResult = DialogResult.Cancel };
                
                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(buttonOk);
                form.Controls.Add(buttonCancel);
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;
                
                var result = form.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(textBox.Text))
                {
                    try
                    {
                        // Check if manufacturer already exists
                        var manufacturers = ProductManager.GetManufacturers();
                        if (manufacturers.Any(m => m.ManufacturerName.Equals(textBox.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                        {
                            MessageBox.Show("This manufacturer already exists.", "Duplicate Manufacturer", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        // Add the new manufacturer
                        var manufacturer = new Manufacturer
                        {
                            ManufacturerName = textBox.Text.Trim(),
                            Description = ""
                        };
                        
                        ProductManager.AddManufacturer(manufacturer);
                        LoadManufacturers();
                        MessageBox.Show("Manufacturer added successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding manufacturer: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadLowStockProducts(int warehouseId)
        {
            try
            {
                // Use the unified method with the correct parameter
                var lowStockProducts = InventoryManager.GetLowStockProducts(warehouseId);
                
                // Properly handle the inventory collection that contains Product objects
                var filteredProducts = lowStockProducts
                    .Where(i => i.Product != null)
                    .Select(i => i.Product!)
                    .Where(p => showInactiveProducts || p.IsActive)
                    .ToList();
                    
                // Use the filtered products list as needed
                // For example, display in a separate grid:
                // lowStockGridView.DataSource = new BindingSource { DataSource = filteredProducts };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading low stock products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
