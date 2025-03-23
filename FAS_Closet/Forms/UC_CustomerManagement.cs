using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    // Use a non-partial class to avoid conflict with Designer.cs
    public class UcCustomerManagement : UserControl
    {
        // Fixed nullable fields
        private TextBox? txtCustomerName;
        private TextBox? txtCustomerEmail;
        private TextBox? txtCustomerPhone;
        private TextBox? txtCustomerAddress;
        private TextBox? txtCustomerId;
        private TextBox? txtLoyaltyPoints;
        private DataGridView? dataGridViewPurchaseHistory;
        private TextBox? txtSearchCustomer;
        private DataGridView? dgvCustomers;
        private System.ComponentModel.IContainer? components = null;

        public UcCustomerManagement()
        {
            InitializeUserInterface();
            LoadCustomers();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeUserInterface()
        {
            components = new System.ComponentModel.Container();
            
            // Create all controls
            txtCustomerName = new TextBox();
            txtCustomerEmail = new TextBox();
            txtCustomerPhone = new TextBox();
            txtCustomerAddress = new TextBox();
            txtCustomerId = new TextBox();
            txtLoyaltyPoints = new TextBox();
            dataGridViewPurchaseHistory = new DataGridView();
            txtSearchCustomer = new TextBox();
            dgvCustomers = new DataGridView();
            
            Button btnSaveCustomerInfo = new Button();
            Button btnViewPurchaseHistory = new Button();
            Button btnManageLoyaltyPoints = new Button();
            
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

            // Configure textboxes
            txtCustomerName.Location = new Point(130, 20);
            txtCustomerName.Size = new Size(200, 25);
            txtCustomerName.PlaceholderText = "Enter customer name";

            txtCustomerEmail.Location = new Point(130, 55);
            txtCustomerEmail.Size = new Size(200, 25);
            txtCustomerEmail.PlaceholderText = "Enter email";

            txtCustomerPhone.Location = new Point(130, 90);
            txtCustomerPhone.Size = new Size(200, 25);
            txtCustomerPhone.PlaceholderText = "Enter phone number";

            txtCustomerAddress.Location = new Point(130, 125);
            txtCustomerAddress.Size = new Size(200, 50);
            txtCustomerAddress.PlaceholderText = "Enter address";
            txtCustomerAddress.Multiline = true;

            txtCustomerId.Location = new Point(460, 20);
            txtCustomerId.Size = new Size(100, 25);
            txtCustomerId.PlaceholderText = "ID";
            txtCustomerId.ReadOnly = true;

            txtLoyaltyPoints.Location = new Point(460, 55);
            txtLoyaltyPoints.Size = new Size(100, 25);
            txtLoyaltyPoints.PlaceholderText = "Points";
            txtLoyaltyPoints.ReadOnly = true;
            
            txtSearchCustomer.Location = new Point(460, 90);
            txtSearchCustomer.Size = new Size(150, 25);
            txtSearchCustomer.PlaceholderText = "Search customers";
            txtSearchCustomer.TextChanged += TxtSearchCustomer_TextChanged;

            // Configure buttons
            btnSaveCustomerInfo.Text = "Save Customer";
            btnSaveCustomerInfo.Location = new Point(130, 190);
            btnSaveCustomerInfo.Size = new Size(120, 30);
            btnSaveCustomerInfo.Click += btnSaveCustomerInfo_Click;

            btnViewPurchaseHistory.Text = "View History";
            btnViewPurchaseHistory.Location = new Point(260, 190);
            btnViewPurchaseHistory.Size = new Size(120, 30);
            btnViewPurchaseHistory.Click += btnViewPurchaseHistory_Click;

            btnManageLoyaltyPoints.Text = "Loyalty Points";
            btnManageLoyaltyPoints.Location = new Point(390, 190);
            btnManageLoyaltyPoints.Size = new Size(120, 30);
            btnManageLoyaltyPoints.Click += btnManageLoyaltyPoints_Click;

            // Configure DataGridViews
            dgvCustomers.Location = new Point(20, 230);
            dgvCustomers.Size = new Size(280, 200);
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;
            
            dataGridViewPurchaseHistory.Location = new Point(320, 230);
            dataGridViewPurchaseHistory.Size = new Size(300, 200);
            dataGridViewPurchaseHistory.AllowUserToAddRows = false;
            dataGridViewPurchaseHistory.ReadOnly = true;
            dataGridViewPurchaseHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add controls to form
            this.Controls.Add(lblCustomerName);
            this.Controls.Add(txtCustomerName);
            this.Controls.Add(lblCustomerEmail);
            this.Controls.Add(txtCustomerEmail);
            this.Controls.Add(lblCustomerPhone);
            this.Controls.Add(txtCustomerPhone);
            this.Controls.Add(lblCustomerAddress);
            this.Controls.Add(txtCustomerAddress);
            this.Controls.Add(lblCustomerId);
            this.Controls.Add(txtCustomerId);
            this.Controls.Add(lblLoyaltyPoints);
            this.Controls.Add(txtSearchCustomer);
            this.Controls.Add(btnSaveCustomerInfo);
            this.Controls.Add(btnViewPurchaseHistory);
            this.Controls.Add(btnManageLoyaltyPoints);
            this.Controls.Add(dgvCustomers);
            this.Controls.Add(dataGridViewPurchaseHistory);
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

        private void dgvCustomers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCustomers?.SelectedRows.Count > 0 && dgvCustomers.SelectedRows[0].DataBoundItem is Customer customer)
            {
                if (txtCustomerId != null && 
                    txtCustomerName != null && 
                    txtCustomerEmail != null && 
                    txtCustomerPhone != null && 
                    txtCustomerAddress != null && 
                    txtLoyaltyPoints != null)
                {
                    txtCustomerId.Text = customer.CustomerID.ToString();
                    txtCustomerName.Text = customer.Name;
                    txtCustomerEmail.Text = customer.Email;
                    txtCustomerPhone.Text = customer.Phone;
                    txtCustomerAddress.Text = customer.Address;
                    
                    try
                    {
                        int loyaltyPoints = CustomerManager.GetLoyaltyPointsByCustomerId(customer.CustomerID);
                        txtLoyaltyPoints.Text = loyaltyPoints.ToString();
                    }
                    catch
                    {
                        txtLoyaltyPoints.Text = "0";
                    }
                }
            }
        }

        private void TxtSearchCustomer_TextChanged(object? sender, EventArgs e)
        {
            string searchTerm = txtSearchCustomer!.Text.ToLower();
            
            try
            {
                var allCustomers = CustomerManager.GetCustomers();
                var filteredCustomers = allCustomers
                    .Where(c => c.Name.ToLower().Contains(searchTerm) || 
                               c.Email.ToLower().Contains(searchTerm) ||
                               c.Phone.Contains(searchTerm))
                    .ToList();
                
                dgvCustomers!.DataSource = filteredCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}");
            }
        }

        private void btnSaveCustomerInfo_Click(object? sender, EventArgs e)
        {
            if (txtCustomerName == null || 
                txtCustomerEmail == null || 
                txtCustomerPhone == null || 
                txtCustomerAddress == null ||
                txtCustomerId == null)
            {
                MessageBox.Show("UI components not initialized properly");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerPhone.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerAddress.Text))
            {
                MessageBox.Show("All fields are required");
                return;
            }
            
            try
            {
                var customer = new Customer
                {
                    Name = txtCustomerName.Text,
                    Email = txtCustomerEmail.Text,
                    Phone = txtCustomerPhone.Text,
                    Address = txtCustomerAddress.Text
                };
                
                if (!string.IsNullOrEmpty(txtCustomerId.Text) && int.TryParse(txtCustomerId.Text, out int customerId))
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

        private void btnViewPurchaseHistory_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId!.Text) || !int.TryParse(txtCustomerId.Text, out int customerId))
            {
                MessageBox.Show("Please select a customer first");
                return;
            }
            
            try
            {
                var orders = OrderManager.GetOrdersByCustomerId(customerId);
                dataGridViewPurchaseHistory!.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading purchase history: {ex.Message}");
            }
        }

        private void btnManageLoyaltyPoints_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId!.Text) || !int.TryParse(txtCustomerId.Text, out int _))
            {
                MessageBox.Show("Please select a customer first");
                return;
            }
            
            MessageBox.Show("Loyalty points management feature is coming soon");
        }
        
        private void ClearForm()
        {
            txtCustomerId?.Clear();
            txtCustomerName?.Clear();
            txtCustomerEmail?.Clear();
            txtCustomerPhone?.Clear();
            txtCustomerAddress?.Clear();
            txtLoyaltyPoints?.Clear();
        }
    }
}
