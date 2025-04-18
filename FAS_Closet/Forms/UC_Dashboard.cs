using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Extensions; // Add this to use StringExtensions

namespace FASCloset.Forms
{
    public partial class UcDashboard : UserControl
    {
        // Font constants
        private const string DEFAULT_FONT_FAMILY = "Segoe UI";
        
        public UcDashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void LoadVIPCustomers()
        {
            try
            {
                // Ensure the DataGridView is initialized and exists
                if (dgvVIPCustomers == null)
                {
                    MessageBox.Show("DataGridView is not initialized.");
                    return;
                }

                // Clear any existing rows in the DataGridView before populating
                dgvVIPCustomers.Rows.Clear();

                // Example: Get the top customers based on their total spending
                var customers = CustomerManager.GetCustomers(); // Assuming this method fetches all customers
                var customerWithLoyaltyList = new List<CustomerWithLoyalty>();

                // Loop through each customer and calculate their total amount spent, then determine their loyalty points
                foreach (var customer in customers)
                {
                    var orders = OrderManager.GetOrdersByCustomerId(customer.CustomerID);
                    decimal totalSpent = orders.Sum(o => o.TotalAmount);
                    int loyaltyPoints = (int)(totalSpent / 10); // 1 loyalty point for every $10 spent

                    // Create a custom object to hold customer information and their loyalty points
                    var customerWithLoyalty = new CustomerWithLoyalty
                    {
                        Customer = customer,
                        TotalSpent = totalSpent,
                        LoyaltyPoints = loyaltyPoints
                    };

                    // Add the customer data to the list
                    customerWithLoyaltyList.Add(customerWithLoyalty);
                }

                // Sort the list by LoyaltyPoints in descending order and take the top 5
                var topVIPCustomers = customerWithLoyaltyList
                    .OrderByDescending(c => c.LoyaltyPoints)
                    .Take(5)
                    .ToList();

                // Load the data into the DataGridView
                foreach (var customerWithLoyalty in topVIPCustomers)
                {
                    // Add a new row with customer name and loyalty points to the DataGridView
                    dgvVIPCustomers.Rows.Add(customerWithLoyalty.Customer.Name, customerWithLoyalty.LoyaltyPoints);
                }
            }
            catch (Exception ex)
            {
                // Display an error message if an exception occurs
                MessageBox.Show($"Error loading VIP customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Create a new class to hold customer data along with their total spending and loyalty points
        public class CustomerWithLoyalty
        {
            public Customer Customer { get; set; }
            public decimal TotalSpent { get; set; }
            public int LoyaltyPoints { get; set; }
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
                dgvBestSellers.ColumnHeadersDefaultCellStyle.Font = new Font(DEFAULT_FONT_FAMILY, 10F, FontStyle.Bold);
                dgvBestSellers.ColumnHeadersHeight = 40;
                dgvBestSellers.DefaultCellStyle.Font = new Font(DEFAULT_FONT_FAMILY, 9.5F);
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
                LoadVIPCustomers();

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
            valueLabel.Font = new Font(DEFAULT_FONT_FAMILY, 18, FontStyle.Bold);
            valueLabel.ForeColor = accentColor;
            
            // Find the title label based on naming convention and apply styling
            var labelControls = this.Controls.OfType<Label>();
            
            foreach (var label in labelControls)
            {
                if (label.Name.Contains(metricName) && label != valueLabel)
                {
                    label.ForeColor = Color.FromArgb(80, 80, 80);
                    label.Font = new Font(DEFAULT_FONT_FAMILY, 10, FontStyle.Regular);
                    break;
                }
            }
        }

        private void UpdateLowStockAlertPanel(List<Product> lowStockItems)
        {
            // Debug information
            Console.WriteLine($"Updating low stock panel with {lowStockItems?.Count ?? 0} items");
            
            // Check if panel exists and handle empty items list
            if (!ValidateLowStockPanel(lowStockItems)) return;
            
            // Show the panel since we have items to display
            lowStockAlertPanel.Visible = true;
            lowStockAlertPanel.BringToFront();
            
            // Clear any existing product items
            ClearExistingLowStockItems();
            
            // Add low stock product items
            AddLowStockItems(lowStockItems);
            
            // Ensure panel is properly positioned and visible
            ConfigureLowStockPanelLayout();
            
            // Debug information
            Console.WriteLine($"Low stock panel configured with {Math.Min(lowStockItems.Count, 5)} items. Panel visible: {lowStockAlertPanel.Visible}");
        }
        
        private bool ValidateLowStockPanel(List<Product> lowStockItems)
        {
            // Check if panel exists
            if (lowStockAlertPanel == null)
            {
                MessageBox.Show("Low stock alert panel is not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            // If no low stock items, hide the panel
            if (lowStockItems == null || lowStockItems.Count == 0)
            {
                lowStockAlertPanel.Visible = false;
                Console.WriteLine("No low stock items, hiding panel");
                return false;
            }
            
            return true;
        }
        
        private void ClearExistingLowStockItems()
        {
            foreach (Control control in lowStockAlertPanel.Controls.Cast<Control>().ToList())
            {
                if (control is Panel itemPanel && control.Location.Y >= 70 && control.Location.Y < 200)
                {
                    lowStockAlertPanel.Controls.Remove(control);
                    control.Dispose();
                }
                else if (control is Label moreLabel && control.Font.Italic)
                {
                    lowStockAlertPanel.Controls.Remove(control);
                    moreLabel.Dispose();
                }
            }
        }
        
        private void AddLowStockItems(List<Product> lowStockItems)
        {
            int yPos = 70;
            int itemHeight = 32;
            int maxItems = Math.Min(lowStockItems.Count, 5); // Show max 5 items to leave room for price
            
            for (int i = 0; i < maxItems; i++)
            {
                AddSingleLowStockItem(lowStockItems[i], yPos);
                yPos += itemHeight;
            }
            
            // If there are more items than we can display
            if (lowStockItems.Count > maxItems)
            {
                AddMoreItemsLabel(lowStockItems.Count - maxItems, yPos);
            }
        }
        
        private void AddSingleLowStockItem(Product product, int yPosition)
        {
            Console.WriteLine($"Adding product {product.ProductName} to low stock panel");
            
            // Create container panel for item
            Panel itemPanel = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(280, 32),
                BackColor = Color.Transparent
            };
            
            // Product name
            Label nameLabel = new Label
            {
                Text = product.ProductName?.Truncate(22) ?? "Unknown Product",
                Location = new Point(5, 7),
                Size = new Size(180, 20),
                Font = new Font(DEFAULT_FONT_FAMILY, 9),
                ForeColor = product.Stock == 0 ? Color.Red : Color.FromArgb(68, 68, 68)
            };
            
            // Current stock
            Label stockLabel = new Label
            {
                Text = product.Stock.ToString(),
                Location = new Point(215, 7),
                Size = new Size(30, 20),
                Font = new Font(DEFAULT_FONT_FAMILY, 9, product.Stock == 0 ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = product.Stock == 0 ? Color.Red : Color.FromArgb(102, 102, 102),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            // Price with additional null check
            Label priceLabel = new Label
            {
                Text = product.Price.ToString("N0"),
                Location = new Point(245, 7),
                Size = new Size(45, 20),
                Font = new Font(DEFAULT_FONT_FAMILY, 9),
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
            
            lowStockAlertPanel.Controls.Add(itemPanel);
        }
        
        private void AddMoreItemsLabel(int remainingCount, int yPosition)
        {
            Label moreLabel = new Label
            {
                Text = $"+ {remainingCount} sản phẩm khác...",
                Location = new Point(10, yPosition + 5),
                Size = new Size(280, 20),
                Font = new Font(DEFAULT_FONT_FAMILY, 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lowStockAlertPanel.Controls.Add(moreLabel);
        }
        
        private void ConfigureLowStockPanelLayout()
        {
            // Ensure panel position is correctly set
            lowStockAlertPanel.Location = new Point(650, 220);
            lowStockAlertPanel.Size = new Size(300, 250);
            lowStockAlertPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            // Make sure the panel is visible and on top
            lowStockAlertPanel.Visible = true;
            lowStockAlertPanel.BringToFront();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
    }
}
