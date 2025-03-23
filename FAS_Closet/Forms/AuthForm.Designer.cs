// This file contains the designer code for the AuthForm, which handles user authentication.

namespace FASCloset.Forms
{
    partial class AuthForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControlAuth;
        private System.Windows.Forms.TabPage tabPageLogin;
        private System.Windows.Forms.TabPage tabPageRegister;
        
        // Controls cho Login Tab
        private System.Windows.Forms.Label lblLoginHeader;
        private System.Windows.Forms.Label lblLoginUsername;
        private System.Windows.Forms.Label lblLoginPassword;
        private System.Windows.Forms.TextBox txtLoginUsername;
        private System.Windows.Forms.TextBox txtLoginPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnSwitchToRegister;
        private System.Windows.Forms.CheckBox chkRememberMe;
        private System.Windows.Forms.LinkLabel lnkForgotPassword;
        private System.Windows.Forms.ErrorProvider errorProviderLogin;

        // Controls cho Register Tab
        private System.Windows.Forms.Label lblRegisterHeader;
        private System.Windows.Forms.Label lblRegUsername;
        private System.Windows.Forms.Label lblRegPassword;
        private System.Windows.Forms.Label lblRegConfirmPassword;
        private System.Windows.Forms.Label lblRegName;
        private System.Windows.Forms.Label lblRegEmail;
        private System.Windows.Forms.Label lblRegPhone;
        private System.Windows.Forms.TextBox txtRegUsername;
        private System.Windows.Forms.TextBox txtRegPassword;
        private System.Windows.Forms.TextBox txtRegConfirmPassword;
        private System.Windows.Forms.TextBox txtRegName;
        private System.Windows.Forms.TextBox txtRegEmail;
        private System.Windows.Forms.TextBox txtRegPhone;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnSwitchToLogin;
        private System.Windows.Forms.ProgressBar progressBarPasswordStrength;
        private System.Windows.Forms.ErrorProvider errorProviderRegister;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) 
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.tabControlAuth = new System.Windows.Forms.TabControl();
            this.tabPageLogin = new System.Windows.Forms.TabPage();
            this.tabPageRegister = new System.Windows.Forms.TabPage();
            this.errorProviderLogin = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProviderRegister = new System.Windows.Forms.ErrorProvider(this.components);

            // --- Login Tab controls ---
            this.lblLoginHeader = new System.Windows.Forms.Label();
            this.lblLoginUsername = new System.Windows.Forms.Label();
            this.lblLoginPassword = new System.Windows.Forms.Label();
            this.txtLoginUsername = new System.Windows.Forms.TextBox();
            this.txtLoginPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnSwitchToRegister = new System.Windows.Forms.Button();
            this.chkRememberMe = new System.Windows.Forms.CheckBox();
            this.lnkForgotPassword = new System.Windows.Forms.LinkLabel();

            // --- Register Tab controls ---
            this.lblRegisterHeader = new System.Windows.Forms.Label();
            this.lblRegUsername = new System.Windows.Forms.Label();
            this.lblRegPassword = new System.Windows.Forms.Label();
            this.lblRegConfirmPassword = new System.Windows.Forms.Label();
            this.lblRegName = new System.Windows.Forms.Label();
            this.lblRegEmail = new System.Windows.Forms.Label();
            this.lblRegPhone = new System.Windows.Forms.Label();
            this.txtRegUsername = new System.Windows.Forms.TextBox();
            this.txtRegPassword = new System.Windows.Forms.TextBox();
            this.txtRegConfirmPassword = new System.Windows.Forms.TextBox();
            this.txtRegName = new System.Windows.Forms.TextBox();
            this.txtRegEmail = new System.Windows.Forms.TextBox();
            this.txtRegPhone = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnSwitchToLogin = new System.Windows.Forms.Button();
            this.progressBarPasswordStrength = new System.Windows.Forms.ProgressBar();

            // 
            // tabControlAuth
            // 
            this.tabControlAuth.Controls.Add(this.tabPageLogin);
            this.tabControlAuth.Controls.Add(this.tabPageRegister);
            this.tabControlAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAuth.Location = new System.Drawing.Point(0, 0);
            this.tabControlAuth.Name = "tabControlAuth";
            this.tabControlAuth.SelectedIndex = 0;
            this.tabControlAuth.Size = new System.Drawing.Size(400, 400);
            // 
            // tabPageLogin
            // 
            this.tabPageLogin.Text = "Đăng Nhập";
            this.tabPageLogin.Padding = new System.Windows.Forms.Padding(10);
            // Cài đặt các control cho tab đăng nhập:
            this.lblLoginHeader.Text = "Đăng Nhập";
            this.lblLoginHeader.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblLoginHeader.AutoSize = true;
            this.lblLoginHeader.Location = new System.Drawing.Point(130, 20);
            
            this.lblLoginUsername.Text = "Tên đăng nhập:";
            this.lblLoginUsername.AutoSize = true;
            this.lblLoginUsername.Location = new System.Drawing.Point(40, 70);
            this.txtLoginUsername.Location = new System.Drawing.Point(180, 65);
            this.txtLoginUsername.Width = 150;
            
            this.lblLoginPassword.Text = "Mật khẩu:";
            this.lblLoginPassword.AutoSize = true;
            this.lblLoginPassword.Location = new System.Drawing.Point(40, 110);
            this.txtLoginPassword.Location = new System.Drawing.Point(180, 105);
            this.txtLoginPassword.Width = 150;
            this.txtLoginPassword.PasswordChar = '*';
            
            this.chkRememberMe.Text = "Nhớ tôi";
            this.chkRememberMe.Location = new System.Drawing.Point(180, 135);
            
            this.lnkForgotPassword.Text = "Quên mật khẩu?";
            this.lnkForgotPassword.Location = new System.Drawing.Point(180, 160);
            
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Location = new System.Drawing.Point(40, 190);
            this.btnLogin.Width = 120;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            
            this.btnSwitchToRegister.Text = "Đăng ký";
            this.btnSwitchToRegister.Location = new System.Drawing.Point(210, 190);
            this.btnSwitchToRegister.Width = 120;
            
            // Thêm các control vào tabPageLogin
            this.tabPageLogin.Controls.Add(this.lblLoginHeader);
            this.tabPageLogin.Controls.Add(this.lblLoginUsername);
            this.tabPageLogin.Controls.Add(this.txtLoginUsername);
            this.tabPageLogin.Controls.Add(this.lblLoginPassword);
            this.tabPageLogin.Controls.Add(this.txtLoginPassword);
            this.tabPageLogin.Controls.Add(this.chkRememberMe);
            this.tabPageLogin.Controls.Add(this.lnkForgotPassword);
            this.tabPageLogin.Controls.Add(this.btnLogin);
            this.tabPageLogin.Controls.Add(this.btnSwitchToRegister);
            
            // 
            // tabPageRegister
            // 
            this.tabPageRegister.Text = "Đăng Ký";
            this.tabPageRegister.Padding = new System.Windows.Forms.Padding(10);
            // Cài đặt các control cho tab đăng ký:
            this.lblRegisterHeader.Text = "Đăng Ký";
            this.lblRegisterHeader.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblRegisterHeader.AutoSize = true;
            this.lblRegisterHeader.Location = new System.Drawing.Point(140, 20);
            
            this.lblRegUsername.Text = "Tên đăng nhập:";
            this.lblRegUsername.AutoSize = true;
            this.lblRegUsername.Location = new System.Drawing.Point(40, 70);
            this.txtRegUsername.Location = new System.Drawing.Point(190, 65);
            this.txtRegUsername.Width = 150;
            
            this.lblRegPassword.Text = "Mật khẩu:";
            this.lblRegPassword.AutoSize = true;
            this.lblRegPassword.Location = new System.Drawing.Point(40, 100);
            this.txtRegPassword.Location = new System.Drawing.Point(190, 95);
            this.txtRegPassword.Width = 150;
            this.txtRegPassword.PasswordChar = '*';
            
            this.lblRegConfirmPassword.Text = "Xác nhận mật khẩu:";
            this.lblRegConfirmPassword.AutoSize = true;
            this.lblRegConfirmPassword.Location = new System.Drawing.Point(40, 130);
            this.txtRegConfirmPassword.Location = new System.Drawing.Point(190, 125);
            this.txtRegConfirmPassword.Width = 150;
            this.txtRegConfirmPassword.PasswordChar = '*';
            
            this.lblRegName.Text = "Tên:";
            this.lblRegName.AutoSize = true;
            this.lblRegName.Location = new System.Drawing.Point(40, 160);
            this.txtRegName.Location = new System.Drawing.Point(190, 155);
            this.txtRegName.Width = 150;
            
            this.lblRegEmail.Text = "Email:";
            this.lblRegEmail.AutoSize = true;
            this.lblRegEmail.Location = new System.Drawing.Point(40, 190);
            this.txtRegEmail.Location = new System.Drawing.Point(190, 185);
            this.txtRegEmail.Width = 150;
            
            this.lblRegPhone.Text = "Số điện thoại:";
            this.lblRegPhone.AutoSize = true;
            this.lblRegPhone.Location = new System.Drawing.Point(40, 220);
            this.txtRegPhone.Location = new System.Drawing.Point(190, 215);
            this.txtRegPhone.Width = 150;
            
            this.progressBarPasswordStrength.Location = new System.Drawing.Point(190, 245);
            this.progressBarPasswordStrength.Width = 150;
            this.progressBarPasswordStrength.Maximum = 100;
            
            this.btnRegister.Text = "Đăng ký";
            this.btnRegister.Location = new System.Drawing.Point(40, 270);
            this.btnRegister.Width = 120;
            this.btnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
            
            this.btnSwitchToLogin.Text = "Đăng nhập";
            this.btnSwitchToLogin.Location = new System.Drawing.Point(210, 270);
            this.btnSwitchToLogin.Width = 120;
            
            // Thêm các control vào tabPageRegister
            this.tabPageRegister.Controls.Add(this.lblRegisterHeader);
            this.tabPageRegister.Controls.Add(this.lblRegUsername);
            this.tabPageRegister.Controls.Add(this.txtRegUsername);
            this.tabPageRegister.Controls.Add(this.lblRegPassword);
            this.tabPageRegister.Controls.Add(this.txtRegPassword);
            this.tabPageRegister.Controls.Add(this.lblRegConfirmPassword);
            this.tabPageRegister.Controls.Add(this.txtRegConfirmPassword);
            this.tabPageRegister.Controls.Add(this.lblRegName);
            this.tabPageRegister.Controls.Add(this.txtRegName);
            this.tabPageRegister.Controls.Add(this.lblRegEmail);
            this.tabPageRegister.Controls.Add(this.txtRegEmail);
            this.tabPageRegister.Controls.Add(this.lblRegPhone);
            this.tabPageRegister.Controls.Add(this.txtRegPhone);
            this.tabPageRegister.Controls.Add(this.progressBarPasswordStrength);
            this.tabPageRegister.Controls.Add(this.btnRegister);
            this.tabPageRegister.Controls.Add(this.btnSwitchToLogin);
            
            // 
            // AuthForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.tabControlAuth);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authentication";
        }
    }
}
