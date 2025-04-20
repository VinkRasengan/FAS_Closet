using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public class OrderForm : Form
    {
        private readonly ErrorProvider _errorProvider = new ErrorProvider();
        private Order _order;
        private List<OrderDetail> _orderDetails = new List<OrderDetail>();
        private List<Product> _products;
        private List<Customer> _customers;
        private bool _isEditMode = false;
        private decimal _totalAmount = 0;

        public Order Order => _order;
        public List<OrderDetail> OrderDetails => _orderDetails;

        public OrderForm()
        {
            InitializeComponent();
            
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.Size = new Size(800, 600);

            _order = new Order
            {
                OrderDate = DateTime.Now
            };
            
            LoadDataSources();
            SetupDataGridView();
            UpdateTotalAmount();
        }

        public OrderForm(Order order, List<OrderDetail> orderDetails) : this()
        {
            _isEditMode = true;
            _order = order;
            _orderDetails = orderDetails ?? new List<OrderDetail>();
            
            LoadOrderData();
            UpdateTotalAmount();
        }

        private ComboBox cmbCustomers;
        private ComboBox cmbPaymentMethod;
        private ComboBox cmbProducts;
        private TextBox txtQuantity;
        private TextBox txtTotalAmount;
        private DateTimePicker dtpOrderDate;
        private DataGridView dgvOrderDetails;
        private Button btnAddProduct;
        private Button btnRemoveProduct;
        private Button btnSaveOrder;
        private Button btnCancel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form setup
            this.Text = _isEditMode ? "Chi tiết đơn hàng" : "Tạo đơn hàng mới";
            this.BackColor = Color.White;

            // Customer Selection
            Label lblCustomer = new Label
            {
                Text = "Khách hàng:",
                Location = new Point(30, 30),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            cmbCustomers = new ComboBox
            {
                Location = new Point(160, 30),
                Size = new Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cmbCustomers.Validating += CmbCustomers_Validating;

            // Order Date
            Label lblOrderDate = new Label
            {
                Text = "Ngày đặt hàng:",
                Location = new Point(30, 70),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            dtpOrderDate = new DateTimePicker
            {
                Location = new Point(160, 70),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Short
            };

            // Payment Method
            Label lblPaymentMethod = new Label
            {
                Text = "Phương thức thanh toán:",
                Location = new Point(380, 70),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            cmbPaymentMethod = new ComboBox
            {
                Location = new Point(540, 70),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            
            // Initialize payment methods
            cmbPaymentMethod.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ tín dụng" });
            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            // Products selection panel
            Panel pnlAddProduct = new Panel
            {
                Location = new Point(30, 110),
                Size = new Size(740, 60),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };

            Label lblProduct = new Label
            {
                Text = "Sản phẩm:",
                Location = new Point(10, 18),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            cmbProducts = new ComboBox
            {
                Location = new Point(100, 18),
                Size = new Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cmbProducts.SelectedValueChanged += CmbProducts_SelectedValueChanged;

            Label lblQuantity = new Label
            {
                Text = "Số lượng:",
                Location = new Point(420, 18),
                Size = new Size(70, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            txtQuantity = new TextBox
            {
                Location = new Point(500, 18),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 9.5F),
                Text = "1"
            };
            txtQuantity.KeyPress += TxtQuantity_KeyPress;

            btnAddProduct = new Button
            {
                Text = "+",
                Location = new Point(600, 15),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddProduct.FlatAppearance.BorderSize = 0;
            btnAddProduct.Click += BtnAddProduct_Click;

            btnRemoveProduct = new Button
            {
                Text = "-",
                Location = new Point(650, 15),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRemoveProduct.FlatAppearance.BorderSize = 0;
            btnRemoveProduct.Click += BtnRemoveProduct_Click;

            // Add controls to panel
            pnlAddProduct.Controls.Add(lblProduct);
            pnlAddProduct.Controls.Add(cmbProducts);
            pnlAddProduct.Controls.Add(lblQuantity);
            pnlAddProduct.Controls.Add(txtQuantity);
            pnlAddProduct.Controls.Add(btnAddProduct);
            pnlAddProduct.Controls.Add(btnRemoveProduct);

            // Order Details Grid
            Label lblOrderDetails = new Label
            {
                Text = "Chi tiết đơn hàng:",
                Location = new Point(30, 180),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            dgvOrderDetails = new DataGridView
            {
                Location = new Point(30, 210),
                Size = new Size(740, 280),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            // Setup columns for the DataGridView
            dgvOrderDetails.Columns.Add("ProductID", "Mã SP");
            dgvOrderDetails.Columns.Add("ProductName", "Tên sản phẩm");
            dgvOrderDetails.Columns.Add("UnitPrice", "Đơn giá");
            dgvOrderDetails.Columns.Add("Quantity", "Số lượng");
            dgvOrderDetails.Columns.Add("Total", "Thành tiền");
            
            dgvOrderDetails.Columns["ProductID"].Visible = false;
            dgvOrderDetails.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            dgvOrderDetails.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderDetails.Columns["Total"].DefaultCellStyle.Format = "N0";
            dgvOrderDetails.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Total amount
            Label lblTotal = new Label
            {
                Text = "Tổng tiền:",
                Location = new Point(500, 500),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10F)
            };

            txtTotalAmount = new TextBox
            {
                Location = new Point(590, 500),
                Size = new Size(180, 25),
                ReadOnly = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right
            };

            // Buttons
            btnSaveOrder = new Button
            {
                Text = _isEditMode ? "Cập nhật" : "Tạo đơn hàng",
                Location = new Point(500, 540),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSaveOrder.FlatAppearance.BorderSize = 0;
            btnSaveOrder.Click += BtnSaveOrder_Click;
            btnSaveOrder.Enabled = !_isEditMode;  // Disable in edit mode (view-only)

            btnCancel = new Button
            {
                Text = "Đóng",
                Location = new Point(640, 540),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            // Add controls to form
            this.Controls.Add(lblCustomer);
            this.Controls.Add(cmbCustomers);
            this.Controls.Add(lblOrderDate);
            this.Controls.Add(dtpOrderDate);
            this.Controls.Add(lblPaymentMethod);
            this.Controls.Add(cmbPaymentMethod);
            this.Controls.Add(pnlAddProduct);
            this.Controls.Add(lblOrderDetails);
            this.Controls.Add(dgvOrderDetails);
            this.Controls.Add(lblTotal);
            this.Controls.Add(txtTotalAmount);
            this.Controls.Add(btnSaveOrder);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
            
            // Handle form closing
            this.FormClosing += OrderForm_FormClosing;
        }

        private void OrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _errorProvider.Clear();
        }

        private void LoadDataSources()
        {
            try
            {
                // Load customers
                _customers = CustomerManager.GetCustomers();
                cmbCustomers.DisplayMember = "Name";
                cmbCustomers.ValueMember = "CustomerID";
                cmbCustomers.DataSource = _customers;

                // Load products
                _products = ProductManager.GetProducts();
                cmbProducts.DisplayMember = "ProductName";
                cmbProducts.ValueMember = "ProductID";
                cmbProducts.DataSource = _products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            // Apply styling
            Color headerColor = Color.FromArgb(0, 123, 255);
            FASCloset.Extensions.DataGridViewStyleHelper.ApplyBasicStyle(dgvOrderDetails, headerColor);
        }

        private void LoadOrderData()
        {
            // Load order data into form controls
            if (_order != null)
            {
                // Set customer
                if (_customers != null && _customers.Any())
                {
                    var customer = _customers.FirstOrDefault(c => c.CustomerID == _order.CustomerID);
                    if (customer != null)
                    {
                        int customerIndex = _customers.IndexOf(customer);
                        if (customerIndex >= 0)
                            cmbCustomers.SelectedIndex = customerIndex;
                    }
                }

                // Set date
                dtpOrderDate.Value = _order.OrderDate;

                // Set payment method
                int paymentIndex = -1;
                switch (_order.PaymentMethod?.ToLower())
                {
                    case "tiền mặt":
                        paymentIndex = 0;
                        break;
                    case "chuyển khoản":
                        paymentIndex = 1;
                        break;
                    case "thẻ tín dụng":
                        paymentIndex = 2;
                        break;
                }
                if (paymentIndex >= 0 && paymentIndex < cmbPaymentMethod.Items.Count)
                    cmbPaymentMethod.SelectedIndex = paymentIndex;

                // Disable editing in view mode
                cmbCustomers.Enabled = false;
                dtpOrderDate.Enabled = false;
                cmbPaymentMethod.Enabled = false;
                cmbProducts.Enabled = false;
                txtQuantity.Enabled = false;
                btnAddProduct.Enabled = false;
                btnRemoveProduct.Enabled = false;

                // Load order details
                UpdateOrderDetailsGrid();
            }
        }

        private void UpdateOrderDetailsGrid()
        {
            dgvOrderDetails.Rows.Clear();

            foreach (var detail in _orderDetails)
            {
                var product = _products?.FirstOrDefault(p => p.ProductID == detail.ProductID);
                string productName = product?.ProductName ?? "Unknown Product";
                decimal total = detail.Quantity * detail.UnitPrice;

                dgvOrderDetails.Rows.Add(
                    detail.ProductID,
                    productName,
                    detail.UnitPrice,
                    detail.Quantity,
                    total
                );
            }

            UpdateTotalAmount();
        }

        private void UpdateTotalAmount()
        {
            _totalAmount = _orderDetails.Sum(d => d.Quantity * d.UnitPrice);
            txtTotalAmount.Text = _totalAmount.ToString("N0");
        }

        private void CmbProducts_SelectedValueChanged(object sender, EventArgs e)
        {
            txtQuantity.Text = "1";  // Reset quantity on product change
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int productId = Convert.ToInt32(cmbProducts.SelectedValue);
            var product = _products.FirstOrDefault(p => p.ProductID == productId);
            
            if (product == null)
            {
                MessageBox.Show("Không tìm thấy thông tin sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if product already exists in order
            var existingDetail = _orderDetails.FirstOrDefault(d => d.ProductID == productId);
            if (existingDetail != null)
            {
                // Update quantity if product already exists
                existingDetail.Quantity += quantity;
            }
            else
            {
                // Add new order detail
                _orderDetails.Add(new OrderDetail
                {
                    ProductID = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }

            UpdateOrderDetailsGrid();
        }

        private void BtnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetails.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dgvOrderDetails.SelectedRows[0].Cells["ProductID"].Value);
                var detailToRemove = _orderDetails.FirstOrDefault(d => d.ProductID == productId);
                
                if (detailToRemove != null)
                {
                    _orderDetails.Remove(detailToRemove);
                    UpdateOrderDetailsGrid();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSaveOrder_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                if (_orderDetails.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào đơn hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    // Update order information
                    _order.CustomerID = Convert.ToInt32(cmbCustomers.SelectedValue);
                    _order.OrderDate = dtpOrderDate.Value;
                    _order.PaymentMethod = cmbPaymentMethod.SelectedItem.ToString();
                    _order.TotalAmount = _totalAmount;

                    // We'll handle saving in the calling form for better transaction handling
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #region Validation Methods
        private void CmbCustomers_Validating(object sender, CancelEventArgs e)
        {
            if (cmbCustomers.SelectedIndex == -1)
            {
                _errorProvider.SetError(cmbCustomers, "Vui lòng chọn khách hàng");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(cmbCustomers, "");
            }
        }
        #endregion
    }
}