// This file defines the AuthForm class, which handles user authentication.

using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class AuthForm : Form
    {
        private const int MinPasswordLength = 6;
        private const int MinUsernameLength = 3;
        private bool _isFormClosing = false;
        
        public AuthForm()
        {
            InitializeComponent();
            
            // Add event handlers
            this.Load += AuthForm_Load;
            this.FormClosing += AuthForm_FormClosing;
            
            // Login field events
            btnSwitchToRegister.Click += BtnSwitchToRegister_Click;
            txtLoginUsername.TextChanged += TxtLoginUsername_TextChanged;
            txtLoginPassword.TextChanged += TxtLoginPassword_TextChanged;
            
            // Register field events
            btnSwitchToLogin.Click += BtnSwitchToLogin_Click;
            txtRegPassword.TextChanged += TxtRegPassword_TextChanged;
            txtRegConfirmPassword.TextChanged += TxtRegConfirmPassword_TextChanged;
            txtRegEmail.TextChanged += TxtRegEmail_TextChanged;
            txtRegPhone.TextChanged += TxtRegPhone_TextChanged;
            txtRegUsername.TextChanged += TxtRegUsername_TextChanged;
            txtRegName.TextChanged += TxtRegName_TextChanged;
            
            // Apply visual styling
            ApplyVisualStyles();
        }

        private void AuthForm_Load(object? sender, EventArgs e)
        {
            // Focus on username field when form loads
            txtLoginUsername.Focus();
            
            // Set default values for better appearance
            progressBarPasswordStrength.Value = 0;
            lblPasswordStrength.Text = "Độ mạnh mật khẩu: Chưa nhập";
            progressBarPasswordStrength.ForeColor = Color.Gray;
        }

        private void AuthForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!_isFormClosing && e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Bạn có chắc muốn thoát ứng dụng?", 
                    "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    _isFormClosing = true;
                    Application.Exit();
                }
            }
        }

        private void ApplyVisualStyles()
        {
            // Apply visual styles to buttons
            btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            
            btnRegister.BackColor = Color.FromArgb(0, 123, 255);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            
            btnSwitchToRegister.BackColor = Color.FromArgb(108, 117, 125);
            btnSwitchToRegister.ForeColor = Color.White;
            btnSwitchToRegister.FlatStyle = FlatStyle.Flat;
            btnSwitchToRegister.FlatAppearance.BorderSize = 0;
            btnSwitchToRegister.Cursor = Cursors.Hand;
            
            btnSwitchToLogin.BackColor = Color.FromArgb(108, 117, 125);
            btnSwitchToLogin.ForeColor = Color.White;
            btnSwitchToLogin.FlatStyle = FlatStyle.Flat;
            btnSwitchToLogin.FlatAppearance.BorderSize = 0;
            btnSwitchToLogin.Cursor = Cursors.Hand;
            
            // Set background color for tab control
            tabControlAuth.BackColor = Color.FromArgb(248, 249, 250);
        }
        
        #region Login Tab Events and Validation
        
        private void TxtLoginUsername_TextChanged(object? sender, EventArgs e)
        {
            ValidateLoginUsername();
            HideLoginError();
        }
        
        private void TxtLoginPassword_TextChanged(object? sender, EventArgs e)
        {
            ValidateLoginPassword();
            HideLoginError();
        }
        
        private bool ValidateLoginUsername()
        {
            if (string.IsNullOrWhiteSpace(txtLoginUsername.Text))
            {
                errorProviderLogin.SetError(txtLoginUsername, "Vui lòng nhập tên đăng nhập");
                return false;
            }
            errorProviderLogin.SetError(txtLoginUsername, "");
            return true;
        }
        
        private bool ValidateLoginPassword()
        {
            if (string.IsNullOrWhiteSpace(txtLoginPassword.Text))
            {
                errorProviderLogin.SetError(txtLoginPassword, "Vui lòng nhập mật khẩu");
                return false;
            }
            errorProviderLogin.SetError(txtLoginPassword, "");
            return true;
        }
        
        private void ShowLoginError(string message)
        {
            lblLoginError.Text = message;
            lblLoginError.Visible = true;
        }
        
        private void HideLoginError()
        {
            lblLoginError.Visible = false;
        }

        private void BtnSwitchToRegister_Click(object? sender, EventArgs e)
        {
            tabControlAuth.SelectedTab = tabPageRegister;
            txtRegUsername.Focus();
            errorProviderLogin.Clear();
            HideLoginError();
        }
        
        #endregion

        #region Register Tab Events and Validation
        
        private void BtnSwitchToLogin_Click(object? sender, EventArgs e)
        {
            tabControlAuth.SelectedTab = tabPageLogin;
            txtLoginUsername.Focus();
            errorProviderRegister.Clear();
            HideRegisterMessages();
        }
        
        private void TxtRegUsername_TextChanged(object? sender, EventArgs e)
        {
            ValidateRegUsername();
            HideRegisterMessages();
        }
        
        private void TxtRegPassword_TextChanged(object? sender, EventArgs e)
        {
            // Update password strength indicator
            int strength = CalculatePasswordStrength(txtRegPassword.Text);
            progressBarPasswordStrength.Value = strength;
            
            // Update password strength label
            if (strength == 0)
            {
                lblPasswordStrength.Text = "Độ mạnh mật khẩu: Chưa nhập";
                progressBarPasswordStrength.ForeColor = Color.Gray;
            }
            else if (strength < 40)
            {
                lblPasswordStrength.Text = "Độ mạnh mật khẩu: Yếu";
                progressBarPasswordStrength.ForeColor = Color.Red;
            }
            else if (strength < 70)
            {
                lblPasswordStrength.Text = "Độ mạnh mật khẩu: Trung bình";
                progressBarPasswordStrength.ForeColor = Color.Orange;
            }
            else
            {
                lblPasswordStrength.Text = "Độ mạnh mật khẩu: Mạnh";
                progressBarPasswordStrength.ForeColor = Color.Green;
            }
            
            ValidateRegPassword();
            
            // Check if passwords match
            if (txtRegConfirmPassword.Text.Length > 0)
            {
                ValidateRegConfirmPassword();
            }
            
            HideRegisterMessages();
        }
        
        private void TxtRegConfirmPassword_TextChanged(object? sender, EventArgs e)
        {
            ValidateRegConfirmPassword();
            HideRegisterMessages();
        }
        
        private void TxtRegName_TextChanged(object? sender, EventArgs e)
        {
            ValidateRegName();
            HideRegisterMessages();
        }
        
        private void TxtRegEmail_TextChanged(object? sender, EventArgs e)
        {
            ValidateRegEmail();
            HideRegisterMessages();
        }
        
        private void TxtRegPhone_TextChanged(object? sender, EventArgs e)
        {
            ValidateRegPhone();
            HideRegisterMessages();
        }
        
        private bool ValidateRegUsername()
        {
            if (string.IsNullOrWhiteSpace(txtRegUsername.Text))
            {
                errorProviderRegister.SetError(txtRegUsername, "Vui lòng nhập tên đăng nhập");
                return false;
            }
            else if (txtRegUsername.Text.Length < MinUsernameLength)
            {
                errorProviderRegister.SetError(txtRegUsername, $"Tên đăng nhập phải có ít nhất {MinUsernameLength} ký tự");
                return false;
            }
            else if (!Regex.IsMatch(txtRegUsername.Text, @"^[a-zA-Z0-9_]+$"))
            {
                errorProviderRegister.SetError(txtRegUsername, "Tên đăng nhập chỉ được chứa ký tự, số và dấu gạch dưới");
                return false;
            }
            
            errorProviderRegister.SetError(txtRegUsername, "");
            return true;
        }
        
        private bool ValidateRegPassword()
        {
            if (string.IsNullOrWhiteSpace(txtRegPassword.Text))
            {
                errorProviderRegister.SetError(txtRegPassword, "Vui lòng nhập mật khẩu");
                return false;
            }
            else if (txtRegPassword.Text.Length < MinPasswordLength)
            {
                errorProviderRegister.SetError(txtRegPassword, $"Mật khẩu phải có ít nhất {MinPasswordLength} ký tự");
                return false;
            }
            
            errorProviderRegister.SetError(txtRegPassword, "");
            return true;
        }
        
        private bool ValidateRegConfirmPassword()
        {
            if (string.IsNullOrWhiteSpace(txtRegConfirmPassword.Text))
            {
                errorProviderRegister.SetError(txtRegConfirmPassword, "Vui lòng xác nhận mật khẩu");
                return false;
            }
            else if (txtRegPassword.Text != txtRegConfirmPassword.Text)
            {
                errorProviderRegister.SetError(txtRegConfirmPassword, "Mật khẩu xác nhận không trùng khớp");
                return false;
            }
            
            errorProviderRegister.SetError(txtRegConfirmPassword, "");
            return true;
        }
        
        private bool ValidateRegName()
        {
            if (string.IsNullOrWhiteSpace(txtRegName.Text))
            {
                errorProviderRegister.SetError(txtRegName, "Vui lòng nhập họ và tên");
                return false;
            }
            else if (txtRegName.Text.Length < 2)
            {
                errorProviderRegister.SetError(txtRegName, "Họ và tên phải có ít nhất 2 ký tự");
                return false;
            }
            
            errorProviderRegister.SetError(txtRegName, "");
            return true;
        }
        
        private bool ValidateRegEmail()
        {
            if (string.IsNullOrWhiteSpace(txtRegEmail.Text))
            {
                errorProviderRegister.SetError(txtRegEmail, "Vui lòng nhập email");
                return false;
            }
            else if (!IsValidEmail(txtRegEmail.Text))
            {
                errorProviderRegister.SetError(txtRegEmail, "Email không đúng định dạng");
                return false;
            }
            
            errorProviderRegister.SetError(txtRegEmail, "");
            return true;
        }
        
        private bool ValidateRegPhone()
        {
            if (string.IsNullOrWhiteSpace(txtRegPhone.Text))
            {
                errorProviderRegister.SetError(txtRegPhone, "Vui lòng nhập số điện thoại");
                return false;
            }
            else if (!IsValidPhoneNumber(txtRegPhone.Text))
            {
                errorProviderRegister.SetError(txtRegPhone, "Số điện thoại không đúng định dạng");
                return false;
            }
            
            errorProviderRegister.SetError(txtRegPhone, "");
            return true;
        }
        
        private void ShowRegisterError(string message)
        {
            lblRegisterError.Text = message;
            lblRegisterError.Visible = true;
            lblRegisterSuccess.Visible = false;
        }
        
        private void ShowRegisterSuccess(string message)
        {
            lblRegisterSuccess.Text = message;
            lblRegisterSuccess.Visible = true;
            lblRegisterSuccess.ForeColor = Color.Green;
            lblRegisterSuccess.Font = new Font(lblRegisterSuccess.Font, FontStyle.Bold);
            lblRegisterError.Visible = false;
        }
        
        private void HideRegisterMessages()
        {
            lblRegisterError.Visible = false;
            lblRegisterSuccess.Visible = false;
        }
        
        #endregion

        // Login logic
        private void btnLogin_Click(object? sender, EventArgs e)
        {
            errorProviderLogin.Clear();
            HideLoginError();
            
            // Validate all inputs
            bool usernameValid = ValidateLoginUsername();
            bool passwordValid = ValidateLoginPassword();
            
            if (!usernameValid || !passwordValid)
            {
                ShowLoginError("Vui lòng điền đầy đủ thông tin đăng nhập");
                return;
            }
            
            Cursor = Cursors.WaitCursor;
            try
            {
                UserManager userManager = new UserManager();
                User? user = userManager.Login(txtLoginUsername.Text, txtLoginPassword.Text);
                
                if (user != null)
                {
                    MainForm mainForm = new MainForm(user);
                    mainForm.Show();
                    this.Hide();
                    mainForm.FormClosed += (s, args) => 
                    {
                        this.Show();
                        txtLoginPassword.Text = string.Empty; // Clear password for security
                        if (!chkRememberMe.Checked)
                            txtLoginUsername.Text = string.Empty;
                        txtLoginUsername.Focus();
                    };
                }
                else
                {
                    ShowLoginError("Tên đăng nhập hoặc mật khẩu không chính xác");
                }
            }
            catch (Exception ex)
            {
                ShowLoginError($"Đã xảy ra lỗi: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // Registration logic
        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            // Validate all inputs
            if (!ValidateAllRegistrationInputs())
                return;

            // Process registration
            Cursor = Cursors.WaitCursor;
            try
            {
                // Check if username already exists
                if (UserManager.IsUsernameTaken(txtRegUsername.Text))
                {
                    ShowRegisterError("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác");
                    errorProviderRegister.SetError(txtRegUsername, "Tên đăng nhập đã tồn tại");
                    txtRegUsername.Focus();
                    return;
                }
                
                // Check if email already exists
                if (UserManager.IsEmailTaken(txtRegEmail.Text))
                {
                    ShowRegisterError("Email này đã được sử dụng, vui lòng dùng email khác");
                    errorProviderRegister.SetError(txtRegEmail, "Email đã tồn tại");
                    txtRegEmail.Focus();
                    return;
                }
                
                // Check if phone already exists
                if (UserManager.IsPhoneTaken(txtRegPhone.Text))
                {
                    ShowRegisterError("Số điện thoại này đã được sử dụng, vui lòng dùng số khác");
                    errorProviderRegister.SetError(txtRegPhone, "Số điện thoại đã tồn tại");
                    txtRegPhone.Focus();
                    return;
                }
                
                // Create password hash and salt
                byte[] passwordHash, passwordSalt;
                PasswordHasher.CreatePasswordHash(txtRegPassword.Text, out passwordHash, out passwordSalt);
                
                // Create user object
                User user = new User
                {
                    Username = txtRegUsername.Text,
                    PasswordHash = Convert.ToBase64String(passwordHash),
                    PasswordSalt = Convert.ToBase64String(passwordSalt),
                    Name = txtRegName.Text,
                    Email = txtRegEmail.Text,
                    Phone = txtRegPhone.Text
                };
                
                // Register user
                UserManager.RegisterUser(user);
                
                // Show success message with more prominent display
                ShowRegisterSuccess("Đăng ký tài khoản thành công! Bạn có thể đăng nhập ngay bây giờ.");
                
                // Make sure we show a MessageBox for more visibility
                MessageBox.Show("Đăng ký tài khoản thành công!\nBạn sẽ được chuyển đến màn hình đăng nhập.", 
                                "Đăng ký thành công", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                
                // Clear form after successful registration
                ClearRegistrationForm();
                
                // Switch to login tab immediately after user acknowledges the message box
                tabControlAuth.SelectedTab = tabPageLogin;
                txtLoginUsername.Text = user.Username;
                txtLoginPassword.Focus();
            }
            catch (Exception ex)
            {
                ShowRegisterError($"Đã xảy ra lỗi: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool ValidateAllRegistrationInputs()
        {
            errorProviderRegister.Clear();
            HideRegisterMessages();
            
            bool isValid = true;
            
            if (!ValidateRegUsername())
                isValid = false;
                
            if (!ValidateRegPassword())
                isValid = false;
                
            if (!ValidateRegConfirmPassword())
                isValid = false;
                
            if (!ValidateRegName())
                isValid = false;
                
            if (!ValidateRegEmail())
                isValid = false;
                
            if (!ValidateRegPhone())
                isValid = false;
            
            if (!isValid)
            {
                ShowRegisterError("Vui lòng điền đầy đủ thông tin và sửa các lỗi");
            }
            
            return isValid;
        }

        private void ClearRegistrationForm()
        {
            txtRegUsername.Text = string.Empty;
            txtRegPassword.Text = string.Empty;
            txtRegConfirmPassword.Text = string.Empty;
            txtRegName.Text = string.Empty;
            txtRegEmail.Text = string.Empty;
            txtRegPhone.Text = string.Empty;
            progressBarPasswordStrength.Value = 0;
            lblPasswordStrength.Text = "Độ mạnh mật khẩu: Chưa nhập";
            errorProviderRegister.Clear();
        }

        // Password strength calculation
        private int CalculatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password)) 
                return 0;
            
            int score = 0;
            
            // Length evaluation
            if (password.Length >= MinPasswordLength)
                score += 10;
            if (password.Length >= 8)
                score += 10;
            if (password.Length >= 10)
                score += 10;
                
            // Character composition
            if (Regex.IsMatch(password, @"[a-z]"))
                score += 10; // Has lowercase
            if (Regex.IsMatch(password, @"[A-Z]"))
                score += 20; // Has uppercase
            if (Regex.IsMatch(password, @"\d"))
                score += 20; // Has digits
            if (Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
                score += 30; // Has special characters
                
            // Complexity checks
            if (Regex.IsMatch(password, @"[a-z]") && Regex.IsMatch(password, @"[A-Z]") && 
                Regex.IsMatch(password, @"\d"))
                score += 10; // Has mixed case and numbers
                
            if (Regex.IsMatch(password, @"[a-zA-Z\d]") && 
                Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
                score += 10; // Has alphanumeric and special chars
            
            return Math.Min(score, 100);
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
                
            try
            {
                // Simple pattern matching
                Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;
                
            // Vietnamese phone number format (can be adjusted to other formats)
            // Accepts formats like: 0912345678, 84912345678, +84912345678
            Regex regex = new Regex(@"^(0|\+?84|84)?([3|5|7|8|9])([0-9]{8})$");
            return regex.IsMatch(phone);
        }
    }
}
