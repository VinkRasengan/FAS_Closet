using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcCustomerManagement : UserControl
    {
        public TextBox TxtCustomerName = new TextBox();
        public TextBox TxtCustomerEmail = new TextBox();
        public TextBox TxtCustomerPhone = new TextBox();
        public TextBox TxtCustomerAddress = new TextBox();
        public TextBox TxtCustomerId = new TextBox();
        public TextBox TxtLoyaltyPoints = new TextBox();
        public DataGridView DataGridViewPurchaseHistory = new DataGridView();
        public TextBox TxtSearchCustomer = new TextBox();

        private Button btnSaveCustomerInfo;
        private Button btnViewPurchaseHistory;
        private Button btnManageLoyaltyPoints;
        private DataGridView dgvCustomers;
        
        public UcCustomerManagement()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            // Create labels
            Label lblCustomerName = new Label();
            lblCustomerName.Text = "Name:";
            lblCustomerName.Location = new Point(20, 20);
            lblCustomerName.Size = new Size(100, 25);
            lblCustomerName.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerEmail = new Label();
            lblCustomerEmail.Text = "Email:";
            lblCustomerEmail.Location = new Point(20, 55);
            lblCustomerEmail.Size = new Size(100, 25);
            lblCustomerEmail.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerPhone = new Label();
            lblCustomerPhone.Text = "Phone:";
            lblCustomerPhone.Location = new Point(20, 90);
            lblCustomerPhone.Size = new Size(100, 25);
            lblCustomerPhone.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerAddress = new Label();
            lblCustomerAddress.Text = "Address:";
            lblCustomerAddress.Location = new Point(20, 125);
            lblCustomerAddress.Size = new Size(100, 25);
            lblCustomerAddress.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerId = new Label();
            lblCustomerId.Text = "Customer ID:";
            lblCustomerId.Location = new Point(350, 20);
            lblCustomerId.Size = new Size(100, 25);
            lblCustomerId.TextAlign = ContentAlignment.MiddleRight;

            Label lblLoyaltyPoints = new Label();
            lblLoyaltyPoints.Text = "Loyalty Points:";
            lblLoyaltyPoints.Location = new Point(350, 55);
            lblLoyaltyPoints.Size = new Size(100, 25);
            lblLoyaltyPoints.TextAlign = ContentAlignment.MiddleRight;

            // Create textboxes
            TxtCustomerName = new TextBox();
            TxtCustomerName.Location = new Point(130, 20);
            TxtCustomerName.Size = new Size(200, 25);
            TxtCustomerName.PlaceholderText = "Enter customer name";

            TxtCustomerEmail = new TextBox();
            TxtCustomerEmail.Location = new Point(130, 55);
            TxtCustomerEmail.Size = new Size(200, 25);
            TxtCustomerEmail.PlaceholderText = "Enter email";

            TxtCustomerPhone = new TextBox();
            TxtCustomerPhone.Location = new Point(130, 90);
            TxtCustomerPhone.Size = new Size(200, 25);
            TxtCustomerPhone.PlaceholderText = "Enter phone number";

            TxtCustomerAddress = new TextBox();
            TxtCustomerAddress.Location = new Point(130, 125);
            TxtCustomerAddress.Size = new Size(200, 50);
            TxtCustomerAddress.PlaceholderText = "Enter address";
            TxtCustomerAddress.Multiline = true;

            TxtCustomerId = new TextBox();
            TxtCustomerId.Location = new Point(460, 20);
            TxtCustomerId.Size = new Size(100, 25);
            TxtCustomerId.PlaceholderText = "ID";
            TxtCustomerId.ReadOnly = true;

            TxtLoyaltyPoints = new TextBox();
            TxtLoyaltyPoints.Location = new Point(460, 55);
            TxtLoyaltyPoints.Size = new Size(100, 25);
            TxtLoyaltyPoints.PlaceholderText = "Points";
            TxtLoyaltyPoints.ReadOnly = true;
            
            TxtSearchCustomer = new TextBox();
            TxtSearchCustomer.Location = new Point(460, 90);
            TxtSearchCustomer.Size = new Size(150, 25);
            TxtSearchCustomer.PlaceholderText = "Search customers";
            TxtSearchCustomer.TextChanged += TxtSearchCustomer_TextChanged;

            // Create buttons
            btnSaveCustomerInfo = new Button();
            btnSaveCustomerInfo.Text = "Save Customer";
            btnSaveCustomerInfo.Location = new Point(130, 190);
            btnSaveCustomerInfo.Size = new Size(120, 30);
            btnSaveCustomerInfo.Click += btnSaveCustomerInfo_Click;

            btnViewPurchaseHistory = new Button();
            btnViewPurchaseHistory.Text = "View History";
            btnViewPurchaseHistory.Location = new Point(260, 190);
            btnViewPurchaseHistory.Size = new Size(120, 30);
            btnViewPurchaseHistory.Click += btnViewPurchaseHistory_Click;

            btnManageLoyaltyPoints = new Button();
            btnManageLoyaltyPoints.Text = "Loyalty Points";
            btnManageLoyaltyPoints.Location = new Point(390, 190);
            btnManageLoyaltyPoints.Size = new Size(120, 30);
            btnManageLoyaltyPoints.Click += btnManageLoyaltyPoints_Click;

            // Create DataGridViews
            dgvCustomers = new DataGridView();
            dgvCustomers.Location = new Point(20, 230);
            dgvCustomers.Size = new Size(280, 200);
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;
            
            DataGridViewPurchaseHistory = new DataGridView();
            DataGridViewPurchaseHistory.Location = new Point(320, 230);
            DataGridViewPurchaseHistory.Size = new Size(300, 200);
            DataGridViewPurchaseHistory.AllowUserToAddRows = false;
            DataGridViewPurchaseHistory.ReadOnly = true;
            DataGridViewPurchaseHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add controls to form
            this.Controls.Add(lblCustomerName);
            this.Controls.Add(TxtCustomerName);
            this.Controls.Add(lblCustomerEmail);
            this.Controls.Add(TxtCustomerEmail);
            this.Controls.Add(lblCustomerPhone);
            this.Controls.Add(TxtCustomerPhone);
            this.Controls.Add(lblCustomerAddress);
            this.Controls.Add(TxtCustomerAddress);
            this.Controls.Add(lblCustomerId);
            this.Controls.Add(TxtCustomerId);
            this.Controls.Add(lblLoyaltyPoints);
            this.Controls.Add(TxtLoyaltyPoints);
            this.Controls.Add(TxtSearchCustomer);
            this.Controls.Add(btnSaveCustomerInfo);
            this.Controls.Add(btnViewPurchaseHistory);
            this.Controls.Add(btnManageLoyaltyPoints);
            this.Controls.Add(dgvCustomers);
            this.Controls.Add(DataGridViewPurchaseHistory);
        }
        
        private void LoadCustomers()
        {
            try
            {
                var customers = CustomerManager.GetCustomers();
                dgvCustomers.DataSource = customers;
                
                if (dgvCustomers.Columns.Count > 0)
                {
                    dgvCustomers.Columns["Address"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}");
            }
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var customer = dgvCustomers.SelectedRows[0].DataBoundItem as Customer;
                if (customer != null)
                {
                    TxtCustomerId.Text = customer.CustomerID.ToString();
                    TxtCustomerName.Text = customer.Name;
                    TxtCustomerEmail.Text = customer.Email;
                    TxtCustomerPhone.Text = customer.Phone;
                    TxtCustomerAddress.Text = customer.Address;
                    
                    try
                    {
                        int loyaltyPoints = CustomerManager.GetLoyaltyPointsByCustomerId(customer.CustomerID);
                        TxtLoyaltyPoints.Text = loyaltyPoints.ToString();
                    }
                    catch
                    {
                        TxtLoyaltyPoints.Text = "0";
                    }
                }
            }
        }

        private void TxtSearchCustomer_TextChanged(object? sender, EventArgs e)
        {
            string searchTerm = TxtSearchCustomer.Text.ToLower();
            
            try
            {
                var allCustomers = CustomerManager.GetCustomers();
                var filteredCustomers = allCustomers
                    .Where(c => c.Name.ToLower().Contains(searchTerm) || 
                               c.Email.ToLower().Contains(searchTerm) ||
                               c.Phone.Contains(searchTerm))
                    .ToList();
                
                dgvCustomers.DataSource = filteredCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}");
            }
        }

        private void btnSaveCustomerInfo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(TxtCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtCustomerPhone.Text) ||
                string.IsNullOrWhiteSpace(TxtCustomerAddress.Text))
            {
                MessageBox.Show("All fields are required");
                return;
            }
            
            try
            {
                var customer = new Customer
                {
                    Name = TxtCustomerName.Text,
                    Email = TxtCustomerEmail.Text,
                    Phone = TxtCustomerPhone.Text,
                    Address = TxtCustomerAddress.Text
                };
                
                if (!string.IsNullOrEmpty(TxtCustomerId.Text) && int.TryParse(TxtCustomerId.Text, out int customerId))
                {
                    // Update existing customer
                    customer.CustomerID = customerId;
                    CustomerManager.UpdateCustomer(customer);
                    MessageBox.Show("Customer updated successfully");
                }
                else
                {
                    // Add new customer
                    CustomerManager.AddCustomer(customer);
                    MessageBox.Show("Customer added successfully");
                    ClearForm();
                }
                
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving customer: {ex.Message}");
            }
        }

        private void btnViewPurchaseHistory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtCustomerId.Text) || !int.TryParse(TxtCustomerId.Text, out int customerId))
            {
                MessageBox.Show("Please select a customer first");
                return;
            }
            
            try
            {
                var orders = OrderManager.GetOrdersByCustomerId(customerId);
                DataGridViewPurchaseHistory.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading purchase history: {ex.Message}");
            }
        }

        private void btnManageLoyaltyPoints_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtCustomerId.Text) || !int.TryParse(TxtCustomerId.Text, out int _))
            {
                MessageBox.Show("Please select a customer first");
                return;
            }
            
            MessageBox.Show("Loyalty points management feature is coming soon");
        }
        
        private void ClearForm()
        {
            TxtCustomerId.Clear();
            TxtCustomerName.Clear();
            TxtCustomerEmail.Clear();
            TxtCustomerPhone.Clear();
            TxtCustomerAddress.Clear();
            TxtLoyaltyPoints.Clear();
        }
    }
}
