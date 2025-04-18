using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace FASCloset.Forms
{
    partial class ProductForm
    {
        private System.ComponentModel.IContainer components = null;
        
        // Form controls
        private Panel panelHeader;
        private Label lblHeader;
        
        private TableLayoutPanel tableLayoutPanel;
        private Label lblProductName;
        private TextBox txtProductName;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Button btnAddCategory;
        private Label lblManufacturer;
        private ComboBox cmbManufacturer;
        private Button btnAddManufacturer;
        private Label lblPrice;
        private TextBox txtPrice;
        private Label lblStock;
        private TextBox txtStock;
        private Label lblDescription;
        private TextBox txtDescription;
        private CheckBox chkIsActive;
        
        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            
            // Form properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 600);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Padding = new Padding(10);
            
            // Components initialization
            panelHeader = new Panel();
            lblHeader = new Label();
            tableLayoutPanel = new TableLayoutPanel();
            lblProductName = new Label();
            txtProductName = new TextBox();
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            btnAddCategory = new Button();
            lblManufacturer = new Label();
            cmbManufacturer = new ComboBox();
            btnAddManufacturer = new Button();
            lblPrice = new Label();
            txtPrice = new TextBox();
            lblStock = new Label();
            txtStock = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            
            // Header Panel
            panelHeader.BackColor = Color.FromArgb(0, 120, 215);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 60;
            panelHeader.Padding = new Padding(20, 0, 0, 0);
            panelHeader.Controls.Add(lblHeader);
            
            // Header Label
            lblHeader.AutoSize = false;
            lblHeader.Dock = DockStyle.Fill;
            lblHeader.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Text = "Product Details";
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            
            // Table Layout Panel
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount = 9;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // Product Name
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // Category
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // Manufacturer
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // Price
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // Stock
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Description
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // Is Active
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));  // Buttons
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Filler
            tableLayoutPanel.Padding = new Padding(20);
            
            // Product Name Label
            lblProductName.AutoSize = true;
            lblProductName.Text = "Product Name:";
            lblProductName.Dock = DockStyle.Fill;
            lblProductName.TextAlign = ContentAlignment.MiddleRight;
            lblProductName.Font = new Font("Segoe UI", 10F);
            
            // Product Name TextBox
            txtProductName.Dock = DockStyle.Fill;
            txtProductName.Font = new Font("Segoe UI", 10F);
            tableLayoutPanel.SetColumnSpan(txtProductName, 2);
            
            // Category Label
            lblCategory.AutoSize = true;
            lblCategory.Text = "Category:";
            lblCategory.Dock = DockStyle.Fill;
            lblCategory.TextAlign = ContentAlignment.MiddleRight;
            lblCategory.Font = new Font("Segoe UI", 10F);
            
            // Category ComboBox
            cmbCategory.Dock = DockStyle.Fill;
            cmbCategory.Font = new Font("Segoe UI", 10F);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Add Category Button
            btnAddCategory.Text = "+";
            btnAddCategory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddCategory.Size = new Size(30, 30);
            btnAddCategory.UseVisualStyleBackColor = true;
            btnAddCategory.Anchor = AnchorStyles.Left;
            btnAddCategory.Click += btnAddCategory_Click;
            
            // Manufacturer Label
            lblManufacturer.AutoSize = true;
            lblManufacturer.Text = "Manufacturer:";
            lblManufacturer.Dock = DockStyle.Fill;
            lblManufacturer.TextAlign = ContentAlignment.MiddleRight;
            lblManufacturer.Font = new Font("Segoe UI", 10F);
            
            // Manufacturer ComboBox
            cmbManufacturer.Dock = DockStyle.Fill;
            cmbManufacturer.Font = new Font("Segoe UI", 10F);
            cmbManufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Add Manufacturer Button
            btnAddManufacturer.Text = "+";
            btnAddManufacturer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddManufacturer.Size = new Size(30, 30);
            btnAddManufacturer.UseVisualStyleBackColor = true;
            btnAddManufacturer.Anchor = AnchorStyles.Left;
            btnAddManufacturer.Click += btnAddManufacturer_Click;
            
            // Price Label
            lblPrice.AutoSize = true;
            lblPrice.Text = "Price:";
            lblPrice.Dock = DockStyle.Fill;
            lblPrice.TextAlign = ContentAlignment.MiddleRight;
            lblPrice.Font = new Font("Segoe UI", 10F);
            
            // Price TextBox
            txtPrice.Dock = DockStyle.Fill;
            txtPrice.Font = new Font("Segoe UI", 10F);
            tableLayoutPanel.SetColumnSpan(txtPrice, 2);
            
            // Stock Label
            lblStock.AutoSize = true;
            lblStock.Text = "Stock:";
            lblStock.Dock = DockStyle.Fill;
            lblStock.TextAlign = ContentAlignment.MiddleRight;
            lblStock.Font = new Font("Segoe UI", 10F);
            
            // Stock TextBox
            txtStock.Dock = DockStyle.Fill;
            txtStock.Font = new Font("Segoe UI", 10F);
            tableLayoutPanel.SetColumnSpan(txtStock, 2);
            
            // Description Label
            lblDescription.AutoSize = true;
            lblDescription.Text = "Description:";
            lblDescription.Dock = DockStyle.Fill;
            lblDescription.TextAlign = ContentAlignment.MiddleRight;
            lblDescription.Font = new Font("Segoe UI", 10F);
            
            // Description TextBox
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Font = new Font("Segoe UI", 10F);
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            tableLayoutPanel.SetColumnSpan(txtDescription, 2);
            
            // Is Active CheckBox
            chkIsActive.AutoSize = true;
            chkIsActive.Text = "Active Product";
            chkIsActive.Checked = true;
            chkIsActive.Font = new Font("Segoe UI", 10F);
            tableLayoutPanel.SetColumnSpan(chkIsActive, 2);
            
            // Save Button
            btnSave.Text = "Save";
            btnSave.Size = new Size(100, 40);
            btnSave.Font = new Font("Segoe UI", 10F);
            btnSave.UseVisualStyleBackColor = false;
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Anchor = AnchorStyles.Right;
            btnSave.Click += btnSave_Click;
            
            // Cancel Button
            btnCancel.Text = "Cancel";
            btnCancel.Size = new Size(100, 40);
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Anchor = AnchorStyles.Left;
            btnCancel.Click += btnCancel_Click;
            
            // Add controls to the TableLayoutPanel
            tableLayoutPanel.Controls.Add(lblProductName, 0, 0);
            tableLayoutPanel.Controls.Add(txtProductName, 1, 0);
            
            tableLayoutPanel.Controls.Add(lblCategory, 0, 1);
            tableLayoutPanel.Controls.Add(cmbCategory, 1, 1);
            tableLayoutPanel.Controls.Add(btnAddCategory, 2, 1);
            
            tableLayoutPanel.Controls.Add(lblManufacturer, 0, 2);
            tableLayoutPanel.Controls.Add(cmbManufacturer, 1, 2);
            tableLayoutPanel.Controls.Add(btnAddManufacturer, 2, 2);
            
            tableLayoutPanel.Controls.Add(lblPrice, 0, 3);
            tableLayoutPanel.Controls.Add(txtPrice, 1, 3);
            
            tableLayoutPanel.Controls.Add(lblStock, 0, 4);
            tableLayoutPanel.Controls.Add(txtStock, 1, 4);
            
            tableLayoutPanel.Controls.Add(lblDescription, 0, 5);
            tableLayoutPanel.Controls.Add(txtDescription, 1, 5);
            
            tableLayoutPanel.Controls.Add(chkIsActive, 1, 6);
            
            // Panel for buttons with FlowLayoutPanel
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Padding = new Padding(0, 5, 0, 0);
            
            tableLayoutPanel.Controls.Add(buttonPanel, 0, 7);
            tableLayoutPanel.SetColumnSpan(buttonPanel, 3);
            
            // Add all to form
            this.Controls.Add(tableLayoutPanel);
            this.Controls.Add(panelHeader);
        }
        
        #endregion
    }
}