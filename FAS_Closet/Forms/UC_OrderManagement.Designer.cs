namespace FASCloset.Forms
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
        private Button btnCreateCustomer;
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

            btnCreateCustomer = new Button();
            btnCreateCustomer.Location = new Point(270, 20); // cạnh cmbCustomer
            btnCreateCustomer.Size = new Size(25, 23);
            btnCreateCustomer.Text = "+";
            btnCreateCustomer.Name = "btnCreateCustomer";
            btnCreateCustomer.Click += new EventHandler(this.HandleCustomerAdd);
            this.Controls.Add(btnCreateCustomer);

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
            this.dgvDraftOrders.Size = new System.Drawing.Size(500, 150); // Adjust size
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
           
            
            // Orders Grid
            this.dgvOrders = new DataGridView();
            this.dgvOrders.Location = new System.Drawing.Point(20, 600);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.Size = new System.Drawing.Size(500, 200);
            this.dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Print Invoice Button
            Button btnPrintInvoice = new Button();
            btnPrintInvoice.Location = new System.Drawing.Point(20, 530);
            btnPrintInvoice.Name = "btnPrintInvoice";
            btnPrintInvoice.Size = new System.Drawing.Size(150, 30);
            btnPrintInvoice.Text = "Print Invoice";
            btnPrintInvoice.UseVisualStyleBackColor = true;
            btnPrintInvoice.Click += new System.EventHandler(this.btnPrintInvoice_Click);

            // Create Order Button
            Button btnCreateOrder = new Button();
            btnCreateOrder.Location = new System.Drawing.Point(20, 300);
            btnCreateOrder.Name = "btnCreateOrder";
            btnCreateOrder.Size = new System.Drawing.Size(150, 30);
            btnCreateOrder.Text = "Create Order";
            btnCreateOrder.UseVisualStyleBackColor = true;
            btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);

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

            // Total Amount TextBox (mới)
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
            this.Controls.Add(btnCreateOrder);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(btnPrintInvoice);
            
            this.Name = "UcOrderManagement";
            this.Size = new System.Drawing.Size(550, 400);
        }

        #endregion
    }
}
