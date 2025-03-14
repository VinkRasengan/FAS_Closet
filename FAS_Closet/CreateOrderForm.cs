using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FASCloset.UI
{
    public partial class CreateOrderForm : Form
    {
        private ComboBox comboBoxProducts;
        private NumericUpDown numericUpDownQuantity;
        private DataGridView dataGridViewOrderItems;

        public CreateOrderForm()
        {
            InitializeComponent();
            InitializeControls();
            LoadSampleProducts();
        }

        private void InitializeControls()
        {
            // ComboBox để chọn sản phẩm
            Label lblProduct = new Label { Text = "Sản phẩm:", Location = new System.Drawing.Point(10, 10) };
            comboBoxProducts = new ComboBox { Location = new System.Drawing.Point(120, 10), Width = 200 };

            // NumericUpDown để chọn số lượng
            Label lblQuantity = new Label { Text = "Số lượng:", Location = new System.Drawing.Point(10, 40) };
            numericUpDownQuantity = new NumericUpDown { Location = new System.Drawing.Point(120, 40), Width = 200, Minimum = 1 };

            // DataGridView để hiển thị các sản phẩm trong đơn hàng
            dataGridViewOrderItems = new DataGridView { Location = new System.Drawing.Point(10, 70), Width = 400, Height = 150 };

            // Nút "Thêm vào đơn hàng"
            Button btnAddItem = new Button { Text = "Thêm vào đơn hàng", Location = new System.Drawing.Point(120, 230) };
            btnAddItem.Click += BtnAddItem_Click;

            // Nút "Đặt hàng"
            Button btnPlaceOrder = new Button { Text = "Đặt hàng", Location = new System.Drawing.Point(120, 260) };
            btnPlaceOrder.Click += BtnPlaceOrder_Click;

            this.Controls.Add(lblProduct);
            this.Controls.Add(comboBoxProducts);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(numericUpDownQuantity);
            this.Controls.Add(dataGridViewOrderItems);
            this.Controls.Add(btnAddItem);
            this.Controls.Add(btnPlaceOrder);
        }

        private void LoadSampleProducts()
        {
            var products = new List<object>
            {
                new { ProductID = 1, Name = "Áo thun", Price = 150000m },
                new { ProductID = 2, Name = "Quần jeans", Price = 300000m }
            };
            comboBoxProducts.DataSource = products;
            comboBoxProducts.DisplayMember = "Name";
            comboBoxProducts.ValueMember = "ProductID";
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            var selectedProduct = (dynamic)comboBoxProducts.SelectedItem;
            int quantity = (int)numericUpDownQuantity.Value;
            var orderItems = dataGridViewOrderItems.DataSource as List<object> ?? new List<object>();
            orderItems.Add(new { ProductName = selectedProduct.Name, Quantity = quantity, Price = selectedProduct.Price });
            dataGridViewOrderItems.DataSource = null;
            dataGridViewOrderItems.DataSource = orderItems;
        }

        private void BtnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrderItems.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào đơn hàng!");
                return;
            }

            MessageBox.Show("Đơn hàng đã được tạo thành công!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Tạo đơn hàng";
            this.Size = new System.Drawing.Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
