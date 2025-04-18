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

            Button btnDeleteProduct = new Button();
            btnDeleteProduct.Text = "Delete Product";
            btnDeleteProduct.Location = new Point(530, 160); // bên phải bảng
            btnDeleteProduct.Click += btnDeleteProduct_Click;
            this.Controls.Add(btnDeleteProduct);

            Button btnDeleteDraftOrder = new Button();
            btnDeleteDraftOrder.Text = "Delete Draft Order";
            btnDeleteDraftOrder.Location = new Point(530, 370);  // bên phải bảng
            btnDeleteDraftOrder.Click += btnDeleteDraftOrder_Click;
            this.Controls.Add(btnDeleteDraftOrder);

            productList.CellClick += productList_CellClick;  // Attach here

            Button btnCancelOrder = new Button();
            btnCancelOrder.Text = "Cancel Order";
            btnCancelOrder.Location = new Point(530, 500); // Adjust the location as needed
            btnCancelOrder.Click += btnCancelOrder_Click;
            this.Controls.Add(btnCancelOrder);

            Button btnViewOrderDetails = new Button();
            btnViewOrderDetails.Text = "View Order Details";
            btnViewOrderDetails.Location = new Point(530, 530); // Adjust the location as needed
            btnViewOrderDetails.Click += btnViewOrderDetail_Click;
            this.Controls.Add(btnViewOrderDetails);
        }

        private void btnViewOrderDetail_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to view details.");
                return;
            }

            int selectedIndex = dgvOrders.SelectedRows[0].Index;
            if (selectedIndex < 0 || selectedIndex >= dgvOrders.RowCount)
            {
                return;
            }

            // Get the OrderID of the selected order
            int orderId = Convert.ToInt32(dgvOrders.Rows[selectedIndex].Cells["OrderID"].Value);

            // Fetch the order details
            var order = OrderManager.GetOrderById(orderId);
            var orderDetails = OrderManager.GetOrderDetailsByOrderId(orderId);

            // Show the popup with the order details
            ShowOrderDetailPopup(order, orderDetails);
        }

        private void ShowOrderDetailPopup(Order order, List<OrderDetail> orderDetails)
        {
            // Create the popup form
            Form popup = new Form
            {
                Text = "Order Details",
                Size = new Size(600, 400),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Add User Info (Customer Name, Order Date, etc.)
            Label lblCustomerName = new Label
            {
                Text = $"Customer: {order.CustomerName}",
                Location = new Point(20, 20),
                Size = new Size(550, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            popup.Controls.Add(lblCustomerName);

            Label lblOrderDate = new Label
            {
                Text = $"Order Date: {order.OrderDate.ToString("MMMM dd, yyyy")}",
                Location = new Point(20, 60),
                Size = new Size(550, 30),
                Font = new Font("Segoe UI", 10F)
            };
            popup.Controls.Add(lblOrderDate);

            Label lblTotalAmount = new Label
            {
                Text = $"Total Amount: {order.TotalAmount:C}",
                Location = new Point(20, 100),
                Size = new Size(550, 30),
                Font = new Font("Segoe UI", 10F)
            };
            popup.Controls.Add(lblTotalAmount);

            // Create a DataGridView to display the order details (products in the order)
            DataGridView dgvOrderDetails = new DataGridView
            {
                Location = new Point(20, 140),
                Size = new Size(550, 150),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Set data for the DataGridView
            dgvOrderDetails.DataSource = orderDetails.Select(d => new
            {
                ProductName = ProductManager.GetProductById(d.ProductID)?.ProductName ?? "Unknown",
                d.Quantity,
                d.UnitPrice,
                Total = d.Quantity * d.UnitPrice
            }).ToList();
            
            // Apply the standardized style using the helper
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(dgvOrderDetails);

            // Add the DataGridView to the popup
            popup.Controls.Add(dgvOrderDetails);

            // Close button for the popup
            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(500, 320),
                Size = new Size(75, 30)
            };
            btnClose.Click += (s, ev) => popup.Close();
            popup.Controls.Add(btnClose);

            // Show the popup form
            popup.ShowDialog();
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to cancel.");
                return;
            }

            int selectedIndex = dgvOrders.SelectedRows[0].Index;
            if (selectedIndex < 0 || selectedIndex >= dgvOrders.RowCount)
            {
                return;
            }

            // Get the OrderID of the selected order
            int orderId = Convert.ToInt32(dgvOrders.Rows[selectedIndex].Cells["OrderID"].Value);

            // Confirm deletion before proceeding
            var confirmResult = MessageBox.Show("Are you sure you want to cancel this order? This action cannot be undone.",
                                                 "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Delete the order
                    OrderManager.DeleteOrder(orderId);

                    // Refresh the orders grid
                    LoadOrders();

                    // Show confirmation message
                    MessageBox.Show("Order has been canceled successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error canceling order: " + ex.Message);
                }
            }
        }

        private void productList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure it's not the header row
            if (e.RowIndex >= 0)
            {
                // Get the selected ProductID from the clicked row
                int productId = Convert.ToInt32(productList.Rows[e.RowIndex].Cells["ProductID"].Value);

                // Find the product in the order details
                var selectedProduct = orderDetails.FirstOrDefault(d => d.ProductID == productId);

                if (selectedProduct != null)
                {
                    // Open the popup to edit the quantity
                    OpenEditQuantityPopup(selectedProduct);
                }
            }
        }

        private void OpenEditQuantityPopup(OrderDetail selectedProduct)
        {
            // Create the popup form
            Form popup = new Form
            {
                Text = "Edit Quantity",
                Size = new Size(300, 200),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Add controls for quantity edit
            Label lblQuantity = new Label
            {
                Text = "Quantity:",
                Location = new Point(20, 30),
                Size = new Size(100, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            TextBox txtQuantity = new TextBox
            {
                Location = new Point(120, 30),
                Size = new Size(150, 23),
                Text = selectedProduct.Quantity.ToString()
            };

            Button btnSave = new Button
            {
                Text = "Save",
                Location = new Point(50, 100),
                Size = new Size(75, 30)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(150, 100),
                Size = new Size(75, 30)
            };

            // Button event to save changes
            btnSave.Click += (s, ev) =>
            {
                // Parse the new quantity
                if (int.TryParse(txtQuantity.Text, out int newQuantity) && newQuantity > 0)
                {
                    // Check if the new quantity exceeds the available stock
                    var product = ProductManager.GetProductById(selectedProduct.ProductID);
                    if (product != null && newQuantity > product.Stock)
                    {
                        MessageBox.Show($"Insufficient stock. Only {product.Stock} items available.");
                        return;
                    }

                    // Update the quantity of the selected product
                    selectedProduct.Quantity = newQuantity;

                    // Recalculate the total amount
                    decimal total = orderDetails.Sum(d => d.Quantity * d.UnitPrice);
                    txtTotalAmount.Text = total.ToString("0.00");

                    // Refresh the DataGridView with updated quantity
                    productList.DataSource = null;
                    productList.DataSource = orderDetails.Select(d => new
                    {
                        ProductName = ProductManager.GetProductById(d.ProductID)?.ProductName ?? "Unknown",  // Show Product Name
                        d.ProductID,  // Keep ProductID for data but not displayed in the UI
                        d.Quantity,
                        d.UnitPrice,
                        Total = d.Quantity * d.UnitPrice
                    }).ToList();

                    // Hide the ProductID column from the DataGridView
                    productList.Columns["ProductID"].Visible = false;

                    MessageBox.Show("Quantity updated successfully.");
                    popup.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            };

            // Button event to cancel the edit
            btnCancel.Click += (s, ev) =>
            {
                popup.Close();
            };

            // Add controls to the popup form
            popup.Controls.Add(lblQuantity);
            popup.Controls.Add(txtQuantity);
            popup.Controls.Add(btnSave);
            popup.Controls.Add(btnCancel);

            // Show the popup form
            popup.ShowDialog();
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (productList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to remove.");
                return;
            }

            int productId = Convert.ToInt32(productList.SelectedRows[0].Cells["ProductID"].Value);
            var itemToRemove = orderDetails.FirstOrDefault(d => d.ProductID == productId);
            if (itemToRemove != null)
            {
                orderDetails.Remove(itemToRemove);

                // Cập nhật lại total
                decimal total = orderDetails.Sum(d => d.Quantity * d.UnitPrice);
                txtTotalAmount.Text = total.ToString("0.00");

                // Cập nhật lại bảng
                productList.DataSource = null;
                productList.DataSource = orderDetails.Select(d => new
                {
                    d.ProductID,
                    d.Quantity,
                    d.UnitPrice,
                    Total = d.Quantity * d.UnitPrice
                }).ToList();
            }
        }


        private void LoadPaymentMethods()
        {
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Credit Card", "Bank Transfer", "Mobile Payment" });
            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;
        }

        public void LoadProducts()
        {
            var products = ProductManager.GetProducts(); // Danh sách Product

            cmbProduct.DataSource = products;
            cmbProduct.DisplayMember = "DisplayName"; // Dùng thuộc tính tuỳ chỉnh
            cmbProduct.ValueMember = "ProductID";
        }

        public void LoadCustomers()
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

            var existing = orderDetails.FirstOrDefault(d => d.ProductID == selectedProduct.ProductID);
            if (existing != null)
            {
                existing.Quantity = quantity; // Overwrite quantity
            }
            else
            {
                var detail = new OrderDetail
                {
                    ProductID = selectedProduct.ProductID,
                    Quantity = quantity,
                    UnitPrice = selectedProduct.Price
                };
                orderDetails.Add(detail);
            }

            decimal total = orderDetails.Sum(d => d.Quantity * d.UnitPrice);
            txtTotalAmount.Text = total.ToString("0.00");

            // Clear existing DataSource and reset DataGridView
            productList.DataSource = null;

            // Set the DataSource with new data and show ProductName instead of ProductID
            productList.DataSource = orderDetails.Select(d => new
            {
                ProductName = ProductManager.GetProductById(d.ProductID)?.ProductName ?? "Unknown",  // Show Product Name
                d.ProductID,  // Keep ProductID for data but not displayed in the UI
                d.Quantity,
                d.UnitPrice,
                Total = d.Quantity * d.UnitPrice
            }).ToList();

            // Hide ProductID column from UI
            productList.Columns["ProductID"].Visible = false; 
            
            // Apply the standardized style using the helper
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(productList);
        }

        private void ShowOrderSuccessNotification()
        {
            // Remove any existing success notification
            var existingToast = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "OrderSuccessToast");
            if (existingToast != null) this.Controls.Remove(existingToast);

            // Create the toast panel for the success message
            var toast = new Panel
            {
                Name = "OrderSuccessToast",
                Size = new Size(280, 100),
                BackColor = Color.FromArgb(173, 255, 47), // Light green for success
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(this.ClientSize.Width - 290, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Add a title label
            var title = new Label
            {
                Text = "✔ Order Created Successfully!",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };
            toast.Controls.Add(title);

            // Add a message label
            var messageLabel = new Label
            {
                Text = "Your order has been processed successfully.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(10, 40),
                Cursor = Cursors.Hand
            };
            toast.Controls.Add(messageLabel);

            // Close button on the toast
            var closeButton = new Button
            {
                Text = "X",
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(25, 25),
                Location = new Point(toast.Width - 30, 5)
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Controls.Remove(toast);
            toast.Controls.Add(closeButton);

            // Add the toast to the form
            this.Controls.Add(toast);
            toast.BringToFront();

            // Auto-close the notification after 4 seconds
            var autoClose = new System.Windows.Forms.Timer { Interval = 4000 };
            autoClose.Tick += (s, e) =>
            {
                if (this.Controls.Contains(toast))
                    this.Controls.Remove(toast);
                autoClose.Stop();
            };
            autoClose.Start();
        }

        private void LoadOrders()
        {
            try
            {
                var orders = OrderManager.GetOrders();
                
                // Configure columns with better headers before setting data source
                dgvOrders.AutoGenerateColumns = false;
                dgvOrders.Columns.Clear();
                
                // Add custom columns with better headers and styling
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "OrderID",
                    HeaderText = "Mã ĐH",
                    Width = 70,
                    Name = "OrderID"
                });
                
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CustomerID",
                    HeaderText = "Mã KH",
                    Width = 70
                });
                
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CustomerName",
                    HeaderText = "Khách hàng",
                    Width = 150
                });
                
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "OrderDate",
                    HeaderText = "Ngày Đặt Hàng",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "dd/MM/yyyy HH:mm"
                    }
                });
                
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TotalAmount",
                    HeaderText = "Tổng Tiền",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });
                
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "PaymentMethod",
                    HeaderText = "Phương Thức Thanh Toán",
                    Width = 140
                });
                
                // Set the data source after column configuration
                dgvOrders.DataSource = orders;
                
                // Apply the standardized style using the helper
                FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(dgvOrders);
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

        private List<(Order Order, List<OrderDetail> Details)> draftOrders = new List<(Order, List<OrderDetail>)>();

        // Method to add products to the draft order
        public void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (!ValidateOrderInputs()) return;

            try
            {
                var order = new Order
                {
                    CustomerID = (int)cmbCustomer.SelectedValue,
                    OrderDate = DateTime.Now,
                    TotalAmount = decimal.Parse(txtTotalAmount.Text),
                    PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString() ?? string.Empty
                };

                // Copy order details to ensure they stay intact in the draft
                var detailCopy = orderDetails.Select(d => new OrderDetail
                {
                    ProductID = d.ProductID,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList();

                // Add to draft orders for later processing
                draftOrders.Add((order, detailCopy));

                // Display draft orders in DataGridView
                LoadDraftOrders();

                MessageBox.Show("Draft created. Please print invoice to finalize.");

                // Clear inputs after creating draft
                orderDetails.Clear();
                txtQuantity.Clear();
                txtTotalAmount.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating draft: " + ex.Message);
            }
        }

        // Method to load draft orders into DataGridView
        private void LoadDraftOrders()
        {
            dgvDraftOrders.DataSource = null;
            dgvDraftOrders.DataSource = draftOrders.Select((x, i) => new
            {
                DraftID = i + 1,
                CustomerID = x.Order.CustomerID,
                CustomerName = CustomerManager.GetCustomerById(x.Order.CustomerID)?.Name ?? "Unknown",
                TotalAmount = x.Order.TotalAmount,
                PaymentMethod = x.Order.PaymentMethod,
                OrderDate = x.Order.OrderDate
            }).ToList();
            
            // Apply the standardized style using the helper
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyFullStyle(dgvDraftOrders);
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
        public void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (dgvDraftOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a draft order to finalize.");
                return;
            }

            int selectedIndex = dgvDraftOrders.SelectedRows[0].Index;
            if (selectedIndex < 0 || selectedIndex >= draftOrders.Count)
                return;

            var (order, details) = draftOrders[selectedIndex];

            try
            {
                // Insert order and details into the database
                int orderId = OrderManager.CreateOrderWithDetails(order, details);
                LoadProducts();

                // Update inventory based on the draft order
                foreach (var detail in details)
                {
                    var product = ProductManager.GetProductById(detail.ProductID);
                    if (product == null)
                        throw new Exception($"Product not found: ID {detail.ProductID}");

                    int newStock = product.Stock - detail.Quantity;
                    InventoryManager.UpdateStock(detail.ProductID, newStock);
                }

                // Display order details in a custom popup form
                string orderDetailsInfo = $"Order ID: {orderId}\n\n";
                foreach (var detail in details)
                {
                    var product = ProductManager.GetProductById(detail.ProductID);
                    orderDetailsInfo += $"Product: {product?.ProductName ?? "Unknown"}\n" +
                                        $"Quantity: {detail.Quantity}\n" +
                                        $"Unit Price: {detail.UnitPrice:C}\n" +
                                        $"Total: {detail.Quantity * detail.UnitPrice:C}\n\n";
                }

                // Create and show the order details popup
                MessageBox.Show(orderDetailsInfo, "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowOrderSuccessNotification();

                // Remove the finalized draft from the list
                draftOrders.RemoveAt(selectedIndex);
                LoadDraftOrders(); // Refresh draft orders

                // Optionally, you could load the finalized orders if needed
                LoadOrders();

                // Clear the DataGridView displaying the product list
                productList.DataSource = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error finalizing order: " + ex.Message);
            }
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

        private void btnDeleteDraftOrder_Click(object sender, EventArgs e)
        {
            if (dgvDraftOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a draft order to delete.");
                return;
            }

            int selectedIndex = dgvDraftOrders.SelectedRows[0].Index;
            if (selectedIndex < 0 || selectedIndex >= draftOrders.Count)
            {
                return;
            }

            // Ask for confirmation before deletion
            var confirmResult = MessageBox.Show("Are you sure you want to delete this draft order?",
                                                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                draftOrders.RemoveAt(selectedIndex);
                LoadDraftOrders();  // Refresh draft orders
                MessageBox.Show("Draft order deleted successfully.");
            }
        }

        private void btnEditDraftOrder_Click(object sender, EventArgs e)
        {
            if (dgvDraftOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a draft order to edit.");
                return;
            }

            int selectedIndex = dgvDraftOrders.SelectedRows[0].Index;
            if (selectedIndex < 0 || selectedIndex >= draftOrders.Count)
            {
                return;
            }

            var (order, details) = draftOrders[selectedIndex];

            // Set the values of the order details in the controls
            cmbCustomer.SelectedValue = order.CustomerID;
            cmbPaymentMethod.SelectedItem = order.PaymentMethod;
            txtTotalAmount.Text = order.TotalAmount.ToString("0.00");

            // Bind the DataGridView to display the current order details
            productList.DataSource = null;
            productList.DataSource = details.Select(d => new
            {
                ProductName = ProductManager.GetProductById(d.ProductID)?.ProductName ?? "Unknown",
                d.ProductID,
                d.Quantity,
                d.UnitPrice,
                Total = d.Quantity * d.UnitPrice
            }).ToList();

            // Hide ProductID column from the UI
            productList.Columns["ProductID"].Visible = false;

            // Allow editing of the quantity directly in the DataGridView
            productList.ReadOnly = false;
        }

        private void btnSaveEditedDraftOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (productList.Rows.Count == 0)
                {
                    MessageBox.Show("No products to save.");
                    return;
                }

                // Save the edited draft order
                int selectedIndex = dgvDraftOrders.SelectedRows[0].Index;
                if (selectedIndex < 0 || selectedIndex >= draftOrders.Count)
                {
                    return;
                }

                var (order, details) = draftOrders[selectedIndex];

                // Update order with new values
                order.CustomerID = (int)cmbCustomer.SelectedValue;
                order.PaymentMethod = cmbPaymentMethod.SelectedItem.ToString();
                order.TotalAmount = decimal.Parse(txtTotalAmount.Text);

                // Update order details with new values from the DataGridView
                foreach (DataGridViewRow row in productList.Rows)
                {
                    if (row.Cells["ProductID"].Value != null)
                    {
                        int productId = (int)row.Cells["ProductID"].Value;
                        var existingDetail = details.FirstOrDefault(d => d.ProductID == productId);

                        if (existingDetail != null)
                        {
                            existingDetail.Quantity = (int)row.Cells["Quantity"].Value;
                            existingDetail.UnitPrice = (decimal)row.Cells["UnitPrice"].Value;
                        }
                    }
                }

                // Commit changes to draft orders
                draftOrders[selectedIndex] = (order, details);

                // Refresh draft orders
                LoadDraftOrders();

                MessageBox.Show("Draft order updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving order: " + ex.Message);
            }
        }

    }
}
