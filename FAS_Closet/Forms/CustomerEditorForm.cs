using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class CustomerEditorForm : Form
    {
        private Customer _customer;

        public CustomerEditorForm(Customer? customer = null)
        {
            InitializeComponent();
            _customer = customer ?? new Customer
            {
                Name = string.Empty,
                Email = string.Empty,
                Phone = string.Empty,
                Address = string.Empty
            };
            if (customer != null)
            {
                txtName.Text = customer.Name;
                txtEmail.Text = customer.Email;
                txtPhone.Text = customer.Phone;
                txtAddress.Text = customer.Address;
                txtLoyaltyPoints.Text = customer.LoyaltyPoints.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _customer.Name = txtName.Text;
            _customer.Email = txtEmail.Text;
            _customer.Phone = txtPhone.Text;
            _customer.Address = txtAddress.Text;
            _customer.LoyaltyPoints = int.Parse(txtLoyaltyPoints.Text);

            if (_customer.CustomerID == 0)
            {
                CustomerManager.AddCustomer(_customer);
            }
            else
            {
                CustomerManager.UpdateCustomer(_customer);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
