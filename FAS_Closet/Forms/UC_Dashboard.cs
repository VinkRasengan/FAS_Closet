using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Extensions; // Add this to use StringExtensions

namespace FASCloset.Forms
{
    public partial class UcDashboard : UserControl
    {
        public UcDashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        public void LoadDashboardData()
        {
            try
            {
                // Load best selling products with improved display
                var bestSellingProducts = ReportManager.GetBestSellingProducts(null, null);
                
                // Configure columns before setting data source
                dgvBestSellers.AutoGenerateColumns = false;
                dgvBestSellers.Columns.Clear();
                
                // Add custom columns with better headers and styling
                dgvBestSellers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ProductName",
                    HeaderText = "Sản Phẩm",
                    Width = 180
                });
                
                dgvBestSellers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CategoryName",
                    HeaderText = "Danh Mục",
                    Width = 120
                });
                
                dgvBestSellers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TotalQuantity",
                    HeaderText = "Số Lượng Đã Bán",
                    Width = 110,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });
                
                dgvBestSellers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TotalRevenue",
                    HeaderText = "Doanh Thu",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });
                
                // Set the data source
                dgvBestSellers.DataSource = bestSellingProducts;
                
                // Apply modern styling
                dgvBestSellers.BorderStyle = BorderStyle.None;
                dgvBestSellers.BackgroundColor = Color.White;
                dgvBestSellers.GridColor = Color.FromArgb(230, 230, 230);
                dgvBestSellers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(93, 64, 150);
                dgvBestSellers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvBestSellers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
                dgvBestSellers.ColumnHeadersHeight = 40;
                dgvBestSellers.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
                dgvBestSellers.RowTemplate.Height = 35;
                dgvBestSellers.RowsDefaultCellStyle.BackColor = Color.White;
                dgvBestSellers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252);
                dgvBestSellers.RowHeadersVisible = false;

                // Load metrics with enhanced styling
                int totalProducts = ProductManager.GetProducts().Count;
                int totalCustomers = CustomerManager.GetCustomers().Count;
                int totalOrders = OrderManager.GetOrders().Count;
                
                lblTotalProducts.Text = totalProducts.ToString("N0");
                lblTotalCustomers.Text = totalCustomers.ToString("N0");
                lblTotalOrders.Text = totalOrders.ToString("N0");
                
                // Style metrics labels
                ApplyMetricStyle(lblTotalProducts, "Sản Phẩm", Color.FromArgb(0, 123, 255));
                ApplyMetricStyle(lblTotalCustomers, "Khách Hàng", Color.FromArgb(40, 167, 69));
                ApplyMetricStyle(lblTotalOrders, "Đơn Hàng", Color.FromArgb(255, 193, 7));

                // Get low stock products
                var lowStockItems = InventoryManager.GetLowStockProducts();
                
                // Update low stock warning with enhanced styling
                int lowStockCount = lowStockItems.Count;
                lblLowStockWarning.Text = lowStockCount.ToString("N0");
                
                // Set low stock warning color based on severity
                Color warningColor;
                if (lowStockCount > 5)
                {
                    warningColor = Color.Red;
                    lblLowStockWarning.ForeColor = warningColor;
                }
                else if (lowStockCount > 0)
                {
                    warningColor = Color.OrangeRed;
                    lblLowStockWarning.ForeColor = warningColor;
                }
                else
                {
                    warningColor = Color.Green;
                    lblLowStockWarning.ForeColor = warningColor;
                    lblLowStockWarning.Text = "0";
                }
                
                // Apply special style to low stock warning
                ApplyMetricStyle(lblLowStockWarning, "Sắp Hết Hàng", warningColor);
                
                // Update Low Stock Alert Panel
                UpdateLowStockAlertPanel(lowStockItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}");
            }
        }

        // Helper method to apply consistent metric styling
        private void ApplyMetricStyle(Label valueLabel, string metricName, Color accentColor)
        {
            // Apply style to the value label
            valueLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            valueLabel.ForeColor = accentColor;
            
            // Find the title label based on naming convention and apply styling
            string titleLabelName = $"lbl{metricName}Title";
            var labelControls = this.Controls.OfType<Label>();
            
            foreach (var label in labelControls)
            {
                if (label.Name.Contains(metricName) && label != valueLabel)
                {
                    label.ForeColor = Color.FromArgb(80, 80, 80);
                    label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    break;
                }
            }
        }

        private void UpdateLowStockAlertPanel(List<Product> lowStockItems)
        {
            // Clear existing panel if it exists
            Panel existingPanel = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "lowStockAlertPanel");
            if (existingPanel != null)
            {
                this.Controls.Remove(existingPanel);
            }
            
            // If no low stock items, don't create the panel
            if (lowStockItems == null || lowStockItems.Count == 0)
            {
                return;
            }
            
            // Create a new panel for low stock alerts - adjust position to not overlap
            Panel lowStockPanel = new Panel
            {
                Name = "lowStockAlertPanel",
                Size = new Size(300, 250),
                Location = new Point(650, 70),  // Positioned higher up to avoid overlap
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(255, 248, 230),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            
            // Add title to the panel
            Label title = new Label
            {
                Text = "⚠️ Sản Phẩm Sắp Hết Hàng",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(204, 102, 0),
                Location = new Point(10, 10),
                Size = new Size(280, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lowStockPanel.Controls.Add(title);
            
            // Add a line separator
            Panel separator = new Panel
            {
                Location = new Point(10, 40),
                Size = new Size(280, 1),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            lowStockPanel.Controls.Add(separator);
            
            // Create column headers
            Label productNameHeader = new Label
            {
                Text = "Tên Sản Phẩm",
                Location = new Point(10, 45),
                Size = new Size(180, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 102, 102)
            };
            
            Label stockHeader = new Label
            {
                Text = "SL",
                Location = new Point(215, 45),
                Size = new Size(30, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 102, 102),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Label priceHeader = new Label
            {
                Text = "Giá",
                Location = new Point(245, 45),
                Size = new Size(45, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 102, 102),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            lowStockPanel.Controls.Add(productNameHeader);
            lowStockPanel.Controls.Add(stockHeader);
            lowStockPanel.Controls.Add(priceHeader);
            
            // Add second separator below headers
            Panel headerSeparator = new Panel
            {
                Location = new Point(10, 68),
                Size = new Size(280, 1),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            lowStockPanel.Controls.Add(headerSeparator);
            
            // Create low stock items list - Debug check
            Console.WriteLine($"Processing {lowStockItems.Count} low stock items");
            foreach (var item in lowStockItems)
            {
                Console.WriteLine($"Product: {item.ProductID} - {item.ProductName} - Stock: {item.Stock} - Price: {item.Price}");
            }
            
            int yPos = 70;
            int itemHeight = 32;
            int maxItems = Math.Min(lowStockItems.Count, 5); // Show max 5 items to leave room for price
            
            for (int i = 0; i < maxItems; i++)
            {
                var product = lowStockItems[i];
                
                Console.WriteLine($"Adding product to panel: {product.ProductName}");
                
                // Create container panel for item
                Panel itemPanel = new Panel
                {
                    Location = new Point(10, yPos),
                    Size = new Size(280, itemHeight),
                    BackColor = Color.Transparent
                };
                
                // Product name
                Label nameLabel = new Label
                {
                    Text = product.ProductName?.Truncate(22) ?? "Unknown Product",
                    Location = new Point(5, 7),
                    Size = new Size(180, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = product.Stock == 0 ? Color.Red : Color.FromArgb(68, 68, 68)
                };
                
                // Current stock
                Label stockLabel = new Label
                {
                    Text = product.Stock.ToString(),
                    Location = new Point(215, 7),
                    Size = new Size(30, 20),
                    Font = new Font("Segoe UI", 9, product.Stock == 0 ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = product.Stock == 0 ? Color.Red : Color.FromArgb(102, 102, 102),
                    TextAlign = ContentAlignment.MiddleRight
                };
                
                // Price with additional null check
                Label priceLabel = new Label
                {
                    Text = product.Price.ToString("N0"),
                    Location = new Point(245, 7),
                    Size = new Size(45, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(0, 123, 255),
                    TextAlign = ContentAlignment.MiddleRight
                };
                
                itemPanel.Controls.Add(nameLabel);
                itemPanel.Controls.Add(stockLabel);
                itemPanel.Controls.Add(priceLabel);
                
                // Add hover effect
                itemPanel.MouseEnter += (s, e) => {
                    itemPanel.BackColor = Color.FromArgb(245, 245, 245);
                };
                itemPanel.MouseLeave += (s, e) => {
                    itemPanel.BackColor = Color.Transparent;
                };
                
                // Add click handler to navigate to inventory management
                int productId = product.ProductID; // Capture in closure
                itemPanel.Click += (s, e) => {
                    // Request navigation to inventory management
                    if (ParentForm is MainForm mainForm)
                    {
                        mainForm.NavigateToInventoryManagement(productId);
                    }
                };
                itemPanel.Cursor = Cursors.Hand;
                
                lowStockPanel.Controls.Add(itemPanel);
                yPos += itemHeight;
            }
            
            // If there are more items than we can display
            if (lowStockItems.Count > maxItems)
            {
                Label moreLabel = new Label
                {
                    Text = $"+ {lowStockItems.Count - maxItems} sản phẩm khác...",
                    Location = new Point(10, yPos + 5),
                    Size = new Size(280, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                lowStockPanel.Controls.Add(moreLabel);
            }
            
            // Add button to inventory management
            Button manageButton = new Button
            {
                Text = "Quản Lý Kho Hàng",
                Location = new Point(75, 210),
                Size = new Size(150, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 153, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            
            // Round the button corners
            manageButton.FlatAppearance.BorderSize = 0;
            
            // Add click handler
            manageButton.Click += (s, e) => {
                if (ParentForm is MainForm mainForm)
                {
                    mainForm.NavigateToInventoryManagement();
                }
            };
            
            lowStockPanel.Controls.Add(manageButton);
            
            // Add the panel to the form
            this.Controls.Add(lowStockPanel);
            lowStockPanel.BringToFront();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
    }
}
