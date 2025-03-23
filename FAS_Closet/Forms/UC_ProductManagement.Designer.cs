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
            CmbManufacturer = new ComboBox();
            FilterPanel = new Panel();
            AddEditPanel = new Panel();
            tableLayoutPanel = new TableLayoutPanel();
            flowButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            btnAddCategory = new Button();
            btnAddManufacturer = new Button();
            RightPanel = new Panel();
            TxtSearch = new TextBox();
            toolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)ProductDisplay).BeginInit();
            AddEditPanel.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            flowButtons.SuspendLayout();
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
            TxtProductName.Location = new Point(124, 3);
            TxtProductName.Name = "TxtProductName";
            TxtProductName.Size = new Size(253, 23);
            TxtProductName.TabIndex = 1;
            toolTip.SetToolTip(TxtProductName, "Nhập tên sản phẩm");
            // 
            // CmbCategory
            // 
            CmbCategory.FormattingEnabled = true;
            CmbCategory.Location = new Point(124, 32);
            CmbCategory.Name = "CmbCategory";
            CmbCategory.Size = new Size(253, 23);
            CmbCategory.TabIndex = 2;
            toolTip.SetToolTip(CmbCategory, "Chọn danh mục sản phẩm");
            // 
            // TxtPrice
            // 
            TxtPrice.Location = new Point(124, 61);
            TxtPrice.Name = "TxtPrice";
            TxtPrice.Size = new Size(253, 23);
            TxtPrice.TabIndex = 3;
            toolTip.SetToolTip(TxtPrice, "Nhập giá sản phẩm (ví dụ: 100.50)");
            // 
            // TxtStock
            // 
            TxtStock.Location = new Point(124, 90);
            TxtStock.Name = "TxtStock";
            TxtStock.Size = new Size(253, 23);
            TxtStock.TabIndex = 4;
            toolTip.SetToolTip(TxtStock, "Nhập số lượng tồn kho");
            // 
            // TxtDescription
            // 
            TxtDescription.Location = new Point(124, 119);
            TxtDescription.Multiline = true;
            TxtDescription.Name = "TxtDescription";
            TxtDescription.Size = new Size(253, 54);
            TxtDescription.TabIndex = 5;
            toolTip.SetToolTip(TxtDescription, "Nhập mô tả sản phẩm");
            // 
            // CmbManufacturer
            // 
            CmbManufacturer.FormattingEnabled = true;
            CmbManufacturer.Location = new Point(124, 148);
            CmbManufacturer.Name = "CmbManufacturer";
            CmbManufacturer.Size = new Size(253, 23);
            CmbManufacturer.TabIndex = 6;
            toolTip.SetToolTip(CmbManufacturer, "Chọn nhà sản xuất sản phẩm");
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
            AddEditPanel.AutoScroll = true;
            AddEditPanel.Controls.Add(tableLayoutPanel);
            AddEditPanel.Location = new Point(10, 355);
            AddEditPanel.Name = "AddEditPanel";
            AddEditPanel.Size = new Size(600, 265);
            AddEditPanel.TabIndex = 7;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.Controls.Add(new Label() { Text = "Tên sản phẩm:", Dock = DockStyle.Fill }, 0, 0);
            tableLayoutPanel.Controls.Add(TxtProductName, 1, 0);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Danh mục:", Dock = DockStyle.Fill }, 0, 1);
            tableLayoutPanel.Controls.Add(CmbCategory, 1, 1);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Giá:", Dock = DockStyle.Fill }, 0, 2);
            tableLayoutPanel.Controls.Add(TxtPrice, 1, 2);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Tồn kho:", Dock = DockStyle.Fill }, 0, 3);
            tableLayoutPanel.Controls.Add(TxtStock, 1, 3);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Mô tả:", Dock = DockStyle.Fill }, 0, 4);
            tableLayoutPanel.Controls.Add(TxtDescription, 1, 4);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Nhà sản xuất:", Dock = DockStyle.Fill }, 0, 5);
            tableLayoutPanel.Controls.Add(CmbManufacturer, 1, 5);
            tableLayoutPanel.Controls.Add(flowButtons, 0, 6);
            tableLayoutPanel.Location = new Point(191, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 7;
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.Size = new Size(406, 262);
            tableLayoutPanel.TabIndex = 0;
            tableLayoutPanel.Paint += OnTableLayoutPanelPaint; // Changed to unique method name
            // 
            // flowButtons
            // 
            tableLayoutPanel.SetColumnSpan(flowButtons, 2);
            flowButtons.Controls.Add(btnSave);
            flowButtons.Controls.Add(btnCancel);
            flowButtons.Controls.Add(btnAddCategory);
            flowButtons.Controls.Add(btnAddManufacturer);
            flowButtons.Location = new Point(3, 179);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new Size(400, 100);
            flowButtons.TabIndex = 6;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(3, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 6;
            btnSave.Text = "Lưu";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(109, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Hủy";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnAddCategory
            // 
            btnAddCategory.Location = new Point(215, 3);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(100, 30);
            btnAddCategory.TabIndex = 8;
            btnAddCategory.Text = "Thêm Danh Mục";
            btnAddCategory.Click += btnAddCategory_Click;
            // 
            // btnAddManufacturer
            // 
            btnAddManufacturer.Location = new Point(321, 3);
            btnAddManufacturer.Name = "btnAddManufacturer";
            btnAddManufacturer.Size = new Size(100, 30);
            btnAddManufacturer.TabIndex = 9;
            btnAddManufacturer.Text = "Thêm Nhà Sản Xuất";
            btnAddManufacturer.Click += btnAddManufacturer_Click;
            // 
            // RightPanel
            // 
            RightPanel.Controls.Add(TxtSearch);
            RightPanel.Dock = DockStyle.Right;
            RightPanel.Location = new Point(620, 0);
            RightPanel.Name = "RightPanel";
            RightPanel.Size = new Size(200, 620);
            RightPanel.TabIndex = 8;
            // 
            // TxtSearch
            // 
            TxtSearch.Location = new Point(10, 10);
            TxtSearch.Name = "TxtSearch";
            TxtSearch.Size = new Size(180, 23);
            TxtSearch.TabIndex = 0;
            TxtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // UcProductManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ProductDisplay);
            Controls.Add(FilterPanel);
            Controls.Add(AddEditPanel);
            Controls.Add(RightPanel);
            Name = "UcProductManagement";
            Size = new Size(820, 620);
            ((System.ComponentModel.ISupportInitialize)ProductDisplay).EndInit();
            AddEditPanel.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            flowButtons.ResumeLayout(false);
            RightPanel.ResumeLayout(false);
            RightPanel.PerformLayout();
            ResumeLayout(false);
        }

        public System.Windows.Forms.DataGridView ProductDisplay;
        public System.Windows.Forms.TextBox TxtProductName;
        public System.Windows.Forms.ComboBox CmbCategory;
        public System.Windows.Forms.TextBox TxtPrice;
        public System.Windows.Forms.TextBox TxtStock;
        public System.Windows.Forms.TextBox TxtDescription;
        public System.Windows.Forms.ComboBox CmbManufacturer;
        public System.Windows.Forms.Panel FilterPanel;
        public System.Windows.Forms.Panel AddEditPanel;
        public System.Windows.Forms.Panel RightPanel;
        public System.Windows.Forms.TextBox TxtSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Button btnAddManufacturer;
        private System.ComponentModel.IContainer components;
        private FlowLayoutPanel flowButtons;

        // Renamed method to avoid ambiguity
        private void OnTableLayoutPanelPaint(object sender, PaintEventArgs e)
        {
            // Add your paint logic here
        }
    }
}
