using System;
using System.Windows.Forms;

namespace FASCloset.UI
{
    public partial class ProductForm : Form
    {
        private TextBox txtName;
        private TextBox txtPrice;

        public string ProductName { get; private set; }
        public decimal ProductPrice { get; private set; }

        public ProductForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        public ProductForm(Product product) : this()
        {
            txtName.Text = product.Name;
            txtPrice.Text = product.Price.ToString();
        }

        private void InitializeControls()
        {
            // Label và TextBox cho tên sản phẩm
            Label lblName = new Label { Text = "Tên sản phẩm:", Location = new System.Drawing.Point(10, 10) };
            txtName = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 200 };

            // Label và TextBox cho giá
            Label lblPrice = new Label { Text = "Giá:", Location = new System.Drawing.Point(10, 40) };
            txtPrice = new TextBox { Location = new System.Drawing.Point(120, 40), Width = 200 };

            // Nút "Lưu"
            Button btnSave = new Button { Text = "Lưu", Location = new System.Drawing.Point(120, 100) };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            this.Controls.Add(btnSave);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Giá phải là một số hợp lệ!");
                return;
            }

            ProductName = txtName.Text;
            ProductPrice = price;

            // Lưu dữ liệu (sẽ tích hợp với cơ sở dữ liệu sau)
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Thêm/Sửa sản phẩm";
            this.Size = new System.Drawing.Size(350, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
