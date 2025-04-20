using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.ComponentModel;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class CustomerForm : Form
    {
        private readonly ErrorProvider _errorProvider = new ErrorProvider();
        private bool _isEditMode = false;
        private Customer _customer = new Customer();

        public Customer Customer => _customer;

        public CustomerForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;

            // Setting up null event handlers to prevent errors
            this.FormClosing += CustomerForm_FormClosing;
        }

        public CustomerForm(Customer customer) : this()
        {
            _isEditMode = true;
            _customer = customer;
            this.Text = "Chỉnh sửa khách hàng";
            LoadCustomerData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Thêm khách hàng mới";
            this.Size = new Size(450, 400);
            this.BackColor = Color.White;
            
            // Create labels
            Label lblCustomerName = new Label
            {
                Text = "Tên khách hàng:",
                Location = new Point(30, 30),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblCustomerEmail = new Label
            {
                Text = "Email:",
                Location = new Point(30, 70),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblCustomerPhone = new Label
            {
                Text = "Điện thoại:",
                Location = new Point(30, 110),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblCustomerAddress = new Label
            {
                Text = "Địa chỉ:",
                Location = new Point(30, 150),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            txtCustomerName = new TextBox
            {
                Location = new Point(150, 30),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtCustomerName.Validating += TxtCustomerName_Validating;

            txtCustomerEmail = new TextBox
            {
                Location = new Point(150, 70),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtCustomerEmail.Validating += TxtCustomerEmail_Validating;

            txtCustomerPhone = new TextBox
            {
                Location = new Point(150, 110),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtCustomerPhone.Validating += TxtCustomerPhone_Validating;

            txtCustomerAddress = new TextBox
            {
                Location = new Point(150, 150),
                Size = new Size(250, 80),
                Multiline = true,
                Font = new Font("Segoe UI", 9.5F)
            };
            txtCustomerAddress.Validating += TxtCustomerAddress_Validating;

            // Save button
            btnSave = new Button
            {
                Text = "Lưu",
                Location = new Point(150, 280),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            // Cancel button
            btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(270, 280),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            // Add controls to form
            this.Controls.Add(lblCustomerName);
            this.Controls.Add(txtCustomerName);
            this.Controls.Add(lblCustomerEmail);
            this.Controls.Add(txtCustomerEmail);
            this.Controls.Add(lblCustomerPhone);
            this.Controls.Add(txtCustomerPhone);
            this.Controls.Add(lblCustomerAddress);
            this.Controls.Add(txtCustomerAddress);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtCustomerName;
        private TextBox txtCustomerEmail;
        private TextBox txtCustomerPhone;
        private TextBox txtCustomerAddress;
        private Button btnSave;
        private Button btnCancel;

        private void LoadCustomerData()
        {
            if (_customer != null)
            {
                txtCustomerName.Text = _customer.Name;
                txtCustomerEmail.Text = _customer.Email;
                txtCustomerPhone.Text = _customer.Phone;
                txtCustomerAddress.Text = _customer.Address;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                _customer.Name = txtCustomerName.Text.Trim();
                _customer.Email = txtCustomerEmail.Text.Trim();
                _customer.Phone = txtCustomerPhone.Text.Trim();
                _customer.Address = txtCustomerAddress.Text.Trim();

                try
                {
                    if (_isEditMode)
                    {
                        // Update existing customer
                        CustomerManager.UpdateCustomer(_customer);
                    }
                    else
                    {
                        // Add new customer
                        CustomerManager.AddCustomer(_customer);
                    }
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CustomerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _errorProvider.Clear();
        }

        #region Validation Methods
        private void TxtCustomerName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                _errorProvider.SetError(txtCustomerName, "Vui lòng nhập tên khách hàng");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtCustomerName, "");
            }
        }

        private void TxtCustomerEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCustomerEmail.Text))
            {
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(txtCustomerEmail.Text, emailPattern))
                {
                    _errorProvider.SetError(txtCustomerEmail, "Email không hợp lệ");
                    e.Cancel = true;
                }
                else
                {
                    _errorProvider.SetError(txtCustomerEmail, "");
                }
            }
            else
            {
                _errorProvider.SetError(txtCustomerEmail, "Vui lòng nhập email");
                e.Cancel = true;
            }
        }

        private void TxtCustomerPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerPhone.Text))
            {
                _errorProvider.SetError(txtCustomerPhone, "Vui lòng nhập số điện thoại");
                e.Cancel = true;
            }
            else
            {
                string phonePattern = @"^(0|\+84)\d{9,10}$";
                if (!Regex.IsMatch(txtCustomerPhone.Text, phonePattern))
                {
                    _errorProvider.SetError(txtCustomerPhone, "Số điện thoại không hợp lệ (định dạng: 0xxxxxxxxx hoặc +84xxxxxxxxx)");
                    e.Cancel = true;
                }
                else
                {
                    _errorProvider.SetError(txtCustomerPhone, "");
                }
            }
        }

        private void TxtCustomerAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerAddress.Text))
            {
                _errorProvider.SetError(txtCustomerAddress, "Vui lòng nhập địa chỉ");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtCustomerAddress, "");
            }
        }
        #endregion
    }
}