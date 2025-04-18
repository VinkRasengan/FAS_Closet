using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using FASCloset.Models;
using FASCloset.Services;
using System.ComponentModel;

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
        public Button btnSave;
        public Button btnCancel;
        
        private bool isEditMode = false;
        private int editingCustomerId = 0;
        private ErrorProvider errorProvider;

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
            errorProvider = new ErrorProvider(this);

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
            btnSave = new Button();
            btnCancel = new Button();

            // Create labels
            Label lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ KHÁCH HÀNG";
            lblTitle.Location = new Point(20, 10);
            lblTitle.Size = new Size(300, 35);
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(25, 118, 210);
            
            Panel customerDetailPanel = new Panel();
            customerDetailPanel.Location = new Point(20, 50);
            customerDetailPanel.Size = new Size(620, 200);
            customerDetailPanel.BorderStyle = BorderStyle.FixedSingle;
            customerDetailPanel.BackColor = Color.WhiteSmoke;

            Label lblCustomerName = new Label();
            lblCustomerName.Text = "Tên khách hàng:";
            lblCustomerName.Location = new Point(20, 20);
            lblCustomerName.Size = new Size(120, 25);
            lblCustomerName.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerName.Font = new Font("Segoe UI Semibold", 9.5F);

            Label lblCustomerEmail = new Label();
            lblCustomerEmail.Text = "Email:";
            lblCustomerEmail.Location = new Point(20, 55);
            lblCustomerEmail.Size = new Size(120, 25);
            lblCustomerEmail.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerEmail.Font = new Font("Segoe UI Semibold", 9.5F);

            Label lblCustomerPhone = new Label();
            lblCustomerPhone.Text = "Điện thoại:";
            lblCustomerPhone.Location = new Point(20, 90);
            lblCustomerPhone.Size = new Size(120, 25);
            lblCustomerPhone.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerPhone.Font = new Font("Segoe UI Semibold", 9.5F);

            Label lblCustomerAddress = new Label();
            lblCustomerAddress.Text = "Địa chỉ:";
            lblCustomerAddress.Location = new Point(20, 125);
            lblCustomerAddress.Size = new Size(120, 25);
            lblCustomerAddress.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerAddress.Font = new Font("Segoe UI Semibold", 9.5F);

            Label lblCustomerId = new Label();
            lblCustomerId.Text = "Mã khách hàng:";
            lblCustomerId.Location = new Point(350, 20);
            lblCustomerId.Size = new Size(120, 25);
            lblCustomerId.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerId.Font = new Font("Segoe UI Semibold", 9.5F);
            lblCustomerId.Visible = false;

            Label lblLoyaltyPoints = new Label();
            lblLoyaltyPoints.Text = "Điểm tích lũy:";
            lblLoyaltyPoints.Location = new Point(350, 55);
            lblLoyaltyPoints.Size = new Size(120, 25);
            lblLoyaltyPoints.TextAlign = ContentAlignment.MiddleLeft;
            lblLoyaltyPoints.Font = new Font("Segoe UI Semibold", 9.5F);

            // Configure textboxes
            txtCustomerName.Location = new Point(140, 20);
            txtCustomerName.Size = new Size(200, 25);
            txtCustomerName.PlaceholderText = "Nhập tên khách hàng";
            txtCustomerName.Validating += TxtCustomerName_Validating;

            txtCustomerEmail.Location = new Point(140, 55);
            txtCustomerEmail.Size = new Size(200, 25);
            txtCustomerEmail.PlaceholderText = "Nhập email";
            txtCustomerEmail.Validating += TxtCustomerEmail_Validating;

            txtCustomerPhone.Location = new Point(140, 90);
            txtCustomerPhone.Size = new Size(200, 25);
            txtCustomerPhone.PlaceholderText = "Nhập số điện thoại";
            txtCustomerPhone.Validating += TxtCustomerPhone_Validating;

            txtCustomerAddress.Location = new Point(140, 125);
            txtCustomerAddress.Size = new Size(410, 50);
            txtCustomerAddress.PlaceholderText = "Nhập địa chỉ";
            txtCustomerAddress.Multiline = true;
            txtCustomerAddress.Validating += TxtCustomerAddress_Validating;

            txtCustomerId.Location = new Point(470, 20);
            txtCustomerId.Size = new Size(80, 25);
            txtCustomerId.ReadOnly = true;
            txtCustomerId.BackColor = Color.WhiteSmoke;
            txtCustomerId.BorderStyle = BorderStyle.FixedSingle;
            txtCustomerId.Visible = false;

            txtLoyaltyPoints.Location = new Point(470, 55);
            txtLoyaltyPoints.Size = new Size(80, 25);
            txtLoyaltyPoints.ReadOnly = true;
            txtLoyaltyPoints.BackColor = Color.WhiteSmoke;
            txtLoyaltyPoints.BorderStyle = BorderStyle.FixedSingle;
            
            Panel searchPanel = new Panel();
            searchPanel.Location = new Point(660, 50);
            searchPanel.Size = new Size(180, 35);
            searchPanel.BorderStyle = BorderStyle.FixedSingle;
            searchPanel.BackColor = Color.White;
            
            Label lblSearch = new Label();
            lblSearch.Text = "🔍";
            lblSearch.Location = new Point(5, 3);
            lblSearch.Size = new Size(25, 25);
            lblSearch.Font = new Font("Segoe UI", 14F);
            lblSearch.ForeColor = Color.FromArgb(25, 118, 210);
            
            txtSearchCustomer.Location = new Point(35, 5);
            txtSearchCustomer.Size = new Size(140, 25);
            txtSearchCustomer.PlaceholderText = "Tìm kiếm...";
            txtSearchCustomer.BorderStyle = BorderStyle.None;
            txtSearchCustomer.TextChanged += TxtSearchCustomer_TextChanged;
            
            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearchCustomer);

            // Configure buttons
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.BackColor = Color.FromArgb(0, 123, 255);
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Text = "Thêm mới";
            btnAdd.Location = new Point(20, 265);
            btnAdd.Size = new Size(120, 30);
            btnAdd.Click += btnAddCustomer_Click;

            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.BackColor = Color.FromArgb(40, 167, 69);
            btnEdit.ForeColor = Color.White;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.Text = "Sửa";
            btnEdit.Location = new Point(150, 265);
            btnEdit.Size = new Size(120, 30);
            btnEdit.Click += btnEditCustomer_Click;

            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Text = "Xóa";
            btnDelete.Location = new Point(280, 265);
            btnDelete.Size = new Size(120, 30);
            btnDelete.Click += btnDeleteCustomer_Click;

            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Text = "Làm mới";
            btnRefresh.Location = new Point(410, 265);
            btnRefresh.Size = new Size(120, 30);
            btnRefresh.Click += BtnRefresh_Click;
            
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.BackColor = Color.FromArgb(0, 123, 255);
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor = Cursors.Hand;
            btnSave.Text = "Lưu";
            btnSave.Location = new Point(20, 265);
            btnSave.Size = new Size(120, 30);
            btnSave.Visible = false;
            btnSave.Click += BtnSave_Click;
            
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.ForeColor = Color.White;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Text = "Hủy";
            btnCancel.Location = new Point(150, 265);
            btnCancel.Size = new Size(120, 30);
            btnCancel.Visible = false;
            btnCancel.Click += BtnCancel_Click;

            // Adjusted width for dgvCustomers
            dgvCustomers.Location = new Point(20, 310);
            dgvCustomers.Size = new Size(400, 280); 
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

            // Create label for purchase history
            Label lblPurchaseHistory = new Label();
            lblPurchaseHistory.Text = "LỊCH SỬ MUA HÀNG";
            lblPurchaseHistory.Location = new Point(440, 265);
            lblPurchaseHistory.Size = new Size(200, 30);
            lblPurchaseHistory.Font = new Font("Segoe UI Semibold", 10F);
            lblPurchaseHistory.ForeColor = Color.FromArgb(93, 64, 150);
            
            // Adjusted width for dataGridViewPurchaseHistory
            dataGridViewPurchaseHistory.Location = new Point(440, 310); 
            dataGridViewPurchaseHistory.Size = new Size(400, 280); 
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
            customerDetailPanel.Controls.Add(lblCustomerName);
            customerDetailPanel.Controls.Add(txtCustomerName);
            customerDetailPanel.Controls.Add(lblCustomerEmail);
            customerDetailPanel.Controls.Add(txtCustomerEmail);
            customerDetailPanel.Controls.Add(lblCustomerPhone);
            customerDetailPanel.Controls.Add(txtCustomerPhone);
            customerDetailPanel.Controls.Add(lblCustomerAddress);
            customerDetailPanel.Controls.Add(txtCustomerAddress);
            customerDetailPanel.Controls.Add(lblCustomerId);
            customerDetailPanel.Controls.Add(txtCustomerId);
            customerDetailPanel.Controls.Add(lblLoyaltyPoints);
            customerDetailPanel.Controls.Add(txtLoyaltyPoints);
            
            this.Controls.Add(lblTitle);
            this.Controls.Add(customerDetailPanel);
            this.Controls.Add(searchPanel);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
            this.Controls.Add(dgvCustomers);
            this.Controls.Add(lblPurchaseHistory);
            this.Controls.Add(dataGridViewPurchaseHistory);
        }

        private void UC_CustomerManagement_Load(object sender, EventArgs e)
        {
            RefreshCustomerList();
            ClearInputFields();
        }

        private void RefreshCustomerList()
        {
            var customers = CustomerManager.GetCustomers();
            dgvCustomers.DataSource = customers;
            
            // Thiết lập tiêu đề cột
            dgvCustomers.Columns["CustomerID"].HeaderText = "Mã KH";
            dgvCustomers.Columns["Name"].HeaderText = "Tên khách hàng";
            dgvCustomers.Columns["Email"].HeaderText = "Email";
            dgvCustomers.Columns["Phone"].HeaderText = "Số điện thoại";
            dgvCustomers.Columns["Address"].HeaderText = "Địa chỉ";
        }

        private void ClearInputFields()
        {
            txtCustomerName.Text = string.Empty;
            txtCustomerEmail.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;
            txtCustomerAddress.Text = string.Empty;
            
            errorProvider.Clear();
            isEditMode = false;
            editingCustomerId = 0;
            
            // Hiển thị nút Thêm và ẩn nút Lưu, Hủy
            btnAdd.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private bool ValidateCustomerData()
        {
            bool isValid = true;
            
            // Validate name
            if (string.IsNullOrWhiteSpace(txtCustomerName?.Text))
            {
                errorProvider.SetError(txtCustomerName!, "Vui lòng nhập tên khách hàng");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtCustomerName!, "");
            }
            
            // Validate email
            if (!string.IsNullOrWhiteSpace(txtCustomerEmail?.Text))
            {
                // Email format validation
                string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                if (!Regex.IsMatch(txtCustomerEmail.Text, emailPattern))
                {
                    errorProvider.SetError(txtCustomerEmail!, "Địa chỉ email không hợp lệ");
                    isValid = false;
                }
                else
                {
                    errorProvider.SetError(txtCustomerEmail!, "");
                }
            }
            else
            {
                errorProvider.SetError(txtCustomerEmail!, "");
            }
            
            // Validate phone
            if (string.IsNullOrWhiteSpace(txtCustomerPhone?.Text))
            {
                errorProvider.SetError(txtCustomerPhone!, "Vui lòng nhập số điện thoại");
                isValid = false;
            }
            else
            {
                // Phone format validation for Vietnamese phone numbers
                string phonePattern = @"^(0|\+84)(\d{9,10})$";
                if (!Regex.IsMatch(txtCustomerPhone.Text, phonePattern))
                {
                    errorProvider.SetError(txtCustomerPhone!, "Số điện thoại không hợp lệ");
                    isValid = false;
                }
                else
                {
                    errorProvider.SetError(txtCustomerPhone!, "");
                }
            }
            
            // Validate address
            if (string.IsNullOrWhiteSpace(txtCustomerAddress?.Text))
            {
                errorProvider.SetError(txtCustomerAddress!, "Vui lòng nhập địa chỉ");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtCustomerAddress!, "");
            }
            
            return isValid;
        }

        private void TxtCustomerName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                errorProvider.SetError(txtCustomerName, "Vui lòng nhập tên khách hàng");
            }
            else
            {
                errorProvider.SetError(txtCustomerName, "");
            }
        }

        private void TxtCustomerEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCustomerEmail.Text))
            {
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(txtCustomerEmail.Text, emailPattern))
                {
                    errorProvider.SetError(txtCustomerEmail, "Email không hợp lệ");
                }
                else
                {
                    errorProvider.SetError(txtCustomerEmail, "");
                }
            }
            else
            {
                errorProvider.SetError(txtCustomerEmail, "");
            }
        }

        private void TxtCustomerPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerPhone.Text))
            {
                errorProvider.SetError(txtCustomerPhone, "Vui lòng nhập số điện thoại");
            }
            else
            {
                string phonePattern = @"^(0|\+84)\d{9,10}$";
                if (!Regex.IsMatch(txtCustomerPhone.Text, phonePattern))
                {
                    errorProvider.SetError(txtCustomerPhone, "Số điện thoại không hợp lệ (định dạng: 0xxxxxxxxx hoặc +84xxxxxxxxx)");
                }
                else
                {
                    errorProvider.SetError(txtCustomerPhone, "");
                }
            }
        }

        private void TxtCustomerAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerAddress.Text))
            {
                errorProvider.SetError(txtCustomerAddress, "Vui lòng nhập địa chỉ");
            }
            else
            {
                errorProvider.SetError(txtCustomerAddress, "");
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (ValidateCustomerData())
            {
                Customer newCustomer = new Customer
                {
                    Name = txtCustomerName.Text,
                    Email = txtCustomerEmail.Text,
                    Phone = txtCustomerPhone.Text,
                    Address = txtCustomerAddress.Text
                };

                try
                {
                    CustomerManager.AddCustomer(newCustomer);
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
                    ClearInputFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy khách hàng được chọn
            var selectedRow = dgvCustomers.SelectedRows[0];
            editingCustomerId = Convert.ToInt32(selectedRow.Cells["CustomerID"].Value);

            // Hiển thị thông tin khách hàng lên form
            txtCustomerName.Text = selectedRow.Cells["Name"].Value.ToString();
            txtCustomerEmail.Text = selectedRow.Cells["Email"].Value.ToString();
            txtCustomerPhone.Text = selectedRow.Cells["Phone"].Value.ToString();
            txtCustomerAddress.Text = selectedRow.Cells["Address"].Value.ToString();

            // Chuyển sang chế độ chỉnh sửa
            isEditMode = true;
            
            // Hiển thị nút Lưu và Hủy, ẩn nút Thêm
            btnAdd.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvCustomers.SelectedRows[0];
            int customerId = Convert.ToInt32(selectedRow.Cells["CustomerID"].Value);
            string customerName = selectedRow.Cells["Name"].Value.ToString();

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{customerName}'?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    CustomerManager.DeleteCustomer(customerId);
                    MessageBox.Show("Xóa khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshCustomerList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCustomerList();
            ClearInputFields();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateCustomerData())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin khách hàng hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var customer = new Customer
                {
                    CustomerID = editingCustomerId,
                    Name = txtCustomerName.Text,
                    Email = txtCustomerEmail.Text,
                    Phone = txtCustomerPhone.Text,
                    Address = txtCustomerAddress.Text
                };

                CustomerManager.UpdateCustomer(customer);
                MessageBox.Show("Cập nhật khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Cập nhật danh sách và đặt lại form
                RefreshCustomerList();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void dgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditCustomer_Click(sender, e);
            }
        }

        private void ShowCustomerPurchaseHistory(int customerId)
        {
            // TODO: Hiển thị lịch sử mua hàng của khách hàng
            // Có thể thêm code để hiển thị danh sách đơn hàng của khách hàng
        }

        private void dgvCustomers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCustomers?.SelectedRows.Count > 0 && dgvCustomers.SelectedRows[0].DataBoundItem is Customer customer)
            {
                DisplayCustomerData(customer);
                LoadCustomerPurchaseHistory(customer.CustomerID);
            }
        }
        
        private void DisplayCustomerData(Customer customer)
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
        
        private void LoadCustomerPurchaseHistory(int customerId)
        {
            try
            {
                var orders = OrderManager.GetOrdersByCustomerId(customerId);
                
                // Clear existing rows
                dataGridViewPurchaseHistory.Rows.Clear();
                
                // Add orders to the purchase history grid
                foreach (var order in orders)
                {
                    dataGridViewPurchaseHistory.Rows.Add(
                        order.OrderID,
                        order.OrderDate.ToString("dd/MM/yyyy"),
                        order.TotalAmount,
                        order.PaymentMethod
                    );
                }
            }
            catch (Exception ex)
            {
                // Just log the exception but don't show message box to avoid overwhelming the user
                Console.WriteLine($"Error loading purchase history: {ex.Message}");
            }
        }

        private void TxtSearchCustomer_TextChanged(object? sender, EventArgs e)
        {
            string searchTerm = txtSearchCustomer?.Text.ToLower() ?? string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    // If search box is empty, load all customers
                    LoadCustomers();
                    return;
                }
                
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
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = CustomerManager.GetCustomers();
                dgvCustomers.DataSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

