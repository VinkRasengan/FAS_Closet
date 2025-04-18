using System;
using System.ComponentModel;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class CategoryForm : Form
    {
        private readonly Category _category;
        private readonly bool _isEditMode;
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        /// <summary>
        /// Gets the category after form is submitted
        /// </summary>
        public Category Category => _category;

        public CategoryForm(Category category = null)
        {
            InitializeComponent();
            _category = category ?? new Category { IsActive = true };
            _isEditMode = category != null;
            
            // Set form title
            this.Text = _isEditMode ? "Edit Category" : "Add New Category";
            lblHeader.Text = _isEditMode ? "Edit Category" : "Add New Category";
            btnSave.Text = _isEditMode ? "Update" : "Add";
            
            // Set up validation
            txtCategoryName.Validating += TxtCategoryName_Validating;
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            // If editing, fill the form with category data
            if (_isEditMode)
            {
                txtCategoryName.Text = _category.CategoryName;
                txtDescription.Text = _category.Description;
                chkIsActive.Checked = _category.IsActive;
            }
            
            // Focus on the first field
            txtCategoryName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                try
                {
                    // Check if category name already exists (only for new categories)
                    if (!_isEditMode && ProductManager.IsCategoryNameTaken(txtCategoryName.Text.Trim()))
                    {
                        _errorProvider.SetError(txtCategoryName, "Category name already exists");
                        return;
                    }
                    
                    // Update category object
                    _category.CategoryName = txtCategoryName.Text.Trim();
                    _category.Description = txtDescription.Text.Trim();
                    _category.IsActive = chkIsActive.Checked;
                    
                    if (_isEditMode)
                    {
                        // Update existing category
                        ProductManager.UpdateCategory(_category);
                    }
                    else
                    {
                        // Add new category
                        _category.CreatedDate = DateTime.Now;
                        ProductManager.AddCategory(_category);
                    }
                    
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving category: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        
        private void TxtCategoryName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                _errorProvider.SetError(txtCategoryName, "Category name is required");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtCategoryName, "");
            }
        }

        private void CategoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _errorProvider.Dispose();
        }
    }
}