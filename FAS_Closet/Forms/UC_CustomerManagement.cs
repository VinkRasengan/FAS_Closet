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
        public Button btnAdd;
        public Button btnEdit;
        public Button btnDelete;
        public Button btnRefresh;

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

            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();

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
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.BackColor = Color.FromArgb(0, 123, 255);
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Text = "Thêm";
            btnAdd.Location = new Point(130, 190);
            btnAdd.Size = new Size(120, 30);
            btnAdd.Click += btnAddCustomer_Click;

            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.BackColor = Color.FromArgb(40, 167, 69);
            btnEdit.ForeColor = Color.White;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.Text = "Sửa";
            btnEdit.Location = new Point(260, 190);
            btnEdit.Size = new Size(120, 30);

            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Text = "Xóa";
            btnDelete.Location = new Point(390, 190);
            btnDelete.Size = new Size(120, 30);
            btnDelete.Click += btnDeleteCustomer_Click;

            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Text = "Làm mới";
            btnRefresh.Location = new Point(520, 190);
            btnRefresh.Size = new Size(120, 30);

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
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(dgvCustomers);
            this.Controls.Add(dataGridViewPurchaseHistory);
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
