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

            // CATEGORY SECTION
            Label lblCategory = new Label
            {
                Text = "Product Categories",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(12, 9),
                AutoSize = true
            };

            dataGridViewCategories = new DataGridView
            {
                Location = new Point(12, 40),
                Size = new Size(500, 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dataGridViewCategories.SelectionChanged += DataGridViewCategories_SelectionChanged;

            Label lblCategoryName = new Label
            {
                Text = "Category Name:",
                Location = new Point(530, 40),
                Size = new Size(100, 23)
            };

            txtCategoryName = new TextBox
            {
                Location = new Point(640, 40),
                Size = new Size(150, 23)
            };

            Label lblCategoryDesc = new Label
            {
                Text = "Description:",
                Location = new Point(530, 70),
                Size = new Size(100, 23)
            };

            txtCategoryDescription = new TextBox
            {
                Location = new Point(640, 70),
                Size = new Size(150, 23)
            };

            btnAddCategory = new Button
            {
                Text = "Add",
                Location = new Point(530, 110),
                Size = new Size(80, 30)
            };
            btnAddCategory.Click += BtnAddCategory_Click;

            btnUpdateCategory = new Button
            {
                Text = "Update",
                Location = new Point(620, 110),
                Size = new Size(80, 30)
            };
            btnUpdateCategory.Click += BtnUpdateCategory_Click;

            btnDeleteCategory = new Button
            {
                Text = "Delete",
                Location = new Point(710, 110),
                Size = new Size(80, 30)
            };
            btnDeleteCategory.Click += BtnDeleteCategory_Click;

            // INVENTORY SECTION
            this.lblTitle = new Label();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.Location = new Point(12, 220);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(209, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Inventory Management";

            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.dataGridViewLowStock = new DataGridView();
            this.lblLowStockTitle = new Label();
            this.txtProductId = new TextBox();
            this.txtStockQuantity = new TextBox();
            this.btnUpdateStock = new Button();
            this.TxtSearchProductId = new TextBox();
            this.btnSearch = new Button();
            this.btnTransferStock = new Button();
            this.btnViewAllWarehouses = new Button();


            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(new Label() { Text = "Product ID:", AutoSize = true, Location = new Point(10, 15) });
            this.panel1.Controls.Add(new Label() { Text = "New Quantity:", AutoSize = true, Location = new Point(10, 45) });
            this.panel1.Controls.Add(this.txtProductId);
            this.panel1.Controls.Add(this.txtStockQuantity);
            this.panel1.Controls.Add(this.btnUpdateStock);
            this.panel1.Controls.Add(this.btnTransferStock);
            this.panel1.Location = new Point(12, 270);
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

            this.btnViewAllWarehouses.Location = new Point(350, 300);
            this.btnViewAllWarehouses.Name = "btnViewAllWarehouses";
            this.btnViewAllWarehouses.Size = new Size(130, 30);
            this.btnViewAllWarehouses.TabIndex = 7;
            this.btnViewAllWarehouses.Text = "View All Products";
            this.btnViewAllWarehouses.UseVisualStyleBackColor = true;
            //this.btnViewAllWarehouses.Click += new EventHandler(this.btnViewAllWarehouses_Click);

            this.lblLowStockTitle.AutoSize = true;
            this.lblLowStockTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblLowStockTitle.Location = new Point(12, 400);
            this.lblLowStockTitle.Name = "lblLowStockTitle";
            this.lblLowStockTitle.Size = new Size(165, 21);
            this.lblLowStockTitle.TabIndex = 10;
            this.lblLowStockTitle.Text = "Low Stock Products";

            this.TxtSearchProductId.Location = new Point(592, 400);
            this.TxtSearchProductId.Name = "TxtSearchProductId";
            this.TxtSearchProductId.Size = new Size(100, 23);
            this.TxtSearchProductId.TabIndex = 11;
            this.TxtSearchProductId.PlaceholderText = "Search by ID...";
            this.TxtSearchProductId.TextChanged += new EventHandler(this.TxtSearchProductId_TextChanged);

            this.btnSearch.Location = new Point(698, 400);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(90, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;

            this.dataGridViewLowStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLowStock.Location = new Point(12, 450);
            this.dataGridViewLowStock.Name = "dataGridViewLowStock";
            this.dataGridViewLowStock.Size = new Size(776, 177);
            this.dataGridViewLowStock.TabIndex = 13;

            this.Controls.Add(lblCategory);
            this.Controls.Add(this.dataGridViewCategories);
            this.Controls.Add(lblCategoryName);
            this.Controls.Add(this.txtCategoryName);
            this.Controls.Add(lblCategoryDesc);
            this.Controls.Add(this.txtCategoryDescription);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.btnUpdateCategory);
            this.Controls.Add(this.btnDeleteCategory);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnViewAllWarehouses);
            this.Controls.Add(this.lblLowStockTitle);
            this.Controls.Add(this.TxtSearchProductId);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dataGridViewLowStock);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UcInventoryManagement";
            this.Size = new Size(800, 740);
        }

        public Label lblTitle;
        public Label lblLowStockTitle;
        public Panel panel1;
        public Panel panel2;
        public DataGridView dataGridViewLowStock;
        public TextBox txtProductId;
        public TextBox txtStockQuantity;
        public Button btnUpdateStock;
        public Button btnTransferStock;
        public Button btnViewAllWarehouses;
        public TextBox TxtSearchProductId;
        public Button btnSearch;

        private DataGridView dataGridViewCategories;
        private TextBox txtCategoryName;
        private TextBox txtCategoryDescription;
        private Button btnAddCategory;
        private Button btnUpdateCategory;
        private Button btnDeleteCategory;
    }
}
