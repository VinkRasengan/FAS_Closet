using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace FASCloset.Forms
{
    public partial class UcProductManagement : UserControl
    {
        private enum Mode { View, Add, Edit }
        private Mode currentMode = Mode.View;
        private List<Product> products = new List<Product>();

        public UcProductManagement()
        {
            InitializeComponent();
            LoadProducts();
            LoadCategories();
            LoadManufacturers(); // Make sure manufacturers are loaded
        }

        private void InitializeAddProductUI()
        {
            TxtProductName.Text = "";
            CmbCategory.SelectedIndex = -1;
            CmbManufacturer.SelectedIndex = -1;
            TxtPrice.Text = "";
            TxtStock.Text = "";
            TxtDescription.Text = "";
        }

        public void btnAdd_Click(object sender, EventArgs e)
        {
            currentMode = Mode.Add;
            InitializeAddProductUI();
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
                        ProductDisplay.Refresh();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateProductInput(out decimal price, out int stock)
        {
            price = 0;
            stock = 0;
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(TxtProductName.Text) || string.IsNullOrWhiteSpace(TxtDescription.Text))
            {
                MessageBox.Show("Tên sản phẩm và mô tả là bắt buộc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (CmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (CmbManufacturer.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhà sản xuất.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Kiểm tra giá
            if (string.IsNullOrWhiteSpace(TxtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập giá.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!decimal.TryParse(TxtPrice.Text.Trim(), out price))
            {
                MessageBox.Show("Giá phải là một số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Kiểm tra tồn kho
            if (string.IsNullOrWhiteSpace(TxtStock.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng tồn kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!int.TryParse(TxtStock.Text.Trim(), out stock))
            {
                MessageBox.Show("Số lượng tồn kho phải là một số nguyên hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidateProductInput(out decimal price, out int stock))
                {
                    return;
                }

                // Trim dữ liệu
                string productName = TxtProductName.Text.Trim();
                string description = TxtDescription.Text.Trim();
                
                // Fix nullable warnings by providing safe default values
                int categoryId = 0;
                if (CmbCategory.SelectedValue != null)
                {
                    categoryId = Convert.ToInt32(CmbCategory.SelectedValue);
                }
                
                int manufacturerId = 0;
                if (CmbManufacturer.SelectedValue != null)
                {
                    manufacturerId = Convert.ToInt32(CmbManufacturer.SelectedValue);
                }

                // Kiểm tra tên sản phẩm duy nhất
                if (currentMode == Mode.Add)
                {
                    var existingProduct = ProductManager.GetProductByName(productName);
                    if (existingProduct != null)
                    {
                        MessageBox.Show("Sản phẩm với tên này đã tồn tại. Vui lòng chọn một tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (currentMode == Mode.Edit && ProductDisplay.SelectedRows.Count > 0)
                {
                    var selectedProduct = (Product)ProductDisplay.SelectedRows[0].DataBoundItem;
                    if (productName != selectedProduct.ProductName)
                    {
                        var existingProduct = ProductManager.GetProductByName(productName);
                        if (existingProduct != null && existingProduct.ProductID != selectedProduct.ProductID)
                        {
                            MessageBox.Show("Sản phẩm với tên này đã tồn tại. Vui lòng chọn một tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if (currentMode == Mode.Add)
                {
                    var product = new Product
                    {
                        ProductName = productName,
                        CategoryID = categoryId,
                        ManufacturerID = manufacturerId,
                        Price = price,
                        Stock = stock,
                        Description = description
                    };
                    ProductManager.AddProduct(product);
                }
                else if (currentMode == Mode.Edit && ProductDisplay.SelectedRows.Count > 0)
                {
                    var selectedProduct = (Product)ProductDisplay.SelectedRows[0].DataBoundItem;
                    selectedProduct.ProductName = productName;
                    selectedProduct.CategoryID = categoryId;
                    selectedProduct.ManufacturerID = manufacturerId;
                    selectedProduct.Price = price;
                    selectedProduct.Stock = stock;
                    selectedProduct.Description = description;
                    ProductManager.UpdateProduct(selectedProduct);
                }

                LoadProducts();
                ProductDisplay.Refresh();
                HideAddEditPanel();
                ClearAddEditPanel();
                MessageBox.Show("Sản phẩm đã được lưu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e) => HideAddEditPanel();

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
            AddEditPanel.Visible = true;
            AddEditPanel.BringToFront();
            FilterPanel.Visible = false; // Hide FilterPanel to avoid overlap
            ProductDisplay.Enabled = false; // Disable ProductDisplay in add/edit mode
        }

        private void HideAddEditPanel()
        {
            AddEditPanel.Visible = false;
            FilterPanel.Visible = true;
            ProductDisplay.Enabled = true;
            currentMode = Mode.View;
        }

        private void ClearAddEditPanel()
        {
            TxtProductName.Clear();
            CmbCategory.SelectedIndex = -1;
            CmbManufacturer.SelectedIndex = -1;
            TxtPrice.Clear();
            TxtStock.Clear();
            TxtDescription.Clear();
        }

        private void FillAddEditPanel(Product product)
        {
            TxtProductName.Text = product.ProductName;
            CmbCategory.SelectedValue = product.CategoryID;
            CmbManufacturer.SelectedValue = product.ManufacturerID;
            TxtPrice.Text = product.Price.ToString();
            TxtStock.Text = product.Stock.ToString();
            TxtDescription.Text = product.Description;
        }

        private void LoadCategories()
        {
            var categories = ProductManager.GetCategories();
            CmbCategory.DisplayMember = "CategoryName";
            CmbCategory.ValueMember = "CategoryID";
            CmbCategory.DataSource = new BindingSource { DataSource = categories };
        }

        private void LoadManufacturers()
        {
            var manufacturers = ProductManager.GetManufacturers();
            CmbManufacturer.DisplayMember = "ManufacturerName";
            CmbManufacturer.ValueMember = "ManufacturerID";
            CmbManufacturer.DataSource = new BindingSource { DataSource = manufacturers };
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = TxtSearch.Text.ToLower();
            var filteredProducts = ProductManager.GetProducts().Where(p => p.ProductName.ToLower().Contains(searchText)).ToList();
            ProductDisplay.DataSource = new BindingSource { DataSource = filteredProducts };
        }

        private void BtnConfirmAdd_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các trường nhập liệu
            string productName = TxtProductName.Text.Trim();
            string category = CmbCategory.SelectedItem?.ToString() ?? string.Empty; // Ensure non-null value
            string priceText = TxtPrice.Text.Trim();
            string stockText = TxtStock.Text.Trim();
            string description = TxtDescription.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(category) ||
                string.IsNullOrEmpty(priceText) || string.IsNullOrEmpty(stockText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển đổi giá và số lượng tồn kho sang kiểu số
            if (!decimal.TryParse(priceText, out decimal price) || !int.TryParse(stockText, out int stock))
            {
                MessageBox.Show("Giá hoặc số lượng tồn kho không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Fix nullable warning by providing a safe default value
            int categoryId = 0;
            if (CmbCategory.SelectedValue != null)
            {
                categoryId = Convert.ToInt32(CmbCategory.SelectedValue);
            }

            // Tạo đối tượng sản phẩm mới
            Product newProduct = new Product
            {
                ProductName = productName,
                CategoryID = categoryId,
                Price = price,
                Stock = stock,
                Description = description
            };

            // Thêm sản phẩm vào danh sách hoặc cơ sở dữ liệu
            AddProduct(newProduct);

            // Ẩn AddEditPanel sau khi thêm thành công
            AddEditPanel.Visible = false;

            // Làm mới giao diện (nếu cần)
            LoadProducts();

            // Thông báo thành công
            MessageBox.Show("Sản phẩm đã được thêm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Xóa dữ liệu trong các trường nhập liệu để chuẩn bị cho lần thêm tiếp theo
            ClearAddEditPanel();
        }

        private void AddProduct(Product product)
        {
            // Giả sử bạn có một danh sách sản phẩm (List<Product> products)
            products.Add(product);
            // Hoặc lưu vào cơ sở dữ liệu nếu bạn sử dụng DB
            // Ví dụ: dbContext.Products.Add(product); dbContext.SaveChanges();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại để nhập tên danh mục
            string categoryName = Prompt.ShowDialog("Nhập tên danh mục:", "Thêm Danh Mục");

            if (!string.IsNullOrEmpty(categoryName)) // Kiểm tra tên không rỗng
            {
                // Kiểm tra xem danh mục đã tồn tại chưa
                var existingCategories = ProductManager.GetCategories();
                if (existingCategories.Any(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Danh mục này đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Thêm danh mục mới
                var category = new Category
                {
                    CategoryName = categoryName,
                    Description = "",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
                ProductManager.AddCategory(category);
                LoadCategories(); // Cập nhật danh sách danh mục trong ComboBox
                MessageBox.Show("Danh mục đã được thêm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAddManufacturer_Click(object sender, EventArgs e)
        {
            string manufacturerName = Prompt.ShowDialog("Nhập tên nhà sản xuất:", "Thêm Nhà Sản Xuất");
            if (!string.IsNullOrEmpty(manufacturerName))
            {
                var existingManufacturers = ProductManager.GetManufacturers();
                if (existingManufacturers.Any(m => m.ManufacturerName.Equals(manufacturerName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Nhà sản xuất này đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var manufacturer = new Manufacturer
                {
                    ManufacturerName = manufacturerName,
                    Description = ""
                };
                ProductManager.AddManufacturer(manufacturer);
                LoadManufacturers();
                MessageBox.Show("Nhà sản xuất đã được thêm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox inputBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "OK", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : string.Empty;
            }
        }

        private void TableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {
            // Add your paint logic here
        }

        // Remove duplicate TableLayoutPanel_Paint method - we're using OnTableLayoutPanelPaint now
    }
}
