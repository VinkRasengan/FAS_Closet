﻿namespace FASCloset.Forms
{
    partial class UcOrderManagement
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // UI Controls
        public TextBox txtTotalAmount;
        public ComboBox cmbPaymentMethod;
        private DataGridView dgvOrders;
        public ComboBox cmbProduct;
        public TextBox txtQuantity;
        private List<OrderDetail> orderDetails = new List<OrderDetail>();
        public ComboBox cmbCustomer;
        private DataGridView productList;
        private DataGridView dgvDraftOrders;
        private Label lblSuccessOrder;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.cmbCustomer = new ComboBox();
            this.cmbCustomer.Location = new System.Drawing.Point(120, 20);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(150, 23);
            this.cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblProductDetails = new Label();
            lblProductDetails.Text = "Order Product Detail";
            lblProductDetails.Location = new Point(20, 140);  // Set location above the product list table
            lblProductDetails.Size = new Size(200, 20);  // Set size of the label
            lblProductDetails.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblProductDetails.ForeColor = Color.Black;
            this.Controls.Add(lblProductDetails);

            this.productList = new DataGridView();
            this.productList.Location = new System.Drawing.Point(20, 160);  // Below the label
            this.productList.Name = "productList";
            this.productList.Size = new System.Drawing.Size(500, 130);
            this.productList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.productList.AllowUserToAddRows = false;
            this.productList.ReadOnly = true;
            this.productList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.productList.ColumnHeadersVisible = true; // Ensure column headers are visible
            this.productList.BackColor = Color.White;
            this.Controls.Add(this.productList);

            Label draftOrderDetail = new Label();
            draftOrderDetail.Text = "Draft Orders";
            draftOrderDetail.Location = new Point(20, 350);  // Set location above the product list table
            draftOrderDetail.Size = new Size(200, 20);  // Set size of the label
            draftOrderDetail.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            draftOrderDetail.ForeColor = Color.Black;
            this.Controls.Add(draftOrderDetail);

            this.dgvDraftOrders = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDraftOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDraftOrders
            // 
            this.dgvDraftOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDraftOrders.Location = new System.Drawing.Point(20, 370); 
            this.dgvDraftOrders.Name = "dgvDraftOrders";
            this.dgvDraftOrders.Size = new System.Drawing.Size(500, 80); // Adjust size
            this.dgvDraftOrders.TabIndex = 0;
            this.Controls.Add(this.dgvDraftOrders);

            ((System.ComponentModel.ISupportInitialize)(this.dgvDraftOrders)).EndInit();
            this.ResumeLayout(false);

            // Payment Method
            this.cmbPaymentMethod = new ComboBox();
            this.cmbPaymentMethod.Location = new System.Drawing.Point(120, 50);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(150, 23);
            this.cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;

            Label orderDetail = new Label();
            orderDetail.Text = "Success Orders";
            orderDetail.Location = new Point(20, 480);  // Set location above the product list table
            orderDetail.Size = new Size(200, 20);  // Set size of the label
            orderDetail.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            orderDetail.ForeColor = Color.Black;
            this.Controls.Add(orderDetail);

            // Orders Grid
            this.dgvOrders = new DataGridView();
            this.dgvOrders.Location = new System.Drawing.Point(20, 500);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.Size = new System.Drawing.Size(500, 200);
            this.dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.lblSuccessOrder = new Label();
            this.lblSuccessOrder.Name = "lblSuccessOrder";
            this.lblSuccessOrder.Text = "";
            this.lblSuccessOrder.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblSuccessOrder.ForeColor = Color.Green;
            this.lblSuccessOrder.Size = new Size(500, 30);
            this.lblSuccessOrder.Location = new Point(20, 335);
            this.lblSuccessOrder.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(this.lblSuccessOrder);

            // Labels
            Label lblCustomerId = new Label();
            lblCustomerId.Location = new System.Drawing.Point(20, 23);
            lblCustomerId.Size = new System.Drawing.Size(100, 23);
            lblCustomerId.Text = "Customer ID:";

            Label lblTotalAmount = new Label();
            lblTotalAmount.Location = new Point(20, 80);
            lblTotalAmount.Size = new Size(100, 23);
            lblTotalAmount.Text = "Total Amount:";

            // Total Amount TextBox
            this.txtTotalAmount = new TextBox();
            this.txtTotalAmount.Location = new Point(120, 80);
            this.txtTotalAmount.Size = new Size(150, 23);
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.BackColor = SystemColors.Window;
            this.txtTotalAmount.ForeColor = Color.Black;

            // Add vào form
            this.Controls.Add(lblTotalAmount);
            this.Controls.Add(this.txtTotalAmount);

            Label lblPaymentMethod = new Label();
            lblPaymentMethod.Location = new System.Drawing.Point(20, 53);
            lblPaymentMethod.Size = new System.Drawing.Size(100, 23);
            lblPaymentMethod.Text = "Payment Method:";

            // Product Dropdown
            ComboBox cmbProduct = new ComboBox();
            cmbProduct.Location = new Point(400, 20);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(300, 23);
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProduct = cmbProduct;

            // Quantity Input
            TextBox txtQuantity = new TextBox();
            txtQuantity.Location = new Point(400, 50);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(150, 23);
            this.txtQuantity = txtQuantity;

            Button btnAddProduct = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 123, 255), // Màu xanh biển
                ForeColor = Color.White, // Màu chữ trắng
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 }, // Không có viền
                Cursor = Cursors.Hand,
                Text = "Select This Product",
                Location = new Point(400, 80),
                Size = new Size(150, 30) // Kích thước nút
            };

            // Bo góc với bán kính 30
            btnAddProduct.Region = new Region(GetRoundRectangle(btnAddProduct.ClientRectangle, 30));

            // Thêm hiệu ứng hover
            btnAddProduct.MouseEnter += (sender, e) =>
            {
                btnAddProduct.BackColor = Color.FromArgb(28, 89, 164); // Màu xanh đậm khi hover
            };
            btnAddProduct.MouseLeave += (sender, e) =>
            {
                btnAddProduct.BackColor = Color.FromArgb(0, 123, 255); // Màu xanh biển khi không hover
            };

            // Đăng ký sự kiện click
            btnAddProduct.Click += new EventHandler(this.btnAddProduct_Click);

            // Thêm nút vào form
            this.Controls.Add(btnAddProduct);

            // Product Label
            Label lblProduct = new Label();
            lblProduct.Location = new Point(300, 23);
            lblProduct.Size = new Size(80, 23);
            lblProduct.Text = "Product:";

            // Quantity Label
            Label lblQuantity = new Label();
            lblQuantity.Location = new Point(300, 53);
            lblQuantity.Size = new Size(80, 23);
            lblQuantity.Text = "Quantity:";

            // Add new controls
            this.Controls.Add(lblProduct);
            this.Controls.Add(this.cmbProduct);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(btnAddProduct);

            // Add controls
            this.Controls.Add(lblCustomerId);
            this.Controls.Add(this.cmbCustomer);

            this.Controls.Add(lblPaymentMethod);
            this.Controls.Add(this.cmbPaymentMethod);
            this.Controls.Add(this.dgvOrders);        

            this.Name = "UcOrderManagement";
            this.Size = new System.Drawing.Size(550, 400);
        }

        #endregion
    }
}
