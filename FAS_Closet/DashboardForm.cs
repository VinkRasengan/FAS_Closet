using System;
using System.Windows.Forms;

namespace FASCloset.UI
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            // Tạo MenuStrip
            MenuStrip menuStrip = new MenuStrip();
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Menu "Quản lý sản phẩm"
            ToolStripMenuItem productMenu = new ToolStripMenuItem("Quản lý sản phẩm");
            productMenu.Click += (sender, e) => OpenForm<ProductManagementForm>();
            menuStrip.Items.Add(productMenu);

            // Menu "Quản lý đơn hàng"
            ToolStripMenuItem orderMenu = new ToolStripMenuItem("Quản lý đơn hàng");
            orderMenu.Click += (sender, e) => OpenForm<OrderManagementForm>();
            menuStrip.Items.Add(orderMenu);

            // Menu "Quản lý khách hàng"
            ToolStripMenuItem customerMenu = new ToolStripMenuItem("Quản lý khách hàng");
            customerMenu.Click += (sender, e) => OpenForm<CustomerManagementForm>();
            menuStrip.Items.Add(customerMenu);

            // Menu "Quản lý kho"
            ToolStripMenuItem inventoryMenu = new ToolStripMenuItem("Quản lý kho");
            inventoryMenu.Click += (sender, e) => OpenForm<InventoryManagementForm>();
            menuStrip.Items.Add(inventoryMenu);

            // Menu "Báo cáo"
            ToolStripMenuItem reportMenu = new ToolStripMenuItem("Báo cáo");
            reportMenu.Click += (sender, e) => OpenForm<ReportForm>();
            menuStrip.Items.Add(reportMenu);

            // Menu "Thống kê"
            ToolStripMenuItem statsMenu = new ToolStripMenuItem("Thống kê");
            statsMenu.Click += (sender, e) => OpenForm<StatisticsForm>();
            menuStrip.Items.Add(statsMenu);
        }

        private void OpenForm<T>() where T : Form, new()
        {
            T form = new T();
            form.ShowDialog();
        }

        // Phương thức InitializeComponent do WinForms Designer tạo
        private void InitializeComponent()
        {
            this.Text = "FAS Closet - Dashboard";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
