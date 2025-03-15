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
        }

        private void btnLogin_Click(object sender, EventArgs e)
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

            if (errorProvider1.GetError(txtUsername) == "" && errorProvider1.GetError(txtPassword) == "")
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
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
