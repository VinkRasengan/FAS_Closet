using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class ProductInventoryManagementForm : Form
    {
        public ProductInventoryManagementForm()
        {
            InitializeComponent();
            LoadProducts();
            LoadInventory();
            panelProductEditor.Visible = false;
            panelInventoryEditor.Visible = false;
        }
        
        #region Product Management

        private void LoadProducts()
        {
            var products = ProductManager.GetProducts();
            dataGridViewProducts.DataSource = products;
        }
        
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            ClearProductEditor();
            panelProductEditor.Visible = true;
        }
        
        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedProduct = dataGridViewProducts.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    LoadProductEditor(selectedProduct);
                    panelProductEditor.Visible = true;
                }
            }
        }
        
        private void btnDeleteProduct_Click(object sender, EventArgs e)
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
        
        private void btnSaveProduct_Click(object sender, EventArgs e)
        {
            Product product;
            if (string.IsNullOrWhiteSpace(txtProductID.Text))
            {
                product = new Product
                {
                    ProductName = txtProductName.Text,
                    Description = txtDescription.Text
                };
            }
            else
            {
                product = new Product
                {
                    ProductID = int.Parse(txtProductID.Text),
                    ProductName = txtProductName.Text,
                    Description = txtDescription.Text
                };
            }
            
            if(decimal.TryParse(txtPrice.Text, out decimal price))
            {
                product.Price = price;
            }
            else
            {
                MessageBox.Show("Invalid price.");
                return;
            }
            
            if (product.ProductID == 0)
                ProductManager.AddProduct(product);
            else
                ProductManager.UpdateProduct(product);
            
            LoadProducts();
            panelProductEditor.Visible = false;
        }
        
        private void btnCancelProduct_Click(object sender, EventArgs e)
        {
            panelProductEditor.Visible = false;
        }
        
        private void LoadProductEditor(Product product)
        {
            txtProductID.Text = product.ProductID.ToString();
            txtProductName.Text = product.ProductName;
            txtPrice.Text = product.Price.ToString();
            txtDescription.Text = product.Description;
        }
        
        private void ClearProductEditor()
        {
            txtProductID.Clear();
            txtProductName.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
        }
        
        #endregion

        #region Inventory Management

        private void LoadInventory()
        {
            var inventory = InventoryManager.GetLowStockProducts();
            dataGridViewInventory.DataSource = inventory;
        }
        
        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            if (dataGridViewInventory.SelectedRows.Count > 0)
            {
                var selectedInventory = dataGridViewInventory.SelectedRows[0].DataBoundItem as Inventory;
                if (selectedInventory != null)
                {
                    LoadInventoryEditor(selectedInventory);
                    panelInventoryEditor.Visible = true;
                }
            }
        }
        
        private void btnSetThreshold_Click(object sender, EventArgs e)
        {
            // Tương tự như UpdateStock, load thông tin tồn kho để chỉnh sửa ngưỡng
            if (dataGridViewInventory.SelectedRows.Count > 0)
            {
                var selectedInventory = dataGridViewInventory.SelectedRows[0].DataBoundItem as Inventory;
                if (selectedInventory != null)
                {
                    LoadInventoryEditor(selectedInventory);
                    panelInventoryEditor.Visible = true;
                }
            }
        }
        
        private void btnCheckLowStock_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }
        
        private void btnSaveInventory_Click(object sender, EventArgs e)
        {
            if (dataGridViewInventory.SelectedRows.Count > 0)
            {
                var selectedInventory = dataGridViewInventory.SelectedRows[0].DataBoundItem as Inventory;
                if (selectedInventory != null)
                {
                    if (int.TryParse(txtStock.Text, out int stock))
                        selectedInventory.StockQuantity = stock;
                    else
                    {
                        MessageBox.Show("Invalid stock value.");
                        return;
                    }
                    
                    if (int.TryParse(txtThreshold.Text, out int threshold))
                        selectedInventory.MinimumStockThreshold = threshold;
                    else
                    {
                        MessageBox.Show("Invalid threshold value.");
                        return;
                    }
                    
                    InventoryManager.UpdateStock(selectedInventory.ProductID, selectedInventory.StockQuantity);
                    InventoryManager.SetMinimumStockThreshold(selectedInventory.ProductID, selectedInventory.MinimumStockThreshold);
                    LoadInventory();
                    panelInventoryEditor.Visible = false;
                }
            }
        }
        
        private void btnCancelInventory_Click(object sender, EventArgs e)
        {
            panelInventoryEditor.Visible = false;
        }
        
        private void LoadInventoryEditor(Inventory inv)
        {
            txtInvID.Text = inv.ProductID.ToString();
            txtStock.Text = inv.StockQuantity.ToString();
            txtThreshold.Text = inv.MinimumStockThreshold.ToString();
        }
        
        #endregion
    }
}
