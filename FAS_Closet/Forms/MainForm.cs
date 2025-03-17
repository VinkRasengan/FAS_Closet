using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class MainForm : Form
    {
        private enum Mode { View, Add, Edit }
        private Mode currentMode = Mode.View;

        public MainForm(User user)
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome, " + user.Name;

            // Gọi sự kiện Products để đảm bảo giao diện khởi tạo đúng
            productsToolStripMenuItem_Click(this, EventArgs.Empty);
        }

        // Sự kiện cho btnLogout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Sự kiện Menu
        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            leftPanel.Controls.Clear();
            leftPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnCategorize, btnDataManagement });
            leftPanel.AutoScrollPosition = new Point(0, 0);
            leftPanel.ScrollControlIntoView(btnAdd);
            btnAdd.Visible = true;
            Console.WriteLine("btnAdd.Visible: " + btnAdd.Visible);
            Console.WriteLine("btnAdd.Enabled: " + btnAdd.Enabled);
            Console.WriteLine("leftPanel.Controls.Count: " + leftPanel.Controls.Count);
            LoadProducts();
            HideAddEditPanel();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            leftPanel.Controls.Clear();
            var btnUpdateInventory = new Button { Text = "Cập nhật tồn kho", Size = new Size(180, 40), Margin = new Padding(5), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Visible = true };
            var btnAlertStock = new Button { Text = "Cảnh báo hết hàng", Size = new Size(180, 40), Margin = new Padding(5), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Visible = true };
            leftPanel.Controls.AddRange(new[] { btnUpdateInventory, btnAlertStock });
            HideAddEditPanel();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e) => ShowCustomButtons("Tạo đơn hàng", "Xử lý thanh toán", "In hóa đơn");
        private void customersToolStripMenuItem_Click(object sender, EventArgs e) => ShowCustomButtons("Lưu thông tin", "Lịch sử mua hàng", "Tích điểm");
        private void reportsToolStripMenuItem_Click(object sender, EventArgs e) => ShowCustomButtons("Thống kê doanh số", "Xuất báo cáo");

        private void ShowCustomButtons(params string[] texts)
        {
            leftPanel.Controls.Clear();
            foreach (var txt in texts)
            {
                var btn = new Button { Text = txt, Size = new Size(180, 40), Margin = new Padding(5), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Visible = true };
                leftPanel.Controls.Add(btn);
            }
            HideAddEditPanel();
        }

        // Sự kiện nút
        private void btnAdd_Click(object sender, EventArgs e)
        {
            currentMode = Mode.Add;
            ClearAddEditPanel();
            ShowAddEditPanel();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (productDisplay.SelectedRows.Count > 0)
            {
                currentMode = Mode.Edit;
                var selectedProduct = productDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    FillAddEditPanel(selectedProduct);
                    ShowAddEditPanel();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (productDisplay.SelectedRows.Count > 0)
            {
                var selectedProduct = productDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    ProductManager.DeleteProduct(selectedProduct.ProductID);
                    LoadProducts();
                }
            }
        }

        // Sự kiện Add/Edit
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Add)
            {
                var product = new Product
                {
                    ProductName = txtProductName.Text,
                    CategoryID = (int)cmbCategory.SelectedValue!,
                    Price = decimal.Parse(txtPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    Description = txtDescription.Text
                };
                ProductManager.AddProduct(product);
            }
            else if (currentMode == Mode.Edit && productDisplay.SelectedRows.Count > 0)
            {
                var selectedProduct = productDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    selectedProduct.ProductName = txtProductName.Text;
                    selectedProduct.CategoryID = (int)cmbCategory.SelectedValue!;
                    selectedProduct.Price = decimal.Parse(txtPrice.Text);
                    selectedProduct.Stock = int.Parse(txtStock.Text);
                    selectedProduct.Description = txtDescription.Text;
                    ProductManager.UpdateProduct(selectedProduct);
                }
            }
            LoadProducts();
            HideAddEditPanel();
        }

        private void btnCancel_Click(object sender, EventArgs e) => HideAddEditPanel();

        // Hàm phụ trợ
        private void ShowAddEditPanel()
        {
            filterPanel.Visible = true;
            addEditPanel.Visible = true;
            addEditPanel.BringToFront();
            rightPanel.Controls.SetChildIndex(addEditPanel, 0);
        }

        private void HideAddEditPanel()
        {
            addEditPanel.Visible = false;
            filterPanel.Visible = true;
            currentMode = Mode.View;
        }

        private void ClearAddEditPanel()
        {
            txtProductName.Clear();
            cmbCategory.SelectedIndex = -1;
            txtPrice.Clear();
            txtStock.Clear();
            txtDescription.Clear();
        }

        private void FillAddEditPanel(Product product)
        {
            txtProductName.Text = product.ProductName;
            cmbCategory.SelectedValue = product.CategoryID;
            txtPrice.Text = product.Price.ToString();
            txtStock.Text = product.Stock.ToString();
            txtDescription.Text = product.Description;
        }

        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCategory = cmbFilterCategory.SelectedItem as Category;
            if (selectedCategory != null)
            {
                if (selectedCategory.CategoryID == 0) LoadProducts();
                else LoadProductsByCategory(selectedCategory.CategoryID);
            }
        }

        private void LoadProducts()
        {
            var products = ProductManager.GetProducts();
            productDisplay.DataSource = new BindingSource { DataSource = products };
        }

        private void LoadProductsByCategory(int categoryId)
        {
            var products = ProductManager.GetProductsByCategory(categoryId);
            productDisplay.DataSource = new BindingSource { DataSource = products };
        }
    }
}
