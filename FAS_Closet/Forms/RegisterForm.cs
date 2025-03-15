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
        private readonly UserManager userManager;
        private const string RegistrationFailed = "Registration Failed";

        public RegisterForm()
        {
            userManager = new UserManager();
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username is required.");
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is required.");
            }
            else if (txtPassword.Text.Length < 8)
            {
                errorProvider1.SetError(txtPassword, "Password must be at least 8 characters long.");
            }
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, "Confirm Password is required.");
            }
            else if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Passwords do not match.");
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider1.SetError(txtName, "Name is required.");
            }
            else if (txtName.Text.Any(char.IsDigit))
            {
                errorProvider1.SetError(txtName, "Name should not contain numbers.");
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Email is required.");
            }
            else if (!Regex.IsMatch(txtEmail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                errorProvider1.SetError(txtEmail, "Invalid email address.");
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Phone number is required.");
            }

            if (errorProvider1.GetError(txtUsername) == "" && errorProvider1.GetError(txtPassword) == "" &&
                errorProvider1.GetError(txtConfirmPassword) == "" && errorProvider1.GetError(txtName) == "" &&
                errorProvider1.GetError(txtEmail) == "" && errorProvider1.GetError(txtPhone) == "")
            {
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
}
