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
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F)); // Increase width for button panel
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
            this.buttonPanel.WrapContents = false;
            this.buttonPanel.AutoScroll = true;
            this.buttonPanel.Padding = new Padding(10);
            mainLayout.Controls.Add(this.buttonPanel, 0, 0);

            ShowProductManagementButtons();

            InitializeAddEditPanel();
            LoadCategories();
            LoadProducts();
        }

        private void CreateStyledButton(ref Button btn, string text, EventHandler? clickHandler)
        {
            btn = new Button
            {
                Text = text,
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            if (clickHandler != null)
                btn.Click += clickHandler;
        }

        private void ShowProductManagementButtons()
        {
            buttonPanel.Controls.Clear();
            CreateStyledButton(ref btnAdd, "Thêm", btnAdd_Click);
            CreateStyledButton(ref btnEdit, "Sửa", btnEdit_Click);
            CreateStyledButton(ref btnDelete, "Xóa", btnDelete_Click);
            CreateStyledButton(ref btnCategorize, "Phân loại", null);
            CreateStyledButton(ref btnDataManagement, "Quản lý dữ liệu", null);
            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnCategorize);
            buttonPanel.Controls.Add(btnDataManagement);
        }

        private void ShowInventoryButtons()
        {
            buttonPanel.Controls.Clear();
            Button btnUpdateInventory = new Button
            {
                Text = "Cập nhật tồn kho",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Button btnAlertStock = new Button
            {
                Text = "Cảnh báo hết hàng",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            buttonPanel.Controls.Add(btnUpdateInventory);
            buttonPanel.Controls.Add(btnAlertStock);
        }

        private void ShowOrderManagementButtons()
        {
            buttonPanel.Controls.Clear();
            Button btnCreateOrder = new Button
            {
                Text = "Tạo đơn hàng",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Button btnProcessPayment = new Button
            {
                Text = "Xử lý thanh toán",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Button btnPrintInvoice = new Button
            {
                Text = "In hóa đơn",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            buttonPanel.Controls.Add(btnCreateOrder);
            buttonPanel.Controls.Add(btnProcessPayment);
            buttonPanel.Controls.Add(btnPrintInvoice);
        }

        private void ShowCustomerManagementButtons()
        {
            buttonPanel.Controls.Clear();
            Button btnSaveCustomerInfo = new Button
            {
                Text = "Lưu thông tin khách hàng",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Button btnViewPurchaseHistory = new Button
            {
                Text = "Lịch sử mua hàng",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Button btnAccumulatePoints = new Button
            {
                Text = "Tích điểm",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            buttonPanel.Controls.Add(btnSaveCustomerInfo);
            buttonPanel.Controls.Add(btnViewPurchaseHistory);
            buttonPanel.Controls.Add(btnAccumulatePoints);
        }

        private void ShowReportButtons()
        {
            buttonPanel.Controls.Clear();
            Button btnSalesReport = new Button
            {
                Text = "Thống kê doanh số",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Button btnDetailedReport = new Button
            {
                Text = "Xuất báo cáo chi tiết",
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            buttonPanel.Controls.Add(btnSalesReport);
            buttonPanel.Controls.Add(btnDetailedReport);
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
            ShowProductManagementButtons();
            // Update DataGridView and other UI elements for product management
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowInventoryButtons();
            // Update DataGridView and other UI elements for inventory management
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrderManagementButtons();
            // Update DataGridView and other UI elements for order management
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCustomerManagementButtons();
            // Update DataGridView and other UI elements for customer management
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowReportButtons();
            // Update DataGridView and other UI elements for reporting
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
