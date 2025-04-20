using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading.Tasks;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class StockUpdateForm : Form
    {
        private Product _product;
        private int _currentStock;
        private bool _isProcessing = false;
        
        public StockUpdateForm(Product product)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
            _currentStock = product.Stock;
            
            InitializeComponent();
            LoadProductDetails();
        }

        private void InitializeComponent()
        {
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(420, 370);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Text = "Update Stock";
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Setup panels
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(0, 123, 255)
            };
            
            Label lblHeader = new Label
            {
                Text = "Stock Update",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlHeader.Controls.Add(lblHeader);
            
            Panel pnlMain = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(420, 270),
                Padding = new Padding(20)
            };
            
            Panel pnlActions = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            
            // Product details fields
            Label lblProductName = new Label
            {
                Text = "Tên sản phẩm:",
                Location = new Point(20, 20),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Label txtProductName = new Label
            {
                Location = new Point(150, 20),
                Size = new Size(250, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            Label lblCurrentStock = new Label
            {
                Text = "Tồn kho hiện tại:",
                Location = new Point(20, 60),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Label txtCurrentStock = new Label
            {
                Location = new Point(150, 60),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            Label lblCategory = new Label
            {
                Text = "Danh mục:",
                Location = new Point(20, 100),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Label txtCategory = new Label
            {
                Location = new Point(150, 100),
                Size = new Size(250, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // New stock input
            Label lblNewStock = new Label
            {
                Text = "Số lượng mới:",
                Location = new Point(20, 150),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            NumericUpDown numNewStock = new NumericUpDown
            {
                Location = new Point(150, 150),
                Size = new Size(100, 25),
                Minimum = 0,
                Maximum = 9999,
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.DarkBlue
            };
            
            // Making increment/decrement buttons more visible and usable
            numNewStock.Increment = 1;
            numNewStock.DecimalPlaces = 0;
            
            // Stock adjustment buttons for quick changes
            Button btnMinus10 = new Button
            {
                Text = "-10",
                Location = new Point(150, 185),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(220, 53, 69), // Red
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
            btnMinus10.FlatAppearance.BorderSize = 0;
            
            Button btnMinus5 = new Button
            {
                Text = "-5",
                Location = new Point(195, 185),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(255, 193, 7), // Yellow
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
            btnMinus5.FlatAppearance.BorderSize = 0;
            
            Button btnPlus5 = new Button
            {
                Text = "+5",
                Location = new Point(240, 185),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(255, 193, 7), // Yellow
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
            btnPlus5.FlatAppearance.BorderSize = 0;
            
            Button btnPlus10 = new Button
            {
                Text = "+10",
                Location = new Point(285, 185),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(40, 167, 69), // Green
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
            btnPlus10.FlatAppearance.BorderSize = 0;
            
            // Note field
            Label lblNote = new Label
            {
                Text = "Ghi chú:",
                Location = new Point(20, 225),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtNote = new TextBox
            {
                Location = new Point(150, 225),
                Size = new Size(250, 25),
                Multiline = true
            };

            // Action buttons
            Button btnUpdate = new Button
            {
                Text = "Cập nhật",
                Location = new Point(220, 10),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            
            Button btnCancel = new Button
            {
                Text = "Hủy",
                DialogResult = DialogResult.Cancel,
                Location = new Point(320, 10),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            
            // Status label
            Label lblStatus = new Label
            {
                Location = new Point(20, 270),
                Size = new Size(380, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Green,
                Visible = false
            };
            
            // Progress indicator
            ProgressBar progressBar = new ProgressBar
            {
                Location = new Point(150, 270),
                Size = new Size(250, 10),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };
            
            // Add controls to panels
            pnlMain.Controls.Add(lblProductName);
            pnlMain.Controls.Add(txtProductName);
            pnlMain.Controls.Add(lblCurrentStock);
            pnlMain.Controls.Add(txtCurrentStock);
            pnlMain.Controls.Add(lblCategory);
            pnlMain.Controls.Add(txtCategory);
            pnlMain.Controls.Add(lblNewStock);
            pnlMain.Controls.Add(numNewStock);
            pnlMain.Controls.Add(btnMinus10);
            pnlMain.Controls.Add(btnMinus5);
            pnlMain.Controls.Add(btnPlus5);
            pnlMain.Controls.Add(btnPlus10);
            pnlMain.Controls.Add(lblNote);
            pnlMain.Controls.Add(txtNote);
            pnlMain.Controls.Add(lblStatus);
            pnlMain.Controls.Add(progressBar);
            
            pnlActions.Controls.Add(btnUpdate);
            pnlActions.Controls.Add(btnCancel);
            
            // Add panels to form
            this.Controls.Add(pnlActions);
            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlHeader);
            
            // Add event handlers
            btnUpdate.Click += BtnUpdate_Click;
            btnCancel.Click += BtnCancel_Click;
            btnMinus10.Click += (s, e) => AdjustStock(numNewStock, -10);
            btnMinus5.Click += (s, e) => AdjustStock(numNewStock, -5);
            btnPlus5.Click += (s, e) => AdjustStock(numNewStock, 5);
            btnPlus10.Click += (s, e) => AdjustStock(numNewStock, 10);
            
            this.AcceptButton = btnUpdate;
            this.CancelButton = btnCancel;
            
            // Store references as fields
            this.txtProductName = txtProductName;
            this.txtCurrentStock = txtCurrentStock;
            this.txtCategory = txtCategory;
            this.numNewStock = numNewStock;
            this.txtNote = txtNote;
            this.lblStatus = lblStatus;
            this.progressBar = progressBar;
            
            this.ResumeLayout(false);
        }
        
        // Form controls as fields
        private Label txtProductName;
        private Label txtCurrentStock;
        private Label txtCategory;
        private NumericUpDown numNewStock;
        private TextBox txtNote;
        private Label lblStatus;
        private ProgressBar progressBar;
        
        private void LoadProductDetails()
        {
            txtProductName.Text = _product.ProductName;
            txtCurrentStock.Text = _product.Stock.ToString();
            txtCategory.Text = _product.CategoryName;
            numNewStock.Value = _product.Stock;
        }
        
        private void AdjustStock(NumericUpDown control, int amount)
        {
            decimal newValue = control.Value + amount;
            if (newValue < control.Minimum)
                newValue = control.Minimum;
            else if (newValue > control.Maximum)
                newValue = control.Maximum;
                
            control.Value = newValue;
        }
        
        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            // Check if we're already processing a request
            if (_isProcessing)
                return;
            
            // Get the new stock quantity
            int newStock = (int)numNewStock.Value;
            
            // No change, no need to update
            if (newStock == _currentStock)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            
            _isProcessing = true;
            
            // Show progress indicators
            progressBar.Value = 0;
            progressBar.Visible = true;
            lblStatus.Text = "Đang cập nhật số lượng...";
            lblStatus.ForeColor = Color.Blue;
            lblStatus.Visible = true;
            
            try
            {
                // Disable the update button during processing
                ((Button)sender).Enabled = false;
                
                // Use the async method to avoid blocking the UI
                await InventoryManager.UpdateStockAsync(_product.ProductID, newStock);
                
                // Update the form's current values
                _currentStock = newStock;
                txtCurrentStock.Text = newStock.ToString();
                
                // Also update the product object in case it's used later
                _product.Stock = newStock;
                
                // Show success message
                lblStatus.Text = $"Đã cập nhật số lượng thành công!";
                lblStatus.ForeColor = Color.Green;
                
                // Close form after a brief delay
                await Task.Delay(1000);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                // Show error message
                lblStatus.Text = $"Lỗi: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                ((Button)sender).Enabled = true; // Re-enable the button
            }
            finally
            {
                progressBar.Visible = false;
                _isProcessing = false;
            }
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        // Make corners rounded
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Create rounded corners
            GraphicsPath path = new GraphicsPath();
            int radius = 15;
            
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddLine(radius, 0, this.Width - radius, 0);
            path.AddArc(this.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
            path.AddLine(this.Width, radius, this.Width, this.Height - radius);
            path.AddArc(this.Width - radius * 2, this.Height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddLine(this.Width - radius, this.Height, radius, this.Height);
            path.AddArc(0, this.Height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            
            this.Region = new Region(path);
        }
    }
}