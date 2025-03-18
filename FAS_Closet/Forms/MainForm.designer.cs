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
        private System.Windows.Forms.Button btnInventoryManagement;
        private System.Windows.Forms.Button btnOrderManagement;
        private System.Windows.Forms.Button btnCustomerManagement;
        private System.Windows.Forms.Button btnRevenueReport;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.FlowLayoutPanel featureToolbarPanel; // Thêm dòng này

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            headerPanel = new Panel();
            lblWelcome = new Label();
            btnLogout = new Button();
            mainSplit = new SplitContainer();
            leftPanel = new FlowLayoutPanel();
            btnProductManagement = new Button();
            btnInventoryManagement = new Button();
            btnOrderManagement = new Button();
            btnCustomerManagement = new Button();
            btnRevenueReport = new Button();
            btnDashboard = new Button();
            contentPanel = new Panel();
            featureToolbarPanel = new FlowLayoutPanel();
            headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainSplit).BeginInit();
            mainSplit.Panel1.SuspendLayout();
            mainSplit.Panel2.SuspendLayout();
            mainSplit.SuspendLayout();
            leftPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(248, 249, 250);
            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Controls.Add(btnLogout);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(10);
            headerPanel.Size = new Size(1200, 50);
            headerPanel.TabIndex = 0;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 12F);
            lblWelcome.Location = new Point(0, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(113, 21);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome, User";
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(1108, 12);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(80, 30);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // mainSplit
            // 
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.Location = new Point(0, 50);
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
            mainSplit.Size = new Size(1200, 650);
            mainSplit.SplitterDistance = 300;
            mainSplit.TabIndex = 1;
            // 
            // leftPanel
            // 
            leftPanel.AutoScroll = true;
            leftPanel.BackColor = Color.FromArgb(250, 250, 250);
            leftPanel.Controls.Add(btnProductManagement);
            leftPanel.Controls.Add(btnInventoryManagement);
            leftPanel.Controls.Add(btnOrderManagement);
            leftPanel.Controls.Add(btnCustomerManagement);
            leftPanel.Controls.Add(btnRevenueReport);
            leftPanel.Controls.Add(btnDashboard);
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.FlowDirection = FlowDirection.TopDown;
            leftPanel.Location = new Point(0, 0);
            leftPanel.Name = "leftPanel";
            leftPanel.Padding = new Padding(10);
            leftPanel.Size = new Size(300, 650);
            leftPanel.TabIndex = 0;
            leftPanel.WrapContents = false;
            // 
            // btnProductManagement
            // 
            btnProductManagement.BackColor = Color.FromArgb(0, 123, 255);
            btnProductManagement.FlatStyle = FlatStyle.Flat;
            btnProductManagement.ForeColor = Color.White;
            btnProductManagement.Location = new Point(15, 15);
            btnProductManagement.Margin = new Padding(5);
            btnProductManagement.Name = "btnProductManagement";
            btnProductManagement.Size = new Size(220, 40);
            btnProductManagement.TabIndex = 0;
            btnProductManagement.Text = "Quản lý sản phẩm";
            btnProductManagement.UseVisualStyleBackColor = false;
            btnProductManagement.Click += btnProductManagement_Click;
            // 
            // btnInventoryManagement
            // 
            btnInventoryManagement.BackColor = Color.FromArgb(0, 123, 255);
            btnInventoryManagement.FlatStyle = FlatStyle.Flat;
            btnInventoryManagement.ForeColor = Color.White;
            btnInventoryManagement.Location = new Point(15, 65);
            btnInventoryManagement.Margin = new Padding(5);
            btnInventoryManagement.Name = "btnInventoryManagement";
            btnInventoryManagement.Size = new Size(220, 40);
            btnInventoryManagement.TabIndex = 1;
            btnInventoryManagement.Text = "Quản lý kho hàng";
            btnInventoryManagement.UseVisualStyleBackColor = false;
            btnInventoryManagement.Click += btnInventoryManagement_Click;
            // 
            // btnOrderManagement
            // 
            btnOrderManagement.BackColor = Color.FromArgb(0, 123, 255);
            btnOrderManagement.FlatStyle = FlatStyle.Flat;
            btnOrderManagement.ForeColor = Color.White;
            btnOrderManagement.Location = new Point(15, 115);
            btnOrderManagement.Margin = new Padding(5);
            btnOrderManagement.Name = "btnOrderManagement";
            btnOrderManagement.Size = new Size(220, 40);
            btnOrderManagement.TabIndex = 2;
            btnOrderManagement.Text = "Quản lý đơn hàng";
            btnOrderManagement.UseVisualStyleBackColor = false;
            btnOrderManagement.Click += btnOrderManagement_Click;
            // 
            // btnCustomerManagement
            // 
            btnCustomerManagement.BackColor = Color.FromArgb(0, 123, 255);
            btnCustomerManagement.FlatStyle = FlatStyle.Flat;
            btnCustomerManagement.ForeColor = Color.White;
            btnCustomerManagement.Location = new Point(15, 165);
            btnCustomerManagement.Margin = new Padding(5);
            btnCustomerManagement.Name = "btnCustomerManagement";
            btnCustomerManagement.Size = new Size(220, 40);
            btnCustomerManagement.TabIndex = 3;
            btnCustomerManagement.Text = "Quản lý khách hàng";
            btnCustomerManagement.UseVisualStyleBackColor = false;
            btnCustomerManagement.Click += btnCustomerManagement_Click;
            // 
            // btnRevenueReport
            // 
            btnRevenueReport.BackColor = Color.FromArgb(0, 123, 255);
            btnRevenueReport.FlatStyle = FlatStyle.Flat;
            btnRevenueReport.ForeColor = Color.White;
            btnRevenueReport.Location = new Point(15, 215);
            btnRevenueReport.Margin = new Padding(5);
            btnRevenueReport.Name = "btnRevenueReport";
            btnRevenueReport.Size = new Size(220, 40);
            btnRevenueReport.TabIndex = 4;
            btnRevenueReport.Text = "Báo cáo doanh thu";
            btnRevenueReport.UseVisualStyleBackColor = false;
            btnRevenueReport.Click += btnRevenueReport_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(0, 123, 255);
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(15, 265);
            btnDashboard.Margin = new Padding(5);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(220, 40);
            btnDashboard.TabIndex = 5;
            btnDashboard.Text = "Dashboard";
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.White;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(0, 50);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(896, 600);
            contentPanel.TabIndex = 0;
            // 
            // featureToolbarPanel
            // 
            featureToolbarPanel.BackColor = Color.LightGray;
            featureToolbarPanel.Dock = DockStyle.Top;
            featureToolbarPanel.Location = new Point(0, 0);
            featureToolbarPanel.Name = "featureToolbarPanel";
            featureToolbarPanel.Padding = new Padding(5);
            featureToolbarPanel.Size = new Size(896, 50);
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
            mainSplit.Panel1.ResumeLayout(false);
            mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplit).EndInit();
            mainSplit.ResumeLayout(false);
            leftPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
