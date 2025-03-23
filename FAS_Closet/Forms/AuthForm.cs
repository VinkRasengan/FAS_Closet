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
        private bool _isFormClosing = false;
        
        public AuthForm()
        {
            InitializeComponent();
            
            // Add event handlers
            this.Load += AuthForm_Load;
            this.FormClosing += AuthForm_FormClosing;
            btnSwitchToRegister.Click += BtnSwitchToRegister_Click;
            btnSwitchToLogin.Click += BtnSwitchToLogin_Click;
            txtRegPassword.TextChanged += TxtRegPassword_TextChanged;
            txtRegConfirmPassword.TextChanged += TxtRegConfirmPassword_TextChanged;
            txtRegEmail.TextChanged += TxtRegEmail_TextChanged;
            txtRegPhone.TextChanged += TxtRegPhone_TextChanged;
            txtRegUsername.TextChanged += TxtRegUsername_TextChanged;
            
            // Apply some UI improvements
            ApplyVisualStyles();
        }

        private void AuthForm_Load(object? sender, EventArgs e)
        {
            // Focus on username field when form loads
            txtLoginUsername.Focus();
        }

        private void AuthForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!_isFormClosing && e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit the application?",
                    "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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

        private void BtnSwitchToRegister_Click(object? sender, EventArgs e)
        {
            tabControlAuth.SelectedTab = tabPageRegister;
            txtRegUsername.Focus();
        }

        private void BtnSwitchToLogin_Click(object? sender, EventArgs e)
        {
            tabControlAuth.SelectedTab = tabPageLogin;
            txtLoginUsername.Focus();
        }

        // Password strength indicator
        private void TxtRegPassword_TextChanged(object? sender, EventArgs e)
        {
            // Update password strength indicator
            int strength = CalculatePasswordStrength(txtRegPassword.Text);
            progressBarPasswordStrength.Value = strength;
            
            // Set color based on strength
            if (strength < 40)
                progressBarPasswordStrength.ForeColor = Color.Red;
            else if (strength < 70)
                progressBarPasswordStrength.ForeColor = Color.Orange;
            else
                progressBarPasswordStrength.ForeColor = Color.Green;
                
            // Check if passwords match
            if (txtRegConfirmPassword.Text.Length > 0)
            {
                ValidatePasswordMatch();
            }
        }
        
        // Confirm password validation
        private void TxtRegConfirmPassword_TextChanged(object? sender, EventArgs e)
        {
            ValidatePasswordMatch();
        }
        
        private void ValidatePasswordMatch()
        {
            if (txtRegPassword.Text != txtRegConfirmPassword.Text)
            {
                errorProviderRegister.SetError(txtRegConfirmPassword, "Passwords do not match");
            }
            else
            {
                errorProviderRegister.SetError(txtRegConfirmPassword, "");
            }
        }
        
        // Email validation
        private void TxtRegEmail_TextChanged(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegEmail.Text) && !IsValidEmail(txtRegEmail.Text))
            {
                errorProviderRegister.SetError(txtRegEmail, "Invalid email format");
            }
            else
            {
                errorProviderRegister.SetError(txtRegEmail, "");
            }
        }
        
        // Phone number validation
        private void TxtRegPhone_TextChanged(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegPhone.Text) && !IsValidPhoneNumber(txtRegPhone.Text))
            {
                errorProviderRegister.SetError(txtRegPhone, "Invalid phone format");
            }
            else
            {
                errorProviderRegister.SetError(txtRegPhone, "");
            }
        }
        
        // Username validation
        private void TxtRegUsername_TextChanged(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegUsername.Text) && txtRegUsername.Text.Length < 3)
            {
                errorProviderRegister.SetError(txtRegUsername, "Username must be at least 3 characters");
            }
            else
            {
                errorProviderRegister.SetError(txtRegUsername, "");
            }
        }

        // Login logic
        private void btnLogin_Click(object? sender, EventArgs e)
        {
            errorProviderLogin.Clear();
            bool hasError = false;
            
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtLoginUsername.Text))
            {
                errorProviderLogin.SetError(txtLoginUsername, "Username is required");
                hasError = true;
            }
            
            if (string.IsNullOrWhiteSpace(txtLoginPassword.Text))
            {
                errorProviderLogin.SetError(txtLoginPassword, "Password is required");
                hasError = true;
            }
            
            if (!hasError)
            {
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
                        MessageBox.Show("Invalid username or password. Please try again.", 
                            "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during login: {ex.Message}", 
                        "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        // Registration logic - fixed method name to follow naming convention
        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            // Extracted to helper methods to reduce cognitive complexity
            if (!ValidateRegistrationInputs())
                return;

            ProcessRegistration();
        }
        
        private bool ValidateRegistrationInputs()
        {
            errorProviderRegister.Clear();
            bool hasError = false;
            
            // Validate username
            if (string.IsNullOrWhiteSpace(txtRegUsername.Text))
            {
                errorProviderRegister.SetError(txtRegUsername, "Username is required");
                hasError = true;
            }
            else if (txtRegUsername.Text.Length < 3)
            {
                errorProviderRegister.SetError(txtRegUsername, "Username must be at least 3 characters");
                hasError = true;
            }
            
            // Validate password
            if (string.IsNullOrWhiteSpace(txtRegPassword.Text))
            {
                errorProviderRegister.SetError(txtRegPassword, "Password is required");
                hasError = true;
            }
            else if (txtRegPassword.Text.Length < MinPasswordLength)
            {
                errorProviderRegister.SetError(txtRegPassword, $"Password must be at least {MinPasswordLength} characters");
                hasError = true;
            }
            
            // Validate password confirmation
            if (string.IsNullOrWhiteSpace(txtRegConfirmPassword.Text))
            {
                errorProviderRegister.SetError(txtRegConfirmPassword, "Please confirm your password");
                hasError = true;
            }
            else if (txtRegPassword.Text != txtRegConfirmPassword.Text)
            {
                errorProviderRegister.SetError(txtRegConfirmPassword, "Passwords do not match");
                hasError = true;
            }
            
            // Validate name
            if (string.IsNullOrWhiteSpace(txtRegName.Text))
            {
                errorProviderRegister.SetError(txtRegName, "Name is required");
                hasError = true;
            }
            
            // Validate email
            if (string.IsNullOrWhiteSpace(txtRegEmail.Text))
            {
                errorProviderRegister.SetError(txtRegEmail, "Email is required");
                hasError = true;
            }
            else if (!IsValidEmail(txtRegEmail.Text))
            {
                errorProviderRegister.SetError(txtRegEmail, "Invalid email format");
                hasError = true;
            }
            
            // Validate phone
            if (string.IsNullOrWhiteSpace(txtRegPhone.Text))
            {
                errorProviderRegister.SetError(txtRegPhone, "Phone number is required");
                hasError = true;
            }
            else if (!IsValidPhoneNumber(txtRegPhone.Text))
            {
                errorProviderRegister.SetError(txtRegPhone, "Invalid phone format");
                hasError = true;
            }
            
            return !hasError;
        }
        
        private void ProcessRegistration()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // Check if username already exists
                if (UserManager.IsUsernameTaken(txtRegUsername.Text))
                {
                    errorProviderRegister.SetError(txtRegUsername, "Username already exists");
                    Cursor = Cursors.Default;
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
                
                MessageBox.Show("Registration successful! You can now log in with your new account.", 
                    "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Switch to login tab and pre-fill username
                tabControlAuth.SelectedTab = tabPageLogin;
                txtLoginUsername.Text = txtRegUsername.Text;
                txtLoginPassword.Focus();
                
                // Clear registration form
                ClearRegistrationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}", 
                    "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
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
        }

        private static int CalculatePasswordStrength(string password)
        {
            int score = 0;
            if (string.IsNullOrEmpty(password)) return 0;
            
            // Length check
            if (password.Length >= MinPasswordLength) score += 20;
            if (password.Length >= 10) score += 10;
            
            // Character variety checks
            if (Regex.IsMatch(password, @"\d")) score += 15; // Numbers
            if (Regex.IsMatch(password, @"[a-z]")) score += 15; // Lowercase
            if (Regex.IsMatch(password, @"[A-Z]")) score += 15; // Uppercase
            if (Regex.IsMatch(password, @"[^a-zA-Z0-9]")) score += 25; // Special chars
            
            return Math.Min(score, 100); // Fix: Math.min -> Math.Min
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPhoneNumber(string phone)
        {
            // Simple pattern check: allow digits, spaces, dashes, and parentheses
            return Regex.IsMatch(phone, @"^[\d\s\(\)\-\+]+$");
        }
    }
}
