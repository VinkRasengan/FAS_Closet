using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class OrderEditorForm : Form
    {
        private Order _order;

        public OrderEditorForm(Order? order = null)
        {
            InitializeComponent();
            _order = order ?? new Order
            {
                PaymentMethod = string.Empty
            };
            if (order != null)
            {
                txtCustomerID.Text = order.CustomerID.ToString();
                txtTotalAmount.Text = order.TotalAmount.ToString();
                txtPaymentMethod.Text = order.PaymentMethod;
                // Load other order details if necessary
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _order.CustomerID = int.Parse(txtCustomerID.Text);
            _order.TotalAmount = decimal.Parse(txtTotalAmount.Text);
            _order.PaymentMethod = txtPaymentMethod.Text;
            _order.OrderDate = DateTime.Now;

            if (_order.OrderID == 0)
            {
                OrderManager.AddOrder(_order);
            }
            else
            {
                OrderManager.UpdateOrder(_order);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
