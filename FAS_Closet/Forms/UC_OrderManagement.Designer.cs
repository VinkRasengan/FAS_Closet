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

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

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

            // Add Product Button
            Button btnAddProduct = new Button();
            btnAddProduct.Location = new Point(400, 80);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(150, 30);
            btnAddProduct.Text = "Select This Product";
            btnAddProduct.Click += new EventHandler(this.btnAddProduct_Click);

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

            // Main action buttons
            this.btnAdd = new Button();
            this.btnAdd.Location = new Point(20, 300);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(100, 30);
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.BackColor = Color.FromArgb(0, 123, 255);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.Text = "Thêm";
            this.Controls.Add(this.btnAdd);

            this.btnEdit = new Button();
            this.btnEdit.Location = new Point(130, 300);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(100, 30);
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.BackColor = Color.FromArgb(40, 167, 69);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.Cursor = Cursors.Hand;
            this.btnEdit.Text = "Sửa";
            this.Controls.Add(this.btnEdit);

            this.btnDelete = new Button();
            this.btnDelete.Location = new Point(240, 300);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(100, 30);
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.Text = "Xóa";
            this.Controls.Add(this.btnDelete);

            this.btnRefresh = new Button();
            this.btnRefresh.Location = new Point(350, 300);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(100, 30);
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Font = new Font("Segoe UI", 10F);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Cursor = Cursors.Hand;
            this.btnRefresh.Text = "Làm mới";
            this.Controls.Add(this.btnRefresh);

            this.Name = "UcOrderManagement";
            this.Size = new System.Drawing.Size(550, 400);
        }

        #endregion
    }
}
