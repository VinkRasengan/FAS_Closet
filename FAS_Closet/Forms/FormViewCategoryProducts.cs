using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class FormViewCategoryProducts : Form
    {
        public FormViewCategoryProducts(string categoryName, List<Product> products)
        {
            InitializeComponent();
            this.Text = $"Products in Category: {categoryName}";
            dataGridViewProducts.AutoGenerateColumns = true;
            dataGridViewProducts.DataSource = products;
        }
    }

    public partial class FormViewCategoryProducts
    {
        private System.Windows.Forms.DataGridView dataGridViewProducts;

        private void InitializeComponent()
        {
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.SuspendLayout();

            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProducts.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.RowHeadersVisible = false;
            this.dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProducts.Size = new System.Drawing.Size(800, 450);
            this.dataGridViewProducts.TabIndex = 0;

            // 
            // FormViewCategoryProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewProducts);
            this.Name = "FormViewCategoryProducts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Products by Category";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
