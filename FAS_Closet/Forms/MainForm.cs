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
            ucProductManagement = new UcProductManagement();
            LoadUserControl(ucProductManagement);
        }

        private void btnInventoryManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Cập nhật tồn kho", "Cảnh báo sắp hết" });
            LoadUserControl(new UcInventoryManagement
            {
                txtProductId = new TextBox(),
                txtStockQuantity = new TextBox(),
                dataGridViewLowStock = new DataGridView(),
                TxtSearchProductId = new TextBox()
            });
        }

        private void btnOrderManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Tạo đơn hàng", "Xử lý thanh toán", "In hóa đơn" });
            LoadUserControl(new UcOrderManagement
            {
                txtCustomerId = new TextBox(),
                txtTotalAmount = new TextBox(),
                cmbPaymentMethod = new ComboBox()
            });
        }

        private void btnCustomerManagement_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Lưu thông tin", "Lịch sử mua hàng", "Tích điểm" });
            LoadUserControl(new UcCustomerManagement
            {
                TxtCustomerName = new TextBox(),
                TxtCustomerEmail = new TextBox(),
                TxtCustomerPhone = new TextBox(),
                TxtCustomerAddress = new TextBox(),
                TxtCustomerId = new TextBox(),
                TxtLoyaltyPoints = new TextBox(),
                DataGridViewPurchaseHistory = new DataGridView(),
                TxtSearchCustomer = new TextBox()
            });
        }

        private void btnRevenueReport_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Thống kê doanh số", "Xuất báo cáo chi tiết" });
            LoadUserControl(new UcRevenueReport
            {
                DateTimePickerStartDate = new DateTimePicker(),
                DateTimePickerEndDate = new DateTimePicker(),
                DataGridViewReport = new DataGridView(),
                ProgressBarReport = new ProgressBar()
            });
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            UpdateFeatureToolbar(new string[] { "Sản phẩm bán chạy" });
            LoadUserControl(new UcDashboard());
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
                    FlatStyle = FlatStyle.Flat,
                    Enabled = ucProductManagement != null // Disable if ucProductManagement is not initialized
                };

                // Liên kết sự kiện Click của nút với các phương thức tương ứng trong UcProductManagement
                if (feature == "Thêm")
                {
                    btn.Click += (s, e) =>
                    {
                        if (ucProductManagement != null)
                            ucProductManagement.btnAdd_Click(s ?? this, e);
                        else
                            MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
                    };
                }
                else if (feature == "Sửa")
                {
                    btn.Click += (s, e) =>
                    {
                        if (ucProductManagement != null)
                            ucProductManagement.btnEdit_Click(s ?? this, e);
                        else
                            MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
                    };
                }
                else if (feature == "Xóa")
                {
                    btn.Click += (s, e) =>
                    {
                        if (ucProductManagement != null)
                            ucProductManagement.btnDelete_Click(s ?? this, e);
                        else
                            MessageBox.Show("Vui lòng chọn Quản lý sản phẩm trước.");
                    };
                }
                else
                {
                    // Ví dụ: khi nhấn vào nút tính năng, hiển thị thông báo (bạn có thể gán các sự kiện cụ thể)
                    btn.Click += (s, e) => MessageBox.Show("Chức năng: " + feature);
                }

                featureToolbarPanel.Controls.Add(btn);
            }
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {
            // Phương thức này được để trống có chủ ý.
        }
    }
}
