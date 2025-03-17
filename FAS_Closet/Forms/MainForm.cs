// This file defines the MainForm class, which is the main application window.

using System;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Forms; // Add this line
using FASCloset.Services; // Add this line

namespace FASCloset.Forms
{
    public partial class MainForm : Form
    {
        private Label lblWelcome = null!;
        private Button btnLogout = null!;
        private DataGridView productDisplay = null!;
        private FlowLayoutPanel buttonPanel = null!;
        private Button btnAdd = null!, btnEdit = null!, btnDelete = null!, btnCategorize = null!, btnDataManagement = null!;
        private Panel addEditPanel = null!;
        private TextBox txtProductName = null!, txtPrice = null!, txtStock = null!, txtDescription = null!;
        private ComboBox cmbCategory = null!;
        private ComboBox cmbFilterCategory = null!; // Add this line
        private Button btnSave = null!, btnCancel = null!;
        private enum Mode { View, Add, Edit }
        private Mode currentMode = Mode.View;

        public MainForm(User user)
        {
            InitializeComponent();
            InitializeCustomComponents();
            lblWelcome.Text = "Welcome, " + user.Name; // Move this line after InitializeCustomComponents
        }

        private void InitializeCustomComponents()
        {
            this.ClientSize = new System.Drawing.Size(800, 600);

            var headerPanel = new Panel();
            headerPanel.Height = 60;
            headerPanel.Dock = DockStyle.Top;

            var headerInfoPanel = new Panel();
            headerInfoPanel.Dock = DockStyle.Top;
            headerInfoPanel.Height = 40;

            this.lblWelcome = new Label();
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Arial", 12F);
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblWelcome.Dock = DockStyle.Left;
            headerInfoPanel.Controls.Add(this.lblWelcome);

            this.btnLogout = new Button();
            this.btnLogout.Text = "Logout";
            this.btnLogout.Width = 80;
            this.btnLogout.Height = 30;
            this.btnLogout.Dock = DockStyle.Right;
            this.btnLogout.Click += new EventHandler(this.btnLogout_Click);
            headerInfoPanel.Controls.Add(this.btnLogout);

            var filterPanel = new Panel();
            filterPanel.Dock = DockStyle.Fill;
            filterPanel.Padding = new Padding(5, 0, 5, 0);

            this.cmbFilterCategory = new ComboBox();
            this.cmbFilterCategory.Dock = DockStyle.Fill;
            this.cmbFilterCategory.SelectedIndexChanged += new EventHandler(this.cmbFilterCategory_SelectedIndexChanged);
            filterPanel.Controls.Add(this.cmbFilterCategory);

            headerPanel.Controls.Add(filterPanel);
            headerPanel.Controls.Add(headerInfoPanel);
            this.Controls.Add(headerPanel);

            var mainLayout = new TableLayoutPanel();
            mainLayout.ColumnCount = 2;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowCount = 1;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Dock = DockStyle.Fill;
            this.Controls.Add(mainLayout);

            this.productDisplay = new DataGridView();
            this.productDisplay.Dock = DockStyle.Fill;
            this.productDisplay.ReadOnly = true;
            this.productDisplay.AllowUserToAddRows = false;
            this.productDisplay.AllowUserToDeleteRows = false;
            this.productDisplay.MultiSelect = false;
            this.productDisplay.AutoGenerateColumns = false;
            this.productDisplay.DataSource = new BindingSource();
            this.productDisplay.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "ProductID" });
            this.productDisplay.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "ProductName" });
            this.productDisplay.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Category", DataPropertyName = "CategoryID" });
            this.productDisplay.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Price", DataPropertyName = "Price" });
            this.productDisplay.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Stock", DataPropertyName = "Stock" });
            this.productDisplay.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", DataPropertyName = "Description" });
            mainLayout.Controls.Add(this.productDisplay, 1, 0);

            this.buttonPanel = new FlowLayoutPanel();
            this.buttonPanel.Dock = DockStyle.Fill;
            this.buttonPanel.FlowDirection = FlowDirection.TopDown;
            mainLayout.Controls.Add(this.buttonPanel, 0, 0);

            this.btnAdd = new Button();
            this.btnAdd.Text = "Thêm";
            this.btnAdd.Size = new System.Drawing.Size(150, 30);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.buttonPanel.Controls.Add(this.btnAdd);

            this.btnEdit = new Button();
            this.btnEdit.Text = "Sửa";
            this.btnEdit.Size = new System.Drawing.Size(150, 30);
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.buttonPanel.Controls.Add(this.btnEdit);

            this.btnDelete = new Button();
            this.btnDelete.Text = "Xóa";
            this.btnDelete.Size = new System.Drawing.Size(150, 30);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.buttonPanel.Controls.Add(this.btnDelete);

            this.btnCategorize = new Button();
            this.btnCategorize.Text = "Phân loại";
            this.btnCategorize.Size = new System.Drawing.Size(150, 30);
            this.buttonPanel.Controls.Add(this.btnCategorize);

            this.btnDataManagement = new Button();
            this.btnDataManagement.Text = "Quản lý dữ liệu";
            this.btnDataManagement.Size = new System.Drawing.Size(150, 30);
            this.buttonPanel.Controls.Add(this.btnDataManagement);

            InitializeAddEditPanel();
            LoadCategories();
            LoadProducts();
        }

        private void InitializeAddEditPanel()
        {
            addEditPanel = new Panel();
            addEditPanel.Visible = false;
            addEditPanel.Dock = DockStyle.Top;
            addEditPanel.Height = 200;

            var lblProductName = new Label { Text = "Product Name:", AutoSize = true };
            txtProductName = new TextBox { Width = 150 };

            var lblCategory = new Label { Text = "Category:", AutoSize = true };
            cmbCategory = new ComboBox { Width = 150 };
            LoadCategories();

            var lblPrice = new Label { Text = "Price:", AutoSize = true };
            txtPrice = new TextBox { Width = 150 };

            var lblStock = new Label { Text = "Stock:", AutoSize = true };
            txtStock = new TextBox { Width = 150 };

            var lblDescription = new Label { Text = "Description:", AutoSize = true };
            txtDescription = new TextBox { Width = 150 };

            btnSave = new Button { Text = "Save", Width = 75 };
            btnSave.Click += new EventHandler(this.btnSave_Click);

            btnCancel = new Button { Text = "Cancel", Width = 75 };
            btnCancel.Click += new EventHandler(this.btnCancel_Click);

            var layout = new TableLayoutPanel { ColumnCount = 2, RowCount = 6, Dock = DockStyle.Fill };
            layout.Controls.Add(lblProductName, 0, 0);
            layout.Controls.Add(txtProductName, 1, 0);
            layout.Controls.Add(lblCategory, 0, 1);
            layout.Controls.Add(cmbCategory, 1, 1);
            layout.Controls.Add(lblPrice, 0, 2);
            layout.Controls.Add(txtPrice, 1, 2);
            layout.Controls.Add(lblStock, 0, 3);
            layout.Controls.Add(txtStock, 1, 3);
            layout.Controls.Add(lblDescription, 0, 4);
            layout.Controls.Add(txtDescription, 1, 4);
            layout.Controls.Add(btnSave, 0, 5);
            layout.Controls.Add(btnCancel, 1, 5);

            addEditPanel.Controls.Add(layout);
            this.Controls.Add(addEditPanel);
        }

        private void LoadCategories()
        {
            var categories = new List<Category>();
            categories.Add(new Category { CategoryID = 0, CategoryName = "All" });
            categories.AddRange(CategoryManager.GetCategories());

            cmbFilterCategory.DataSource = categories;
            cmbFilterCategory.DisplayMember = "CategoryName";
            cmbFilterCategory.ValueMember = "CategoryID";
            cmbFilterCategory.SelectedIndex = 0;

            cmbCategory.DataSource = CategoryManager.GetCategories();
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryID";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void btnAdd_Click(object? sender, EventArgs e) // Fix nullability
        {
            currentMode = Mode.Add;
            ClearAddEditPanel();
            ShowAddEditPanel();
        }

        private void btnEdit_Click(object? sender, EventArgs e) // Fix nullability
        {
            if (productDisplay.SelectedRows.Count > 0)
            {
                currentMode = Mode.Edit;
                var selectedProduct = productDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    FillAddEditPanel(selectedProduct);
                    ShowAddEditPanel();
                }
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e) // Fix nullability
        {
            if (productDisplay.SelectedRows.Count > 0)
            {
                var selectedProduct = productDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    ProductManager.DeleteProduct(selectedProduct.ProductID);
                    LoadProducts();
                }
            }
        }

        private void cmbFilterCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var selectedCategory = cmbFilterCategory.SelectedItem as Category;
            if (selectedCategory != null)
            {
                int categoryId = selectedCategory.CategoryID;
                if (categoryId == 0)
                {
                    LoadProducts();
                }
                else
                {
                    LoadProductsByCategory(categoryId);
                }
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (currentMode == Mode.Add)
            {
                var product = new Product
                {
                    ProductName = txtProductName.Text,
                    CategoryID = (int)cmbCategory.SelectedValue!,
                    Price = decimal.Parse(txtPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    Description = txtDescription.Text
                };
                ProductManager.AddProduct(product);
            }
            else if (currentMode == Mode.Edit)
            {
                var selectedProduct = productDisplay.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    selectedProduct.ProductName = txtProductName.Text;
                    selectedProduct.CategoryID = (int)cmbCategory.SelectedValue!;
                    selectedProduct.Price = decimal.Parse(txtPrice.Text);
                    selectedProduct.Stock = int.Parse(txtStock.Text);
                    selectedProduct.Description = txtDescription.Text;
                    ProductManager.UpdateProduct(selectedProduct);
                }
            }
            LoadProducts();
            HideAddEditPanel();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            HideAddEditPanel();
        }

        private void ClearAddEditPanel()
        {
            txtProductName.Clear();
            cmbCategory.SelectedIndex = -1;
            txtPrice.Clear();
            txtStock.Clear();
            txtDescription.Clear();
        }

        private void FillAddEditPanel(Product product)
        {
            txtProductName.Text = product.ProductName;
            cmbCategory.SelectedValue = product.CategoryID;
            txtPrice.Text = product.Price.ToString();
            txtStock.Text = product.Stock.ToString();
            txtDescription.Text = product.Description;
        }

        private void ShowAddEditPanel()
        {
            addEditPanel.Visible = true;
        }

        private void HideAddEditPanel()
        {
            addEditPanel.Visible = false;
            currentMode = Mode.View;
        }

        private void LoadProducts()
        {
            var products = ProductManager.GetProducts();
            ((BindingSource)this.productDisplay.DataSource).DataSource = products; // Modify this line
        }

        private void LoadProductsByCategory(int categoryId)
        {
            var products = ProductManager.GetProductsByCategory(categoryId);
            ((BindingSource)this.productDisplay.DataSource).DataSource = products; // Modify this line
        }
    }
}
