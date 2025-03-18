using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;
using System.ComponentModel;

namespace FASCloset.Forms
{
    public partial class UcProductManagement : UserControl
    {
        private enum Mode { View, Add, Edit }
        private Mode currentMode = Mode.View;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required DataGridView ProductDisplay { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required TextBox TxtProductName { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required ComboBox CmbCategory { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required TextBox TxtPrice { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required TextBox TxtStock { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required TextBox TxtDescription { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required Panel FilterPanel { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required Panel AddEditPanel { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public required Panel RightPanel { get; set; }

        public UcProductManagement()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            ProductDisplay = new DataGridView();
            TxtProductName = new TextBox();
            CmbCategory = new ComboBox();
            TxtPrice = new TextBox();
            TxtStock = new TextBox();
            TxtDescription = new TextBox();
            FilterPanel = new Panel();
            AddEditPanel = new Panel();
            RightPanel = new Panel();
            // Initialize other components and set properties
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            currentMode = Mode.Add;
            ClearAddEditPanel();
            ShowAddEditPanel();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (ProductDisplay.SelectedRows.Count > 0)
            {
                currentMode = Mode.Edit;
                var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    FillAddEditPanel(selectedProduct);
                    ShowAddEditPanel();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ProductDisplay.SelectedRows.Count > 0)
            {
                var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    ProductManager.DeleteProduct(selectedProduct.ProductID);
                    LoadProducts();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Add)
            {
                var product = new Product
                {
                    ProductName = TxtProductName.Text,
                    CategoryID = (int)CmbCategory.SelectedValue!,
                    Price = decimal.Parse(TxtPrice.Text),
                    Stock = int.Parse(TxtStock.Text),
                    Description = TxtDescription.Text
                };
                ProductManager.AddProduct(product);
            }
            else if (currentMode == Mode.Edit && ProductDisplay.SelectedRows.Count > 0)
            {
                var selectedProduct = ProductDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    selectedProduct.ProductName = TxtProductName.Text;
                    selectedProduct.CategoryID = (int)CmbCategory.SelectedValue!;
                    selectedProduct.Price = decimal.Parse(TxtPrice.Text);
                    selectedProduct.Stock = int.Parse(TxtStock.Text);
                    selectedProduct.Description = TxtDescription.Text;
                    ProductManager.UpdateProduct(selectedProduct);
                }
            }
            LoadProducts();
            HideAddEditPanel();
        }

        private void btnCancel_Click(object sender, EventArgs e) => HideAddEditPanel();

        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cmbFilterCategory && cmbFilterCategory.SelectedItem is Category selectedCategory)
            {
                if (selectedCategory.CategoryID == 0) LoadProducts();
                else LoadProductsByCategory(selectedCategory.CategoryID);
            }
        }

        private void LoadProducts()
        {
            var products = ProductManager.GetProducts();
            ProductDisplay.DataSource = new BindingSource { DataSource = products };
        }

        private void LoadProductsByCategory(int categoryId)
        {
            var products = ProductManager.GetProductsByCategory(categoryId);
            ProductDisplay.DataSource = new BindingSource { DataSource = products };
        }

        private void ShowAddEditPanel()
        {
            FilterPanel.Visible = true;
            AddEditPanel.Visible = true;
            AddEditPanel.BringToFront();
            RightPanel.Controls.SetChildIndex(AddEditPanel, 0);
        }

        private void HideAddEditPanel()
        {
            AddEditPanel.Visible = false;
            FilterPanel.Visible = true;
            currentMode = Mode.View;
        }

        private void ClearAddEditPanel()
        {
            TxtProductName.Clear();
            CmbCategory.SelectedIndex = -1;
            TxtPrice.Clear();
            TxtStock.Clear();
            TxtDescription.Clear();
        }

        private void FillAddEditPanel(Product product)
        {
            TxtProductName.Text = product.ProductName;
            CmbCategory.SelectedValue = product.CategoryID;
            TxtPrice.Text = product.Price.ToString();
            TxtStock.Text = product.Stock.ToString();
            TxtDescription.Text = product.Description;
        }
    }
}
