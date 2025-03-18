using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;
using System.ComponentModel;
using System.Linq;

namespace FASCloset.Forms
{
    public partial class UcProductManagement : UserControl
    {
        private enum Mode { View, Add, Edit }
        private Mode currentMode = Mode.View;

        public UcProductManagement()
        {
            InitializeComponent();
            LoadProducts();
            LoadCategories();
        }

        public void btnAdd_Click(object sender, EventArgs e)
        {
            currentMode = Mode.Add;
            ClearAddEditPanel();
            ShowAddEditPanel();
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
                }
            }
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            if (ProductDisplay.SelectedRows.Count > 0)
            {
                var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác Nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ProductManager.DeleteProduct(selectedProduct.ProductID);
                        LoadProducts();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtProductName.Text) || string.IsNullOrWhiteSpace(TxtDescription.Text))
                {
                    MessageBox.Show("Tên sản phẩm và mô tả là bắt buộc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                TxtProductName.Text = TxtProductName.Text.Trim();
                TxtDescription.Text = TxtDescription.Text.Trim();

                decimal price;
                if (!decimal.TryParse(TxtPrice.Text, out price))
                {
                    MessageBox.Show("Giá phải là một số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int stock;
                if (!int.TryParse(TxtStock.Text, out stock))
                {
                    MessageBox.Show("Số lượng tồn kho phải là một số nguyên hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (currentMode == Mode.Add)
                {
                    var product = new Product
                    {
                        ProductName = TxtProductName.Text,
                        CategoryID = (int)CmbCategory.SelectedValue!,
                        Price = price,
                        Stock = stock,
                        Description = TxtDescription.Text
                    };
                    ProductManager.AddProduct(product);
                }
                else if (currentMode == Mode.Edit && ProductDisplay.SelectedRows.Count > 0)
                {
                    var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                    if (selectedProduct != null)
                    {
                        selectedProduct.ProductName = TxtProductName.Text;
                        selectedProduct.CategoryID = (int)CmbCategory.SelectedValue!;
                        selectedProduct.Price = price;
                        selectedProduct.Stock = stock;
                        selectedProduct.Description = TxtDescription.Text;
                        ProductManager.UpdateProduct(selectedProduct);
                    }
                }
                LoadProducts();
                HideAddEditPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => HideAddEditPanel();

        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cmbFilterCategory && cmbFilterCategory.SelectedItem is Category selectedCategory)
            {
                if (selectedCategory.CategoryID == 0) LoadProducts();
                else LoadProductsByCategory(selectedCategory.CategoryID);
            }
        }

        private void LoadProducts()
        {
            var products = ProductManager.GetProducts();
            ProductDisplay.DataSource = new BindingSource { DataSource = products };
        }

        private void LoadProductsByCategory(int categoryId)
        {
            var products = ProductManager.GetProductsByCategory(categoryId);
            ProductDisplay.DataSource = new BindingSource { DataSource = products };
        }

        private void ShowAddEditPanel()
        {
            FilterPanel.Visible = true;
            AddEditPanel.Visible = true;
            AddEditPanel.BringToFront();
        }

        private void HideAddEditPanel()
        {
            AddEditPanel.Visible = false;
            FilterPanel.Visible = true;
            currentMode = Mode.View;
        }

        private void ClearAddEditPanel()
        {
            TxtProductName.Clear();
            CmbCategory.SelectedIndex = -1;
            TxtPrice.Clear();
            TxtStock.Clear();
            TxtDescription.Clear();
        }

        private void FillAddEditPanel(Product product)
        {
            TxtProductName.Text = product.ProductName;
            CmbCategory.SelectedValue = product.CategoryID;
            TxtPrice.Text = product.Price.ToString();
            TxtStock.Text = product.Stock.ToString();
            TxtDescription.Text = product.Description;
        }

        private void LoadCategories()
        {
            var categories = ProductManager.GetCategories();
            CmbCategory.DisplayMember = "CategoryName";
            CmbCategory.ValueMember = "CategoryID";
            CmbCategory.DataSource = categories;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = TxtSearch.Text.ToLower();
            var filteredProducts = ProductManager.GetProducts().Where(p => p.ProductName.ToLower().Contains(searchText)).ToList();
            ProductDisplay.DataSource = new BindingSource { DataSource = filteredProducts };
        }
    }
}
