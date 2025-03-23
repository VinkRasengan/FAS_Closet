using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class TransferStockForm : Form
    {
        private Product selectedProduct;
        private List<Warehouse> warehouses;
        
        public TransferStockForm(Product product)
        {
            InitializeComponent();
            selectedProduct = product;
            LoadWarehouses();
            InitializeProductInfo();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Transfer Stock Between Warehouses";
            this.Size = new System.Drawing.Size(500, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Product info panel
            Panel productPanel = new Panel();
            productPanel.Dock = DockStyle.Top;
            productPanel.Height = 80;
            productPanel.Padding = new Padding(10);
            
            Label lblProductTitle = new Label();
            lblProductTitle.Text = "Product Details";
            lblProductTitle.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
            lblProductTitle.Location = new System.Drawing.Point(10, 10);
            lblProductTitle.AutoSize = true;
            
            lblProductInfo = new Label();
            lblProductInfo.Location = new System.Drawing.Point(10, 30);
            lblProductInfo.AutoSize = true;
            lblProductInfo.MaximumSize = new System.Drawing.Size(480, 0);
            
            productPanel.Controls.Add(lblProductTitle);
            productPanel.Controls.Add(lblProductInfo);
            
            // Transfer panel
            Panel transferPanel = new Panel();
            transferPanel.Dock = DockStyle.Fill;
            transferPanel.Padding = new Padding(10);
            
            // From warehouse
            Label lblFromWarehouse = new Label();
            lblFromWarehouse.Text = "From Warehouse:";
            lblFromWarehouse.Location = new System.Drawing.Point(10, 20);
            lblFromWarehouse.AutoSize = true;
            
            cmbFromWarehouse = new ComboBox();
            cmbFromWarehouse.Location = new System.Drawing.Point(130, 20);
            cmbFromWarehouse.Size = new System.Drawing.Size(200, 23);
            cmbFromWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFromWarehouse.SelectedIndexChanged += CmbFromWarehouse_SelectedIndexChanged;
            
            lblCurrentStock = new Label();
            lblCurrentStock.Location = new System.Drawing.Point(130, 50);
            lblCurrentStock.AutoSize = true;
            lblCurrentStock.Text = "Current Stock: 0";
            
            // To warehouse
            Label lblToWarehouse = new Label();
            lblToWarehouse.Text = "To Warehouse:";
            lblToWarehouse.Location = new System.Drawing.Point(10, 80);
            lblToWarehouse.AutoSize = true;
            
            cmbToWarehouse = new ComboBox();
            cmbToWarehouse.Location = new System.Drawing.Point(130, 80);
            cmbToWarehouse.Size = new System.Drawing.Size(200, 23);
            cmbToWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Transfer quantity
            Label lblQuantity = new Label();
            lblQuantity.Text = "Quantity to Transfer:";
            lblQuantity.Location = new System.Drawing.Point(10, 110);
            lblQuantity.AutoSize = true;
            
            numQuantity = new NumericUpDown();
            numQuantity.Location = new System.Drawing.Point(130, 110);
            numQuantity.Size = new System.Drawing.Size(80, 23);
            numQuantity.Minimum = 1;
            numQuantity.Maximum = 1000;
            numQuantity.Value = 1;
            
            // Buttons
            Button btnTransfer = new Button();
            btnTransfer.Text = "Transfer";
            btnTransfer.Size = new System.Drawing.Size(100, 30);
            btnTransfer.Location = new System.Drawing.Point(130, 150);
            btnTransfer.Click += BtnTransfer_Click;
            
            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new System.Drawing.Size(100, 30);
            btnCancel.Location = new System.Drawing.Point(240, 150);
            btnCancel.Click += BtnCancel_Click;
            
            transferPanel.Controls.Add(lblFromWarehouse);
            transferPanel.Controls.Add(cmbFromWarehouse);
            transferPanel.Controls.Add(lblCurrentStock);
            transferPanel.Controls.Add(lblToWarehouse);
            transferPanel.Controls.Add(cmbToWarehouse);
            transferPanel.Controls.Add(lblQuantity);
            transferPanel.Controls.Add(numQuantity);
            transferPanel.Controls.Add(btnTransfer);
            transferPanel.Controls.Add(btnCancel);
            
            // Add panels to form
            this.Controls.Add(productPanel);
            this.Controls.Add(transferPanel);
        }
        
        private Label lblProductInfo;
        private ComboBox cmbFromWarehouse;
        private ComboBox cmbToWarehouse;
        private Label lblCurrentStock;
        private NumericUpDown numQuantity;
        
        private void InitializeProductInfo()
        {
            lblProductInfo.Text = $"ID: {selectedProduct.ProductID}\r\n" +
                                 $"Name: {selectedProduct.ProductName}\r\n" +
                                 $"Category: {selectedProduct.CategoryName}\r\n" +
                                 $"Total Stock: {selectedProduct.Stock}";
        }
        
        private void LoadWarehouses()
        {
            try
            {
                warehouses = WarehouseManager.GetWarehouses();
                
                cmbFromWarehouse.DisplayMember = "Name";
                cmbFromWarehouse.ValueMember = "WarehouseID";
                cmbFromWarehouse.DataSource = new BindingSource { DataSource = warehouses };
                
                List<Warehouse> toWarehouses = new List<Warehouse>(warehouses);
                cmbToWarehouse.DisplayMember = "Name";
                cmbToWarehouse.ValueMember = "WarehouseID";
                cmbToWarehouse.DataSource = new BindingSource { DataSource = toWarehouses };
                
                // Ensure different default selections
                if (cmbToWarehouse.Items.Count > 1)
                    cmbToWarehouse.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading warehouses: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void CmbFromWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFromWarehouse.SelectedItem is Warehouse selectedWarehouse)
            {
                UpdateFromWarehouseStock(selectedWarehouse.WarehouseID);
            }
        }
        
        private void UpdateFromWarehouseStock(int warehouseId)
        {
            try
            {
                var inventoryItem = InventoryManager.GetInventoryByProductAndWarehouse(selectedProduct.ProductID, warehouseId);
                if (inventoryItem != null)
                {
                    lblCurrentStock.Text = $"Current Stock: {inventoryItem.StockQuantity}";
                    numQuantity.Maximum = inventoryItem.StockQuantity;
                    numQuantity.Value = Math.Min(inventoryItem.StockQuantity, numQuantity.Value);
                }
                else
                {
                    lblCurrentStock.Text = "Current Stock: 0";
                    numQuantity.Maximum = 0;
                    numQuantity.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving stock information: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFromWarehouse.SelectedItem == null || cmbToWarehouse.SelectedItem == null)
                {
                    MessageBox.Show("Please select both source and destination warehouses.", "Missing Selection", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                int fromWarehouseId = (int)cmbFromWarehouse.SelectedValue;
                int toWarehouseId = (int)cmbToWarehouse.SelectedValue;
                int quantity = (int)numQuantity.Value;
                
                if (fromWarehouseId == toWarehouseId)
                {
                    MessageBox.Show("Source and destination warehouses must be different.", "Invalid Selection", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (quantity <= 0)
                {
                    MessageBox.Show("Please enter a quantity greater than zero.", "Invalid Quantity", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Confirm transfer
                if (MessageBox.Show($"Are you sure you want to transfer {quantity} units of {selectedProduct.ProductName} " +
                    $"from {((Warehouse)cmbFromWarehouse.SelectedItem).Name} to {((Warehouse)cmbToWarehouse.SelectedItem).Name}?", 
                    "Confirm Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Perform transfer
                    InventoryManager.TransferStock(selectedProduct.ProductID, fromWarehouseId, toWarehouseId, quantity);
                    
                    MessageBox.Show("Stock transferred successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error transferring stock: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
