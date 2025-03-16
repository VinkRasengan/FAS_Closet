using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
            // Sự kiện chuyển tab
            btnSwitchToRegister.Click += BtnSwitchToRegister_Click;
            btnSwitchToLogin.Click += BtnSwitchToLogin_Click;
            txtRegPassword.TextChanged += TxtRegPassword_TextChanged;
        }

        private void BtnSwitchToRegister_Click(object? sender, EventArgs e)
        {
            tabControlAuth.SelectedTab = tabPageRegister;
        }

        private void BtnSwitchToLogin_Click(object? sender, EventArgs e)
        {
            tabControlAuth.SelectedTab = tabPageLogin;
        }

        // Xử lý đăng nhập
        private void btnLogin_Click(object? sender, EventArgs e)
        {
            errorProviderLogin.Clear();
            bool hasError = false;
            if (string.IsNullOrWhiteSpace(txtLoginUsername.Text))
            {
                errorProviderLogin.SetError(txtLoginUsername, "Username is required.");
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtLoginPassword.Text))
            {
                errorProviderLogin.SetError(txtLoginPassword, "Password is required.");
                hasError = true;
            }
            if (!hasError)
            {
                UserManager userManager = new UserManager();
                User? user = userManager.Login(txtLoginUsername.Text, txtLoginPassword.Text);
                if (user != null)
                {
                    MainForm mainForm = new MainForm(user);
                    mainForm.Show();
                    this.Hide();
                    mainForm.FormClosed += (s, args) => { this.Show(); };
                }
                else
                {
                    errorProviderLogin.SetError(txtLoginUsername, "Invalid credentials.");
                    errorProviderLogin.SetError(txtLoginPassword, "Invalid credentials.");
                }
            }
        }

        // Xử lý đăng ký (giản lược một số kiểm tra)
        private void btnRegister_Click(object? sender, EventArgs e)
        {
            errorProviderRegister.Clear();
            // Kiểm tra các trường bắt buộc (bạn có thể mở rộng kiểm tra tương tự như RegisterForm cũ)
            if (string.IsNullOrWhiteSpace(txtRegUsername.Text) ||
                string.IsNullOrWhiteSpace(txtRegPassword.Text) ||
                string.IsNullOrWhiteSpace(txtRegConfirmPassword.Text) ||
                txtRegPassword.Text != txtRegConfirmPassword.Text)
            {
                errorProviderRegister.SetError(txtRegUsername, "Please fill all required fields correctly.");
                return;
            }
            if (UserManager.IsUsernameTaken(txtRegUsername.Text))
            {
                errorProviderRegister.SetError(txtRegUsername, "Username already exists.");
                return;
            }
            byte[] passwordHash, passwordSalt;
            PasswordHasher.CreatePasswordHash(txtRegPassword.Text, out passwordHash, out passwordSalt);
            User user = new User
            {
                Username = txtRegUsername.Text,
                Name = txtRegName.Text,
                Email = txtRegEmail.Text,
                Phone = txtRegPhone.Text,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            try
            {
                UserManager.RegisterUser(user);
                MessageBox.Show("Registration successful. Please log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlAuth.SelectedTab = tabPageLogin;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtRegPassword_TextChanged(object? sender, EventArgs e)
        {
            // Update password strength indicator
            int strength = CalculatePasswordStrength(txtRegPassword.Text);
            progressBarPasswordStrength.Value = strength;
        }

        private static int CalculatePasswordStrength(string password)
        {
            int score = 0;
            if (password.Length >= 8) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"\d")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[^\w\d\s]")) score++;
            return score * 20;
        }
    }
}
