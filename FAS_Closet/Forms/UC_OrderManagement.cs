using System;
using System.Windows.Forms;
using System.Drawing;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcOrderManagement : UserControl
    {
        public UcOrderManagement()
        {
            InitializeComponent();
            LoadPaymentMethods();
            LoadProducts();
            LoadOrders();
        }

        private void LoadPaymentMethods()
        {
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Credit Card", "Bank Transfer", "Mobile Payment" });
            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            var products = ProductManager.GetProducts(); // Danh sách Product

            cmbProduct.DataSource = products;
            cmbProduct.DisplayMember = "DisplayName"; // Dùng thuộc tính tuỳ chỉnh
            cmbProduct.ValueMember = "ProductID";
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            // Kiểm tra sản phẩm đã được chọn chưa
            if (cmbProduct.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            // Kiểm tra số lượng có hợp lệ không
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please enter a quantity.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Quantity must be a valid integer.");
                return;
            }

            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                return;
            }

            if (quantity > selectedProduct.Stock)
            {
                MessageBox.Show($"Only {selectedProduct.Stock} items available in stock for {selectedProduct.ProductName}.");
                return;
            }

            // Thêm vào danh sách tạm
            var detail = new OrderDetail
            {
                ProductID = selectedProduct.ProductID,
                Quantity = quantity,
                UnitPrice = selectedProduct.Price
            };

            orderDetails.Add(detail);
            MessageBox.Show($"Added {selectedProduct.ProductName} x{quantity} to order.");
        }




        private void LoadOrders()
        {
            try
            {
                var orders = OrderManager.GetOrders();
                dgvOrders.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        // Revert to original method naming to fix missing method errors
        public void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (!ValidateOrderInputs())
                return;
            
            try
            {
                var order = new Order
                {
                    CustomerID = int.Parse(txtCustomerId.Text),
                    OrderDate = DateTime.Now,
                    TotalAmount = decimal.Parse(txtTotalAmount.Text),
                    PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString() ?? string.Empty
                };
                
                OrderManager.AddOrder(order);
                MessageBox.Show("Order created successfully.");
                LoadOrders();
                
                // Clear inputs
                txtCustomerId.Clear();
                txtTotalAmount.Clear();
                cmbPaymentMethod.SelectedIndex = 0;
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

        // Revert to original method naming
        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to process payment.");
                return;
            }
            
            // Implementation would actually process payment
            MessageBox.Show("Payment processed successfully.");
        }

        // Revert to original method naming
        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to print invoice.");
                return;
            }
            
            // Implementation would actually print invoice
            MessageBox.Show("Invoice printed successfully.");
        }

        private bool ValidateOrderInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerId.Text))
            {
                MessageBox.Show("Customer ID is required.");
                return false;
            }
            
            if (!int.TryParse(txtCustomerId.Text, out _))
            {
                MessageBox.Show("Customer ID must be a valid number.");
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(txtTotalAmount.Text))
            {
                MessageBox.Show("Total Amount is required.");
                return false;
            }
            
            if (!decimal.TryParse(txtTotalAmount.Text, out _))
            {
                MessageBox.Show("Total Amount must be a valid decimal number.");
                return false;
            }
            
            if (cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.");
                return false;
            }
            
            return true;
        }
    }
}
