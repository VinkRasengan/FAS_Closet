using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
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
            this.SuspendLayout();
            
            // Form settings
            this.Text = "Cập nhật số lượng";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.Size = new Size(450, 400);
            this.Padding = new Padding(15);
            this.Font = new Font("Segoe UI", 9.75F);
            
            // Header panel
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(0, 123, 255),
                Height = 60
            };
            
            Label lblHeader = new Label
            {
                Text = "Cập nhật số lượng kho",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Dock = DockStyle.Fill
            };
            
            pnlHeader.Controls.Add(lblHeader);
            
            // Main panel
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            
            // Product info
            Label lblProductName = new Label
            {
                Text = "Sản phẩm:",
                Location = new Point(20, 20),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Label txtProductName = new Label
            {
                Location = new Point(150, 20),
                Size = new Size(250, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9.75F, FontStyle.Bold),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            Label lblCurrentStock = new Label
            {
                Text = "Số lượng hiện tại:",
                Location = new Point(20, 60),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Label txtCurrentStock = new Label
            {
                Location = new Point(150, 60),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.75F, FontStyle.Bold),
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
            
            Label lblNote = new Label
            {
                Text = "Ghi chú:",
                Location = new Point(20, 230),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtNote = new TextBox
            {
                Location = new Point(150, 230),
                Size = new Size(250, 50),
                Multiline = true
            };
            
            Panel pnlActions = new Panel
            {
                Location = new Point(0, 300),
                Size = new Size(450, 60),
                Dock = DockStyle.Bottom
            };
            
            Button btnUpdate = new Button
            {
                Text = "Cập nhật",
                Location = new Point(150, 15),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                DialogResult = DialogResult.None
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            
            Button btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(280, 15),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            
            // Status label for displaying messages
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
            if (_isProcessing)
                return;
                
            int newStock = (int)numNewStock.Value;
            
            // Confirm if this is a significant change
            if (Math.Abs(newStock - _currentStock) > 20)
            {
                var result = MessageBox.Show(
                    $"Bạn đang thay đổi số lượng từ {_currentStock} thành {newStock}.\nThay đổi này khá lớn, bạn có chắc chắn không?",
                    "Xác nhận thay đổi lớn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                    
                if (result != DialogResult.Yes)
                    return;
            }
            
            try
            {
                _isProcessing = true;
                
                // Show progress indicator
                progressBar.Visible = true;
                lblStatus.Text = "Đang cập nhật số lượng...";
                lblStatus.ForeColor = Color.Blue;
                lblStatus.Visible = true;
                
                // Use BackgroundWorker for better UI responsiveness
                var worker = new BackgroundWorker();
                worker.DoWork += (s, args) =>
                {
                    try
                    {
                        // Update stock in database
                        InventoryManager.UpdateStock(_product.ProductID, newStock);
                        
                        // Optional: Log the stock change with note
                        if (!string.IsNullOrWhiteSpace(txtNote.Text))
                        {
                            // You could implement a method to log inventory changes
                            // InventoryManager.LogInventoryChange(_product.ProductID, _currentStock, newStock, txtNote.Text);
                        }
                        
                        args.Result = true;
                    }
                    catch (Exception ex)
                    {
                        args.Result = ex;
                    }
                };
                
                worker.RunWorkerCompleted += (s, args) =>
                {
                    progressBar.Visible = false;
                    _isProcessing = false;
                    
                    if (args.Result is Exception ex)
                    {
                        lblStatus.Text = $"Lỗi: {ex.Message}";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblStatus.Text = $"Đã cập nhật số lượng thành công!";
                        lblStatus.ForeColor = Color.Green;
                        
                        // Update the form's current values
                        _currentStock = newStock;
                        txtCurrentStock.Text = newStock.ToString();
                        
                        // Also update the product object in case it's used later
                        _product.Stock = newStock;
                        
                        // Close form after a brief delay
                        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                        timer.Interval = 1500;
                        timer.Tick += (sender, e) => 
                        {
                            timer.Stop();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        };
                        timer.Start();
                    }
                    
                    lblStatus.Visible = true;
                };
                
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                _isProcessing = false;
                progressBar.Visible = false;
                lblStatus.Text = $"Lỗi: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                lblStatus.Visible = true;
            }
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        // Make corners rounded
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Remove when applied to real buttons
            ApplyRoundedCorners(e.Graphics);
        }
        
        private void ApplyRoundedCorners(Graphics g)
        {
            int radius = 10;
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath path = CreateRoundedRectangle(rect, radius))
            {
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    this.Region = new Region(path);
                    g.DrawPath(pen, path);
                }
            }
        }
        
        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            
            // Top left corner
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            
            // Top edge and top right corner
            path.AddArc(rect.Width - diameter, rect.Y, diameter, diameter, 270, 90);
            
            // Right edge and bottom right corner
            path.AddArc(rect.Width - diameter, rect.Height - diameter, diameter, diameter, 0, 90);
            
            // Bottom edge and bottom left corner
            path.AddArc(rect.X, rect.Height - diameter, diameter, diameter, 90, 90);
            
            path.CloseAllFigures();
            return path;
        }
    }
}