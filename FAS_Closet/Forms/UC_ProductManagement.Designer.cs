namespace FASCloset.Forms
{
    partial class UcProductManagement
    {
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            
            // Error Provider for validation
            errorProvider = new ErrorProvider(components);
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            
            // Tooltips for user guidance
            toolTip = new ToolTip(components);
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            
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
            btnDuplicate = new Button(); // Duplicate button for Add/Edit panel
            RightPanel = new Panel();
            TxtSearch = new TextBox();
            ChkShowInactive = new CheckBox(); // Checkbox for showing inactive products
            ChkIsActive = new CheckBox(); // Checkbox for product active status
            
            // Filter Panel Components
            FilterPanel.BackColor = Color.WhiteSmoke;
            Label lblFilter = new Label();
            lblFilter.Text = "Filter by Category:";
            lblFilter.AutoSize = true;
            lblFilter.Location = new Point(10, 15);
            
            CmbFilterCategory = new ComboBox();
            CmbFilterCategory.Location = new Point(120, 12);
            CmbFilterCategory.Size = new Size(200, 23);
            CmbFilterCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbFilterCategory.SelectedIndexChanged += cmbFilterCategory_SelectedIndexChanged;
            
            btnShowLowStock = new Button();
            btnShowLowStock.Text = "Show Low Stock";
            btnShowLowStock.Location = new Point(340, 10);
            btnShowLowStock.Size = new Size(120, 25);
            btnShowLowStock.Click += btnShowLowStock_Click;
            
            FilterPanel.Controls.Add(lblFilter);
            FilterPanel.Controls.Add(CmbFilterCategory);
            FilterPanel.Controls.Add(btnShowLowStock);
            
            // Check for inactive products
            ChkShowInactive.Text = "Show Inactive Products";
            ChkShowInactive.AutoSize = true;
            ChkShowInactive.Location = new Point(470, 15);
            ChkShowInactive.CheckedChanged += ChkShowInactive_CheckedChanged;
            FilterPanel.Controls.Add(ChkShowInactive);

            // Initialize DataGridView
            ((System.ComponentModel.ISupportInitialize)ProductDisplay).BeginInit();
            ProductDisplay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ProductDisplay.Location = new Point(10, 50);
            ProductDisplay.Size = new Size(600, 200);
            ProductDisplay.TabIndex = 0;
            ProductDisplay.AllowUserToAddRows = false;
            ProductDisplay.MultiSelect = false;
            ProductDisplay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProductDisplay.ReadOnly = true;
            ProductDisplay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ProductDisplay.RowsDefaultCellStyle.BackColor = Color.White;
            ProductDisplay.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            ProductDisplay.CellFormatting += ProductDisplay_CellFormatting;

            // Setup Input Fields
            TxtProductName.Location = new Point(124, 3);
            TxtProductName.Size = new Size(253, 23);
            TxtProductName.TabIndex = 1;
            TxtProductName.Validating += TxtProductName_Validating;
            toolTip.SetToolTip(TxtProductName, "Enter product name");

            CmbCategory.FormattingEnabled = true;
            CmbCategory.Location = new Point(124, 32);
            CmbCategory.Size = new Size(253, 23);
            CmbCategory.TabIndex = 2;
            CmbCategory.Validating += CmbCategory_Validating;
            toolTip.SetToolTip(CmbCategory, "Select product category");

            TxtPrice.Location = new Point(124, 61);
            TxtPrice.Size = new Size(253, 23);
            TxtPrice.TabIndex = 3;
            TxtPrice.KeyPress += TxtPrice_KeyPress;
            TxtPrice.Validating += TxtPrice_Validating;
            toolTip.SetToolTip(TxtPrice, "Enter price (e.g., 100.50)");

            TxtStock.Location = new Point(124, 90);
            TxtStock.Size = new Size(253, 23);
            TxtStock.TabIndex = 4;
            TxtStock.KeyPress += TxtStock_KeyPress;
            TxtStock.Validating += TxtStock_Validating;
            toolTip.SetToolTip(TxtStock, "Enter stock quantity");

            TxtDescription.Location = new Point(124, 119);
            TxtDescription.Multiline = true;
            TxtDescription.Size = new Size(253, 54);
            TxtDescription.TabIndex = 5;
            TxtDescription.Validating += TxtDescription_Validating;
            toolTip.SetToolTip(TxtDescription, "Enter product description");

            CmbManufacturer.FormattingEnabled = true;
            CmbManufacturer.Location = new Point(124, 179);
            CmbManufacturer.Size = new Size(253, 23);
            CmbManufacturer.TabIndex = 6;
            toolTip.SetToolTip(CmbManufacturer, "Select manufacturer");

            // Add Active Status Checkbox
            ChkIsActive.Text = "Active";
            ChkIsActive.Location = new Point(124, 208);
            ChkIsActive.Size = new Size(100, 24);
            ChkIsActive.Checked = true;
            ChkIsActive.TabIndex = 7;
            toolTip.SetToolTip(ChkIsActive, "Check to keep product active, uncheck to archive");

            // Filter Panel
            FilterPanel.Location = new Point(10, 260);
            FilterPanel.Size = new Size(600, 40);
            FilterPanel.TabIndex = 6;

            // Add/Edit Panel
            AddEditPanel.AutoScroll = true;
            AddEditPanel.Controls.Add(tableLayoutPanel);
            AddEditPanel.Location = new Point(10, 310);
            AddEditPanel.Size = new Size(600, 300);
            AddEditPanel.TabIndex = 7;
            AddEditPanel.Visible = false;

            // Table Layout Panel
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.Controls.Add(new Label() { Text = "Product Name:", Dock = DockStyle.Fill }, 0, 0);
            tableLayoutPanel.Controls.Add(TxtProductName, 1, 0);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Category:", Dock = DockStyle.Fill }, 0, 1);
            tableLayoutPanel.Controls.Add(CmbCategory, 1, 1);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Price:", Dock = DockStyle.Fill }, 0, 2);
            tableLayoutPanel.Controls.Add(TxtPrice, 1, 2);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Stock:", Dock = DockStyle.Fill }, 0, 3);
            tableLayoutPanel.Controls.Add(TxtStock, 1, 3);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Description:", Dock = DockStyle.Fill }, 0, 4);
            tableLayoutPanel.Controls.Add(TxtDescription, 1, 4);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Manufacturer:", Dock = DockStyle.Fill }, 0, 5);
            tableLayoutPanel.Controls.Add(CmbManufacturer, 1, 5);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Status:", Dock = DockStyle.Fill }, 0, 6);
            tableLayoutPanel.Controls.Add(ChkIsActive, 1, 6);
            tableLayoutPanel.Controls.Add(flowButtons, 0, 7);
            tableLayoutPanel.Location = new Point(20, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 8;
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.Size = new Size(406, 290);
            tableLayoutPanel.TabIndex = 0;
            tableLayoutPanel.Paint += OnTableLayoutPanelPaint;

            // Button Panel
            tableLayoutPanel.SetColumnSpan(flowButtons, 2);
            flowButtons.Controls.Add(btnSave);
            flowButtons.Controls.Add(btnCancel);
            flowButtons.Controls.Add(btnDuplicate);
            flowButtons.Controls.Add(btnAddCategory);
            flowButtons.Controls.Add(btnAddManufacturer);
            flowButtons.Location = new Point(3, 235);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new Size(400, 40);
            flowButtons.TabIndex = 8;

            // Save Button
            btnSave.Location = new Point(3, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 30);
            btnSave.TabIndex = 9;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;

            // Cancel Button
            btnCancel.Location = new Point(89, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 30);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            
            // Duplicate Button
            btnDuplicate.Location = new Point(175, 3);
            btnDuplicate.Name = "btnDuplicate";
            btnDuplicate.Size = new Size(80, 30);
            btnDuplicate.TabIndex = 11;
            btnDuplicate.Text = "Duplicate";
            btnDuplicate.UseVisualStyleBackColor = true;
            btnDuplicate.Click += btnDuplicate_Click;
            btnDuplicate.Visible = false; // Initially hidden, will be shown in edit mode

            // Add Category Button
            btnAddCategory.Location = new Point(261, 3);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(100, 30);
            btnAddCategory.TabIndex = 12;
            btnAddCategory.Text = "Add Category";
            btnAddCategory.UseVisualStyleBackColor = true;
            btnAddCategory.Click += btnAddCategory_Click;

            // Add Manufacturer Button
            btnAddManufacturer.Location = new Point(3, 39);
            btnAddManufacturer.Name = "btnAddManufacturer";
            btnAddManufacturer.Size = new Size(120, 30);
            btnAddManufacturer.TabIndex = 13;
            btnAddManufacturer.Text = "Add Manufacturer";
            btnAddManufacturer.UseVisualStyleBackColor = true;
            btnAddManufacturer.Click += btnAddManufacturer_Click;

            // Right Panel
            RightPanel.Controls.Add(TxtSearch);
            RightPanel.Dock = DockStyle.Right;
            RightPanel.Location = new Point(620, 0);
            RightPanel.Name = "RightPanel";
            RightPanel.Size = new Size(200, 620);
            RightPanel.TabIndex = 8;

            // Search Box
            TxtSearch.Location = new Point(10, 10);
            TxtSearch.Name = "TxtSearch";
            TxtSearch.Size = new Size(180, 23);
            TxtSearch.TabIndex = 0;
            TxtSearch.PlaceholderText = "Search products...";
            TxtSearch.TextChanged += TxtSearch_TextChanged;

            // Add controls to the user control
            this.Controls.Add(ProductDisplay);
            this.Controls.Add(FilterPanel);
            this.Controls.Add(AddEditPanel);
            this.Controls.Add(RightPanel);

            ((System.ComponentModel.ISupportInitialize)ProductDisplay).EndInit();
            AddEditPanel.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            flowButtons.ResumeLayout(false);
            RightPanel.ResumeLayout(false);
            RightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
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
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Button btnAddManufacturer;
        private System.ComponentModel.IContainer components;
        private FlowLayoutPanel flowButtons;
        private ComboBox CmbFilterCategory;
        private CheckBox ChkShowInactive;
        private CheckBox ChkIsActive;
        private Button btnShowLowStock;
        private ErrorProvider errorProvider;
        
        // Keep these field declarations but don't initialize them in InitializeComponent
        // They'll be initialized in MainForm and referenced through public methods
        public Button btnAdd;
        public Button btnEdit; 
        public Button btnDelete;

        // Renamed method to avoid ambiguity
        private void OnTableLayoutPanelPaint(object sender, PaintEventArgs e)
        {
            // Add your paint logic here
        }
    }
}
