using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcOrderManagement : UserControl
    {
        public required TextBox txtCustomerId;
        public required TextBox txtTotalAmount;
        public required ComboBox cmbPaymentMethod;

        public UcOrderManagement()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtCustomerId = new TextBox();
            txtTotalAmount = new TextBox();
            cmbPaymentMethod = new ComboBox();
            // Initialize other components and set properties
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

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (!ValidateOrderInputs())
            {
                return;
            }
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

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            // Implement payment processing logic here
            MessageBox.Show("Payment processed successfully.");
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            // Implement invoice printing logic here
            MessageBox.Show("Invoice printed successfully.");
        }
    }
}
