using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using FASCloset.Models;
using FASCloset.Services;
using System.Linq;
using System.Collections.Generic;

namespace FASCloset.Forms
{
    public partial class ProductForm : Form
    {
        // Sự kiện tĩnh để thông báo khi có thay đổi ở category
        public static event EventHandler CategoryChanged;

        private Product _product;
        private readonly bool _isEditMode;
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        /// <summary>
        /// Gets the product after form is submitted
        /// </summary>
        public Product Product => _product;

        public ProductForm(Product product = null)
        {
            InitializeComponent();
            _product = product ?? new Product { IsActive = true };
            _isEditMode = product != null;
            
            // Set form title
            this.Text = _isEditMode ? "Edit Product" : "Add New Product";
            lblHeader.Text = _isEditMode ? "Edit Product" : "Add New Product";
            btnSave.Text = _isEditMode ? "Update" : "Add";
            
            // Register events
            Load += ProductForm_Load;
            FormClosing += ProductForm_FormClosing;
            
            // Register validation events
            txtProductName.Validating += TxtProductName_Validating;
            cmbCategory.Validating += CmbCategory_Validating;
            txtPrice.Validating += TxtPrice_Validating;
            txtStock.Validating += TxtStock_Validating;
            txtDescription.Validating += TxtDescription_Validating;
            
            // Input constraints
            txtPrice.KeyPress += TxtPrice_KeyPress;
            txtStock.KeyPress += TxtStock_KeyPress;
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadManufacturers();
            
            // If editing, fill form with product data
            if (_isEditMode)
            {
                FillFormWithProductData();
            }
            
            // Focus on the first field
            txtProductName.Focus();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = ProductManager.GetCategories();
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.DataSource = new BindingSource { DataSource = categories };
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
                
                // Add an empty manufacturer option
                var manufacturerList = new List<Manufacturer>
                {
                    new Manufacturer { ManufacturerID = 0, ManufacturerName = "-- Select Manufacturer --" }
                };
                manufacturerList.AddRange(manufacturers);
                
                cmbManufacturer.DisplayMember = "ManufacturerName";
                cmbManufacturer.ValueMember = "ManufacturerID";
                cmbManufacturer.DataSource = new BindingSource { DataSource = manufacturerList };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading manufacturers: {ex.Message}", "Data Loading Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillFormWithProductData()
        {
            txtProductName.Text = _product.ProductName;
            
            // Set the category
            for (int i = 0; i < cmbCategory.Items.Count; i++)
            {
                if (cmbCategory.Items[i] is Category category && category.CategoryID == _product.CategoryID)
                {
                    cmbCategory.SelectedIndex = i;
                    break;
                }
            }
            
            // Set the manufacturer
            if (_product.ManufacturerID.HasValue && _product.ManufacturerID.Value > 0)
            {
                for (int i = 0; i < cmbManufacturer.Items.Count; i++)
                {
                    if (cmbManufacturer.Items[i] is Manufacturer manufacturer && 
                        manufacturer.ManufacturerID == _product.ManufacturerID.Value)
                    {
                        cmbManufacturer.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbManufacturer.SelectedIndex = 0; // Select "-- Select Manufacturer --"
            }
            
            txtPrice.Text = _product.Price.ToString("0.00");
            txtStock.Text = _product.Stock.ToString();
            txtDescription.Text = _product.Description;
            chkIsActive.Checked = _product.IsActive;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                try
                {
                    // Populate the product object with form data
                    _product.ProductName = txtProductName.Text.Trim();
                    _product.CategoryID = ((Category)cmbCategory.SelectedItem).CategoryID;
                    
                    var selectedManufacturer = (Manufacturer)cmbManufacturer.SelectedItem;
                    _product.ManufacturerID = selectedManufacturer.ManufacturerID > 0 ? 
                        (int?)selectedManufacturer.ManufacturerID : null;
                    
                    _product.Price = decimal.Parse(txtPrice.Text);
                    _product.Stock = int.Parse(txtStock.Text);
                    _product.Description = txtDescription.Text.Trim();
                    _product.IsActive = chkIsActive.Checked;
                    
                    // Return the product to the calling form
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving product: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please correct the validation errors before saving.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            using (var categoryForm = new CategoryForm())
            {
                if (categoryForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload categories and select the newly added one
                    LoadCategories();
                    
                    // Try to select the newly added category
                    for (int i = 0; i < cmbCategory.Items.Count; i++)
                    {
                        if (cmbCategory.Items[i] is Category category && 
                            category.CategoryID == categoryForm.Category.CategoryID)
                        {
                            cmbCategory.SelectedIndex = i;
                            break;
                        }
                    }

                    // Raise the CategoryChanged event
                    CategoryChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void btnAddManufacturer_Click(object sender, EventArgs e)
        {
            using (var manufacturerForm = new ManufacturerForm())
            {
                if (manufacturerForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload manufacturers and select the newly added one
                    LoadManufacturers();
                    
                    // Try to select the newly added manufacturer
                    for (int i = 0; i < cmbManufacturer.Items.Count; i++)
                    {
                        if (cmbManufacturer.Items[i] is Manufacturer manufacturer && 
                            manufacturer.ManufacturerID == manufacturerForm.Manufacturer.ManufacturerID)
                        {
                            cmbManufacturer.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        #region Validation Methods
        
        private void TxtProductName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                _errorProvider.SetError(txtProductName, "Product name is required");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtProductName, "");
            }
        }

        private void CmbCategory_Validating(object sender, CancelEventArgs e)
        {
            if (cmbCategory.SelectedItem == null)
            {
                _errorProvider.SetError(cmbCategory, "Please select a category");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(cmbCategory, "");
            }
        }

        private void TxtPrice_Validating(object sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                _errorProvider.SetError(txtPrice, "Please enter a valid price");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtPrice, "");
            }
        }

        private void TxtStock_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                _errorProvider.SetError(txtStock, "Please enter a valid stock quantity");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtStock, "");
            }
        }

        private void TxtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                _errorProvider.SetError(txtDescription, "Description is required");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtDescription, "");
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
            if (e.KeyChar == '.' && sender is TextBox txt && txt.Text.IndexOf('.') > -1)
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

        private void ProductForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _errorProvider.Dispose();
        }
    }
}