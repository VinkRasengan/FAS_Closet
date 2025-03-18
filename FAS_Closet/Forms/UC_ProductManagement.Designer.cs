namespace FASCloset.Forms
{
    partial class UcProductManagement
    {
        private void InitializeComponent()
        {
            this.ProductDisplay = new System.Windows.Forms.DataGridView();
            this.TxtProductName = new System.Windows.Forms.TextBox();
            this.CmbCategory = new System.Windows.Forms.ComboBox();
            this.TxtPrice = new System.Windows.Forms.TextBox();
            this.TxtStock = new System.Windows.Forms.TextBox();
            this.TxtDescription = new System.Windows.Forms.TextBox();
            this.FilterPanel = new System.Windows.Forms.Panel();
            this.AddEditPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.TxtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProductDisplay)).BeginInit();
            this.RightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProductDisplay
            // 
            this.ProductDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductDisplay.Location = new System.Drawing.Point(10, 50);
            this.ProductDisplay.Name = "ProductDisplay";
            this.ProductDisplay.Size = new System.Drawing.Size(600, 200);
            this.ProductDisplay.TabIndex = 0;
            // 
            // TxtProductName
            // 
            this.TxtProductName.Location = new System.Drawing.Point(120, 10);
            this.TxtProductName.Name = "TxtProductName";
            this.TxtProductName.Size = new System.Drawing.Size(200, 20);
            this.TxtProductName.TabIndex = 1;
            // 
            // CmbCategory
            // 
            this.CmbCategory.FormattingEnabled = true;
            this.CmbCategory.Location = new System.Drawing.Point(120, 40);
            this.CmbCategory.Name = "CmbCategory";
            this.CmbCategory.Size = new System.Drawing.Size(200, 21);
            this.CmbCategory.TabIndex = 2;
            // 
            // TxtPrice
            // 
            this.TxtPrice.Location = new System.Drawing.Point(120, 70);
            this.TxtPrice.Name = "TxtPrice";
            this.TxtPrice.Size = new System.Drawing.Size(200, 20);
            this.TxtPrice.TabIndex = 3;
            // 
            // TxtStock
            // 
            this.TxtStock.Location = new System.Drawing.Point(120, 100);
            this.TxtStock.Name = "TxtStock";
            this.TxtStock.Size = new System.Drawing.Size(200, 20);
            this.TxtStock.TabIndex = 4;
            // 
            // TxtDescription
            // 
            this.TxtDescription.Location = new System.Drawing.Point(120, 130);
            this.TxtDescription.Multiline = true;
            this.TxtDescription.Name = "TxtDescription";
            this.TxtDescription.Size = new System.Drawing.Size(200, 60);
            this.TxtDescription.TabIndex = 5;
            // 
            // FilterPanel
            // 
            this.FilterPanel.Location = new System.Drawing.Point(10, 260);
            this.FilterPanel.Name = "FilterPanel";
            this.FilterPanel.Size = new System.Drawing.Size(600, 100);
            this.FilterPanel.TabIndex = 6;
            // 
            // AddEditPanel
            // 
            this.AddEditPanel.Location = new System.Drawing.Point(10, 370);
            this.AddEditPanel.Name = "AddEditPanel";
            this.AddEditPanel.Size = new System.Drawing.Size(600, 200);
            this.AddEditPanel.TabIndex = 7;
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.AddEditPanel);
            this.RightPanel.Controls.Add(this.FilterPanel);
            this.RightPanel.Controls.Add(this.ProductDisplay);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(0, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(800, 600);
            this.RightPanel.TabIndex = 8;
            // 
            // TxtSearch
            // 
            this.TxtSearch.Location = new System.Drawing.Point(10, 10);
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.Size = new System.Drawing.Size(200, 20);
            this.TxtSearch.TabIndex = 9;
            this.TxtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // UcProductManagement
            // 
            this.Controls.Add(this.TxtSearch);
            this.Controls.Add(this.RightPanel);
            this.Name = "UcProductManagement";
            this.Size = new System.Drawing.Size(800, 600);
            ((System.ComponentModel.ISupportInitialize)(this.ProductDisplay)).EndInit();
            this.RightPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
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
    }
}
