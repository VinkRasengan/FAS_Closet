using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FASCloset.UI
{
    public partial class ProductManagementForm : Form
    {
        private DataGridView dataGridViewProducts;
        private List<Product> products; // Change to use a list of Product objects

        public ProductManagementForm()
        {
            InitializeComponent();
            InitializeControls();
            LoadSampleData();
        }

        private void InitializeControls()
        {
            // Tạo DataGridView để hiển thị danh sách sản phẩm
            dataGridViewProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true
            };
            this.Controls.Add(dataGridViewProducts);

            // Tạo các nút: Thêm, Sửa, Xóa
            Button btnAdd = new Button { Text = "Thêm", Dock = DockStyle.Bottom, Height = 30 };
            btnAdd.Click += BtnAdd_Click;
            Button btnEdit = new Button { Text = "Sửa", Dock = DockStyle.Bottom, Height = 30 };
            btnEdit.Click += BtnEdit_Click;
            Button btnDelete = new Button { Text = "Xóa", Dock = DockStyle.Bottom, Height = 30 };
            btnDelete.Click += BtnDelete_Click;

            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
        }

        private void LoadSampleData()
        {
            // Dữ liệu mẫu (sẽ thay bằng dữ liệu từ cơ sở dữ liệu sau này)
            products = new List<Product>
            {
                new Product { ProductID = 1, Name = "Áo thun", Price = 150000m },
                new Product { ProductID = 2, Name = "Quần jeans", Price = 300000m }
            };
            dataGridViewProducts.DataSource = products;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ProductForm addForm = new ProductForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                products.Add(new Product { Name = addForm.ProductName, Price = addForm.ProductPrice });
                dataGridViewProducts.DataSource = null;
                dataGridViewProducts.DataSource = products; // Cập nhật lại danh sách sau khi thêm
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedProduct = (Product)dataGridViewProducts.SelectedRows[0].DataBoundItem;
                ProductForm editForm = new ProductForm(selectedProduct);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    selectedProduct.Name = editForm.ProductName;
                    selectedProduct.Price = editForm.ProductPrice;
                    dataGridViewProducts.DataSource = null;
                    dataGridViewProducts.DataSource = products; // Cập nhật lại danh sách sau khi sửa
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa!");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedProduct = (Product)dataGridViewProducts.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    products.Remove(selectedProduct);
                    dataGridViewProducts.DataSource = null;
                    dataGridViewProducts.DataSource = products; // Cập nhật lại danh sách sau khi xóa
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa!");
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý sản phẩm";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
