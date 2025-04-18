using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace FASCloset.Forms
{
    partial class CategoryForm
    {
        private System.ComponentModel.IContainer components = null;
        
        private Panel panelHeader;
        private Label lblHeader;
        private TableLayoutPanel tableLayoutPanel;
        private Label lblCategoryName;
        private TextBox txtCategoryName;
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
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Padding = new Padding(10);
            this.Load += CategoryForm_Load;
            this.FormClosing += CategoryForm_FormClosing;
            
            // Components initialization
            panelHeader = new Panel();
            lblHeader = new Label();
            tableLayoutPanel = new TableLayoutPanel();
            lblCategoryName = new Label();
            txtCategoryName = new TextBox();
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
            lblHeader.Text = "Category Details";
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            
            // Table Layout Panel
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel.Padding = new Padding(20);
            
            // Category Name Label
            lblCategoryName.AutoSize = true;
            lblCategoryName.Text = "Category Name:";
            lblCategoryName.Dock = DockStyle.Fill;
            lblCategoryName.TextAlign = ContentAlignment.MiddleRight;
            lblCategoryName.Font = new Font("Segoe UI", 10F);
            
            // Category Name TextBox
            txtCategoryName.Dock = DockStyle.Fill;
            txtCategoryName.Font = new Font("Segoe UI", 10F);
            
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
            
            // Is Active CheckBox
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            chkIsActive.Text = "Active";
            chkIsActive.Dock = DockStyle.Fill;
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
            tableLayoutPanel.Controls.Add(lblCategoryName, 0, 0);
            tableLayoutPanel.Controls.Add(txtCategoryName, 1, 0);
            tableLayoutPanel.Controls.Add(lblDescription, 0, 1);
            tableLayoutPanel.Controls.Add(txtDescription, 1, 1);
            tableLayoutPanel.Controls.Add(chkIsActive, 0, 2);
            
            // Panel for buttons with FlowLayoutPanel
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Padding = new Padding(0, 5, 0, 0);
            
            tableLayoutPanel.Controls.Add(buttonPanel, 0, 3);
            tableLayoutPanel.SetColumnSpan(buttonPanel, 2);
            
            // Add all to form
            this.Controls.Add(tableLayoutPanel);
            this.Controls.Add(panelHeader);
        }
        
        #endregion
    }
}