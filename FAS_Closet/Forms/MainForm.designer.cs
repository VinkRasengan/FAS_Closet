// This file contains the designer code for the MainForm, which is the main application window.

namespace FASCloset.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        
        // Các điều khiển chính
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.SplitContainer mainSplit;
        private System.Windows.Forms.FlowLayoutPanel leftPanel;
        private System.Windows.Forms.Button btnProductManagement;
        private System.Windows.Forms.Button btnOrderManagement;
        private System.Windows.Forms.Button btnCustomerManagement;
        private System.Windows.Forms.Button btnRevenueReport;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnNotificationSettings;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.FlowLayoutPanel featureToolbarPanel;
        private System.Windows.Forms.Panel warehousePanel;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.ComboBox cmbWarehouses;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            headerPanel = new Panel();
            label2 = new Label();
            label1 = new Label();
            warehousePanel = new Panel();
            lblWarehouse = new Label();
            cmbWarehouses = new ComboBox();
            lblWelcome = new Label();
            btnLogout = new Button();
            mainSplit = new SplitContainer();
            leftPanel = new FlowLayoutPanel();
            btnProductManagement = new Button();
            btnOrderManagement = new Button();
            btnCustomerManagement = new Button();
            btnRevenueReport = new Button();
            btnDashboard = new Button();
            contentPanel = new Panel();
            featureToolbarPanel = new FlowLayoutPanel();
            btnNotificationSettings = new Button();
            headerPanel.SuspendLayout();
            warehousePanel.SuspendLayout();
            ((ISupportInitialize)mainSplit).BeginInit();
            mainSplit.Panel1.SuspendLayout();
            mainSplit.Panel2.SuspendLayout();
            mainSplit.SuspendLayout();
            leftPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.Navy;
            headerPanel.Controls.Add(label2);
            headerPanel.Controls.Add(label1);
            headerPanel.Controls.Add(warehousePanel);
            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(10);
            headerPanel.Size = new Size(1200, 66);
            headerPanel.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(74, 27);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 5;
            label2.Text = "Closet";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(15, 12);
            label1.Name = "label1";
            label1.Size = new Size(67, 37);
            label1.TabIndex = 4;
            label1.Text = "FAS";
            // 
            // warehousePanel
            // 
            warehousePanel.Controls.Add(lblWarehouse);
            warehousePanel.Controls.Add(cmbWarehouses);
            warehousePanel.Location = new Point(867, 12);
            warehousePanel.Name = "warehousePanel";
            warehousePanel.Size = new Size(320, 40);
            warehousePanel.TabIndex = 2;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblWarehouse.ForeColor = Color.White;
            lblWarehouse.Location = new Point(3, 15);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(123, 17);
            lblWarehouse.TabIndex = 0;
            // 
            // cmbWarehouses
            // 
            cmbWarehouses.BackColor = Color.White;
            cmbWarehouses.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouses.Location = new Point(128, 12);
            cmbWarehouses.Name = "cmbWarehouses";
            cmbWarehouses.Size = new Size(180, 23);
            cmbWarehouses.TabIndex = 1;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Location = new Point(304, 25);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(113, 21);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome, User";
            // 
            // btnLogout
            // 
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            btnLogout.ForeColor = Color.White;
            btnLogout.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Text = "🚪 Đăng xuất";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.Padding = new Padding(15, 0, 0, 0);
            btnLogout.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogout.Location = new Point(15, 350);
            btnLogout.Margin = new Padding(5, 20, 5, 5);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(260, 45);
            btnLogout.TabIndex = 1;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // mainSplit
            // 
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.Location = new Point(0, 66);
            mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            mainSplit.Panel1.Controls.Add(leftPanel);
            // 
            // mainSplit.Panel2
            // 
            mainSplit.Panel2.Controls.Add(contentPanel);
            mainSplit.Panel2.Controls.Add(featureToolbarPanel);
            mainSplit.Size = new Size(1200, 634);
            mainSplit.SplitterDistance = 300;
            mainSplit.TabIndex = 1;
            // 
            // leftPanel
            // 
            leftPanel.AutoScroll = true;
            leftPanel.BackColor = Color.FromArgb(248, 249, 250);
            leftPanel.Controls.Add(btnDashboard);
            leftPanel.Controls.Add(btnProductManagement);
            leftPanel.Controls.Add(btnOrderManagement);
            leftPanel.Controls.Add(btnCustomerManagement);
            leftPanel.Controls.Add(btnRevenueReport);
            leftPanel.Controls.Add(btnNotificationSettings);
            leftPanel.Controls.Add(btnLogout);
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.FlowDirection = FlowDirection.TopDown;
            leftPanel.Location = new Point(0, 0);
            leftPanel.Name = "leftPanel";
            leftPanel.Padding = new Padding(10, 15, 10, 15);
            leftPanel.Size = new Size(300, 634);
            leftPanel.TabIndex = 0;
            leftPanel.WrapContents = false;
            // 
            // btnProductManagement
            // 
            btnProductManagement.FlatStyle = FlatStyle.Flat;
            btnProductManagement.BackColor = Color.FromArgb(248, 249, 250);
            btnProductManagement.ForeColor = Color.FromArgb(52, 58, 64);
            btnProductManagement.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            btnProductManagement.FlatAppearance.BorderSize = 0;
            btnProductManagement.Cursor = Cursors.Hand;
            btnProductManagement.Text = "📦 Quản lý sản phẩm";
            btnProductManagement.TextAlign = ContentAlignment.MiddleLeft;
            btnProductManagement.Padding = new Padding(15, 0, 0, 0);
            btnProductManagement.ImageAlign = ContentAlignment.MiddleLeft;
            btnProductManagement.Location = new Point(15, 75);
            btnProductManagement.Margin = new Padding(5);
            btnProductManagement.Name = "btnProductManagement";
            btnProductManagement.Size = new Size(260, 45);
            btnProductManagement.TabIndex = 0;
            btnProductManagement.UseVisualStyleBackColor = false;
            btnProductManagement.Click += btnProductManagement_Click;
            // 
            // btnOrderManagement
            // 
            btnOrderManagement.FlatStyle = FlatStyle.Flat;
            btnOrderManagement.BackColor = Color.FromArgb(248, 249, 250);
            btnOrderManagement.ForeColor = Color.FromArgb(52, 58, 64);
            btnOrderManagement.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            btnOrderManagement.FlatAppearance.BorderSize = 0;
            btnOrderManagement.Cursor = Cursors.Hand;
            btnOrderManagement.Text = "🛒 Đơn hàng";
            btnOrderManagement.TextAlign = ContentAlignment.MiddleLeft;
            btnOrderManagement.Padding = new Padding(15, 0, 0, 0);
            btnOrderManagement.ImageAlign = ContentAlignment.MiddleLeft;
            btnOrderManagement.Location = new Point(15, 130);
            btnOrderManagement.Margin = new Padding(5);
            btnOrderManagement.Name = "btnOrderManagement";
            btnOrderManagement.Size = new Size(260, 45);
            btnOrderManagement.TabIndex = 1;
            btnOrderManagement.UseVisualStyleBackColor = false;
            btnOrderManagement.Click += btnOrderManagement_Click;
            // 
            // btnCustomerManagement
            // 
            btnCustomerManagement.FlatStyle = FlatStyle.Flat;
            btnCustomerManagement.BackColor = Color.FromArgb(248, 249, 250);
            btnCustomerManagement.ForeColor = Color.FromArgb(52, 58, 64);
            btnCustomerManagement.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            btnCustomerManagement.FlatAppearance.BorderSize = 0;
            btnCustomerManagement.Cursor = Cursors.Hand;
            btnCustomerManagement.Text = "👥 Khách hàng";
            btnCustomerManagement.TextAlign = ContentAlignment.MiddleLeft;
            btnCustomerManagement.Padding = new Padding(15, 0, 0, 0);
            btnCustomerManagement.ImageAlign = ContentAlignment.MiddleLeft;
            btnCustomerManagement.Location = new Point(15, 185);
            btnCustomerManagement.Margin = new Padding(5);
            btnCustomerManagement.Name = "btnCustomerManagement";
            btnCustomerManagement.Size = new Size(260, 45);
            btnCustomerManagement.TabIndex = 2;
            btnCustomerManagement.UseVisualStyleBackColor = false;
            btnCustomerManagement.Click += btnCustomerManagement_Click;
            // 
            // btnRevenueReport
            // 
            btnRevenueReport.FlatStyle = FlatStyle.Flat;
            btnRevenueReport.BackColor = Color.FromArgb(248, 249, 250);
            btnRevenueReport.ForeColor = Color.FromArgb(52, 58, 64);
            btnRevenueReport.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            btnRevenueReport.FlatAppearance.BorderSize = 0;
            btnRevenueReport.Cursor = Cursors.Hand;
            btnRevenueReport.Text = "📈 Báo cáo";
            btnRevenueReport.TextAlign = ContentAlignment.MiddleLeft;
            btnRevenueReport.Padding = new Padding(15, 0, 0, 0);
            btnRevenueReport.ImageAlign = ContentAlignment.MiddleLeft;
            btnRevenueReport.Location = new Point(15, 240);
            btnRevenueReport.Margin = new Padding(5);
            btnRevenueReport.Name = "btnRevenueReport";
            btnRevenueReport.Size = new Size(260, 45);
            btnRevenueReport.TabIndex = 3;
            btnRevenueReport.UseVisualStyleBackColor = false;
            btnRevenueReport.Click += btnRevenueReport_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.BackColor = Color.FromArgb(0, 123, 255);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.Cursor = Cursors.Hand;
            btnDashboard.Text = "📊 Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Padding = new Padding(15, 0, 0, 0);
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(15, 20);
            btnDashboard.Margin = new Padding(5);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(260, 45);
            btnDashboard.TabIndex = 5;
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // btnNotificationSettings
            // 
            btnNotificationSettings.FlatStyle = FlatStyle.Flat;
            btnNotificationSettings.BackColor = Color.FromArgb(248, 249, 250);
            btnNotificationSettings.ForeColor = Color.FromArgb(52, 58, 64);
            btnNotificationSettings.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            btnNotificationSettings.FlatAppearance.BorderSize = 0;
            btnNotificationSettings.Cursor = Cursors.Hand;
            btnNotificationSettings.Text = "⚙️ Cài đặt";
            btnNotificationSettings.TextAlign = ContentAlignment.MiddleLeft;
            btnNotificationSettings.Padding = new Padding(15, 0, 0, 0);
            btnNotificationSettings.ImageAlign = ContentAlignment.MiddleLeft;
            btnNotificationSettings.Location = new Point(15, 295);
            btnNotificationSettings.Margin = new Padding(5);
            btnNotificationSettings.Name = "btnNotificationSettings";
            btnNotificationSettings.Size = new Size(260, 45);
            btnNotificationSettings.TabIndex = 6;
            btnNotificationSettings.UseVisualStyleBackColor = false;
            btnNotificationSettings.Click += btnNotificationSettings_Click;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.White;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(0, 55);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(10);
            contentPanel.Size = new Size(896, 579);
            contentPanel.TabIndex = 0;
            // 
            // featureToolbarPanel
            // 
            featureToolbarPanel.BackColor = Color.FromArgb(233, 236, 239);
            featureToolbarPanel.Dock = DockStyle.Top;
            featureToolbarPanel.Location = new Point(0, 0);
            featureToolbarPanel.Name = "featureToolbarPanel";
            featureToolbarPanel.Padding = new Padding(10, 5, 10, 5);
            featureToolbarPanel.Size = new Size(896, 55);
            featureToolbarPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1200, 700);
            Controls.Add(mainSplit);
            Controls.Add(headerPanel);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainForm";
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            warehousePanel.ResumeLayout(false);
            warehousePanel.PerformLayout();
            mainSplit.Panel1.ResumeLayout(false);
            mainSplit.Panel2.ResumeLayout(false);
            ((ISupportInitialize)mainSplit).EndInit();
            mainSplit.ResumeLayout(false);
            leftPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
        private Label label1;
        private Label label2;
    }
}
