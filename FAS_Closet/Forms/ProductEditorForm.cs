using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class ProductEditorForm : Form
    {
        private Product _product;

        public ProductEditorForm(Product? product = null)
        {
            InitializeComponent();
            _product = product ?? new Product();
            if (product != null)
            {
                txtProductName.Text = product.ProductName;
                txtPrice.Text = product.Price.ToString();
                txtDescription.Text = product.Description;
                // Load categories and set selected category
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _product.ProductName = txtProductName.Text;
            _product.Price = decimal.Parse(txtPrice.Text);
            _product.Description = txtDescription.Text;
            // Set category ID

            if (_product.ProductID == 0)
            {
                ProductManager.AddProduct(_product);
            }
            else
            {
                ProductManager.UpdateProduct(_product);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
