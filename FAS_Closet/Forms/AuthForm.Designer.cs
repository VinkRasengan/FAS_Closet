using System.IO;

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
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;

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
            pictureBox1 = new PictureBox();
            lblLoginHeader = new Label();
            lblLoginUsername = new Label();
            txtLoginUsername = new TextBox();
            lblLoginPassword = new Label();
            txtLoginPassword = new TextBox();
            chkRememberMe = new CheckBox();
            lnkForgotPassword = new LinkLabel();
            lblLoginError = new Label();
            btnLogin = new Button();
            btnSwitchToRegister = new Button();
            pictureBox2 = new PictureBox();
            tabPageRegister = new TabPage();
            pictureBox3 = new PictureBox();
            lblRegisterHeader = new Label();
            lblRegUsername = new Label();
            txtRegUsername = new TextBox();
            lblRegPassword = new Label();
            txtRegPassword = new TextBox();
            progressBarPasswordStrength = new ProgressBar();
            lblPasswordStrength = new Label();
            lblRegConfirmPassword = new Label();
            txtRegConfirmPassword = new TextBox();
            lblRegName = new Label();
            txtRegName = new TextBox();
            lblRegEmail = new Label();
            txtRegEmail = new TextBox();
            lblRegPhone = new Label();
            txtRegPhone = new TextBox();
            lblRegisterError = new Label();
            lblRegisterSuccess = new Label();
            btnRegister = new Button();
            btnSwitchToLogin = new Button();
            errorProviderLogin = new ErrorProvider(components);
            errorProviderRegister = new ErrorProvider(components);
            tabControlAuth.SuspendLayout();
            tabPageLogin.SuspendLayout();
            ((ISupportInitialize)pictureBox1).BeginInit();
            ((ISupportInitialize)pictureBox2).BeginInit();
            tabPageRegister.SuspendLayout();
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
            // 
            // tabPageLogin
            // 
            tabPageLogin.BackColor = Color.FromArgb(0, 0, 80);
            tabPageLogin.BackgroundImageLayout = ImageLayout.Center;
            tabPageLogin.Controls.Add(pictureBox1);
            tabPageLogin.Controls.Add(lblLoginHeader);
            tabPageLogin.Controls.Add(lblLoginUsername);
            tabPageLogin.Controls.Add(txtLoginUsername);
            tabPageLogin.Controls.Add(lblLoginPassword);
            tabPageLogin.Controls.Add(txtLoginPassword);
            tabPageLogin.Controls.Add(chkRememberMe);
            tabPageLogin.Controls.Add(lnkForgotPassword);
            tabPageLogin.Controls.Add(lblLoginError);
            tabPageLogin.Controls.Add(btnLogin);
            tabPageLogin.Controls.Add(btnSwitchToRegister);
            tabPageLogin.Controls.Add(pictureBox2);
            tabPageLogin.ForeColor = SystemColors.ControlText;
            tabPageLogin.Location = new Point(4, 24);
            tabPageLogin.Name = "tabPageLogin";
            tabPageLogin.Padding = new Padding(15);
            tabPageLogin.Size = new Size(779, 472);
            tabPageLogin.TabIndex = 0;
            tabPageLogin.Text = "Đăng Nhập";
            // 
            // pictureBox1
            //
            string imagePath = Path.Combine(Application.StartupPath, @"..\..\..\..\Assets\Images\loginbg.jpg");
            pictureBox1.BackgroundImage = Image.FromFile(imagePath);
            pictureBox1.Location = new Point(-4, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(407, 472);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // lblLoginHeader
            // 
            lblLoginHeader.AutoSize = true;
            lblLoginHeader.BackColor = Color.White;
            lblLoginHeader.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblLoginHeader.ForeColor = Color.FromArgb(0, 0, 192);
            lblLoginHeader.Location = new Point(441, 80);
            lblLoginHeader.Name = "lblLoginHeader";
            lblLoginHeader.Size = new Size(161, 32);
            lblLoginHeader.TabIndex = 0;
            lblLoginHeader.Text = "ĐĂNG NHẬP";
            // 
            // lblLoginUsername
            // 
            lblLoginUsername.AutoSize = true;
            lblLoginUsername.BackColor = Color.White;
            lblLoginUsername.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLoginUsername.Location = new Point(449, 130);
            lblLoginUsername.Name = "lblLoginUsername";
            lblLoginUsername.Size = new Size(102, 17);
            lblLoginUsername.TabIndex = 1;
            lblLoginUsername.Text = "Tên đăng nhập:";
            // 
            // txtLoginUsername
            // 
            txtLoginUsername.BorderStyle = BorderStyle.FixedSingle;
            txtLoginUsername.Font = new Font("Segoe UI", 10F);
            txtLoginUsername.Location = new Point(449, 155);
            txtLoginUsername.Name = "txtLoginUsername";
            txtLoginUsername.Size = new Size(285, 25);
            txtLoginUsername.TabIndex = 2;
            // 
            // lblLoginPassword
            // 
            lblLoginPassword.AutoSize = true;
            lblLoginPassword.BackColor = Color.White;
            lblLoginPassword.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLoginPassword.Location = new Point(449, 192);
            lblLoginPassword.Name = "lblLoginPassword";
            lblLoginPassword.Size = new Size(69, 17);
            lblLoginPassword.TabIndex = 3;
            lblLoginPassword.Text = "Mật khẩu:";
            // 
            // txtLoginPassword
            // 
            txtLoginPassword.BorderStyle = BorderStyle.FixedSingle;
            txtLoginPassword.Font = new Font("Segoe UI", 10F);
            txtLoginPassword.Location = new Point(449, 217);
            txtLoginPassword.Name = "txtLoginPassword";
            txtLoginPassword.PasswordChar = '●';
            txtLoginPassword.Size = new Size(285, 25);
            txtLoginPassword.TabIndex = 4;
            // 
            // chkRememberMe
            // 
            chkRememberMe.AutoSize = true;
            chkRememberMe.BackColor = Color.White;
            chkRememberMe.Font = new Font("Segoe UI", 9F);
            chkRememberMe.Location = new Point(449, 253);
            chkRememberMe.Name = "chkRememberMe";
            chkRememberMe.Size = new Size(66, 19);
            chkRememberMe.TabIndex = 5;
            chkRememberMe.Text = "Nhớ tôi";
            chkRememberMe.UseVisualStyleBackColor = false;
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.AutoSize = true;
            lnkForgotPassword.BackColor = Color.White;
            lnkForgotPassword.Font = new Font("Segoe UI", 9F);
            lnkForgotPassword.LinkColor = Color.FromArgb(0, 0, 192);
            lnkForgotPassword.Location = new Point(640, 257);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(94, 15);
            lnkForgotPassword.TabIndex = 6;
            lnkForgotPassword.TabStop = true;
            lnkForgotPassword.Text = "Quên mật khẩu?";
            // 
            // lblLoginError
            // 
            lblLoginError.BackColor = Color.White;
            lblLoginError.Font = new Font("Segoe UI", 9F);
            lblLoginError.ForeColor = Color.Red;
            lblLoginError.Location = new Point(449, 275);
            lblLoginError.Name = "lblLoginError";
            lblLoginError.Size = new Size(285, 40);
            lblLoginError.TabIndex = 7;
            lblLoginError.TextAlign = ContentAlignment.MiddleCenter;
            lblLoginError.Visible = false;
            // 
            // btnLogin
            // 
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Location = new Point(449, 326);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(127, 40);
            btnLogin.TabIndex = 8;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnSwitchToRegister
            // 
            btnSwitchToRegister.FlatStyle = FlatStyle.Flat;
            btnSwitchToRegister.BackColor = Color.FromArgb(108, 117, 125);
            btnSwitchToRegister.ForeColor = Color.White;
            btnSwitchToRegister.Font = new Font("Segoe UI", 10F);
            btnSwitchToRegister.FlatAppearance.BorderSize = 0;
            btnSwitchToRegister.Cursor = Cursors.Hand;
            btnSwitchToRegister.Location = new Point(607, 326);
            btnSwitchToRegister.Name = "btnSwitchToRegister";
            btnSwitchToRegister.Size = new Size(127, 40);
            btnSwitchToRegister.TabIndex = 9;
            btnSwitchToRegister.Text = "Đăng ký";
            btnSwitchToRegister.UseVisualStyleBackColor = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.Location = new Point(399, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(384, 476);
            pictureBox2.TabIndex = 11;
            pictureBox2.TabStop = false;
            // 
            // tabPageRegister
            // 
            tabPageRegister.AutoScroll = true;
            tabPageRegister.BackColor = Color.FromArgb(248, 249, 250);
            tabPageRegister.Controls.Add(pictureBox3);
            tabPageRegister.Controls.Add(lblRegisterHeader);
            tabPageRegister.Controls.Add(lblRegUsername);
            tabPageRegister.Controls.Add(txtRegUsername);
            tabPageRegister.Controls.Add(lblRegPassword);
            tabPageRegister.Controls.Add(txtRegPassword);
            tabPageRegister.Controls.Add(progressBarPasswordStrength);
            tabPageRegister.Controls.Add(lblPasswordStrength);
            tabPageRegister.Controls.Add(lblRegConfirmPassword);
            tabPageRegister.Controls.Add(txtRegConfirmPassword);
            tabPageRegister.Controls.Add(lblRegName);
            tabPageRegister.Controls.Add(txtRegName);
            tabPageRegister.Controls.Add(lblRegEmail);
            tabPageRegister.Controls.Add(txtRegEmail);
            tabPageRegister.Controls.Add(lblRegPhone);
            tabPageRegister.Controls.Add(txtRegPhone);
            tabPageRegister.Controls.Add(lblRegisterError);
            tabPageRegister.Controls.Add(lblRegisterSuccess);
            tabPageRegister.Controls.Add(btnRegister);
            tabPageRegister.Controls.Add(btnSwitchToLogin);
            tabPageRegister.Location = new Point(4, 24);
            tabPageRegister.Name = "tabPageRegister";
            tabPageRegister.Padding = new Padding(15);
            tabPageRegister.Size = new Size(779, 472);
            tabPageRegister.TabIndex = 1;
            tabPageRegister.Text = "Đăng Ký";
            // 
            // pictureBox3
            // 
            string imagePathSignup = Path.Combine(Application.StartupPath, @"..\..\..\..\Assets\Images\signupbg.jpg");
            pictureBox3.BackgroundImage = Image.FromFile(imagePathSignup); pictureBox3.Location = new Point(-4, 3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(407, 568);
            pictureBox3.TabIndex = 19;
            pictureBox3.TabStop = false;
            // 
            // lblRegisterHeader
            // 
            lblRegisterHeader.AutoSize = true;
            lblRegisterHeader.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblRegisterHeader.ForeColor = Color.FromArgb(0, 0, 192);
            lblRegisterHeader.Location = new Point(444, 15);
            lblRegisterHeader.Name = "lblRegisterHeader";
            lblRegisterHeader.Size = new Size(263, 32);
            lblRegisterHeader.TabIndex = 0;
            lblRegisterHeader.Text = "ĐĂNG KÝ TÀI KHOẢN";
            // 
            // lblRegUsername
            // 
            lblRegUsername.AutoSize = true;
            lblRegUsername.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblRegUsername.Location = new Point(444, 67);
            lblRegUsername.Name = "lblRegUsername";
            lblRegUsername.Size = new Size(102, 17);
            lblRegUsername.TabIndex = 1;
            lblRegUsername.Text = "Tên đăng nhập:";
            // 
            // txtRegUsername
            // 
            txtRegUsername.BorderStyle = BorderStyle.FixedSingle;
            txtRegUsername.Font = new Font("Segoe UI", 10F);
            txtRegUsername.Location = new Point(444, 92);
            txtRegUsername.Name = "txtRegUsername";
            txtRegUsername.Size = new Size(277, 25);
            txtRegUsername.TabIndex = 2;
            // 
            // lblRegPassword
            // 
            lblRegPassword.AutoSize = true;
            lblRegPassword.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblRegPassword.Location = new Point(444, 132);
            lblRegPassword.Name = "lblRegPassword";
            lblRegPassword.Size = new Size(69, 17);
            lblRegPassword.TabIndex = 3;
            lblRegPassword.Text = "Mật khẩu:";
            // 
            // txtRegPassword
            // 
            txtRegPassword.BorderStyle = BorderStyle.FixedSingle;
            txtRegPassword.Font = new Font("Segoe UI", 10F);
            txtRegPassword.Location = new Point(444, 157);
            txtRegPassword.Name = "txtRegPassword";
            txtRegPassword.PasswordChar = '●';
            txtRegPassword.Size = new Size(277, 25);
            txtRegPassword.TabIndex = 4;
            // 
            // progressBarPasswordStrength
            // 
            progressBarPasswordStrength.Location = new Point(444, 192);
            progressBarPasswordStrength.Name = "progressBarPasswordStrength";
            progressBarPasswordStrength.Size = new Size(277, 10);
            progressBarPasswordStrength.TabIndex = 5;
            // 
            // lblPasswordStrength
            // 
            lblPasswordStrength.AutoSize = true;
            lblPasswordStrength.Font = new Font("Segoe UI", 8F);
            lblPasswordStrength.Location = new Point(444, 207);
            lblPasswordStrength.Name = "lblPasswordStrength";
            lblPasswordStrength.Size = new Size(168, 13);
            lblPasswordStrength.TabIndex = 6;
            lblPasswordStrength.Text = "Độ mạnh mật khẩu: Chưa nhập";
            // 
            // lblRegConfirmPassword
            // 
            lblRegConfirmPassword.AutoSize = true;
            lblRegConfirmPassword.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblRegConfirmPassword.Location = new Point(444, 227);
            lblRegConfirmPassword.Name = "lblRegConfirmPassword";
            lblRegConfirmPassword.Size = new Size(129, 17);
            lblRegConfirmPassword.TabIndex = 7;
            lblRegConfirmPassword.Text = "Xác nhận mật khẩu:";
            // 
            // txtRegConfirmPassword
            // 
            txtRegConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtRegConfirmPassword.Font = new Font("Segoe UI", 10F);
            txtRegConfirmPassword.Location = new Point(444, 252);
            txtRegConfirmPassword.Name = "txtRegConfirmPassword";
            txtRegConfirmPassword.PasswordChar = '●';
            txtRegConfirmPassword.Size = new Size(277, 25);
            txtRegConfirmPassword.TabIndex = 8;
            // 
            // lblRegName
            // 
            lblRegName.AutoSize = true;
            lblRegName.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblRegName.Location = new Point(444, 292);
            lblRegName.Name = "lblRegName";
            lblRegName.Size = new Size(71, 17);
            lblRegName.TabIndex = 9;
            lblRegName.Text = "Họ và tên:";
            // 
            // txtRegName
            // 
            txtRegName.BorderStyle = BorderStyle.FixedSingle;
            txtRegName.Font = new Font("Segoe UI", 10F);
            txtRegName.Location = new Point(444, 317);
            txtRegName.Name = "txtRegName";
            txtRegName.Size = new Size(277, 25);
            txtRegName.TabIndex = 10;
            // 
            // lblRegEmail
            // 
            lblRegEmail.AutoSize = true;
            lblRegEmail.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblRegEmail.Location = new Point(444, 357);
            lblRegEmail.Name = "lblRegEmail";
            lblRegEmail.Size = new Size(43, 17);
            lblRegEmail.TabIndex = 11;
            lblRegEmail.Text = "Email:";
            // 
            // txtRegEmail
            // 
            txtRegEmail.BorderStyle = BorderStyle.FixedSingle;
            txtRegEmail.Font = new Font("Segoe UI", 10F);
            txtRegEmail.Location = new Point(444, 382);
            txtRegEmail.Name = "txtRegEmail";
            txtRegEmail.Size = new Size(277, 25);
            txtRegEmail.TabIndex = 12;
            // 
            // lblRegPhone
            // 
            lblRegPhone.AutoSize = true;
            lblRegPhone.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblRegPhone.Location = new Point(444, 422);
            lblRegPhone.Name = "lblRegPhone";
            lblRegPhone.Size = new Size(91, 17);
            lblRegPhone.TabIndex = 13;
            lblRegPhone.Text = "Số điện thoại:";
            // 
            // txtRegPhone
            // 
            txtRegPhone.BorderStyle = BorderStyle.FixedSingle;
            txtRegPhone.Font = new Font("Segoe UI", 10F);
            txtRegPhone.Location = new Point(444, 447);
            txtRegPhone.Name = "txtRegPhone";
            txtRegPhone.Size = new Size(277, 25);
            txtRegPhone.TabIndex = 14;
            // 
            // lblRegisterError
            // 
            lblRegisterError.Font = new Font("Segoe UI", 9F);
            lblRegisterError.ForeColor = Color.Red;
            lblRegisterError.Location = new Point(449, 486);
            lblRegisterError.Name = "lblRegisterError";
            lblRegisterError.Size = new Size(260, 40);
            lblRegisterError.TabIndex = 15;
            lblRegisterError.TextAlign = ContentAlignment.MiddleCenter;
            lblRegisterError.Visible = false;
            // 
            // lblRegisterSuccess
            // 
            lblRegisterSuccess.Font = new Font("Segoe UI", 9F);
            lblRegisterSuccess.ForeColor = Color.Green;
            lblRegisterSuccess.Location = new Point(448, 481);
            lblRegisterSuccess.Name = "lblRegisterSuccess";
            lblRegisterSuccess.Size = new Size(268, 40);
            lblRegisterSuccess.TabIndex = 16;
            lblRegisterSuccess.TextAlign = ContentAlignment.MiddleCenter;
            lblRegisterSuccess.Visible = false;
            // 
            // btnRegister
            // 
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.BackColor = Color.FromArgb(40, 167, 69);
            btnRegister.ForeColor = Color.White;
            btnRegister.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Location = new Point(446, 529);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(117, 40);
            btnRegister.TabIndex = 17;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += BtnRegister_Click;
            // 
            // btnSwitchToLogin
            // 
            btnSwitchToLogin.FlatStyle = FlatStyle.Flat;
            btnSwitchToLogin.BackColor = Color.FromArgb(108, 117, 125);
            btnSwitchToLogin.ForeColor = Color.White;
            btnSwitchToLogin.Font = new Font("Segoe UI", 10F);
            btnSwitchToLogin.FlatAppearance.BorderSize = 0;
            btnSwitchToLogin.Cursor = Cursors.Hand;
            btnSwitchToLogin.Location = new Point(604, 529);
            btnSwitchToLogin.Name = "btnSwitchToLogin";
            btnSwitchToLogin.Size = new Size(117, 40);
            btnSwitchToLogin.TabIndex = 18;
            btnSwitchToLogin.Text = "Đã có tài khoản";
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

            // Đặt ToolTip cho các button chính
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnLogin, "Đăng nhập vào hệ thống");
            toolTip.SetToolTip(btnSwitchToRegister, "Chuyển sang đăng ký tài khoản mới");
            toolTip.SetToolTip(btnRegister, "Tạo tài khoản mới");
            toolTip.SetToolTip(btnSwitchToLogin, "Quay lại màn hình đăng nhập");
            toolTip.SetToolTip(lnkForgotPassword, "Lấy lại mật khẩu nếu bạn quên");
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
            tabPageLogin.PerformLayout();
            ((ISupportInitialize)pictureBox1).EndInit();
            ((ISupportInitialize)pictureBox2).EndInit();
            tabPageRegister.ResumeLayout(false);
            tabPageRegister.PerformLayout();
            ((ISupportInitialize)pictureBox3).EndInit();
            ((ISupportInitialize)errorProviderLogin).EndInit();
            ((ISupportInitialize)errorProviderRegister).EndInit();
            ResumeLayout(false);
        }
    }
}
