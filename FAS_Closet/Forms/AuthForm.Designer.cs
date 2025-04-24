using System.IO;
using System.Drawing.Drawing2D;

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
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Panel pnlLoginForm;
        private Panel pnlRegisterForm;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) 
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AuthForm));
            tabControlAuth = new TabControl();
            tabPageLogin = new TabPage();
            pnlLoginForm = new Panel();
            pictureBox1 = new PictureBox();
            lblLoginHeader = new Label();
            txtLoginUsername = new TextBox();
            txtLoginPassword = new TextBox();
            chkRememberMe = new CheckBox();
            lnkForgotPassword = new LinkLabel();
            lblLoginError = new Label();
            btnLogin = new Button();
            btnSwitchToRegister = new Button();
            pictureBox2 = new PictureBox();
            tabPageRegister = new TabPage();
            pnlRegisterForm = new Panel();
            pictureBox3 = new PictureBox();
            lblRegisterHeader = new Label();
            txtRegUsername = new TextBox();
            txtRegPassword = new TextBox();
            progressBarPasswordStrength = new ProgressBar();
            lblPasswordStrength = new Label();
            txtRegConfirmPassword = new TextBox();
            txtRegName = new TextBox();
            txtRegEmail = new TextBox();
            txtRegPhone = new TextBox();
            lblRegisterError = new Label();
            lblRegisterSuccess = new Label();
            btnRegister = new Button();
            btnSwitchToLogin = new Button();
            errorProviderLogin = new ErrorProvider(components);
            errorProviderRegister = new ErrorProvider(components);
            tabControlAuth.SuspendLayout();
            tabPageLogin.SuspendLayout();
            pnlLoginForm.SuspendLayout();
            ((ISupportInitialize)pictureBox1).BeginInit();
            ((ISupportInitialize)pictureBox2).BeginInit();
            tabPageRegister.SuspendLayout();
            pnlRegisterForm.SuspendLayout();
            ((ISupportInitialize)pictureBox3).BeginInit();
            ((ISupportInitialize)errorProviderLogin).BeginInit();
            ((ISupportInitialize)errorProviderRegister).BeginInit();
            SuspendLayout();
            // 
            // tabControlAuth
            // 
            tabControlAuth.Controls.Add(tabPageLogin);
            tabControlAuth.Controls.Add(tabPageRegister);
            tabControlAuth.Dock = DockStyle.Fill;
            tabControlAuth.Location = new Point(0, 0);
            tabControlAuth.Name = "tabControlAuth";
            tabControlAuth.SelectedIndex = 0;
            tabControlAuth.Size = new Size(787, 500);
            tabControlAuth.TabIndex = 0;
            tabControlAuth.ItemSize = new Size(0, 1);
            tabControlAuth.SizeMode = TabSizeMode.Fixed;
            tabControlAuth.Appearance = TabAppearance.FlatButtons;
            // 
            // tabPageLogin
            // 
            tabPageLogin.BackColor = Color.White;
            tabPageLogin.Controls.Add(pnlLoginForm);
            tabPageLogin.Controls.Add(pictureBox1);
            tabPageLogin.Location = new Point(4, 5);
            tabPageLogin.Name = "tabPageLogin";
            tabPageLogin.Size = new Size(779, 472);
            tabPageLogin.TabIndex = 0;
            tabPageLogin.Text = "";
            // 
            // pnlLoginForm
            // 
            pnlLoginForm.BackColor = Color.White;
            pnlLoginForm.Controls.Add(lblLoginHeader);
            pnlLoginForm.Controls.Add(txtLoginUsername);
            pnlLoginForm.Controls.Add(txtLoginPassword);
            pnlLoginForm.Controls.Add(chkRememberMe);
            pnlLoginForm.Controls.Add(lnkForgotPassword);
            pnlLoginForm.Controls.Add(lblLoginError);
            pnlLoginForm.Controls.Add(btnLogin);
            pnlLoginForm.Controls.Add(btnSwitchToRegister);
            pnlLoginForm.Dock = DockStyle.Right;
            pnlLoginForm.Location = new Point(399, 0);
            pnlLoginForm.Name = "pnlLoginForm";
            pnlLoginForm.Size = new Size(380, 472);
            pnlLoginForm.TabIndex = 12;
            pnlLoginForm.Padding = new Padding(30);
            // 
            // pictureBox1
            //
            try
            {
                // Try loading image from assets folder relative to executable first
                string exeImagePath = Path.Combine(Application.StartupPath, "Assets", "Images", "loginbg.jpg");
                if (File.Exists(exeImagePath))
                {
                    pictureBox1.Image = Image.FromFile(exeImagePath);
                }
                else
                {
                    // Fallback to development path
                    string devImagePath = Path.Combine(Application.StartupPath, @"..\..\..\..\Assets\Images\loginbg.jpg");
                    if (File.Exists(devImagePath))
                    {
                        pictureBox1.Image = Image.FromFile(devImagePath);
                    }
                    else
                    {
                        // If both paths fail, set a placeholder color
                        pictureBox1.BackColor = Color.LightBlue;
                    }
                }
            }
            catch (Exception)
            {
                // Handle loading errors gracefully
                pictureBox1.BackColor = Color.LightBlue;
            }
            
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(399, 472);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // lblLoginHeader
            // 
            lblLoginHeader.AutoSize = true;
            lblLoginHeader.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblLoginHeader.ForeColor = Color.FromArgb(0, 123, 255);
            lblLoginHeader.Location = new Point(33, 50);
            lblLoginHeader.Name = "lblLoginHeader";
            lblLoginHeader.Size = new Size(161, 32);
            lblLoginHeader.TabIndex = 0;
            lblLoginHeader.Text = "ĐĂNG NHẬP";
            // 
            // txtLoginUsername
            // 
            txtLoginUsername.BorderStyle = BorderStyle.FixedSingle;
            txtLoginUsername.Font = new Font("Segoe UI", 11F);
            txtLoginUsername.Location = new Point(33, 120);
            txtLoginUsername.Name = "txtLoginUsername";
            txtLoginUsername.Size = new Size(314, 30);
            txtLoginUsername.TabIndex = 2;
            txtLoginUsername.PlaceholderText = "Tên đăng nhập";
            txtLoginUsername.Padding = new Padding(8);
            // 
            // txtLoginPassword
            // 
            txtLoginPassword.BorderStyle = BorderStyle.FixedSingle;
            txtLoginPassword.Font = new Font("Segoe UI", 11F);
            txtLoginPassword.Location = new Point(33, 170);
            txtLoginPassword.Name = "txtLoginPassword";
            txtLoginPassword.PasswordChar = '●';
            txtLoginPassword.Size = new Size(314, 30);
            txtLoginPassword.TabIndex = 4;
            txtLoginPassword.PlaceholderText = "Mật khẩu";
            txtLoginPassword.Padding = new Padding(8);
            // 
            // chkRememberMe
            // 
            chkRememberMe.AutoSize = true;
            chkRememberMe.Font = new Font("Segoe UI", 10F);
            chkRememberMe.Location = new Point(33, 220);
            chkRememberMe.Name = "chkRememberMe";
            chkRememberMe.Size = new Size(80, 19);
            chkRememberMe.TabIndex = 5;
            chkRememberMe.Text = "Nhớ tôi";
            chkRememberMe.UseVisualStyleBackColor = false;
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.AutoSize = true;
            lnkForgotPassword.Font = new Font("Segoe UI", 10F);
            lnkForgotPassword.LinkColor = Color.FromArgb(0, 123, 255);
            lnkForgotPassword.Location = new Point(235, 220);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(112, 19);
            lnkForgotPassword.TabIndex = 6;
            lnkForgotPassword.TabStop = true;
            lnkForgotPassword.Text = "Quên mật khẩu?";
            // 
            // lblLoginError
            // 
            lblLoginError.Font = new Font("Segoe UI", 9.5F);
            lblLoginError.ForeColor = Color.Red;
            lblLoginError.Location = new Point(33, 250);
            lblLoginError.Name = "lblLoginError";
            lblLoginError.Size = new Size(314, 40);
            lblLoginError.TabIndex = 7;
            lblLoginError.TextAlign = ContentAlignment.MiddleLeft;
            lblLoginError.Visible = false;
            // 
            // btnLogin
            // 
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Location = new Point(33, 300);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(314, 45);
            btnLogin.TabIndex = 8;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnSwitchToRegister
            // 
            btnSwitchToRegister.FlatStyle = FlatStyle.Flat;
            btnSwitchToRegister.BackColor = Color.FromArgb(240, 240, 240);
            btnSwitchToRegister.ForeColor = Color.FromArgb(60, 60, 60);
            btnSwitchToRegister.Font = new Font("Segoe UI", 10F);
            btnSwitchToRegister.FlatAppearance.BorderSize = 1;
            btnSwitchToRegister.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnSwitchToRegister.Cursor = Cursors.Hand;
            btnSwitchToRegister.Location = new Point(33, 360);
            btnSwitchToRegister.Name = "btnSwitchToRegister";
            btnSwitchToRegister.Size = new Size(314, 45);
            btnSwitchToRegister.TabIndex = 9;
            btnSwitchToRegister.Text = "Chưa có tài khoản? Đăng ký";
            btnSwitchToRegister.UseVisualStyleBackColor = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.Visible = false;
            // 
            // tabPageRegister
            // 
            tabPageRegister.AutoScroll = true;
            tabPageRegister.BackColor = Color.White;
            tabPageRegister.Controls.Add(pnlRegisterForm);
            tabPageRegister.Controls.Add(pictureBox3);
            tabPageRegister.Location = new Point(4, 5);
            tabPageRegister.Name = "tabPageRegister";
            tabPageRegister.Size = new Size(779, 472);
            tabPageRegister.TabIndex = 1;
            tabPageRegister.Text = "";
            // 
            // pnlRegisterForm
            // 
            pnlRegisterForm.AutoScroll = true;
            pnlRegisterForm.BackColor = Color.White;
            pnlRegisterForm.Controls.Add(lblRegisterHeader);
            pnlRegisterForm.Controls.Add(txtRegUsername);
            pnlRegisterForm.Controls.Add(txtRegPassword);
            pnlRegisterForm.Controls.Add(progressBarPasswordStrength);
            pnlRegisterForm.Controls.Add(lblPasswordStrength);
            pnlRegisterForm.Controls.Add(txtRegConfirmPassword);
            pnlRegisterForm.Controls.Add(txtRegName);
            pnlRegisterForm.Controls.Add(txtRegEmail);
            pnlRegisterForm.Controls.Add(txtRegPhone);
            pnlRegisterForm.Controls.Add(lblRegisterError);
            pnlRegisterForm.Controls.Add(lblRegisterSuccess);
            pnlRegisterForm.Controls.Add(btnRegister);
            pnlRegisterForm.Controls.Add(btnSwitchToLogin);
            pnlRegisterForm.Dock = DockStyle.Right;
            pnlRegisterForm.Location = new Point(399, 0);
            pnlRegisterForm.Name = "pnlRegisterForm";
            pnlRegisterForm.Size = new Size(380, 472);
            pnlRegisterForm.TabIndex = 20;
            pnlRegisterForm.Padding = new Padding(30);
            pnlRegisterForm.AutoScroll = true;
            // 
            // pictureBox3
            //
            try
            {
                // Try loading image from assets folder relative to executable first
                string exeImagePath = Path.Combine(Application.StartupPath, "Assets", "Images", "signupbg.jpg");
                if (File.Exists(exeImagePath))
                {
                    pictureBox3.Image = Image.FromFile(exeImagePath);
                }
                else
                {
                    // Fallback to development path
                    string devImagePath = Path.Combine(Application.StartupPath, @"..\..\..\..\Assets\Images\signupbg.jpg");
                    if (File.Exists(devImagePath))
                    {
                        pictureBox3.Image = Image.FromFile(devImagePath);
                    }
                    else
                    {
                        // If both paths fail, set a placeholder color
                        pictureBox3.BackColor = Color.LightBlue;
                    }
                }
            }
            catch (Exception)
            {
                // Handle loading errors gracefully
                pictureBox3.BackColor = Color.LightBlue;
            }
            
            pictureBox3.Dock = DockStyle.Left;
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(399, 472);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 19;
            pictureBox3.TabStop = false;
            // 
            // lblRegisterHeader
            // 
            lblRegisterHeader.AutoSize = true;
            lblRegisterHeader.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblRegisterHeader.ForeColor = Color.FromArgb(0, 123, 255);
            lblRegisterHeader.Location = new Point(33, 30);
            lblRegisterHeader.Name = "lblRegisterHeader";
            lblRegisterHeader.Size = new Size(263, 32);
            lblRegisterHeader.TabIndex = 0;
            lblRegisterHeader.Text = "ĐĂNG KÝ TÀI KHOẢN";
            // 
            // txtRegUsername
            // 
            txtRegUsername.BorderStyle = BorderStyle.FixedSingle;
            txtRegUsername.Font = new Font("Segoe UI", 11F);
            txtRegUsername.Location = new Point(33, 80);
            txtRegUsername.Name = "txtRegUsername";
            txtRegUsername.Size = new Size(314, 30);
            txtRegUsername.TabIndex = 2;
            txtRegUsername.PlaceholderText = "Tên đăng nhập";
            txtRegUsername.Padding = new Padding(8);
            // 
            // txtRegPassword
            // 
            txtRegPassword.BorderStyle = BorderStyle.FixedSingle;
            txtRegPassword.Font = new Font("Segoe UI", 11F);
            txtRegPassword.Location = new Point(33, 130);
            txtRegPassword.Name = "txtRegPassword";
            txtRegPassword.PasswordChar = '●';
            txtRegPassword.Size = new Size(314, 30);
            txtRegPassword.TabIndex = 4;
            txtRegPassword.PlaceholderText = "Mật khẩu";
            txtRegPassword.Padding = new Padding(8);
            // 
            // progressBarPasswordStrength
            // 
            progressBarPasswordStrength.Location = new Point(33, 170);
            progressBarPasswordStrength.Name = "progressBarPasswordStrength";
            progressBarPasswordStrength.Size = new Size(314, 10);
            progressBarPasswordStrength.TabIndex = 5;
            // 
            // lblPasswordStrength
            // 
            lblPasswordStrength.AutoSize = true;
            lblPasswordStrength.Font = new Font("Segoe UI", 9F);
            lblPasswordStrength.Location = new Point(33, 185);
            lblPasswordStrength.Name = "lblPasswordStrength";
            lblPasswordStrength.Size = new Size(168, 13);
            lblPasswordStrength.TabIndex = 6;
            lblPasswordStrength.Text = "Độ mạnh mật khẩu: Chưa nhập";
            lblPasswordStrength.ForeColor = Color.Gray;
            // 
            // txtRegConfirmPassword
            // 
            txtRegConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtRegConfirmPassword.Font = new Font("Segoe UI", 11F);
            txtRegConfirmPassword.Location = new Point(33, 215);
            txtRegConfirmPassword.Name = "txtRegConfirmPassword";
            txtRegConfirmPassword.PasswordChar = '●';
            txtRegConfirmPassword.Size = new Size(314, 30);
            txtRegConfirmPassword.TabIndex = 8;
            txtRegConfirmPassword.PlaceholderText = "Xác nhận mật khẩu";
            txtRegConfirmPassword.Padding = new Padding(8);
            // 
            // txtRegName
            // 
            txtRegName.BorderStyle = BorderStyle.FixedSingle;
            txtRegName.Font = new Font("Segoe UI", 11F);
            txtRegName.Location = new Point(33, 265);
            txtRegName.Name = "txtRegName";
            txtRegName.Size = new Size(314, 30);
            txtRegName.TabIndex = 10;
            txtRegName.PlaceholderText = "Họ và tên";
            txtRegName.Padding = new Padding(8);
            // 
            // txtRegEmail
            // 
            txtRegEmail.BorderStyle = BorderStyle.FixedSingle;
            txtRegEmail.Font = new Font("Segoe UI", 11F);
            txtRegEmail.Location = new Point(33, 315);
            txtRegEmail.Name = "txtRegEmail";
            txtRegEmail.Size = new Size(314, 30);
            txtRegEmail.TabIndex = 12;
            txtRegEmail.PlaceholderText = "Email";
            txtRegEmail.Padding = new Padding(8);
            // 
            // txtRegPhone
            // 
            txtRegPhone.BorderStyle = BorderStyle.FixedSingle;
            txtRegPhone.Font = new Font("Segoe UI", 11F);
            txtRegPhone.Location = new Point(33, 365);
            txtRegPhone.Name = "txtRegPhone";
            txtRegPhone.Size = new Size(314, 30);
            txtRegPhone.TabIndex = 14;
            txtRegPhone.PlaceholderText = "Số điện thoại";
            txtRegPhone.Padding = new Padding(8);
            // 
            // lblRegisterError
            // 
            lblRegisterError.Font = new Font("Segoe UI", 9.5F);
            lblRegisterError.ForeColor = Color.Red;
            lblRegisterError.Location = new Point(33, 405);
            lblRegisterError.Name = "lblRegisterError";
            lblRegisterError.Size = new Size(314, 35);
            lblRegisterError.TabIndex = 15;
            lblRegisterError.TextAlign = ContentAlignment.MiddleLeft;
            lblRegisterError.Visible = false;
            // 
            // lblRegisterSuccess
            // 
            lblRegisterSuccess.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblRegisterSuccess.ForeColor = Color.Green;
            lblRegisterSuccess.Location = new Point(33, 405);
            lblRegisterSuccess.Name = "lblRegisterSuccess";
            lblRegisterSuccess.Size = new Size(314, 35);
            lblRegisterSuccess.TabIndex = 16;
            lblRegisterSuccess.TextAlign = ContentAlignment.MiddleLeft;
            lblRegisterSuccess.Visible = false;
            // 
            // btnRegister
            // 
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.BackColor = Color.FromArgb(40, 167, 69);
            btnRegister.ForeColor = Color.White;
            btnRegister.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Location = new Point(33, 450);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(314, 45);
            btnRegister.TabIndex = 17;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += BtnRegister_Click;
            // 
            // btnSwitchToLogin
            // 
            btnSwitchToLogin.FlatStyle = FlatStyle.Flat;
            btnSwitchToLogin.BackColor = Color.FromArgb(240, 240, 240);
            btnSwitchToLogin.ForeColor = Color.FromArgb(60, 60, 60);
            btnSwitchToLogin.Font = new Font("Segoe UI", 10F);
            btnSwitchToLogin.FlatAppearance.BorderSize = 1;
            btnSwitchToLogin.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnSwitchToLogin.Cursor = Cursors.Hand;
            btnSwitchToLogin.Location = new Point(33, 510);
            btnSwitchToLogin.Name = "btnSwitchToLogin";
            btnSwitchToLogin.Size = new Size(314, 45);
            btnSwitchToLogin.TabIndex = 18;
            btnSwitchToLogin.Text = "Đã có tài khoản? Đăng nhập";
            btnSwitchToLogin.UseVisualStyleBackColor = false;
            // 
            // errorProviderLogin
            // 
            errorProviderLogin.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProviderLogin.ContainerControl = this;
            // 
            // errorProviderRegister
            // 
            errorProviderRegister.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProviderRegister.ContainerControl = this;

            // 
            // AuthForm
            // 
            ClientSize = new Size(787, 500);
            Controls.Add(tabControlAuth);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "AuthForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FAS Closet - Đăng Nhập";
            tabControlAuth.ResumeLayout(false);
            tabPageLogin.ResumeLayout(false);
            pnlLoginForm.ResumeLayout(false);
            pnlLoginForm.PerformLayout();
            ((ISupportInitialize)pictureBox1).EndInit();
            ((ISupportInitialize)pictureBox2).EndInit();
            tabPageRegister.ResumeLayout(false);
            pnlRegisterForm.ResumeLayout(false);
            pnlRegisterForm.PerformLayout();
            ((ISupportInitialize)pictureBox3).EndInit();
            ((ISupportInitialize)errorProviderLogin).EndInit();
            ((ISupportInitialize)errorProviderRegister).EndInit();
            ResumeLayout(false);
        }
    }
}
