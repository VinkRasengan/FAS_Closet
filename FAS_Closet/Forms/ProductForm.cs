using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private static void LoadProducts()
        {
            var products = ProductManager.GetProducts();
            if (dataGridViewProducts != null)
            {
                dataGridViewProducts.DataSource = products;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var productEditorForm = new ProductEditorForm();
            if (productEditorForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedProduct = dataGridViewProducts.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    var productEditorForm = new ProductEditorForm(selectedProduct);
                    if (productEditorForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadProducts();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedProduct = dataGridViewProducts.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    ProductManager.DeleteProduct(selectedProduct.ProductID);
                    LoadProducts();
                }
            }
        }
    }
}
