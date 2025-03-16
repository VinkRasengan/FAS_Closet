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
            var productForm = new ProductForm();
            productForm.MdiParent = this;
            productForm.Show();
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
            var customerForm = new CustomerForm();
            customerForm.MdiParent = this;
            customerForm.Show();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportForm();
            reportForm.MdiParent = this;
            reportForm.Show();
        }
    }
}
