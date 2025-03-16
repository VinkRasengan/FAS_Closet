using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class CustomerManagementForm : Form
    {
        public CustomerManagementForm()
        {
            InitializeComponent();
            LoadCustomers();
            panelCustomerDetails.Visible = false;
        }
        
        private void LoadCustomers()
        {
            var customers = CustomerManager.GetCustomers();
            dataGridViewCustomers.DataSource = customers;
        }
        
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            ClearCustomerDetails();
            panelCustomerDetails.Visible = true;
        }
        
        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count > 0)
            {
                var selectedCustomer = dataGridViewCustomers.SelectedRows[0].DataBoundItem as Customer;
                if (selectedCustomer != null)
                {
                    LoadCustomerDetails(selectedCustomer);
                    panelCustomerDetails.Visible = true;
                }
            }
        }
        
        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count > 0)
            {
                var selectedCustomer = dataGridViewCustomers.SelectedRows[0].DataBoundItem as Customer;
                if (selectedCustomer != null)
                {
                    CustomerManager.DeleteCustomer(selectedCustomer.CustomerID);
                    LoadCustomers();
                }
            }
        }
        
        private void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            Customer customer;
            if (string.IsNullOrWhiteSpace(txtCustomerID.Text))
            {
                // Thêm mới khách hàng
                customer = new Customer
                {
                    Name = txtName.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text
                };
            }
            else
            {
                customer = new Customer
                {
                    CustomerID = int.Parse(txtCustomerID.Text),
                    Name = txtName.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text
                };
            }
            
            if (customer.CustomerID == 0)
                CustomerManager.AddCustomer(customer);
            else
                CustomerManager.UpdateCustomer(customer);
                
            LoadCustomers();
            panelCustomerDetails.Visible = false;
        }
        
        private void LoadCustomerDetails(Customer customer)
        {
            txtCustomerID.Text = customer.CustomerID.ToString();
            txtName.Text = customer.Name;
            txtEmail.Text = customer.Email;
            txtPhone.Text = customer.Phone;
            txtAddress.Text = customer.Address;
        }
        
        private void ClearCustomerDetails()
        {
            txtCustomerID.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
        }
    }
}
