using System;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class MainForm : Form
    {
        private UcProductManagement? ucProductManagement = null;
        private UcInventoryManagement? ucInventoryManagement = null;
        private UcOrderManagement? ucOrderManagement = null;
        private UcCustomerManagement? ucCustomerManagement = null;
        private UcRevenueReport? ucRevenueReport = null;
        private UcDashboard? ucDashboard = null;

        public MainForm(User user)
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome, " + user.Name;
        }

        // Sự kiện cho nút Logout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Các sự kiện điều hướng load các UserControl và cập nhật thanh tính năng
        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Thêm", "Sửa", "Xóa", "Phân loại" });
            
            if (ucProductManagement == null)
            {
                ucProductManagement = new UcProductManagement();
            }
            
            LoadUserControl(ucProductManagement);
        }

        private void btnInventoryManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Cập nhật tồn kho", "Cảnh báo sắp hết" });
            
            if (ucInventoryManagement == null)
            {
                ucInventoryManagement = new UcInventoryManagement();
                ucInventoryManagement.txtProductId = new TextBox();
                ucInventoryManagement.txtStockQuantity = new TextBox();
                ucInventoryManagement.dataGridViewLowStock = new DataGridView();
                ucInventoryManagement.TxtSearchProductId = new TextBox();
            }
            
            LoadUserControl(ucInventoryManagement);
        }

        private void btnOrderManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Tạo đơn hàng", "Xử lý thanh toán", "In hóa đơn" });
            
            if (ucOrderManagement == null)
            {
                ucOrderManagement = new UcOrderManagement();
                ucOrderManagement.txtCustomerId = new TextBox();
                ucOrderManagement.txtTotalAmount = new TextBox();
                ucOrderManagement.cmbPaymentMethod = new ComboBox();
            }
            
            LoadUserControl(ucOrderManagement);
        }

        private void btnCustomerManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Lưu thông tin", "Lịch sử mua hàng", "Tích điểm" });
            
            if (ucCustomerManagement == null)
            {
                ucCustomerManagement = new UcCustomerManagement();
                ucCustomerManagement.TxtCustomerName = new TextBox();
                ucCustomerManagement.TxtCustomerEmail = new TextBox();
                ucCustomerManagement.TxtCustomerPhone = new TextBox();
                ucCustomerManagement.TxtCustomerAddress = new TextBox();
                ucCustomerManagement.TxtCustomerId = new TextBox();
                ucCustomerManagement.TxtLoyaltyPoints = new TextBox();
                ucCustomerManagement.DataGridViewPurchaseHistory = new DataGridView();
                ucCustomerManagement.TxtSearchCustomer = new TextBox();
            }
            
            LoadUserControl(ucCustomerManagement);
        }

        private void btnRevenueReport_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Thống kê doanh số", "Xuất báo cáo chi tiết" });
            
            if (ucRevenueReport == null)
            {
                ucRevenueReport = new UcRevenueReport();
                ucRevenueReport.DateTimePickerStartDate = new DateTimePicker();
                ucRevenueReport.DateTimePickerEndDate = new DateTimePicker();
                ucRevenueReport.DataGridViewReport = new DataGridView();
                ucRevenueReport.ProgressBarReport = new ProgressBar();
            }
            
            LoadUserControl(ucRevenueReport);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Sản phẩm bán chạy" });
            
            if (ucDashboard == null)
            {
                ucDashboard = new UcDashboard();
            }
            
            LoadUserControl(ucDashboard);
        }

        // Hàm phụ trợ để load một UserControl vào contentPanel
        private void LoadUserControl(UserControl userControl)
        {
            contentPanel.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(userControl);
        }

        // Hàm cập nhật thanh quản lý tính năng dựa trên danh sách các tính năng
        private void UpdateFeatureToolbar(string[] features)
        {
            featureToolbarPanel.Controls.Clear();
            foreach (var feature in features)
            {
                var btn = new Button
                {
                    Text = feature,
                    AutoSize = true,
                    Margin = new Padding(5),
                    BackColor = Color.FromArgb(0, 123, 255),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                // Liên kết sự kiện Click của nút với các phương thức tương ứng trong UcProductManagement
                switch (feature)
                {
                    case "Thêm":
                        btn.Click += (s, e) =>
                        {
                            if (ucProductManagement != null)
                                ucProductManagement.btnAdd_Click(s ?? this, e);
                            else
                                MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
                        };
                        break;
                    case "Sửa":
                        btn.Click += (s, e) =>
                        {
                            if (ucProductManagement != null)
                                ucProductManagement.btnEdit_Click(s ?? this, e);
                            else
                                MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
                        };
                        break;
                    case "Xóa":
                        btn.Click += (s, e) =>
                        {
                            if (ucProductManagement != null)
                                ucProductManagement.btnDelete_Click(s ?? this, e);
                            else
                                MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
                        };
                        break;
                    case "Tạo đơn hàng":
                        btn.Click += (s, e) =>
                        {
                            if (ucOrderManagement != null)
                                ucOrderManagement.btnCreateOrder_Click(s ?? this, e);
                            else
                                MessageBox.Show("Vui lòng chọn Quản lý đơn hàng trước.");
                        };
                        break;
                    case "Cập nhật tồn kho":
                        btn.Click += (s, e) =>
                        {
                            if (ucInventoryManagement != null)
                                ucInventoryManagement.btnUpdateStock_Click(s ?? this, e);
                            else
                                MessageBox.Show("Vui lòng chọn Quản lý kho hàng trước.");
                        };
                        break;
                    default:
                        // Ví dụ: khi nhấn vào nút tính năng, hiển thị thông báo (bạn có thể gán các sự kiện cụ thể)
                        btn.Click += (s, e) => MessageBox.Show("Chức năng: " + feature);
                        break;
                }

                featureToolbarPanel.Controls.Add(btn);
            }
        }
    }
}
