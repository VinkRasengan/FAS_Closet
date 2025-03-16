namespace FASCloset.Forms
{
    partial class ProductInventoryManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageProducts;
        private System.Windows.Forms.TabPage tabPageInventory;

        // Controls cho Product tab
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnEditProduct;
        private System.Windows.Forms.Button btnDeleteProduct;
        private System.Windows.Forms.Panel panelProductEditor;
        private System.Windows.Forms.Label lblProductID;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSaveProduct;
        private System.Windows.Forms.Button btnCancelProduct;

        // Controls cho Inventory tab
        private System.Windows.Forms.DataGridView dataGridViewInventory;
        private System.Windows.Forms.Button btnUpdateStock;
        private System.Windows.Forms.Button btnSetThreshold;
        private System.Windows.Forms.Button btnCheckLowStock;
        private System.Windows.Forms.Panel panelInventoryEditor;
        private System.Windows.Forms.Label lblInvID;
        private System.Windows.Forms.TextBox txtInvID;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.Button btnSaveInventory;
        private System.Windows.Forms.Button btnCancelInventory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageProducts = new System.Windows.Forms.TabPage();
            this.tabPageInventory = new System.Windows.Forms.TabPage();

            // ----- Product Tab -----
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.btnEditProduct = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.panelProductEditor = new System.Windows.Forms.Panel();
            this.lblProductID = new System.Windows.Forms.Label();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnSaveProduct = new System.Windows.Forms.Button();
            this.btnCancelProduct = new System.Windows.Forms.Button();

            // ----- Inventory Tab -----
            this.dataGridViewInventory = new System.Windows.Forms.DataGridView();
            this.btnUpdateStock = new System.Windows.Forms.Button();
            this.btnSetThreshold = new System.Windows.Forms.Button();
            this.btnCheckLowStock = new System.Windows.Forms.Button();
            this.panelInventoryEditor = new System.Windows.Forms.Panel();
            this.lblInvID = new System.Windows.Forms.Label();
            this.txtInvID = new System.Windows.Forms.TextBox();
            this.lblStock = new System.Windows.Forms.Label();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.btnSaveInventory = new System.Windows.Forms.Button();
            this.btnCancelInventory = new System.Windows.Forms.Button();

            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageProducts);
            this.tabControlMain.Controls.Add(this.tabPageInventory);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(800, 600);
            // 
            // tabPageProducts
            // 
            this.tabPageProducts.Text = "Products";
            this.tabPageProducts.Padding = new System.Windows.Forms.Padding(10);
            // DataGridView for products
            this.dataGridViewProducts.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewProducts.Size = new System.Drawing.Size(760, 300);
            this.dataGridViewProducts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            // Buttons
            this.btnAddProduct.Text = "Add";
            this.btnAddProduct.Location = new System.Drawing.Point(10, 320);
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            this.btnEditProduct.Text = "Edit";
            this.btnEditProduct.Location = new System.Drawing.Point(100, 320);
            this.btnEditProduct.Click += new System.EventHandler(this.btnEditProduct_Click);
            this.btnDeleteProduct.Text = "Delete";
            this.btnDeleteProduct.Location = new System.Drawing.Point(190, 320);
            this.btnDeleteProduct.Click += new System.EventHandler(this.btnDeleteProduct_Click);
            // Panel for product editor
            this.panelProductEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProductEditor.Location = new System.Drawing.Point(10, 360);
            this.panelProductEditor.Size = new System.Drawing.Size(760, 150);
            // Controls inside product editor panel
            this.lblProductID.Text = "Product ID:";
            this.lblProductID.Location = new System.Drawing.Point(10, 10);
            this.txtProductID.Location = new System.Drawing.Point(100, 10);
            this.txtProductID.ReadOnly = true;
            this.lblProductName.Text = "Name:";
            this.lblProductName.Location = new System.Drawing.Point(10, 40);
            this.txtProductName.Location = new System.Drawing.Point(100, 40);
            this.lblPrice.Text = "Price:";
            this.lblPrice.Location = new System.Drawing.Point(10, 70);
            this.txtPrice.Location = new System.Drawing.Point(100, 70);
            this.lblDescription.Text = "Description:";
            this.lblDescription.Location = new System.Drawing.Point(10, 100);
            this.txtDescription.Location = new System.Drawing.Point(100, 100);
            this.txtDescription.Size = new System.Drawing.Size(300, 40);
            this.btnSaveProduct.Text = "Save";
            this.btnSaveProduct.Location = new System.Drawing.Point(420, 40);
            this.btnSaveProduct.Click += new System.EventHandler(this.btnSaveProduct_Click);
            this.btnCancelProduct.Text = "Cancel";
            this.btnCancelProduct.Location = new System.Drawing.Point(420, 80);
            this.btnCancelProduct.Click += new System.EventHandler(this.btnCancelProduct_Click);
            // Add controls to panelProductEditor
            this.panelProductEditor.Controls.Add(this.lblProductID);
            this.panelProductEditor.Controls.Add(this.txtProductID);
            this.panelProductEditor.Controls.Add(this.lblProductName);
            this.panelProductEditor.Controls.Add(this.txtProductName);
            this.panelProductEditor.Controls.Add(this.lblPrice);
            this.panelProductEditor.Controls.Add(this.txtPrice);
            this.panelProductEditor.Controls.Add(this.lblDescription);
            this.panelProductEditor.Controls.Add(this.txtDescription);
            this.panelProductEditor.Controls.Add(this.btnSaveProduct);
            this.panelProductEditor.Controls.Add(this.btnCancelProduct);
            // Add controls to tabPageProducts
            this.tabPageProducts.Controls.Add(this.dataGridViewProducts);
            this.tabPageProducts.Controls.Add(this.btnAddProduct);
            this.tabPageProducts.Controls.Add(this.btnEditProduct);
            this.tabPageProducts.Controls.Add(this.btnDeleteProduct);
            this.tabPageProducts.Controls.Add(this.panelProductEditor);

            // 
            // tabPageInventory
            // 
            this.tabPageInventory.Text = "Inventory";
            this.tabPageInventory.Padding = new System.Windows.Forms.Padding(10);
            // DataGridView for inventory
            this.dataGridViewInventory.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewInventory.Size = new System.Drawing.Size(760, 300);
            // Buttons
            this.btnUpdateStock.Text = "Update Stock";
            this.btnUpdateStock.Location = new System.Drawing.Point(10, 320);
            this.btnUpdateStock.Click += new System.EventHandler(this.btnUpdateStock_Click);
            this.btnSetThreshold.Text = "Set Threshold";
            this.btnSetThreshold.Location = new System.Drawing.Point(120, 320);
            this.btnSetThreshold.Click += new System.EventHandler(this.btnSetThreshold_Click);
            this.btnCheckLowStock.Text = "Check Low Stock";
            this.btnCheckLowStock.Location = new System.Drawing.Point(250, 320);
            this.btnCheckLowStock.Click += new System.EventHandler(this.btnCheckLowStock_Click);
            // Panel for inventory editor
            this.panelInventoryEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInventoryEditor.Location = new System.Drawing.Point(10, 360);
            this.panelInventoryEditor.Size = new System.Drawing.Size(760, 150);
            // Controls inside inventory editor panel
            this.lblInvID.Text = "Inventory ID:";
            this.lblInvID.Location = new System.Drawing.Point(10, 10);
            this.txtInvID.Location = new System.Drawing.Point(100, 10);
            this.txtInvID.ReadOnly = true;
            this.lblStock.Text = "Stock:";
            this.lblStock.Location = new System.Drawing.Point(10, 40);
            this.txtStock.Location = new System.Drawing.Point(100, 40);
            this.lblThreshold.Text = "Threshold:";
            this.lblThreshold.Location = new System.Drawing.Point(10, 70);
            this.txtThreshold.Location = new System.Drawing.Point(100, 70);
            this.btnSaveInventory.Text = "Save";
            this.btnSaveInventory.Location = new System.Drawing.Point(250, 40);
            this.btnSaveInventory.Click += new System.EventHandler(this.btnSaveInventory_Click);
            this.btnCancelInventory.Text = "Cancel";
            this.btnCancelInventory.Location = new System.Drawing.Point(250, 80);
            this.btnCancelInventory.Click += new System.EventHandler(this.btnCancelInventory_Click);
            // Add controls to panelInventoryEditor
            this.panelInventoryEditor.Controls.Add(this.lblInvID);
            this.panelInventoryEditor.Controls.Add(this.txtInvID);
            this.panelInventoryEditor.Controls.Add(this.lblStock);
            this.panelInventoryEditor.Controls.Add(this.txtStock);
            this.panelInventoryEditor.Controls.Add(this.lblThreshold);
            this.panelInventoryEditor.Controls.Add(this.txtThreshold);
            this.panelInventoryEditor.Controls.Add(this.btnSaveInventory);
            this.panelInventoryEditor.Controls.Add(this.btnCancelInventory);
            // Add controls to tabPageInventory
            this.tabPageInventory.Controls.Add(this.dataGridViewInventory);
            this.tabPageInventory.Controls.Add(this.btnUpdateStock);
            this.tabPageInventory.Controls.Add(this.btnSetThreshold);
            this.tabPageInventory.Controls.Add(this.btnCheckLowStock);
            this.tabPageInventory.Controls.Add(this.panelInventoryEditor);
            
            // 
            // ProductInventoryManagementForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControlMain);
            this.Text = "Product and Inventory Management";
        }
    }
}
