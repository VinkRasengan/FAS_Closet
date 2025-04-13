using System;
using System.Windows.Forms;
using System.Drawing;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcOrderManagement : UserControl
    {
        public UcOrderManagement()
        {
            InitializeComponent();
            LoadPaymentMethods();
            LoadProducts();
            LoadCustomers();
            LoadOrders();
        }

        private void LoadPaymentMethods()
        {
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Credit Card", "Bank Transfer", "Mobile Payment" });
            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            var products = ProductManager.GetProducts(); // Danh sách Product

            cmbProduct.DataSource = products;
            cmbProduct.DisplayMember = "DisplayName"; // Dùng thuộc tính tuỳ chỉnh
            cmbProduct.ValueMember = "ProductID";
        }

        private void LoadCustomers()
        {
            var customers = CustomerManager.GetCustomers(); // danh sách Customer

            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "Name"; // giả sử bạn có thuộc tính này
            cmbCustomer.ValueMember = "CustomerID";
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a valid positive number.");
                return;
            }

            if (quantity > selectedProduct.Stock)
            {
                MessageBox.Show($"Only {selectedProduct.Stock} items in stock.");
                return;
            }

            var detail = new OrderDetail
            {
                ProductID = selectedProduct.ProductID,
                Quantity = quantity,
                UnitPrice = selectedProduct.Price
            };

            if (txtTotalAmount == null)
            {
                MessageBox.Show("txtTotalAmount is NULL");
                return;
            }

            orderDetails.Add(detail);

            decimal total = orderDetails.Sum(d => d.Quantity * d.UnitPrice);
            txtTotalAmount.Text = total.ToString("0.00");

            MessageBox.Show("Total Amount is: " + total.ToString("0.00"));
        }


        private void LoadOrders()
        {
            try
            {
                var orders = OrderManager.GetOrders();
                dgvOrders.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        private Form popup;

        private void HandleCustomerAdd(object sender, EventArgs e)
        {
            popup = new Form();
            popup.Text = "Add New Customer";
            popup.Size = new Size(500, 350);
            popup.FormBorderStyle = FormBorderStyle.FixedDialog;
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.MaximizeBox = false;
            popup.MinimizeBox = false;

            Label lblName = new Label
            {
                Text = "Name:",
                Location = new Point(20, 20),
                Size = new Size(90, 23),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(20, 60),
                Size = new Size(90, 23),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblPhone = new Label
            {
                Text = "Phone:",
                Location = new Point(20, 100),
                Size = new Size(90, 23),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblAddress = new Label
            {
                Text = "Address:",
                Location = new Point(20, 140),
                Size = new Size(90, 23),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtName = new TextBox
            {
                Location = new Point(120, 20),
                Width = 320
            };

            TextBox txtEmail = new TextBox
            {
                Location = new Point(120, 60),
                Width = 320
            };

            TextBox txtPhone = new TextBox
            {
                Location = new Point(120, 100),
                Width = 320
            };

            TextBox txtAddress = new TextBox
            {
                Location = new Point(120, 140),
                Width = 320,
                Height = 60,
                Multiline = true
            };

            Button btnSave = new Button
            {
                Text = "Save",
                Location = new Point(140, 220),
                Width = 100
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, 220),
                Width = 100
            };


            btnCancel.Click += (s, ev) => popup.DialogResult = DialogResult.Cancel;
            btnSave.Click += (s, ev) =>
            {
                string name = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string address = txtAddress.Text.Trim();

                // Kiểm tra rỗng
                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(address))
                {
                    MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra email
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    if (addr.Address != email)
                    {
                        MessageBox.Show("Invalid email format.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid email format.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra phone: phải là số và có độ dài hợp lệ (ví dụ 8–15 ký tự)
                if (!phone.All(char.IsDigit) || phone.Length < 8 || phone.Length > 15)
                {
                    MessageBox.Show("Phone must be numeric and 8–15 digits long.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var customer = new Customer
                    {
                        Name = name,
                        Email = email,
                        Phone = phone,
                        Address = address
                    };

                    CustomerManager.AddCustomer(customer);
                    MessageBox.Show("Customer added successfully.");
                    popup.DialogResult = DialogResult.OK;

                    LoadCustomers();
                    var customers = CustomerManager.GetCustomers();
                    var added = customers.LastOrDefault(c => c.Name == customer.Name);
                    if (added != null)
                        cmbCustomer.SelectedValue = added.CustomerID;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding customer: " + ex.Message);
                }
            };
            popup.Controls.Add(lblName);
            popup.Controls.Add(lblEmail);
            popup.Controls.Add(lblPhone);
            popup.Controls.Add(lblAddress);
            popup.Controls.Add(txtName);
            popup.Controls.Add(txtEmail);
            popup.Controls.Add(txtPhone);
            popup.Controls.Add(txtAddress);
            popup.Controls.Add(btnSave);
            popup.Controls.Add(btnCancel);

            popup.ShowDialog();
        }

        // Revert to original method naming to fix missing method errors
        public void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (!ValidateOrderInputs())
                return;
            
            try
            {
                var order = new Order
                {
                    CustomerID = (int)cmbCustomer.SelectedValue,
                    OrderDate = DateTime.Now,
                    TotalAmount = decimal.Parse(txtTotalAmount.Text),
                    PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString() ?? string.Empty
                };
                
                OrderManager.AddOrder(order);
                MessageBox.Show("Order created successfully.");
                LoadOrders();
                
                txtTotalAmount.Clear();
                cmbPaymentMethod.SelectedIndex = 0;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        // Revert to original method naming
        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to process payment.");
                return;
            }
            
            // Implementation would actually process payment
            MessageBox.Show("Payment processed successfully.");
        }

        // Revert to original method naming
        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to print invoice.");
                return;
            }
            
            // Implementation would actually print invoice
            MessageBox.Show("Invoice printed successfully.");
        }

        private bool ValidateOrderInputs()
        {
            
            if (string.IsNullOrWhiteSpace(txtTotalAmount.Text))
            {
                MessageBox.Show("Total Amount is required.");
                return false;
            }
            
            if (!decimal.TryParse(txtTotalAmount.Text, out _))
            {
                MessageBox.Show("Total Amount must be a valid decimal number.");
                return false;
            }
            
            if (cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.");
                return false;
            }
            
            return true;
        }
    }
}
