using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;
using System.ComponentModel; // Add this line

namespace FASCloset.Forms
{
    public partial class UcCustomerManagement : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required TextBox TxtCustomerName { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required TextBox TxtCustomerEmail { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required TextBox TxtCustomerPhone { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required TextBox TxtCustomerAddress { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required TextBox TxtCustomerId { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required TextBox TxtLoyaltyPoints { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required DataGridView DataGridViewPurchaseHistory { get; set; }

        public UcCustomerManagement()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            TxtCustomerName = new TextBox();
            TxtCustomerEmail = new TextBox();
            TxtCustomerPhone = new TextBox();
            TxtCustomerAddress = new TextBox();
            TxtCustomerId = new TextBox();
            TxtLoyaltyPoints = new TextBox();
            DataGridViewPurchaseHistory = new DataGridView();
            // Initialize other components and set properties
            this.Controls.Add(TxtCustomerName);
            this.Controls.Add(TxtCustomerEmail);
            this.Controls.Add(TxtCustomerPhone);
            this.Controls.Add(TxtCustomerAddress);
            this.Controls.Add(TxtCustomerId);
            this.Controls.Add(TxtLoyaltyPoints);
            this.Controls.Add(DataGridViewPurchaseHistory);
        }

        private void btnSaveCustomerInfo_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = new Customer
                {
                    Name = TxtCustomerName.Text,
                    Email = TxtCustomerEmail.Text,
                    Phone = TxtCustomerPhone.Text,
                    Address = TxtCustomerAddress.Text
                };
                CustomerManager.AddCustomer(customer);
                MessageBox.Show("Customer information saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnViewPurchaseHistory_Click(object sender, EventArgs e)
        {
            int customerId = int.Parse(TxtCustomerId.Text);
            var purchaseHistory = OrderManager.GetOrdersByCustomerId(customerId);
            DataGridViewPurchaseHistory.DataSource = purchaseHistory;
        }

        private void btnManageLoyaltyPoints_Click(object sender, EventArgs e)
        {
            int customerId = int.Parse(TxtCustomerId.Text);
            var loyaltyPoints = CustomerManager.GetLoyaltyPointsByCustomerId(customerId);
            TxtLoyaltyPoints.Text = loyaltyPoints.ToString();
        }
    }
}
