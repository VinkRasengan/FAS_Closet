using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = CustomerManager.GetCustomers();
            dataGridViewCustomers.DataSource = customers;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            var customerEditorForm = new CustomerEditorForm();
            if (customerEditorForm.ShowDialog() == DialogResult.OK)
            {
                LoadCustomers();
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count > 0)
            {
                var selectedCustomer = (Customer)dataGridViewCustomers.SelectedRows[0].DataBoundItem;
                var customerEditorForm = new CustomerEditorForm(selectedCustomer);
                if (customerEditorForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers();
                }
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count > 0)
            {
                var selectedCustomer = (Customer)dataGridViewCustomers.SelectedRows[0].DataBoundItem;
                CustomerManager.DeleteCustomer(selectedCustomer.CustomerID);
                LoadCustomers();
            }
        }
    }
}
