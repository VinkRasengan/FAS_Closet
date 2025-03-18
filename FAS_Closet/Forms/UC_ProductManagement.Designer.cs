namespace FASCloset.Forms
{
    partial class UcProductManagement
    {
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ProductDisplay = new DataGridView();
            TxtProductName = new TextBox();
            CmbCategory = new ComboBox();
            TxtPrice = new TextBox();
            TxtStock = new TextBox();
            TxtDescription = new TextBox();
            FilterPanel = new Panel();
            AddEditPanel = new Panel();
            tableLayoutPanel = new TableLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            btnAddCategory = new Button();
            RightPanel = new Panel();
            TxtSearch = new TextBox();
            toolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)ProductDisplay).BeginInit();
            AddEditPanel.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            RightPanel.SuspendLayout();
            SuspendLayout();
            // 
            // ProductDisplay
            // 
            ProductDisplay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ProductDisplay.Location = new Point(10, 50);
            ProductDisplay.Name = "ProductDisplay";
            ProductDisplay.Size = new Size(600, 200);
            ProductDisplay.TabIndex = 0;
            // 
            // TxtProductName
            // 
            TxtProductName.Location = new Point(183, 3);
            TxtProductName.Name = "TxtProductName";
            TxtProductName.Size = new Size(200, 23);
            TxtProductName.TabIndex = 1;
            toolTip.SetToolTip(TxtProductName, "Nhập tên sản phẩm");
            // 
            // CmbCategory
            // 
            CmbCategory.FormattingEnabled = true;
            CmbCategory.Location = new Point(183, 33);
            CmbCategory.Name = "CmbCategory";
            CmbCategory.Size = new Size(200, 23);
            CmbCategory.TabIndex = 2;
            toolTip.SetToolTip(CmbCategory, "Chọn danh mục sản phẩm");
            // 
            // TxtPrice
            // 
            TxtPrice.Location = new Point(183, 63);
            TxtPrice.Name = "TxtPrice";
            TxtPrice.Size = new Size(200, 23);
            TxtPrice.TabIndex = 3;
            toolTip.SetToolTip(TxtPrice, "Nhập giá sản phẩm (ví dụ: 100.50)");
            // 
            // TxtStock
            // 
            TxtStock.Location = new Point(183, 93);
            TxtStock.Name = "TxtStock";
            TxtStock.Size = new Size(200, 23);
            TxtStock.TabIndex = 4;
            toolTip.SetToolTip(TxtStock, "Nhập số lượng tồn kho");
            // 
            // TxtDescription
            // 
            TxtDescription.Location = new Point(183, 123);
            TxtDescription.Multiline = true;
            TxtDescription.Name = "TxtDescription";
            TxtDescription.Size = new Size(200, 54);
            TxtDescription.TabIndex = 5;
            toolTip.SetToolTip(TxtDescription, "Nhập mô tả sản phẩm");
            // 
            // FilterPanel
            // 
            FilterPanel.Location = new Point(10, 260);
            FilterPanel.Name = "FilterPanel";
            FilterPanel.Size = new Size(600, 100);
            FilterPanel.TabIndex = 6;
            // 
            // AddEditPanel
            // 
            AddEditPanel.Controls.Add(tableLayoutPanel);
            AddEditPanel.Location = new Point(10, 370);
            AddEditPanel.Name = "AddEditPanel";
            AddEditPanel.Size = new Size(600, 250);
            AddEditPanel.TabIndex = 7;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.Controls.Add(TxtProductName, 1, 0);
            tableLayoutPanel.Controls.Add(CmbCategory, 1, 1);
            tableLayoutPanel.Controls.Add(TxtPrice, 1, 2);
            tableLayoutPanel.Controls.Add(TxtStock, 1, 3);
            tableLayoutPanel.Controls.Add(TxtDescription, 1, 4);
            tableLayoutPanel.Controls.Add(btnSave, 0, 5);
            tableLayoutPanel.Controls.Add(btnAddCategory, 1, 6);
            tableLayoutPanel.Controls.Add(btnCancel, 1, 5);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 7;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel.Size = new Size(600, 250);
            tableLayoutPanel.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(3, 183);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 6;
            btnSave.Text = "Lưu";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(183, 183);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Hủy";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnAddCategory
            // 
            btnAddCategory.Location = new Point(183, 223);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(100, 30);
            btnAddCategory.TabIndex = 8;
            btnAddCategory.Text = "Thêm Danh Mục";
            btnAddCategory.Click += btnAddCategory_Click;
            // 
            // RightPanel
            // 
            RightPanel.Controls.Add(AddEditPanel);
            RightPanel.Controls.Add(FilterPanel);
            RightPanel.Controls.Add(ProductDisplay);
            RightPanel.Dock = DockStyle.Fill;
            RightPanel.Location = new Point(0, 0);
            RightPanel.Name = "RightPanel";
            RightPanel.Size = new Size(800, 639);
            RightPanel.TabIndex = 8;
            // 
            // TxtSearch
            // 
            TxtSearch.Location = new Point(10, 10);
            TxtSearch.Name = "TxtSearch";
            TxtSearch.Size = new Size(200, 23);
            TxtSearch.TabIndex = 9;
            toolTip.SetToolTip(TxtSearch, "Nhập từ khóa để tìm kiếm sản phẩm");
            TxtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // UcProductManagement
            // 
            Controls.Add(TxtSearch);
            Controls.Add(RightPanel);
            Name = "UcProductManagement";
            Size = new Size(800, 639);
            ((System.ComponentModel.ISupportInitialize)ProductDisplay).EndInit();
            AddEditPanel.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            RightPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        public System.Windows.Forms.DataGridView ProductDisplay;
        public System.Windows.Forms.TextBox TxtProductName;
        public System.Windows.Forms.ComboBox CmbCategory;
        public System.Windows.Forms.TextBox TxtPrice;
        public System.Windows.Forms.TextBox TxtStock;
        public System.Windows.Forms.TextBox TxtDescription;
        public System.Windows.Forms.Panel FilterPanel;
        public System.Windows.Forms.Panel AddEditPanel;
        public System.Windows.Forms.Panel RightPanel;
        public System.Windows.Forms.TextBox TxtSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAddCategory; // Add this line
        private System.ComponentModel.IContainer components;
    }
}
