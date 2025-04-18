using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace FASCloset.Forms
{
    partial class ManufacturerForm
    {
        private System.ComponentModel.IContainer components = null;
        
        private Panel panelHeader;
        private Label lblHeader;
        private TableLayoutPanel tableLayoutPanel;
        private Label lblManufacturerName;
        private TextBox txtManufacturerName;
        private Label lblDescription;
        private TextBox txtDescription;
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
            
            // Components initialization
            panelHeader = new Panel();
            lblHeader = new Label();
            tableLayoutPanel = new TableLayoutPanel();
            lblManufacturerName = new Label();
            txtManufacturerName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
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
            lblHeader.Text = "Manufacturer Details";
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            
            // Table Layout Panel
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel.Padding = new Padding(20);
            
            // Manufacturer Name Label
            lblManufacturerName.AutoSize = true;
            lblManufacturerName.Text = "Manufacturer Name:";
            lblManufacturerName.Dock = DockStyle.Fill;
            lblManufacturerName.TextAlign = ContentAlignment.MiddleRight;
            lblManufacturerName.Font = new Font("Segoe UI", 10F);
            
            // Manufacturer Name TextBox
            txtManufacturerName.Dock = DockStyle.Fill;
            txtManufacturerName.Font = new Font("Segoe UI", 10F);
            
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
            tableLayoutPanel.Controls.Add(lblManufacturerName, 0, 0);
            tableLayoutPanel.Controls.Add(txtManufacturerName, 1, 0);
            tableLayoutPanel.Controls.Add(lblDescription, 0, 1);
            tableLayoutPanel.Controls.Add(txtDescription, 1, 1);
            
            // Panel for buttons with FlowLayoutPanel
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Padding = new Padding(0, 5, 0, 0);
            
            tableLayoutPanel.Controls.Add(buttonPanel, 0, 2);
            tableLayoutPanel.SetColumnSpan(buttonPanel, 2);
            
            // Add all to form
            this.Controls.Add(tableLayoutPanel);
            this.Controls.Add(panelHeader);
        }
        
        #endregion
    }
}