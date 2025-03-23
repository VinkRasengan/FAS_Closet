using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public class AllWarehousesInventoryForm : Form
    {
        private TabControl warehouseTabControl;
        private List<Warehouse> warehouses;
        private Dictionary<int, DataGridView> warehouseGrids = new Dictionary<int, DataGridView>();
        
        public AllWarehousesInventoryForm()
        {
            InitializeComponent();
            LoadWarehouses();
        }
        
        private void InitializeComponent()
        {
            this.Text = "All Warehouses Inventory";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Main tab control
            warehouseTabControl = new TabControl();
            warehouseTabControl.Dock = DockStyle.Fill;
            
            // Add a "Summary" tab for consolidated view
            TabPage summaryTab = new TabPage("Summary");
            warehouseTabControl.TabPages.Add(summaryTab);
            
            // Add summary grid
            DataGridView summaryGrid = new DataGridView();
            summaryGrid.Dock = DockStyle.Fill;
            summaryGrid.AutoGenerateColumns = false;
            summaryGrid.AllowUserToAddRows = false;
            summaryGrid.ReadOnly = true;
            summaryGrid.AllowUserToOrderColumns = true;
            
            // Add summary columns
            summaryGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductID",
                HeaderText = "ID",
                Width = 60
            });
            
            summaryGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Product Name",
                Width = 200
            });
            
            summaryGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Category",
                Width = 100
            });
            
            summaryGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Price",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
            
            summaryGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Total Stock",
                Width = 80
            });
            
            summaryGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsLowStock",
                HeaderText = "Low Stock",
                Width = 80
            });
            
            summaryTab.Controls.Add(summaryGrid);
            
            // Add tab control to form
            this.Controls.Add(warehouseTabControl);
            
            // Add search panel at the bottom
            Panel searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Bottom;
            searchPanel.Height = 40;
            searchPanel.BackColor = Color.WhiteSmoke;
            
            Label lblSearch = new Label();
            lblSearch.Text = "Search:";
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(10, 12);
            
            TextBox txtSearch = new TextBox();
            txtSearch.Location = new Point(60, 10);
            txtSearch.Width = 200;
            txtSearch.TextChanged += (s, e) => PerformSearch(txtSearch.Text);
            
            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);
            
            // Add refresh button
            Button btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(280, 9);
            btnRefresh.Size = new Size(80, 23);
            btnRefresh.Click += (s, e) => LoadAllInventory();
            
            searchPanel.Controls.Add(btnRefresh);
            
            this.Controls.Add(searchPanel);
            
            // Load summary data
            var allProducts = ProductManager.GetProducts(true);
            summaryGrid.DataSource = allProducts;
        }
        
        private void LoadWarehouses()
        {
            try
            {
                // Get all active warehouses
                warehouses = WarehouseManager.GetWarehouses();
                
                // Add a tab for each warehouse
                foreach (var warehouse in warehouses)
                {
                    // Skip if already added
                    if (warehouseGrids.ContainsKey(warehouse.WarehouseID))
                        continue;
                        
                    TabPage warehouseTab = new TabPage(warehouse.Name);
                    warehouseTabControl.TabPages.Add(warehouseTab);
                    
                    // Add grid for this warehouse
                    DataGridView warehouseGrid = new DataGridView();
                    warehouseGrid.Dock = DockStyle.Fill;
                    warehouseGrid.AutoGenerateColumns = false;
                    warehouseGrid.AllowUserToAddRows = false;
                    warehouseGrid.ReadOnly = true;
                    
                    // Add columns
                    warehouseGrid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ProductID",
                        HeaderText = "ID",
                        Width = 60
                    });
                    
                    warehouseGrid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ProductName",
                        HeaderText = "Product Name",
                        Width = 200
                    });
                    
                    warehouseGrid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "CategoryName",
                        HeaderText = "Category",
                        Width = 100
                    });
                    
                    warehouseGrid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Price",
                        HeaderText = "Price",
                        Width = 80,
                        DefaultCellStyle = new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment = DataGridViewContentAlignment.MiddleRight
                        }
                    });
                    
                    warehouseGrid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "StockQuantity",
                        HeaderText = "Stock",
                        Width = 80
                    });
                    
                    warehouseGrid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "MinimumStockThreshold",
                        HeaderText = "Min Threshold",
                        Width = 100
                    });
                    
                    warehouseGrid.Columns.Add(new DataGridViewCheckBoxColumn
                    {
                        DataPropertyName = "IsLowStock",
                        HeaderText = "Low Stock",
                        Width = 80
                    });
                    
                    warehouseTab.Controls.Add(warehouseGrid);
                    warehouseGrids.Add(warehouse.WarehouseID, warehouseGrid);
                }
                
                // Load inventory for all warehouses
                LoadAllInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading warehouses: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadAllInventory()
        {
            foreach (var warehouse in warehouses)
            {
                if (warehouseGrids.TryGetValue(warehouse.WarehouseID, out DataGridView grid))
                {
                    // Load inventory for this warehouse
                    var inventory = InventoryManager.GetWarehouseInventory(warehouse.WarehouseID);
                    grid.DataSource = inventory;
                }
            }
        }
        
        private void PerformSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadAllInventory();
                return;
            }
            
            // Search in all warehouse tabs
            foreach (var warehouse in warehouses)
            {
                if (warehouseGrids.TryGetValue(warehouse.WarehouseID, out DataGridView grid))
                {
                    var inventory = InventoryManager.GetWarehouseInventory(warehouse.WarehouseID);
                    var filtered = inventory.FindAll(item => 
                        item.Product.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        item.Product.ProductID.ToString() == searchText);
                    grid.DataSource = filtered;
                }
            }
        }
    }
}
