using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class LoginForm : Form
    {
        private MainForm? mainForm;

        public LoginForm()
        {
            InitializeComponent();
            AddValidationEvents();
        }

        private void AddValidationEvents()
        {
            txtUsername.Leave += new EventHandler(txtUsername_Leave);
            txtPassword.Leave += new EventHandler(txtPassword_Leave);
        }

        private void txtUsername_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username is required.");
            }
            else
            {
                errorProvider1.SetError(txtUsername, "");
            }
        }

        private void txtPassword_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is required.");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username is required.");
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is required.");
                hasError = true;
            }

            if (!hasError)
            {
                UserManager userManager = new UserManager();
                User? user = userManager.Login(txtUsername.Text, txtPassword.Text);

                if (user != null)
                {
                    mainForm = new MainForm(user);
                    mainForm.FormClosed += MainForm_FormClosed;
                    this.Hide();
                    mainForm.Show();
                }
                else
                {
                    errorProvider1.SetError(txtUsername, "Invalid username or password.");
                    errorProvider1.SetError(txtPassword, "Invalid username or password.");
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
}
