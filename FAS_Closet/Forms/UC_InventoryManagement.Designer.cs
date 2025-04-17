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

            btnAdd = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                Text = "Thêm",
                Location = new Point(530, 110),
                Size = new Size(80, 30)
            };
            btnAdd.Click += BtnAddCategory_Click;

            btnEdit = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                Text = "Sửa",
                Location = new Point(620, 110),
                Size = new Size(80, 30)
            };
            btnEdit.Click += BtnUpdateCategory_Click;

            btnDelete = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                Text = "Xóa",
                Location = new Point(710, 110),
                Size = new Size(80, 30)
            };
            btnDelete.Click += BtnDeleteCategory_Click;

            btnRefresh = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                Text = "Làm mới",
                Location = new Point(800, 110),
                Size = new Size(80, 30)
            };

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
            this.txtProductId = new TextBox();
            this.txtStockQuantity = new TextBox();
            this.btnUpdateStock = new Button();
            this.btnTransferStock = new Button();
            this.btnViewProductsByCategory = new Button();

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

            this.btnViewProductsByCategory.Location = new Point(350, 300);
            this.btnViewProductsByCategory.Name = "btnViewProductsByCategory";
            this.btnViewProductsByCategory.Size = new Size(130, 30);
            this.btnViewProductsByCategory.TabIndex = 7;
            this.btnViewProductsByCategory.Text = "View Products";
            this.btnViewProductsByCategory.UseVisualStyleBackColor = true;
            this.btnViewProductsByCategory.Click += new EventHandler(this.btnViewProductsByCategory_Click);

            this.Controls.Add(lblCategory);
            this.Controls.Add(this.dataGridViewCategories);
            this.Controls.Add(lblCategoryName);
            this.Controls.Add(this.txtCategoryName);
            this.Controls.Add(lblCategoryDesc);
            this.Controls.Add(this.txtCategoryDescription);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnViewProductsByCategory);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UcInventoryManagement";
            this.Size = new Size(800, 740);
        }

        public Label lblTitle;
        public Panel panel1;
        public Panel panel2;
        public DataGridView dataGridViewLowStock;
        public TextBox txtProductId;
        public TextBox txtStockQuantity;
        public Button btnUpdateStock;
        public Button btnTransferStock;
        public Button btnViewProductsByCategory;

        private DataGridView dataGridViewCategories;
        private TextBox txtCategoryName;
        private TextBox txtCategoryDescription;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
    }
}
