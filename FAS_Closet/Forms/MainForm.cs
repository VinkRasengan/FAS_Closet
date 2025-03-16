using System;
using System.Windows.Forms;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(User user)
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome, " + user.Name;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var productInventoryManagementForm = new ProductInventoryManagementForm();
            productInventoryManagementForm.MdiParent = this;
            productInventoryManagementForm.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inventoryForm = new InventoryForm();
            inventoryForm.MdiParent = this;
            inventoryForm.Show();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var orderForm = new OrderForm();
            orderForm.MdiParent = this;
            orderForm.Show();
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var customerManagementForm = new CustomerManagementForm();
            customerManagementForm.MdiParent = this;
            customerManagementForm.Show();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportForm();
            reportForm.MdiParent = this;
            reportForm.Show();
        }
    }
}
