using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class RegisterForm : Form
    {
        private const string RegistrationFailed = "Registration Failed";

        private bool isUsernameValid = false;
        private bool isPasswordValid = false;
        private bool isConfirmPasswordValid = false;
        private bool isNameValid = false;
        private bool isEmailValid = false;
        private bool isPhoneValid = false;

        public RegisterForm()
        {
            InitializeComponent();
            AddValidationEvents();
            btnRegister.Enabled = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
        }

        private void AddValidationEvents()
        {
            txtUsername.Leave += new EventHandler(txtUsername_Leave);
            txtPassword.Leave += new EventHandler(txtPassword_Leave);
            txtConfirmPassword.Leave += new EventHandler(txtConfirmPassword_Leave);
            txtName.Leave += new EventHandler(txtName_Leave);
            txtEmail.Leave += new EventHandler(txtEmail_Leave);
            txtPhone.Leave += new EventHandler(txtPhone_Leave);
        }

        private void txtUsername_Leave(object? sender, EventArgs e)
        {
            errorProvider1.SetError(txtUsername, "");
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username is required.");
                isUsernameValid = false;
            }
            else
            {
                isUsernameValid = true;
            }
            UpdateButtonState();
        }

        private void txtPassword_Leave(object? sender, EventArgs e)
        {
            errorProvider1.SetError(txtPassword, "");
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is required.");
                isPasswordValid = false;
            }
            else if (txtPassword.Text.Length < 8)
            {
                errorProvider1.SetError(txtPassword, "Password must be at least 8 characters long.");
                isPasswordValid = false;
            }
            else
            {
                isPasswordValid = true;
            }
            ValidateConfirmPassword(); // Revalidate confirm password
            UpdateButtonState();
        }

        private void txtConfirmPassword_Leave(object? sender, EventArgs e)
        {
            ValidateConfirmPassword();
            UpdateButtonState();
        }

        private void ValidateConfirmPassword()
        {
            errorProvider1.SetError(txtConfirmPassword, "");
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, "Confirm Password is required.");
                isConfirmPasswordValid = false;
            }
            else if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Passwords do not match.");
                isConfirmPasswordValid = false;
            }
            else
            {
                isConfirmPasswordValid = true;
            }
        }

        private void txtName_Leave(object? sender, EventArgs e)
        {
            errorProvider1.SetError(txtName, "");
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider1.SetError(txtName, "Name is required.");
                isNameValid = false;
            }
            else if (txtName.Text.Any(char.IsDigit))
            {
                errorProvider1.SetError(txtName, "Name should not contain numbers.");
                isNameValid = false;
            }
            else
            {
                isNameValid = true;
            }
            UpdateButtonState();
        }

        private void txtEmail_Leave(object? sender, EventArgs e)
        {
            errorProvider1.SetError(txtEmail, "");
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Email is required.");
                isEmailValid = false;
            }
            else if (!Regex.IsMatch(txtEmail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                errorProvider1.SetError(txtEmail, "Invalid email address.");
                isEmailValid = false;
            }
            else
            {
                isEmailValid = true;
            }
            UpdateButtonState();
        }

        private void txtPhone_Leave(object? sender, EventArgs e)
        {
            errorProvider1.SetError(txtPhone, "");
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Phone number is required.");
                isPhoneValid = false;
            }
            else if (!IsValidVietnamesePhoneNumber(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Invalid phone number. It should start with '02' and be 10 or 11 digits long.");
                isPhoneValid = false;
            }
            else
            {
                isPhoneValid = true;
            }
            UpdateButtonState();
        }

        private static bool IsValidVietnamesePhoneNumber(string phoneNumber)
        {
            // Remove spaces and hyphens
            phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "");

            // Check if it starts with +84 or 0
            if (phoneNumber.StartsWith("+84"))
            {
                phoneNumber = phoneNumber.Substring(3); // Remove +84
            }
            else if (phoneNumber.StartsWith('0'))
            {
                // Keep as is, since Vietnamese numbers typically start with 0
            }
            else
            {
                return false; // Invalid format
            }

            // Check length (typically 9-10 digits after removing country code)
            if (phoneNumber.Length != 9 && phoneNumber.Length != 10)
            {
                return false;
            }

            // Use regex to check remaining digits (should only contain numbers)
            string pattern = @"^\d+$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private void UpdateButtonState()
        {
            btnRegister.Enabled = isUsernameValid && isPasswordValid && isConfirmPasswordValid &&
                                  isNameValid && isEmailValid && isPhoneValid;
            ShowGeneralWarning();
        }

        private void ShowGeneralWarning()
        {
            if (!isUsernameValid || !isPasswordValid || !isConfirmPasswordValid ||
                !isNameValid || !isEmailValid || !isPhoneValid)
            {
                lblWarning.Text = "Vui lòng nhập đầy đủ và chính xác thông tin đăng ký.";
                lblWarning.Visible = true;
            }
            else
            {
                lblWarning.Text = "";
                lblWarning.Visible = false;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (UserManager.IsUsernameTaken(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username already exists.");
                lblWarning.Text = "Username already exists.";
                lblWarning.Visible = true;
                return;
            }

            User user = new User
            {
                Username = txtUsername.Text,
                Name = txtName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text
            };

            PasswordHasher.CreatePasswordHash(txtPassword.Text, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            try
            {
                UserManager.RegisterUser(user);
                MessageBox.Show("Registration successful. Please log in.", "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, RegistrationFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
