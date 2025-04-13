namespace FASCloset.Forms
{
    partial class UcOrderManagement
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // UI Controls
        public TextBox txtCustomerId;
        public TextBox txtTotalAmount;
        public ComboBox cmbPaymentMethod;
        private DataGridView dgvOrders;
        public ComboBox cmbProduct;
        public TextBox txtQuantity;
        private List<OrderDetail> orderDetails = new List<OrderDetail>();
        
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
            
            // Customer ID
            this.txtCustomerId = new TextBox();
            this.txtCustomerId.Location = new System.Drawing.Point(120, 20);
            this.txtCustomerId.Name = "txtCustomerId";
            this.txtCustomerId.Size = new System.Drawing.Size(150, 23);
            
            // Total Amount
            this.txtTotalAmount = new TextBox();
            this.txtTotalAmount.Location = new System.Drawing.Point(120, 50);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(150, 23);
            
            // Payment Method
            this.cmbPaymentMethod = new ComboBox();
            this.cmbPaymentMethod.Location = new System.Drawing.Point(120, 80);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(150, 23);
            this.cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Create Order Button
            Button btnCreateOrder = new Button();
            btnCreateOrder.Location = new System.Drawing.Point(120, 110);
            btnCreateOrder.Name = "btnCreateOrder";
            btnCreateOrder.Size = new System.Drawing.Size(150, 30);
            btnCreateOrder.Text = "Create Order";
            btnCreateOrder.UseVisualStyleBackColor = true;
            btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
            
            // Orders Grid
            this.dgvOrders = new DataGridView();
            this.dgvOrders.Location = new System.Drawing.Point(20, 150);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.Size = new System.Drawing.Size(500, 200);
            this.dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Process Payment Button
            Button btnProcessPayment = new Button();
            btnProcessPayment.Location = new System.Drawing.Point(20, 360);
            btnProcessPayment.Name = "btnProcessPayment";
            btnProcessPayment.Size = new System.Drawing.Size(150, 30);
            btnProcessPayment.Text = "Process Payment";
            btnProcessPayment.UseVisualStyleBackColor = true;
            btnProcessPayment.Click += new System.EventHandler(this.btnProcessPayment_Click);
            
            // Print Invoice Button
            Button btnPrintInvoice = new Button();
            btnPrintInvoice.Location = new System.Drawing.Point(180, 360);
            btnPrintInvoice.Name = "btnPrintInvoice";
            btnPrintInvoice.Size = new System.Drawing.Size(150, 30);
            btnPrintInvoice.Text = "Print Invoice";
            btnPrintInvoice.UseVisualStyleBackColor = true;
            btnPrintInvoice.Click += new System.EventHandler(this.btnPrintInvoice_Click);
            
            // Labels
            Label lblCustomerId = new Label();
            lblCustomerId.Location = new System.Drawing.Point(20, 23);
            lblCustomerId.Size = new System.Drawing.Size(100, 23);
            lblCustomerId.Text = "Customer ID:";
            
            Label lblTotalAmount = new Label();
            lblTotalAmount.Location = new System.Drawing.Point(20, 53);
            lblTotalAmount.Size = new System.Drawing.Size(100, 23);
            lblTotalAmount.Text = "Total Amount:";
            
            Label lblPaymentMethod = new Label();
            lblPaymentMethod.Location = new System.Drawing.Point(20, 83);
            lblPaymentMethod.Size = new System.Drawing.Size(100, 23);
            lblPaymentMethod.Text = "Payment Method:";

            // Product Dropdown
            ComboBox cmbProduct = new ComboBox();
            cmbProduct.Location = new Point(300, 20);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(300, 23);
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProduct = cmbProduct;

            // Quantity Input
            TextBox txtQuantity = new TextBox();
            txtQuantity.Location = new Point(300, 50);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(150, 23);
            this.txtQuantity = txtQuantity;

            // Add Product Button
            Button btnAddProduct = new Button();
            btnAddProduct.Location = new Point(300, 80);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(150, 30);
            btnAddProduct.Text = "Add Product";
            btnAddProduct.Click += new EventHandler(this.btnAddProduct_Click);

            // Product Label
            Label lblProduct = new Label();
            lblProduct.Location = new Point(220, 23);
            lblProduct.Size = new Size(80, 23);
            lblProduct.Text = "Product:";

            // Quantity Label
            Label lblQuantity = new Label();
            lblQuantity.Location = new Point(220, 53);
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
            this.Controls.Add(this.txtCustomerId);
            this.Controls.Add(lblTotalAmount);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(lblPaymentMethod);
            this.Controls.Add(this.cmbPaymentMethod);
            this.Controls.Add(btnCreateOrder);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(btnProcessPayment);
            this.Controls.Add(btnPrintInvoice);
            
            this.Name = "UcOrderManagement";
            this.Size = new System.Drawing.Size(550, 400);
        }

        #endregion
    }
}
