namespace FASCloset.Forms
{
    partial class UcInventoryManagement
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
            components = new System.ComponentModel.Container();
            
            // Create main controls
            this.lblTitle = new Label();
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.dataGridViewInventory = new DataGridView();
            this.dataGridViewLowStock = new DataGridView();
            this.lblLowStockTitle = new Label();
            this.txtProductId = new TextBox();
            this.txtStockQuantity = new TextBox();
            this.btnUpdateStock = new Button();
            this.lblCurrentWarehouse = new Label();
            this.lblLowStockCount = new Label();
            this.TxtSearchProductId = new TextBox();
            this.btnSearch = new Button();
            
            // New controls for warehouse functionality
            this.btnTransferStock = new Button();
            this.btnViewAllWarehouses = new Button();
            
            // Setup main control layout
            this.lblTitle = new Label();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.Location = new Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(209, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Inventory Management";
            
            // Panel for updating stock
            this.panel1 = new Panel();
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(new Label() { Text = "Product ID:", AutoSize = true, Location = new Point(10, 15) });
            this.panel1.Controls.Add(new Label() { Text = "New Quantity:", AutoSize = true, Location = new Point(10, 45) });
            this.panel1.Controls.Add(this.txtProductId);
            this.panel1.Controls.Add(this.txtStockQuantity);
            this.panel1.Controls.Add(this.btnUpdateStock);
            this.panel1.Location = new Point(12, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(325, 119);
            this.panel1.TabIndex = 1;
            
            this.txtProductId.Location = new Point(106, 12);
            this.txtProductId.Name = "txtProductId";
            this.txtProductId.Size = new Size(100, 23);
            this.txtProductId.TabIndex = 2;
            
            this.txtStockQuantity.Location = new Point(106, 42);
            this.txtStockQuantity.Name = "txtStockQuantity";
            this.txtStockQuantity.Size = new Size(100, 23);
            this.txtStockQuantity.TabIndex = 3;
            
            this.btnUpdateStock.Location = new Point(106, 71);
            this.btnUpdateStock.Name = "btnUpdateStock";
            this.btnUpdateStock.Size = new Size(100, 30);
            this.btnUpdateStock.TabIndex = 4;
            this.btnUpdateStock.Text = "Update Stock";
            this.btnUpdateStock.UseVisualStyleBackColor = true;
            this.btnUpdateStock.Click += new EventHandler(this.btnUpdateStock_Click);
            
            // Add transfer stock button
            this.btnTransferStock.Location = new Point(212, 71);
            this.btnTransferStock.Name = "btnTransferStock";
            this.btnTransferStock.Size = new Size(100, 30);
            this.btnTransferStock.TabIndex = 5;
            this.btnTransferStock.Text = "Transfer Stock";
            this.btnTransferStock.UseVisualStyleBackColor = true;
            this.btnTransferStock.Click += new EventHandler(this.btnTransferStock_Click);
            this.panel1.Controls.Add(this.btnTransferStock);
            
            // Panel for warehouse selection
            this.lblCurrentWarehouse = new Label();
            this.lblCurrentWarehouse.AutoSize = true;
            this.lblCurrentWarehouse.Location = new Point(350, 46);
            this.lblCurrentWarehouse.Name = "lblCurrentWarehouse";
            this.lblCurrentWarehouse.Size = new Size(179, 15);
            this.lblCurrentWarehouse.TabIndex = 6;
            this.lblCurrentWarehouse.Text = "Current Warehouse: Main Warehouse";
            
            // Button to view all warehouses
            this.btnViewAllWarehouses = new Button();
            this.btnViewAllWarehouses.Location = new Point(350, 71);
            this.btnViewAllWarehouses.Name = "btnViewAllWarehouses";
            this.btnViewAllWarehouses.Size = new Size(130, 30);
            this.btnViewAllWarehouses.TabIndex = 7;
            this.btnViewAllWarehouses.Text = "View All Warehouses";
            this.btnViewAllWarehouses.UseVisualStyleBackColor = true;
            this.btnViewAllWarehouses.Click += new EventHandler(this.btnViewAllWarehouses_Click);
            
            // Low stock count
            this.lblLowStockCount = new Label();
            this.lblLowStockCount.AutoSize = true;
            this.lblLowStockCount.Location = new Point(350, 111);
            this.lblLowStockCount.Name = "lblLowStockCount";
            this.lblLowStockCount.Size = new Size(119, 15);
            this.lblLowStockCount.TabIndex = 8;
            this.lblLowStockCount.Text = "Low Stock Products: 0";
            
            // Setup inventory grid
            this.dataGridViewInventory = new DataGridView();
            this.dataGridViewInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInventory.Location = new Point(12, 171);
            this.dataGridViewInventory.Name = "dataGridViewInventory";
            this.dataGridViewInventory.Size = new Size(776, 200);
            this.dataGridViewInventory.TabIndex = 9;
            
            // Label for low stock
            this.lblLowStockTitle = new Label();
            this.lblLowStockTitle.AutoSize = true;
            this.lblLowStockTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblLowStockTitle.Location = new Point(12, 384);
            this.lblLowStockTitle.Name = "lblLowStockTitle";
            this.lblLowStockTitle.Size = new Size(165, 21);
            this.lblLowStockTitle.TabIndex = 10;
            this.lblLowStockTitle.Text = "Low Stock Products";
            
            // Search control
            this.TxtSearchProductId = new TextBox();
            this.TxtSearchProductId.Location = new Point(592, 384);
            this.TxtSearchProductId.Name = "TxtSearchProductId";
            this.TxtSearchProductId.Size = new Size(100, 23);
            this.TxtSearchProductId.TabIndex = 11;
            this.TxtSearchProductId.PlaceholderText = "Search by ID...";
            this.TxtSearchProductId.TextChanged += new EventHandler(this.TxtSearchProductId_TextChanged);
            
            this.btnSearch = new Button();
            this.btnSearch.Location = new Point(698, 383);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(90, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            
            // Low stock grid
            this.dataGridViewLowStock = new DataGridView();
            this.dataGridViewLowStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLowStock.Location = new Point(12, 411);
            this.dataGridViewLowStock.Name = "dataGridViewLowStock";
            this.dataGridViewLowStock.Size = new Size(776, 177);
            this.dataGridViewLowStock.TabIndex = 13;
            
            // Add all controls to this user control
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblCurrentWarehouse);
            this.Controls.Add(this.btnViewAllWarehouses);
            this.Controls.Add(this.lblLowStockCount);
            this.Controls.Add(this.dataGridViewInventory);
            this.Controls.Add(this.lblLowStockTitle);
            this.Controls.Add(this.TxtSearchProductId);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dataGridViewLowStock);
            
            // Set basic properties
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UcInventoryManagement";
            this.Size = new Size(800, 600);
        }
        
        private void btnViewAllWarehouses_Click(object sender, EventArgs e)
        {
            try
            {
                // Show all warehouse inventory in a new form with tabs
                using (var allWarehouseForm = new AllWarehousesInventoryForm())
                {
                    allWarehouseForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing all warehouses: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Label lblTitle;
        public Label lblLowStockTitle;
        public Label lblCurrentWarehouse;
        public Label lblLowStockCount;
        public Panel panel1;
        public Panel panel2;
        public DataGridView dataGridViewInventory;
        public DataGridView dataGridViewLowStock;
        public TextBox txtProductId;
        public TextBox txtStockQuantity;
        public Button btnUpdateStock;
        public Button btnTransferStock;
        public Button btnViewAllWarehouses;
        public TextBox TxtSearchProductId;
        public Button btnSearch;
    }
}
