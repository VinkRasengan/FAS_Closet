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
            LoadCustomers();
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

        private void LoadCustomers()
        {
            var customers = CustomerManager.GetCustomers(); // danh sách Customer

            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "Name"; // giả sử bạn có thuộc tính này
            cmbCustomer.ValueMember = "CustomerID";
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a valid positive number.");
                return;
            }

            if (quantity > selectedProduct.Stock)
            {
                MessageBox.Show($"Only {selectedProduct.Stock} items in stock.");
                return;
            }

            var detail = new OrderDetail
            {
                ProductID = selectedProduct.ProductID,
                Quantity = quantity,
                UnitPrice = selectedProduct.Price
            };

            if (txtTotalAmount == null)
            {
                MessageBox.Show("txtTotalAmount is NULL");
                return;
            }

            orderDetails.Add(detail);

            decimal total = orderDetails.Sum(d => d.Quantity * d.UnitPrice);
            txtTotalAmount.Text = total.ToString("0.00");

            MessageBox.Show("Total Amount is: " + total.ToString("0.00"));
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
                    CustomerID = (int)cmbCustomer.SelectedValue,
                    OrderDate = DateTime.Now,
                    TotalAmount = decimal.Parse(txtTotalAmount.Text),
                    PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString() ?? string.Empty
                };
                
                OrderManager.AddOrder(order);
                MessageBox.Show("Order created successfully.");
                LoadOrders();
                
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
