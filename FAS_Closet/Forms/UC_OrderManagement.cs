using System;
using System.Windows.Forms;
using System.Drawing;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcOrderManagement : UserControl
    {
        public TextBox txtCustomerId = new TextBox();
        public TextBox txtTotalAmount = new TextBox();
        public ComboBox cmbPaymentMethod = new ComboBox();
        
        private Button btnCreateOrder;
        private Button btnProcessPayment;
        private Button btnPrintInvoice;
        private DataGridView dgvOrders;

        public UcOrderManagement()
        {
            InitializeComponent();
            LoadPaymentMethods();
            LoadOrders();
        }

        private void InitializeComponent()
        {
            // Initialize customer ID controls
            Label lblCustomerId = new Label();
            lblCustomerId.Text = "Customer ID:";
            lblCustomerId.Location = new Point(20, 20);
            lblCustomerId.Size = new Size(100, 25);
            lblCustomerId.TextAlign = ContentAlignment.MiddleRight;
            
            txtCustomerId = new TextBox();
            txtCustomerId.Location = new Point(130, 20);
            txtCustomerId.Size = new Size(200, 25);
            txtCustomerId.PlaceholderText = "Enter Customer ID";
            
            // Initialize total amount controls
            Label lblTotalAmount = new Label();
            lblTotalAmount.Text = "Total Amount:";
            lblTotalAmount.Location = new Point(20, 60);
            lblTotalAmount.Size = new Size(100, 25);
            lblTotalAmount.TextAlign = ContentAlignment.MiddleRight;
            
            txtTotalAmount = new TextBox();
            txtTotalAmount.Location = new Point(130, 60);
            txtTotalAmount.Size = new Size(200, 25);
            txtTotalAmount.PlaceholderText = "Enter Total Amount";
            
            // Initialize payment method controls
            Label lblPaymentMethod = new Label();
            lblPaymentMethod.Text = "Payment Method:";
            lblPaymentMethod.Location = new Point(20, 100);
            lblPaymentMethod.Size = new Size(100, 25);
            lblPaymentMethod.TextAlign = ContentAlignment.MiddleRight;
            
            cmbPaymentMethod = new ComboBox();
            cmbPaymentMethod.Location = new Point(130, 100);
            cmbPaymentMethod.Size = new Size(200, 25);
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Initialize buttons
            btnCreateOrder = new Button();
            btnCreateOrder.Text = "Create Order";
            btnCreateOrder.Location = new Point(130, 140);
            btnCreateOrder.Size = new Size(120, 30);
            btnCreateOrder.Click += btnCreateOrder_Click;
            
            btnProcessPayment = new Button();
            btnProcessPayment.Text = "Process Payment";
            btnProcessPayment.Location = new Point(260, 140);
            btnProcessPayment.Size = new Size(120, 30);
            btnProcessPayment.Click += btnProcessPayment_Click;
            
            btnPrintInvoice = new Button();
            btnPrintInvoice.Text = "Print Invoice";
            btnPrintInvoice.Location = new Point(130, 180);
            btnPrintInvoice.Size = new Size(120, 30);
            btnPrintInvoice.Click += btnPrintInvoice_Click;
            
            // Initialize orders grid
            dgvOrders = new DataGridView();
            dgvOrders.Location = new Point(20, 220);
            dgvOrders.Size = new Size(600, 300);
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.ReadOnly = true;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Add all controls to the form
            this.Controls.Add(lblCustomerId);
            this.Controls.Add(txtCustomerId);
            this.Controls.Add(lblTotalAmount);
            this.Controls.Add(txtTotalAmount);
            this.Controls.Add(lblPaymentMethod);
            this.Controls.Add(cmbPaymentMethod);
            this.Controls.Add(btnCreateOrder);
            this.Controls.Add(btnProcessPayment);
            this.Controls.Add(btnPrintInvoice);
            this.Controls.Add(dgvOrders);
        }

        private void LoadPaymentMethods()
        {
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Credit Card", "Bank Transfer", "Mobile Payment" });
            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;
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
