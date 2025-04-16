using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;
using System.Linq;

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        public UcInventoryManagement()
        {
            InitializeComponent();
            LoadCategories();
        }

        public void LoadCategories()
        {
            dataGridViewCategories.DataSource = CategoryManager.GetCategories();
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

                MessageBox.Show("Stock updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void btnUpdateStock_Click(object? sender, EventArgs e) => UpdateProductStock();

        
    }
}
