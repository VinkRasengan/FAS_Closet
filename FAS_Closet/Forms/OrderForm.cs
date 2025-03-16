using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            var orders = OrderManager.GetOrders();
            dataGridViewOrders.DataSource = orders;
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            var orderEditorForm = new OrderEditorForm();
            if (orderEditorForm.ShowDialog() == DialogResult.OK)
            {
                LoadOrders();
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                var selectedOrder = dataGridViewOrders.SelectedRows[0].DataBoundItem as Order;
                if (selectedOrder != null)
                {
                    var orderEditorForm = new OrderEditorForm(selectedOrder);
                    if (orderEditorForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadOrders();
                    }
                }
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                var selectedOrder = dataGridViewOrders.SelectedRows[0].DataBoundItem as Order;
                if (selectedOrder != null)
                {
                    OrderManager.DeleteOrder(selectedOrder.OrderID);
                    LoadOrders();
                }
            }
        }
    }
}
