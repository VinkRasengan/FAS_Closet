using System;
using System.Windows.Forms;
using FASCloset.Services;
using System.Drawing;
using System.Linq;
using System.ComponentModel;

namespace FASCloset.Forms
{
    public partial class UcInventoryManagement : UserControl
    {
        public TextBox txtProductId = new TextBox();
        public TextBox txtStockQuantity = new TextBox();
        public DataGridView dataGridViewLowStock = new DataGridView();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBox TxtSearchProductId { get; set; } = new TextBox();

        private Button btnUpdateStock;
        private Button btnLowStockWarning;

        public UcInventoryManagement()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Initialize controls
            txtProductId = new TextBox();
            txtProductId.Location = new Point(150, 20);
            txtProductId.Size = new Size(200, 25);
            txtProductId.PlaceholderText = "Enter Product ID";
            
            Label lblProductId = new Label();
            lblProductId.Text = "Product ID:";
            lblProductId.Location = new Point(20, 20);
            lblProductId.Size = new Size(100, 25);
            lblProductId.TextAlign = ContentAlignment.MiddleRight;
            
            txtStockQuantity = new TextBox();
            txtStockQuantity.Location = new Point(150, 60);
            txtStockQuantity.Size = new Size(200, 25);
            txtStockQuantity.PlaceholderText = "Enter Stock Quantity";
            
            Label lblStockQuantity = new Label();
            lblStockQuantity.Text = "Stock Quantity:";
            lblStockQuantity.Location = new Point(20, 60);
            lblStockQuantity.Size = new Size(100, 25);
            lblStockQuantity.TextAlign = ContentAlignment.MiddleRight;
            
            btnUpdateStock = new Button();
            btnUpdateStock.Text = "Update Stock";
            btnUpdateStock.Location = new Point(150, 100);
            btnUpdateStock.Size = new Size(200, 30);
            btnUpdateStock.Click += btnUpdateStock_Click;
            
            btnLowStockWarning = new Button();
            btnLowStockWarning.Text = "Show Low Stock";
            btnLowStockWarning.Location = new Point(150, 150);
            btnLowStockWarning.Size = new Size(200, 30);
            btnLowStockWarning.Click += btnLowStockWarning_Click;
            
            dataGridViewLowStock = new DataGridView();
            dataGridViewLowStock.Location = new Point(20, 200);
            dataGridViewLowStock.Size = new Size(500, 300);
            dataGridViewLowStock.AllowUserToAddRows = false;
            dataGridViewLowStock.ReadOnly = true;
            dataGridViewLowStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            TxtSearchProductId = new TextBox();
            TxtSearchProductId.Location = new Point(400, 20);
            TxtSearchProductId.Size = new Size(150, 25);
            TxtSearchProductId.PlaceholderText = "Search by Product ID";
            TxtSearchProductId.TextChanged += TxtSearchProductId_TextChanged;
            
            // Add controls to the UserControl
            this.Controls.Add(lblProductId);
            this.Controls.Add(txtProductId);
            this.Controls.Add(lblStockQuantity);
            this.Controls.Add(txtStockQuantity);
            this.Controls.Add(btnUpdateStock);
            this.Controls.Add(btnLowStockWarning);
            this.Controls.Add(dataGridViewLowStock);
            this.Controls.Add(TxtSearchProductId);
        }

        public void btnUpdateStock_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = int.Parse(txtProductId.Text);
                int stockQuantity = int.Parse(txtStockQuantity.Text);
                InventoryManager.UpdateStock(productId, stockQuantity);
                MessageBox.Show("Stock updated successfully.");
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnLowStockWarning_Click(object sender, EventArgs e)
        {
            var lowStockProducts = InventoryManager.GetLowStockProducts();
            dataGridViewLowStock.DataSource = lowStockProducts;
        }

        private void TxtSearchProductId_TextChanged(object? sender, EventArgs e)
        {
            var searchText = TxtSearchProductId.Text;
            if (int.TryParse(searchText, out int productId))
            {
                var filteredProducts = ProductManager.GetProducts().Where(p => p.ProductID == productId).ToList();
                dataGridViewLowStock.DataSource = new BindingSource { DataSource = filteredProducts };
            }
        }
    }
}
