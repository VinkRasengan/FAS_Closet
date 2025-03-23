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
        private System.Windows.Forms.Label lblLoginError;

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
        private System.Windows.Forms.Label lblPasswordStrength;
        private System.Windows.Forms.Label lblRegisterError;
        private System.Windows.Forms.Label lblRegisterSuccess;

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
            this.lblLoginError = new System.Windows.Forms.Label();

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
            this.lblPasswordStrength = new System.Windows.Forms.Label();
            this.lblRegisterError = new System.Windows.Forms.Label();
            this.lblRegisterSuccess = new System.Windows.Forms.Label();

            // 
            // tabControlAuth
            // 
            this.tabControlAuth.Controls.Add(this.tabPageLogin);
            this.tabControlAuth.Controls.Add(this.tabPageRegister);
            this.tabControlAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAuth.Location = new System.Drawing.Point(0, 0);
            this.tabControlAuth.Name = "tabControlAuth";
            this.tabControlAuth.SelectedIndex = 0;
            this.tabControlAuth.Size = new System.Drawing.Size(450, 500);
            this.tabControlAuth.TabIndex = 0;
            
            // 
            // tabPageLogin
            // 
            this.tabPageLogin.Text = "Đăng Nhập";
            this.tabPageLogin.Padding = new System.Windows.Forms.Padding(15);
            this.tabPageLogin.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            
            // Cài đặt các control cho tab đăng nhập:
            this.lblLoginHeader.Text = "Đăng Nhập";
            this.lblLoginHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblLoginHeader.ForeColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.lblLoginHeader.AutoSize = true;
            this.lblLoginHeader.Location = new System.Drawing.Point(150, 30);
            
            this.lblLoginUsername.Text = "Tên đăng nhập:";
            this.lblLoginUsername.AutoSize = true;
            this.lblLoginUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLoginUsername.Location = new System.Drawing.Point(50, 90);
            
            this.txtLoginUsername.Location = new System.Drawing.Point(50, 115);
            this.txtLoginUsername.Width = 320;
            this.txtLoginUsername.Height = 30;
            this.txtLoginUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLoginUsername.BorderStyle = BorderStyle.FixedSingle;
            
            this.lblLoginPassword.Text = "Mật khẩu:";
            this.lblLoginPassword.AutoSize = true;
            this.lblLoginPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLoginPassword.Location = new System.Drawing.Point(50, 160);
            
            this.txtLoginPassword.Location = new System.Drawing.Point(50, 185);
            this.txtLoginPassword.Width = 320;
            this.txtLoginPassword.Height = 30;
            this.txtLoginPassword.PasswordChar = '●';
            this.txtLoginPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLoginPassword.BorderStyle = BorderStyle.FixedSingle;
            
            this.chkRememberMe.Text = "Nhớ tôi";
            this.chkRememberMe.Location = new System.Drawing.Point(50, 225);
            this.chkRememberMe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkRememberMe.AutoSize = true;
            
            this.lnkForgotPassword.Text = "Quên mật khẩu?";
            this.lnkForgotPassword.Location = new System.Drawing.Point(270, 225);
            this.lnkForgotPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkForgotPassword.AutoSize = true;
            
            // Error message label
            this.lblLoginError.AutoSize = false;
            this.lblLoginError.TextAlign = ContentAlignment.MiddleCenter;
            this.lblLoginError.Location = new System.Drawing.Point(50, 260);
            this.lblLoginError.Size = new System.Drawing.Size(320, 40);
            this.lblLoginError.ForeColor = System.Drawing.Color.Red;
            this.lblLoginError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLoginError.Visible = false;
            
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Location = new System.Drawing.Point(50, 310);
            this.btnLogin.Size = new System.Drawing.Size(150, 40);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Cursor = Cursors.Hand;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            
            this.btnSwitchToRegister.Text = "Đăng ký";
            this.btnSwitchToRegister.Location = new System.Drawing.Point(220, 310);
            this.btnSwitchToRegister.Size = new System.Drawing.Size(150, 40);
            this.btnSwitchToRegister.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnSwitchToRegister.ForeColor = System.Drawing.Color.White;
            this.btnSwitchToRegister.FlatStyle = FlatStyle.Flat;
            this.btnSwitchToRegister.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSwitchToRegister.Cursor = Cursors.Hand;
            
            // Thêm các control vào tabPageLogin
            this.tabPageLogin.Controls.Add(this.lblLoginHeader);
            this.tabPageLogin.Controls.Add(this.lblLoginUsername);
            this.tabPageLogin.Controls.Add(this.txtLoginUsername);
            this.tabPageLogin.Controls.Add(this.lblLoginPassword);
            this.tabPageLogin.Controls.Add(this.txtLoginPassword);
            this.tabPageLogin.Controls.Add(this.chkRememberMe);
            this.tabPageLogin.Controls.Add(this.lnkForgotPassword);
            this.tabPageLogin.Controls.Add(this.lblLoginError);
            this.tabPageLogin.Controls.Add(this.btnLogin);
            this.tabPageLogin.Controls.Add(this.btnSwitchToRegister);
            
            // 
            // tabPageRegister
            // 
            this.tabPageRegister.Text = "Đăng Ký";
            this.tabPageRegister.Padding = new System.Windows.Forms.Padding(15);
            this.tabPageRegister.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.tabPageRegister.AutoScroll = true;
            
            // Cài đặt các control cho tab đăng ký:
            this.lblRegisterHeader.Text = "Đăng Ký Tài Khoản";
            this.lblRegisterHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblRegisterHeader.ForeColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.lblRegisterHeader.AutoSize = true;
            this.lblRegisterHeader.Location = new System.Drawing.Point(120, 20);
            
            // Username
            this.lblRegUsername.Text = "Tên đăng nhập:";
            this.lblRegUsername.AutoSize = true;
            this.lblRegUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRegUsername.Location = new System.Drawing.Point(50, 70);
            
            this.txtRegUsername.Location = new System.Drawing.Point(50, 95);
            this.txtRegUsername.Width = 320;
            this.txtRegUsername.Height = 30;
            this.txtRegUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRegUsername.BorderStyle = BorderStyle.FixedSingle;
            
            // Password
            this.lblRegPassword.Text = "Mật khẩu:";
            this.lblRegPassword.AutoSize = true;
            this.lblRegPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRegPassword.Location = new System.Drawing.Point(50, 135);
            
            this.txtRegPassword.Location = new System.Drawing.Point(50, 160);
            this.txtRegPassword.Width = 320;
            this.txtRegPassword.Height = 30;
            this.txtRegPassword.PasswordChar = '●';
            this.txtRegPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRegPassword.BorderStyle = BorderStyle.FixedSingle;
            
            // Password strength
            this.progressBarPasswordStrength.Location = new System.Drawing.Point(50, 195);
            this.progressBarPasswordStrength.Width = 320;
            this.progressBarPasswordStrength.Height = 10;
            this.progressBarPasswordStrength.Maximum = 100;
            
            this.lblPasswordStrength.AutoSize = true;
            this.lblPasswordStrength.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPasswordStrength.Location = new System.Drawing.Point(50, 210);
            this.lblPasswordStrength.Text = "Độ mạnh mật khẩu: Chưa nhập";
            
            // Confirm Password
            this.lblRegConfirmPassword.Text = "Xác nhận mật khẩu:";
            this.lblRegConfirmPassword.AutoSize = true;
            this.lblRegConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRegConfirmPassword.Location = new System.Drawing.Point(50, 230);
            
            this.txtRegConfirmPassword.Location = new System.Drawing.Point(50, 255);
            this.txtRegConfirmPassword.Width = 320;
            this.txtRegConfirmPassword.Height = 30;
            this.txtRegConfirmPassword.PasswordChar = '●';
            this.txtRegConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRegConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            
            // Full Name
            this.lblRegName.Text = "Họ và tên:";
            this.lblRegName.AutoSize = true;
            this.lblRegName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRegName.Location = new System.Drawing.Point(50, 295);
            
            this.txtRegName.Location = new System.Drawing.Point(50, 320);
            this.txtRegName.Width = 320;
            this.txtRegName.Height = 30;
            this.txtRegName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRegName.BorderStyle = BorderStyle.FixedSingle;
            
            // Email
            this.lblRegEmail.Text = "Email:";
            this.lblRegEmail.AutoSize = true;
            this.lblRegEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRegEmail.Location = new System.Drawing.Point(50, 360);
            
            this.txtRegEmail.Location = new System.Drawing.Point(50, 385);
            this.txtRegEmail.Width = 320;
            this.txtRegEmail.Height = 30;
            this.txtRegEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRegEmail.BorderStyle = BorderStyle.FixedSingle;
            
            // Phone
            this.lblRegPhone.Text = "Số điện thoại:";
            this.lblRegPhone.AutoSize = true;
            this.lblRegPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRegPhone.Location = new System.Drawing.Point(50, 425);
            
            this.txtRegPhone.Location = new System.Drawing.Point(50, 450);
            this.txtRegPhone.Width = 320;
            this.txtRegPhone.Height = 30;
            this.txtRegPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRegPhone.BorderStyle = BorderStyle.FixedSingle;
            
            // Error and Success messages
            this.lblRegisterError.AutoSize = false;
            this.lblRegisterError.TextAlign = ContentAlignment.MiddleCenter;
            this.lblRegisterError.Location = new System.Drawing.Point(50, 490);
            this.lblRegisterError.Size = new System.Drawing.Size(320, 40);
            this.lblRegisterError.ForeColor = System.Drawing.Color.Red;
            this.lblRegisterError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRegisterError.Visible = false;
            
            this.lblRegisterSuccess.AutoSize = false;
            this.lblRegisterSuccess.TextAlign = ContentAlignment.MiddleCenter;
            this.lblRegisterSuccess.Location = new System.Drawing.Point(50, 490);
            this.lblRegisterSuccess.Size = new System.Drawing.Size(320, 40);
            this.lblRegisterSuccess.ForeColor = System.Drawing.Color.Green;
            this.lblRegisterSuccess.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRegisterSuccess.Visible = false;
            
            // Buttons
            this.btnRegister.Text = "Đăng ký";
            this.btnRegister.Location = new System.Drawing.Point(50, 540);
            this.btnRegister.Size = new System.Drawing.Size(150, 40);
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.FlatStyle = FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegister.Cursor = Cursors.Hand;
            this.btnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
            
            this.btnSwitchToLogin.Text = "Đã có tài khoản";
            this.btnSwitchToLogin.Location = new System.Drawing.Point(220, 540);
            this.btnSwitchToLogin.Size = new System.Drawing.Size(150, 40);
            this.btnSwitchToLogin.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnSwitchToLogin.ForeColor = System.Drawing.Color.White;
            this.btnSwitchToLogin.FlatStyle = FlatStyle.Flat;
            this.btnSwitchToLogin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSwitchToLogin.Cursor = Cursors.Hand;
            
            // Thêm các control vào tabPageRegister
            this.tabPageRegister.Controls.Add(this.lblRegisterHeader);
            
            this.tabPageRegister.Controls.Add(this.lblRegUsername);
            this.tabPageRegister.Controls.Add(this.txtRegUsername);
            
            this.tabPageRegister.Controls.Add(this.lblRegPassword);
            this.tabPageRegister.Controls.Add(this.txtRegPassword);
            this.tabPageRegister.Controls.Add(this.progressBarPasswordStrength);
            this.tabPageRegister.Controls.Add(this.lblPasswordStrength);
            
            this.tabPageRegister.Controls.Add(this.lblRegConfirmPassword);
            this.tabPageRegister.Controls.Add(this.txtRegConfirmPassword);
            
            this.tabPageRegister.Controls.Add(this.lblRegName);
            this.tabPageRegister.Controls.Add(this.txtRegName);
            
            this.tabPageRegister.Controls.Add(this.lblRegEmail);
            this.tabPageRegister.Controls.Add(this.txtRegEmail);
            
            this.tabPageRegister.Controls.Add(this.lblRegPhone);
            this.tabPageRegister.Controls.Add(this.txtRegPhone);
            
            this.tabPageRegister.Controls.Add(this.lblRegisterError);
            this.tabPageRegister.Controls.Add(this.lblRegisterSuccess);
            
            this.tabPageRegister.Controls.Add(this.btnRegister);
            this.tabPageRegister.Controls.Add(this.btnSwitchToLogin);
            
            // 
            // AuthForm
            // 
            this.ClientSize = new System.Drawing.Size(450, 500);
            this.Controls.Add(this.tabControlAuth);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FAS Closet - Đăng Nhập";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            
            // Error providers
            this.errorProviderLogin.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.errorProviderLogin.ContainerControl = this;
            
            this.errorProviderRegister.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.errorProviderRegister.ContainerControl = this;
        }
    }
}
