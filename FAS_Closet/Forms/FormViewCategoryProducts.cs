using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASCloset.Models;
using System.Drawing;

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
            this.dataGridViewProducts.Location = new System.Drawing.Point(10, 50); // Add some padding around the edges
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.RowHeadersVisible = false;
            this.dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProducts.Size = new System.Drawing.Size(780, 370); // Slightly adjusted size
            this.dataGridViewProducts.TabIndex = 0;
            this.dataGridViewProducts.BackgroundColor = Color.WhiteSmoke;  // Set background color
            this.dataGridViewProducts.BorderStyle = BorderStyle.Fixed3D;  // Add border to the DataGridView
            this.dataGridViewProducts.GridColor = Color.FromArgb(200, 200, 200);  // Light grid color
            this.dataGridViewProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // Alternate row colors
            this.dataGridViewProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);  // Highlight color for selection

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

            // Customizing the title bar
            this.BackColor = Color.FromArgb(50, 50, 255); // Background color for the title bar
            Label titleLabel = new Label();
            titleLabel.Text = $"Products in Category: {this.Text}";
            titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 40;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
