// This file contains the designer code for the MainForm, which is the main application window.

namespace FASCloset.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.productsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ordersItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customersItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.leftPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCategorize = new System.Windows.Forms.Button();
            this.btnDataManagement = new System.Windows.Forms.Button();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilterCategory = new System.Windows.Forms.ComboBox();
            this.productDisplay = new System.Windows.Forms.DataGridView();
            this.addEditPanel = new System.Windows.Forms.Panel();
            this.layout = new System.Windows.Forms.TableLayoutPanel();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // Form
            this.Text = "MainForm";
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Header Panel
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Height = 45;
            this.headerPanel.BackColor = System.Drawing.ColorTranslator.FromHtml("#f8f9fa");
            this.Controls.Add(this.headerPanel);

            // Welcome Label
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            this.lblWelcome.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.headerPanel.Controls.Add(this.lblWelcome);

            // Logout Button
            this.btnLogout.Text = "Logout";
            this.btnLogout.Width = 80;
            this.btnLogout.Height = 30;
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Margin = new System.Windows.Forms.Padding(10);
            this.btnLogout.Click += (s, e) => this.Close();
            this.headerPanel.Controls.Add(this.btnLogout);

            // MenuStrip
            this.mainMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainMenu.BackColor = System.Drawing.Color.White;
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.productsItem, this.inventoryItem, this.ordersItem, this.customersItem, this.reportsItem
            });
            this.Controls.Add(this.mainMenu);

            // Menu Items
            this.productsItem.Text = "Products";
            this.productsItem.Click += productsToolStripMenuItem_Click;
            this.inventoryItem.Text = "Inventory";
            this.inventoryItem.Click += inventoryToolStripMenuItem_Click;
            this.ordersItem.Text = "Orders";
            this.ordersItem.Click += ordersToolStripMenuItem_Click;
            this.customersItem.Text = "Customers";
            this.customersItem.Click += customersToolStripMenuItem_Click;
            this.reportsItem.Text = "Reports";
            this.reportsItem.Click += reportsToolStripMenuItem_Click;

            // SplitContainer
            this.mainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplit.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.mainSplit.Panel1MinSize = 100;
            this.mainSplit.IsSplitterFixed = false;
            this.Controls.Add(this.mainSplit);

            // Left Panel
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.leftPanel.WrapContents = false;
            this.leftPanel.AutoScroll = true;
            this.leftPanel.Padding = new System.Windows.Forms.Padding(10, 60, 10, 10);
            this.leftPanel.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.mainSplit.Panel1.Controls.Add(this.leftPanel);

            // Buttons
            CreateStyledButton(out this.btnAdd, "Thêm", btnAdd_Click);
            CreateStyledButton(out this.btnEdit, "Sửa", btnEdit_Click);
            CreateStyledButton(out this.btnDelete, "Xóa", btnDelete_Click);
            CreateStyledButton(out this.btnCategorize, "Phân loại", null);
            CreateStyledButton(out this.btnDataManagement, "Quản lý dữ liệu", null);
            this.leftPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnAdd, this.btnEdit, this.btnDelete, this.btnCategorize, this.btnDataManagement
            });

            // Right Panel
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.BackColor = System.Drawing.Color.White;
            this.mainSplit.Panel2.Controls.Add(this.rightPanel);

            // Filter Panel
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Height = 40;
            this.filterPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.filterPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.rightPanel.Controls.Add(this.filterPanel);

            // Filter Label
            this.lblFilter.Text = "Filter by Category:";
            this.lblFilter.AutoSize = true;
            this.lblFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 10);
            this.lblFilter.Padding = new System.Windows.Forms.Padding(0, 5, 5, 0);
            this.filterPanel.Controls.Add(this.lblFilter);

            // Filter ComboBox
            this.cmbFilterCategory.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbFilterCategory.Width = 200;
            this.cmbFilterCategory.Font = new System.Drawing.Font("Segoe UI", 10);
            this.cmbFilterCategory.SelectedIndexChanged += cmbFilterCategory_SelectedIndexChanged;
            this.filterPanel.Controls.Add(this.cmbFilterCategory);

            // DataGridView
            this.productDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productDisplay.ReadOnly = true;
            this.productDisplay.AllowUserToAddRows = false;
            this.productDisplay.AllowUserToDeleteRows = false;
            this.productDisplay.MultiSelect = false;
            this.productDisplay.AutoGenerateColumns = false;
            this.productDisplay.BackgroundColor = System.Drawing.Color.White;
            this.productDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productDisplay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "ProductID", Width = 50 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "ProductName", Width = 150 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { HeaderText = "Category", DataPropertyName = "CategoryID", Width = 80 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { HeaderText = "Price", DataPropertyName = "Price", Width = 80 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { HeaderText = "Stock", DataPropertyName = "Stock", Width = 80 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { HeaderText = "Description", DataPropertyName = "Description", AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill }
            });
            this.rightPanel.Controls.Add(this.productDisplay);

            // Add/Edit Panel
            this.addEditPanel.Visible = false;
            this.addEditPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.addEditPanel.Width = 300;
            this.addEditPanel.AutoScroll = true;
            this.addEditPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.addEditPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightPanel.Controls.Add(this.addEditPanel);
            this.rightPanel.Controls.SetChildIndex(this.addEditPanel, 0);

            // Add/Edit Layout
            this.layout.ColumnCount = 2;
            this.layout.RowCount = 6;
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Padding = new System.Windows.Forms.Padding(20);
            this.addEditPanel.Controls.Add(this.layout);

            // Add/Edit Controls
            this.txtProductName.Width = 150;
            this.cmbCategory.Width = 150;
            this.txtPrice.Width = 150;
            this.txtStock.Width = 150;
            this.txtDescription.Width = 150;
            this.btnSave.Text = "Save";
            this.btnSave.Width = 75;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Width = 75;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = System.Drawing.Color.White;

            // Add/Edit Layout Controls
            this.layout.Controls.Add(new System.Windows.Forms.Label { Text = "Product Name:", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) }, 0, 0);
            this.layout.Controls.Add(this.txtProductName, 1, 0);
            this.layout.Controls.Add(new System.Windows.Forms.Label { Text = "Category:", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) }, 0, 1);
            this.layout.Controls.Add(this.cmbCategory, 1, 1);
            this.layout.Controls.Add(new System.Windows.Forms.Label { Text = "Price:", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) }, 0, 2);
            this.layout.Controls.Add(this.txtPrice, 1, 2);
            this.layout.Controls.Add(new System.Windows.Forms.Label { Text = "Stock:", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) }, 0, 3);
            this.layout.Controls.Add(this.txtStock, 1, 3);
            this.layout.Controls.Add(new System.Windows.Forms.Label { Text = "Description:", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) }, 0, 4);
            this.layout.Controls.Add(this.txtDescription, 1, 4);
            this.layout.Controls.Add(this.btnSave, 0, 5);
            this.layout.Controls.Add(this.btnCancel, 1, 5);

            // Add/Edit Events
            this.btnSave.Click += btnSave_Click;
            this.btnCancel.Click += btnCancel_Click;
        }

        private void CreateStyledButton(out System.Windows.Forms.Button button, string text, EventHandler onClick)
        {
            button = new System.Windows.Forms.Button
            {
                Text = text,
                Width = 150,
                Height = 40,
                Margin = new System.Windows.Forms.Padding(10),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(0, 123, 255),
                ForeColor = System.Drawing.Color.White
            };
            if (onClick != null)
            {
                button.Click += onClick;
            }
        }

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem productsItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryItem;
        private System.Windows.Forms.ToolStripMenuItem ordersItem;
        private System.Windows.Forms.ToolStripMenuItem customersItem;
        private System.Windows.Forms.ToolStripMenuItem reportsItem;
        private System.Windows.Forms.SplitContainer mainSplit;
        private System.Windows.Forms.FlowLayoutPanel leftPanel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCategorize;
        private System.Windows.Forms.Button btnDataManagement;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilterCategory;
        private System.Windows.Forms.DataGridView productDisplay;
        private System.Windows.Forms.Panel addEditPanel;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
