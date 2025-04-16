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
        private System.Windows.Forms.Button btnNotificationSettings;
        private System.Windows.Forms.Button btnWarehouseManagement;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.FlowLayoutPanel featureToolbarPanel; // Thêm dòng này
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
            btnInventoryManagement = new Button();
            btnOrderManagement = new Button();
            btnCustomerManagement = new Button();
            btnRevenueReport = new Button();
            btnDashboard = new Button();
            contentPanel = new Panel();
            featureToolbarPanel = new FlowLayoutPanel();
            btnNotificationSettings = new Button();
            btnWarehouseManagement = new Button();
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
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(15, 313);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(222, 41);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Logout";
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
            leftPanel.BackColor = Color.FromArgb(250, 250, 250);
            leftPanel.Controls.Add(btnDashboard);
            leftPanel.Controls.Add(btnProductManagement);
            leftPanel.Controls.Add(btnInventoryManagement);
            leftPanel.Controls.Add(btnOrderManagement);
            leftPanel.Controls.Add(btnCustomerManagement);
            leftPanel.Controls.Add(btnRevenueReport);
            leftPanel.Controls.Add(btnLogout);
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.FlowDirection = FlowDirection.TopDown;
            leftPanel.Location = new Point(0, 0);
            leftPanel.Name = "leftPanel";
            leftPanel.Padding = new Padding(10);
            leftPanel.Size = new Size(300, 634);
            leftPanel.TabIndex = 0;
            leftPanel.WrapContents = false;
            // 
            // btnProductManagement
            // 
            btnProductManagement.BackColor = Color.CornflowerBlue;
            btnProductManagement.FlatStyle = FlatStyle.Flat;
            btnProductManagement.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnProductManagement.ForeColor = Color.White;
            btnProductManagement.Location = new Point(15, 65);
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
            btnInventoryManagement.BackColor = Color.CornflowerBlue;
            btnInventoryManagement.FlatStyle = FlatStyle.Flat;
            btnInventoryManagement.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnInventoryManagement.ForeColor = Color.White;
            btnInventoryManagement.Location = new Point(15, 115);
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
            btnOrderManagement.BackColor = Color.CornflowerBlue;
            btnOrderManagement.FlatStyle = FlatStyle.Flat;
            btnOrderManagement.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnOrderManagement.ForeColor = Color.White;
            btnOrderManagement.Location = new Point(15, 165);
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
            btnCustomerManagement.BackColor = Color.CornflowerBlue;
            btnCustomerManagement.FlatStyle = FlatStyle.Flat;
            btnCustomerManagement.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnCustomerManagement.ForeColor = Color.White;
            btnCustomerManagement.Location = new Point(15, 215);
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
            btnRevenueReport.BackColor = Color.CornflowerBlue;
            btnRevenueReport.FlatStyle = FlatStyle.Flat;
            btnRevenueReport.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnRevenueReport.ForeColor = Color.White;
            btnRevenueReport.Location = new Point(15, 265);
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
            btnDashboard.BackColor = Color.CornflowerBlue;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(15, 15);
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
            contentPanel.Size = new Size(896, 584);
            contentPanel.TabIndex = 0;
            // 
            // featureToolbarPanel
            // 
            featureToolbarPanel.BackColor = SystemColors.GradientInactiveCaption;
            featureToolbarPanel.Dock = DockStyle.Top;
            featureToolbarPanel.Location = new Point(0, 0);
            featureToolbarPanel.Name = "featureToolbarPanel";
            featureToolbarPanel.Padding = new Padding(5);
            featureToolbarPanel.Size = new Size(896, 50);
            featureToolbarPanel.TabIndex = 1;
            // 
            // btnNotificationSettings
            // 
            btnNotificationSettings.BackColor = Color.FromArgb(0, 123, 255);
            btnNotificationSettings.FlatStyle = FlatStyle.Flat;
            btnNotificationSettings.ForeColor = Color.White;
            btnNotificationSettings.Location = new Point(15, 315);
            btnNotificationSettings.Margin = new Padding(5);
            btnNotificationSettings.Name = "btnNotificationSettings";
            btnNotificationSettings.Size = new Size(220, 40);
            btnNotificationSettings.TabIndex = 6;
            btnNotificationSettings.Text = "Notification Settings";
            btnNotificationSettings.UseVisualStyleBackColor = false;
            // 
            // btnWarehouseManagement
            // 
            btnWarehouseManagement.BackColor = Color.FromArgb(0, 123, 255);
            btnWarehouseManagement.FlatStyle = FlatStyle.Flat;
            btnWarehouseManagement.ForeColor = Color.White;
            btnWarehouseManagement.Location = new Point(15, 365);
            btnWarehouseManagement.Margin = new Padding(5);
            btnWarehouseManagement.Name = "btnWarehouseManagement";
            btnWarehouseManagement.Size = new Size(220, 40);
            btnWarehouseManagement.TabIndex = 7;
            btnWarehouseManagement.Text = "Warehouse Management";
            btnWarehouseManagement.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1200, 1200);
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
