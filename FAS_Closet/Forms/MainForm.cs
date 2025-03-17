using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class MainForm : Form
    {
        // HEADER
        private Panel headerPanel = null!;
        private Label lblWelcome = null!;
        private Button btnLogout = null!;

        // MENUS
        private MenuStrip mainMenu = null!;

        // SPLIT
        private SplitContainer mainSplit = null!;

        // LEFT PANEL (Buttons)
        private FlowLayoutPanel leftPanel = null!;
        private Button btnAdd = null!, btnEdit = null!, btnDelete = null!;
        private Button btnCategorize = null!, btnDataManagement = null!;

        // RIGHT PANEL
        private Panel rightPanel = null!;
        private Panel filterPanel = null!;
        private ComboBox cmbFilterCategory = null!;
        private DataGridView productDisplay = null!;

        // ADD/EDIT PANEL
        private Panel addEditPanel = null!;
        private TextBox txtProductName = null!, txtPrice = null!, txtStock = null!, txtDescription = null!;
        private ComboBox cmbCategory = null!;
        private Button btnSave = null!, btnCancel = null!;

        private enum Mode { View, Add, Edit }
        private Mode currentMode = Mode.View;

        public MainForm(User user)
        {
            // XÓA HOẶC BỎ TRỐNG InitializeComponent() ĐỂ TRÁNH TẠO CONTROL TRÙNG
            // InitializeComponent();
            InitializeLayout();
            lblWelcome.Text = "Welcome, " + user.Name;

            // Gọi sự kiện Products để đảm bảo giao diện khởi tạo đúng
            productsToolStripMenuItem_Click(this, EventArgs.Empty);
        }

        private void InitializeLayout()
        {
            // Form
            this.Text = "MainForm";
            this.ClientSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            //
            // 1) Header Panel (Dock=Top)
            //
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.White
            };
            this.Controls.Add(headerPanel);

            lblWelcome = new Label
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Padding = new Padding(10, 10, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(lblWelcome);

            btnLogout = new Button
            {
                Text = "Logout",
                Width = 80,
                Height = 30,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Margin = new Padding(10)
            };
            btnLogout.Click += (s, e) => this.Close();
            headerPanel.Controls.Add(btnLogout);

            //
            // 2) MenuStrip (Dock=Top) — bên dưới Header
            //
            mainMenu = new MenuStrip
            {
                Dock = DockStyle.Top,
                BackColor = Color.White
            };
            var productsItem = new ToolStripMenuItem("Products");
            productsItem.Click += productsToolStripMenuItem_Click;
            var inventoryItem = new ToolStripMenuItem("Inventory");
            inventoryItem.Click += inventoryToolStripMenuItem_Click;
            var ordersItem = new ToolStripMenuItem("Orders");
            ordersItem.Click += ordersToolStripMenuItem_Click;
            var customersItem = new ToolStripMenuItem("Customers");
            customersItem.Click += customersToolStripMenuItem_Click;
            var reportsItem = new ToolStripMenuItem("Reports");
            reportsItem.Click += reportsToolStripMenuItem_Click;

            mainMenu.Items.AddRange(new[]
            {
                productsItem, inventoryItem, ordersItem, customersItem, reportsItem
            });
            this.Controls.Add(mainMenu);

            //
            // 3) SplitContainer (Dock=Fill) — bên dưới MenuStrip
            //
            mainSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 200, // Độ rộng panel trái
                IsSplitterFixed = false
            };
            this.Controls.Add(mainSplit);

            // 3a) Panel trái (FlowLayoutPanel) — chứa nút
            leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                // Tăng top padding để đẩy các nút xuống
                Padding = new Padding(10, 60, 10, 10),
                BackColor = Color.FromArgb(250, 250, 250)
            };
            mainSplit.Panel1.Controls.Add(leftPanel);

            // Tạo các nút
            CreateStyledButton(out btnAdd, "Thêm", btnAdd_Click);
            CreateStyledButton(out btnEdit, "Sửa", btnEdit_Click);
            CreateStyledButton(out btnDelete, "Xóa", btnDelete_Click);
            CreateStyledButton(out btnCategorize, "Phân loại", null);
            CreateStyledButton(out btnDataManagement, "Quản lý dữ liệu", null);

            // Thêm nút vào panel trái
            leftPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnCategorize, btnDataManagement });

            // 3b) Panel phải (chứa Filter + DataGridView)
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            mainSplit.Panel2.Controls.Add(rightPanel);

            // Filter Panel (Dock=Top)
            filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(10, 5, 10, 5),
                BackColor = Color.FromArgb(240, 240, 240)
            };
            rightPanel.Controls.Add(filterPanel);

            var lblFilter = new Label
            {
                Text = "Filter by Category:",
                AutoSize = true,
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 10),
                Padding = new Padding(0, 5, 5, 0)
            };
            filterPanel.Controls.Add(lblFilter);

            cmbFilterCategory = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 200,
                Font = new Font("Segoe UI", 10)
            };
            cmbFilterCategory.SelectedIndexChanged += cmbFilterCategory_SelectedIndexChanged;
            filterPanel.Controls.Add(cmbFilterCategory);

            // DataGridView (Dock=Fill)
            productDisplay = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            productDisplay.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "ProductID", Width = 50 },
                new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "ProductName", Width = 150 },
                new DataGridViewTextBoxColumn { HeaderText = "Category", DataPropertyName = "CategoryID", Width = 80 },
                new DataGridViewTextBoxColumn { HeaderText = "Price", DataPropertyName = "Price", Width = 80 },
                new DataGridViewTextBoxColumn { HeaderText = "Stock", DataPropertyName = "Stock", Width = 80 },
                new DataGridViewTextBoxColumn { HeaderText = "Description", DataPropertyName = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
            });
            rightPanel.Controls.Add(productDisplay);

            // Panel thêm/sửa
            InitializeAddEditPanel();

            // Tải dữ liệu
            LoadCategories();
            LoadProducts();
        }

        private void CreateStyledButton(out Button btn, string text, EventHandler? clickHandler)
        {
            btn = new Button
            {
                Text = text,
                Size = new Size(180, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Visible = true // Đảm bảo nút hiển thị
            };
            if (clickHandler != null) btn.Click += clickHandler;
        }

        private void InitializeAddEditPanel()
        {
            addEditPanel = new Panel
            {
                Visible = false,
                Dock = DockStyle.Top, // Đặt Dock=Top để đẩy DataGridView xuống
                Height = 300, // Tăng chiều cao để chứa toàn bộ nội dung
                AutoScroll = true, // Thêm thanh cuộn nếu nội dung vượt quá
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle // Thêm đường viền
            };
            // Đặt panel này lên trên cùng trong rightPanel
            rightPanel.Controls.Add(addEditPanel);
            rightPanel.Controls.SetChildIndex(addEditPanel, 0);

            var layout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 6,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            addEditPanel.Controls.Add(layout);

            txtProductName = new TextBox { Width = 150 };
            cmbCategory = new ComboBox { Width = 150 };
            txtPrice = new TextBox { Width = 150 };
            txtStock = new TextBox { Width = 150 };
            txtDescription = new TextBox { Width = 150 };

            btnSave = new Button { Text = "Save", Width = 75, BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White };
            btnCancel = new Button { Text = "Cancel", Width = 75, BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White };

            // Thêm label + control
            layout.Controls.Add(new Label { Text = "Product Name:", AutoSize = true, Font = new Font("Segoe UI", 10) }, 0, 0);
            layout.Controls.Add(txtProductName, 1, 0);

            layout.Controls.Add(new Label { Text = "Category:", AutoSize = true, Font = new Font("Segoe UI", 10) }, 0, 1);
            layout.Controls.Add(cmbCategory, 1, 1);

            layout.Controls.Add(new Label { Text = "Price:", AutoSize = true, Font = new Font("Segoe UI", 10) }, 0, 2);
            layout.Controls.Add(txtPrice, 1, 2);

            layout.Controls.Add(new Label { Text = "Stock:", AutoSize = true, Font = new Font("Segoe UI", 10) }, 0, 3);
            layout.Controls.Add(txtStock, 1, 3);

            layout.Controls.Add(new Label { Text = "Description:", AutoSize = true, Font = new Font("Segoe UI", 10) }, 0, 4);
            layout.Controls.Add(txtDescription, 1, 4);

            layout.Controls.Add(btnSave, 0, 5);
            layout.Controls.Add(btnCancel, 1, 5);

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
        }

        // ==================
        // SỰ KIỆN MENU
        // ==================
        private void productsToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            leftPanel.Controls.Clear();
            leftPanel.Controls.AddRange(new Control[] {
                btnAdd,       // Thêm
                btnEdit,      // Sửa
                btnDelete,    // Xóa
                btnCategorize, 
                btnDataManagement
            });

            // Đặt lại vị trí cuộn về đầu
            leftPanel.AutoScrollPosition = new Point(0, 0);
            leftPanel.ScrollControlIntoView(btnAdd);

            // Ensure the Add button is visible
            btnAdd.Visible = true;

            // Kiểm tra trạng thái của btnAdd
            Console.WriteLine("btnAdd.Visible: " + btnAdd.Visible);
            Console.WriteLine("btnAdd.Enabled: " + btnAdd.Enabled);
            Console.WriteLine("leftPanel.Controls.Count: " + leftPanel.Controls.Count);

            LoadProducts();
            HideAddEditPanel();
        }
        private void inventoryToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            leftPanel.Controls.Clear();
            CreateStyledButton(out var btnUpdateInventory, "Cập nhật tồn kho", null);
            CreateStyledButton(out var btnAlertStock, "Cảnh báo hết hàng", null);
            leftPanel.Controls.AddRange(new[] { btnUpdateInventory, btnAlertStock });
            HideAddEditPanel();
        }
        private void ordersToolStripMenuItem_Click(object? sender, EventArgs e) => ShowCustomButtons("Tạo đơn hàng", "Xử lý thanh toán", "In hóa đơn");
        private void customersToolStripMenuItem_Click(object? sender, EventArgs e) => ShowCustomButtons("Lưu thông tin", "Lịch sử mua hàng", "Tích điểm");
        private void reportsToolStripMenuItem_Click(object? sender, EventArgs e) => ShowCustomButtons("Thống kê doanh số", "Xuất báo cáo");

        private void ShowCustomButtons(params string[] texts)
        {
            leftPanel.Controls.Clear();
            foreach (var txt in texts)
            {
                CreateStyledButton(out var btn, txt, null);
                leftPanel.Controls.Add(btn);
            }
            HideAddEditPanel();
        }

        // ==================
        // SỰ KIỆN NÚT
        // ==================
        private void btnAdd_Click(object? sender, EventArgs e)
        {
            currentMode = Mode.Add;
            ClearAddEditPanel();
            ShowAddEditPanel();
        }
        private void btnEdit_Click(object? sender, EventArgs e)
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
        private void btnDelete_Click(object? sender, EventArgs e)
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

        // ==================
        // SỰ KIỆN ADD/EDIT
        // ==================
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
            else if (currentMode == Mode.Edit && productDisplay.SelectedRows.Count > 0)
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
        private void btnCancel_Click(object? sender, EventArgs e) => HideAddEditPanel();

        // ==================
        // HÀM PHỤ TRỢ
        // ==================
        private void ShowAddEditPanel()
        {
            filterPanel.Visible = true;
            addEditPanel.Visible = true;
            addEditPanel.BringToFront();
            rightPanel.Controls.SetChildIndex(addEditPanel, 0); // Đảm bảo ở đầu
        }
        private void HideAddEditPanel()
        {
            addEditPanel.Visible = false;
            filterPanel.Visible = true; // Hiển thị lại filterPanel
            currentMode = Mode.View;
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

        private void cmbFilterCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var selectedCategory = cmbFilterCategory.SelectedItem as Category;
            if (selectedCategory != null)
            {
                if (selectedCategory.CategoryID == 0) LoadProducts();
                else LoadProductsByCategory(selectedCategory.CategoryID);
            }
        }

        private void LoadCategories()
        {
            var categories = new List<Category> { new Category { CategoryID = 0, CategoryName = "All" } };
            categories.AddRange(CategoryManager.GetCategories());
            cmbFilterCategory.DataSource = categories;
            cmbFilterCategory.DisplayMember = "CategoryName";
            cmbFilterCategory.ValueMember = "CategoryID";
            cmbFilterCategory.SelectedIndex = 0;

            // ComboBox trong panel thêm/sửa
            cmbCategory.DataSource = CategoryManager.GetCategories();
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryID";
        }
        private void LoadProducts()
        {
            var products = ProductManager.GetProducts();
            productDisplay.DataSource = new BindingSource { DataSource = products };
        }
        private void LoadProductsByCategory(int categoryId)
        {
            var products = ProductManager.GetProductsByCategory(categoryId);
            productDisplay.DataSource = new BindingSource { DataSource = products };
        }
    }
}
