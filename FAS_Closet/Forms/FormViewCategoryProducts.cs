using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASCloset.Models;
using System.Drawing;
using FASCloset.Extensions;

namespace FASCloset.Forms
{
    public partial class FormViewCategoryProducts : Form
    {
        private string categoryName;

        public FormViewCategoryProducts(string categoryName, List<Product> products)
        {
            this.categoryName = categoryName;
            InitializeComponent();
            this.Text = $"Sản phẩm thuộc danh mục: {categoryName}";
            
            // Configure display settings for products
            SetupDataGridView();
            
            // Set data source after column configuration
            dataGridViewProducts.DataSource = products;
            
            // Apply column formatting for currency and other special columns
            DataGridViewStyleHelper.ApplyColumnFormatting(dataGridViewProducts);
        }
        
        private void SetupDataGridView()
        {
            // Configure columns for better display before setting data source
            dataGridViewProducts.AutoGenerateColumns = false;
            dataGridViewProducts.Columns.Clear();
            
            // Product ID column
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductID",
                HeaderText = "Mã SP",
                Width = 60
            });
            
            // Product name column
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên sản phẩm",
                Width = 200
            });
            
            // Manufacturer column
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ManufacturerName",
                HeaderText = "Nhà sản xuất",
                Width = 150
            });
            
            // Price column with currency formatting
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Giá bán",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            
            // Stock column
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Tồn kho",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            
            // Description column (may be truncated in grid view)
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Mô tả",
                Width = 200
            });
            
            // Apply consistent styling to the DataGridView
            Color categoryHeaderColor = Color.FromArgb(0, 123, 255); // Blue
            DataGridViewStyleHelper.ApplyFullStyle(dataGridViewProducts, categoryHeaderColor);
        }
    }

    public partial class FormViewCategoryProducts
    {
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.Label titleLabel;

        private void InitializeComponent()
        {
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.titleLabel = new System.Windows.Forms.Label();
            
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.SuspendLayout();

            // 
            // titleLabel
            // 
            this.titleLabel.Dock = DockStyle.Top;
            this.titleLabel.BackColor = Color.FromArgb(0, 123, 255);
            this.titleLabel.ForeColor = Color.White;
            this.titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.titleLabel.Text = $"Sản phẩm thuộc danh mục: {this.categoryName}";
            this.titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.titleLabel.Height = 50;
            
            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) | AnchorStyles.Right)));
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProducts.Location = new Point(15, 65);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.Size = new Size(770, 370);
            this.dataGridViewProducts.TabIndex = 0;

            // 
            // FormViewCategoryProducts
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.dataGridViewProducts);
            this.Controls.Add(this.titleLabel);
            this.Name = "FormViewCategoryProducts";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Padding = new Padding(10);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
