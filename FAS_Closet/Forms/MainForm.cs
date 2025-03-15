using System;
using System.Windows.Forms;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(User user)
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome, " + user.Name;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
