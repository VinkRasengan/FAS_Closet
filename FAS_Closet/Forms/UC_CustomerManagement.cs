using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    // Use a non-partial class to avoid conflict with Designer.cs
    public class UcCustomerManagement : UserControl
    {
        // Fixed nullable fields
        private TextBox? txtCustomerName;
        private TextBox? txtCustomerEmail;
        private TextBox? txtCustomerPhone;
        private TextBox? txtCustomerAddress;
        private TextBox? txtCustomerId;
        private TextBox? txtLoyaltyPoints;
        private DataGridView? dataGridViewPurchaseHistory;
        private TextBox? txtSearchCustomer;
        private DataGridView? dgvCustomers;
        private System.ComponentModel.IContainer? components = null;
        public Button btnAddCustomer;
        public Button btnDelete;

        public UcCustomerManagement()
        {
            InitializeUserInterface();
            LoadCustomers();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeUserInterface()
        {
            components = new System.ComponentModel.Container();

            // Create all controls
            txtCustomerName = new TextBox();
            txtCustomerEmail = new TextBox();
            txtCustomerPhone = new TextBox();
            txtCustomerAddress = new TextBox();
            txtCustomerId = new TextBox();
            txtLoyaltyPoints = new TextBox();
            dataGridViewPurchaseHistory = new DataGridView();
            txtSearchCustomer = new TextBox();
            dgvCustomers = new DataGridView();

            Button btnSaveCustomerInfo = new Button();
            Button btnViewPurchaseHistory = new Button();
            Button btnManageLoyaltyPoints = new Button();

            // Create labels
            Label lblCustomerName = new Label();
            lblCustomerName.Text = "Name:";
            lblCustomerName.Location = new Point(20, 20);
            lblCustomerName.Size = new Size(100, 25);
            lblCustomerName.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerEmail = new Label();
            lblCustomerEmail.Text = "Email:";
            lblCustomerEmail.Location = new Point(20, 55);
            lblCustomerEmail.Size = new Size(100, 25);
            lblCustomerEmail.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerPhone = new Label();
            lblCustomerPhone.Text = "Phone:";
            lblCustomerPhone.Location = new Point(20, 90);
            lblCustomerPhone.Size = new Size(100, 25);
            lblCustomerPhone.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerAddress = new Label();
            lblCustomerAddress.Text = "Address:";
            lblCustomerAddress.Location = new Point(20, 125);
            lblCustomerAddress.Size = new Size(100, 25);
            lblCustomerAddress.TextAlign = ContentAlignment.MiddleRight;

            Label lblCustomerId = new Label();
            lblCustomerId.Text = "Customer ID:";
            lblCustomerId.Location = new Point(350, 20);
            lblCustomerId.Size = new Size(100, 25);
            lblCustomerId.TextAlign = ContentAlignment.MiddleRight;

            Label lblLoyaltyPoints = new Label();
            lblLoyaltyPoints.Text = "Loyalty Points:";
            lblLoyaltyPoints.Location = new Point(350, 55);
            lblLoyaltyPoints.Size = new Size(100, 25);
            lblLoyaltyPoints.TextAlign = ContentAlignment.MiddleRight;

            // Configure textboxes
            txtCustomerName.Location = new Point(130, 20);
            txtCustomerName.Size = new Size(200, 25);
            txtCustomerName.PlaceholderText = "Enter customer name";

            txtCustomerEmail.Location = new Point(130, 55);
            txtCustomerEmail.Size = new Size(200, 25);
            txtCustomerEmail.PlaceholderText = "Enter email";

            txtCustomerPhone.Location = new Point(130, 90);
            txtCustomerPhone.Size = new Size(200, 25);
            txtCustomerPhone.PlaceholderText = "Enter phone number";

            txtCustomerAddress.Location = new Point(130, 125);
            txtCustomerAddress.Size = new Size(200, 50);
            txtCustomerAddress.PlaceholderText = "Enter address";
            txtCustomerAddress.Multiline = true;

            txtCustomerId.Location = new Point(460, 20);
            txtCustomerId.Size = new Size(100, 25);
            txtCustomerId.PlaceholderText = "ID";
            txtCustomerId.ReadOnly = true;

            txtLoyaltyPoints.Location = new Point(460, 55);
            txtLoyaltyPoints.Size = new Size(100, 25);
            txtLoyaltyPoints.PlaceholderText = "Points";
            txtLoyaltyPoints.ReadOnly = true;

            txtSearchCustomer.Location = new Point(460, 90);
            txtSearchCustomer.Size = new Size(150, 25);
            txtSearchCustomer.PlaceholderText = "Search customers";
            txtSearchCustomer.TextChanged += TxtSearchCustomer_TextChanged;

            // Configure buttons
            btnSaveCustomerInfo.Text = "Save Customer";
            btnSaveCustomerInfo.Location = new Point(130, 190);
            btnSaveCustomerInfo.Size = new Size(120, 30);
            btnSaveCustomerInfo.Click += btnSaveCustomerInfo_Click;

            btnViewPurchaseHistory.Text = "View History";
            btnViewPurchaseHistory.Location = new Point(260, 190);
            btnViewPurchaseHistory.Size = new Size(120, 30);
            btnViewPurchaseHistory.Click += btnViewPurchaseHistory_Click;

            btnManageLoyaltyPoints.Text = "Loyalty Points";
            btnManageLoyaltyPoints.Location = new Point(390, 190);
            btnManageLoyaltyPoints.Size = new Size(120, 30);
            btnManageLoyaltyPoints.Click += btnManageLoyaltyPoints_Click;

            // Adjusted width for dgvCustomers
            dgvCustomers.Location = new Point(20, 230);
            dgvCustomers.Size = new Size(400, 200); // Increase the width to 400
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;

            // Apply modern styling for dgvCustomers
            dgvCustomers.BorderStyle = BorderStyle.None;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.GridColor = Color.FromArgb(230, 230, 230);
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(93, 64, 150); // Dark Purple
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvCustomers.ColumnHeadersHeight = 40;
            dgvCustomers.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvCustomers.RowTemplate.Height = 35;
            dgvCustomers.RowsDefaultCellStyle.BackColor = Color.White;
            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252);
            dgvCustomers.RowHeadersVisible = false;

            // Adjusted width for dataGridViewPurchaseHistory
            dataGridViewPurchaseHistory.Location = new Point(440, 230); // Adjust position to match new size
            dataGridViewPurchaseHistory.Size = new Size(400, 200); // Increase the width to 400
            dataGridViewPurchaseHistory.AllowUserToAddRows = false;
            dataGridViewPurchaseHistory.ReadOnly = true;
            dataGridViewPurchaseHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Apply modern styling for dataGridViewPurchaseHistory
            dataGridViewPurchaseHistory.BorderStyle = BorderStyle.None;
            dataGridViewPurchaseHistory.BackgroundColor = Color.White;
            dataGridViewPurchaseHistory.GridColor = Color.FromArgb(230, 230, 230);
            dataGridViewPurchaseHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(93, 64, 150); // Dark Purple
            dataGridViewPurchaseHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewPurchaseHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewPurchaseHistory.ColumnHeadersHeight = 40;
            dataGridViewPurchaseHistory.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dataGridViewPurchaseHistory.RowTemplate.Height = 35;
            dataGridViewPurchaseHistory.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewPurchaseHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252);
            dataGridViewPurchaseHistory.RowHeadersVisible = false;

            // Add controls to form
            this.Controls.Add(lblCustomerName);
            this.Controls.Add(txtCustomerName);
            this.Controls.Add(lblCustomerEmail);
            this.Controls.Add(txtCustomerEmail);
            this.Controls.Add(lblCustomerPhone);
            this.Controls.Add(txtCustomerPhone);
            this.Controls.Add(lblCustomerAddress);
            this.Controls.Add(txtCustomerAddress);
            this.Controls.Add(lblCustomerId);
            this.Controls.Add(txtCustomerId);
            this.Controls.Add(lblLoyaltyPoints);
            this.Controls.Add(txtSearchCustomer);
            this.Controls.Add(btnSaveCustomerInfo);
            this.Controls.Add(btnViewPurchaseHistory);
            this.Controls.Add(btnManageLoyaltyPoints);
            this.Controls.Add(dgvCustomers);
            this.Controls.Add(dataGridViewPurchaseHistory);
        }

        private void btnManageLoyaltyPoints_Click(object? sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            int selectedCustomerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["CustomerID"].Value);
            try
            {
                // Get loyalty points of the selected customer
                int loyaltyPoints = GetLoyaltyPointsByCustomerId(selectedCustomerId);

                // Fetch customer details
                var selectedCustomer = CustomerManager.GetCustomerById(selectedCustomerId);

                // Create and display the loyalty points info popup
                Form loyaltyPointsPopup = new Form
                {
                    Text = $"Loyalty Points for {selectedCustomer.Name}",
                    Size = new Size(420, 360),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    BackColor = Color.White
                };

                // Set background color for the form to light blue
                loyaltyPointsPopup.BackColor = Color.FromArgb(173, 216, 230); // Light blue color

                // Create a panel for better control grouping
                Panel panel = new Panel
                {
                    Size = new Size(380, 300),
                    Location = new Point(10, 10),
                    BackColor = Color.FromArgb(255, 255, 255), // White background for content
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(10)
                };
                loyaltyPointsPopup.Controls.Add(panel);

                // Add custom-styled labels for customer info and loyalty points
                Label lblTitle = new Label
                {
                    Text = $"Loyalty Points for {selectedCustomer.Name}",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(25, 118, 210),  // Light blue color for title
                    Location = new Point(10, 10),
                    AutoSize = true
                };
                panel.Controls.Add(lblTitle);

                // Customer Name
                Label lblCustomerName = new Label
                {
                    Text = $"Customer Name: {selectedCustomer.Name}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(10, 50),
                    ForeColor = Color.Black,
                    AutoSize = true
                };
                panel.Controls.Add(lblCustomerName);

                // Customer Email
                Label lblCustomerEmail = new Label
                {
                    Text = $"Email: {selectedCustomer.Email}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(10, 80),
                    ForeColor = Color.Black,
                    AutoSize = true
                };
                panel.Controls.Add(lblCustomerEmail);

                // Customer Phone
                Label lblCustomerPhone = new Label
                {
                    Text = $"Phone: {selectedCustomer.Phone}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(10, 110),
                    ForeColor = Color.Black,
                    AutoSize = true
                };
                panel.Controls.Add(lblCustomerPhone);

                // Customer Address
                Label lblCustomerAddress = new Label
                {
                    Text = $"Address: {selectedCustomer.Address}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(10, 140),
                    ForeColor = Color.Black,
                    AutoSize = true
                };
                panel.Controls.Add(lblCustomerAddress);

                // Loyalty Points
                Label lblLoyaltyPoints = new Label
                {
                    Text = $"Loyalty Points: {loyaltyPoints}",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Location = new Point(10, 180),
                    ForeColor = Color.FromArgb(0, 122, 204),  // Blue color for loyalty points
                    AutoSize = true
                };
                panel.Controls.Add(lblLoyaltyPoints);

                // Add a colorful button to close the popup
                Button btnClose = new Button
                {
                    Text = "Close",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(300, 240),
                    Size = new Size(75, 30),
                    BackColor = Color.FromArgb(25, 118, 210),  // Blue color for close button
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnClose.Click += (s, ev) => loyaltyPointsPopup.Close();
                panel.Controls.Add(btnClose);

                // Show the popup
                loyaltyPointsPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error managing loyalty points: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public void btnAddCustomer_Click(object sender, EventArgs e)
        {
            txtCustomerId.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtCustomerEmail.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;
            txtCustomerAddress.Text = string.Empty;
            txtLoyaltyPoints.Text = string.Empty;

            if (dgvCustomers?.SelectedRows.Count > 0)
            {
                dgvCustomers.ClearSelection();
            }

            MessageBox.Show("Bạn có thể nhập thông tin khách hàng mới.");
        }

        public void AddNewCustomer()
        {
            txtCustomerId.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtCustomerEmail.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;
            txtCustomerAddress.Text = string.Empty;
            txtLoyaltyPoints.Text = string.Empty;

            if (dgvCustomers?.SelectedRows.Count > 0)
            {
                dgvCustomers.ClearSelection();
            }
        }

        public void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId?.Text) || !int.TryParse(txtCustomerId.Text, out int customerId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    CustomerManager.DeleteCustomer(customerId);
                    MessageBox.Show("Xóa khách hàng thành công!");
                    ClearForm();
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}");
                }
            }
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = CustomerManager.GetCustomers();
                
                // Apply better formatting to the grid view before setting the data source
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.Columns.Clear();
                
                // Add custom columns with better headers and formatting
                dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CustomerID",
                    HeaderText = "Mã KH",
                    Width = 60,
                    Name = "CustomerID"
                });
                
                dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Name",
                    HeaderText = "Tên Khách Hàng",
                    Width = 150
                });
                
                dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Email",
                    HeaderText = "Email",
                    Width = 180
                });
                
                dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Phone",
                    HeaderText = "Số Điện Thoại",
                    Width = 120
                });
                
                // Set the data source after configuring columns
                dgvCustomers.DataSource = customers;
                
                // Hide the address column as it's shown in the detail panel
                if (dgvCustomers.Columns.Contains("Address"))
                {
                    dgvCustomers.Columns["Address"].Visible = false;
                }
                
                // Apply modern styling
                dgvCustomers.BorderStyle = BorderStyle.None;
                dgvCustomers.BackgroundColor = Color.White;
                dgvCustomers.GridColor = Color.FromArgb(230, 230, 230);
                dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 118, 210);
                dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
                dgvCustomers.ColumnHeadersHeight = 40;
                dgvCustomers.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
                dgvCustomers.RowTemplate.Height = 35;
                dgvCustomers.RowsDefaultCellStyle.BackColor = Color.White;
                dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252);
                dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCustomers.RowHeadersVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}");
            }
        }

        private void dgvCustomers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCustomers?.SelectedRows.Count > 0 && dgvCustomers.SelectedRows[0].DataBoundItem is Customer customer)
            {
                if (txtCustomerId != null &&
                    txtCustomerName != null &&
                    txtCustomerEmail != null &&
                    txtCustomerPhone != null &&
                    txtCustomerAddress != null &&
                    txtLoyaltyPoints != null)
                {
                    txtCustomerId.Text = customer.CustomerID.ToString();
                    txtCustomerName.Text = customer.Name;
                    txtCustomerEmail.Text = customer.Email;
                    txtCustomerPhone.Text = customer.Phone;
                    txtCustomerAddress.Text = customer.Address;

                    try
                    {
                        int loyaltyPoints = CustomerManager.GetLoyaltyPointsByCustomerId(customer.CustomerID);
                        txtLoyaltyPoints.Text = loyaltyPoints.ToString();
                    }
                    catch
                    {
                        txtLoyaltyPoints.Text = "0";
                    }
                }
            }
        }

        private void TxtSearchCustomer_TextChanged(object? sender, EventArgs e)
        {
            string searchTerm = txtSearchCustomer!.Text.ToLower();

            try
            {
                var allCustomers = CustomerManager.GetCustomers();
                var filteredCustomers = allCustomers
                    .Where(c => c.Name.ToLower().Contains(searchTerm) ||
                               c.Email.ToLower().Contains(searchTerm) ||
                               c.Phone.Contains(searchTerm))
                    .ToList();

                dgvCustomers!.DataSource = filteredCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}");
            }
        }

        private void btnSaveCustomerInfo_Click(object? sender, EventArgs e)
        {
            if (txtCustomerName == null ||
                txtCustomerEmail == null ||
                txtCustomerPhone == null ||
                txtCustomerAddress == null ||
                txtCustomerId == null)
            {
                MessageBox.Show("UI components not initialized properly");
                return;
            }

            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerPhone.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerAddress.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // Validate email format using regex
            string email = txtCustomerEmail.Text.Trim();
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate phone number format (basic validation, e.g., for 10 digits)
            string phone = txtCustomerPhone.Text.Trim();
            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Please enter a valid phone number.", "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var customer = new Customer
                {
                    Name = txtCustomerName.Text,
                    Email = email,
                    Phone = phone,
                    Address = txtCustomerAddress.Text
                };

                if (!string.IsNullOrEmpty(txtCustomerId.Text) && int.TryParse(txtCustomerId.Text, out int customerId))
                {
                    // Update existing customer
                    customer.CustomerID = customerId;
                    CustomerManager.UpdateCustomer(customer);
                    MessageBox.Show("Customer updated successfully.");
                }
                else
                {
                    // Add new customer
                    CustomerManager.AddCustomer(customer);
                    MessageBox.Show("Customer added successfully.");
                    ClearForm();  // Clear form after saving
                }

                LoadCustomers();  // Reload customers list
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validate email using regular expression
        private bool IsValidEmail(string email)
        {
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }

        // Validate phone number (basic validation for 10 digits)
        private bool IsValidPhoneNumber(string phone)
        {
            var phoneRegex = new System.Text.RegularExpressions.Regex(@"^\d{10}$");  // Basic validation: exactly 10 digits
            return phoneRegex.IsMatch(phone);
        }


        private void btnViewPurchaseHistory_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId!.Text) || !int.TryParse(txtCustomerId.Text, out int customerId))
            {
                MessageBox.Show("Please select a customer first");
                return;
            }

            try
            {
                var orders = OrderManager.GetOrdersByCustomerId(customerId);
                dataGridViewPurchaseHistory!.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading purchase history: {ex.Message}");
            }
        }

        public static int GetLoyaltyPointsByCustomerId(int customerId)
        {
            // Step 1: Get all orders by the customer
            var orders = OrderManager.GetOrdersByCustomerId(customerId);

            // Step 2: Sum the TotalAmount of all orders
            decimal totalAmountSpent = orders.Sum(o => o.TotalAmount);

            // Step 3: Calculate loyalty points (1 point for every $10 spent)
            int loyaltyPoints = (int)(totalAmountSpent / 10); // Integer division

            return loyaltyPoints;
        }

        private void ClearForm()
        {
            txtCustomerId?.Clear();
            txtCustomerName?.Clear();
            txtCustomerEmail?.Clear();
            txtCustomerPhone?.Clear();
            txtCustomerAddress?.Clear();
            txtLoyaltyPoints?.Clear();
        }
    }
}
