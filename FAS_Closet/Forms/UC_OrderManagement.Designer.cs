namespace FASCloset.Forms
{
    partial class UcOrderManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            
            // Initialize customer ID controls
            Label lblCustomerId = new Label();
            lblCustomerId.Text = "Customer ID:";
            lblCustomerId.Location = new Point(20, 20);
            lblCustomerId.Size = new Size(100, 25);
            lblCustomerId.TextAlign = ContentAlignment.MiddleRight;
            
            txtCustomerId = new TextBox();
            txtCustomerId.Location = new Point(130, 20);
            txtCustomerId.Size = new Size(200, 25);
            txtCustomerId.PlaceholderText = "Enter Customer ID";
            
            // Initialize total amount controls
            Label lblTotalAmount = new Label();
            lblTotalAmount.Text = "Total Amount:";
            lblTotalAmount.Location = new Point(20, 60);
            lblTotalAmount.Size = new Size(100, 25);
            lblTotalAmount.TextAlign = ContentAlignment.MiddleRight;
            
            txtTotalAmount = new TextBox();
            txtTotalAmount.Location = new Point(130, 60);
            txtTotalAmount.Size = new Size(200, 25);
            txtTotalAmount.PlaceholderText = "Enter Total Amount";
            
            // Initialize payment method controls
            Label lblPaymentMethod = new Label();
            lblPaymentMethod.Text = "Payment Method:";
            lblPaymentMethod.Location = new Point(20, 100);
            lblPaymentMethod.Size = new Size(100, 25);
            lblPaymentMethod.TextAlign = ContentAlignment.MiddleRight;
            
            cmbPaymentMethod = new ComboBox();
            cmbPaymentMethod.Location = new Point(130, 100);
            cmbPaymentMethod.Size = new Size(200, 25);
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Initialize buttons
            Button btnCreateOrder = new Button();
            btnCreateOrder.Text = "Create Order";
            btnCreateOrder.Location = new Point(130, 140);
            btnCreateOrder.Size = new Size(120, 30);
            btnCreateOrder.Click += btnCreateOrder_Click;
            
            Button btnProcessPayment = new Button();
            btnProcessPayment.Text = "Process Payment";
            btnProcessPayment.Location = new Point(260, 140);
            btnProcessPayment.Size = new Size(120, 30);
            btnProcessPayment.Click += btnProcessPayment_Click;
            
            Button btnPrintInvoice = new Button();
            btnPrintInvoice.Text = "Print Invoice";
            btnPrintInvoice.Location = new Point(130, 180);
            btnPrintInvoice.Size = new Size(120, 30);
            btnPrintInvoice.Click += btnPrintInvoice_Click;
            
            // Initialize orders grid
            DataGridView dgvOrders = new DataGridView();
            dgvOrders.Location = new Point(20, 220);
            dgvOrders.Size = new Size(600, 300);
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.ReadOnly = true;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Add all controls to the form
            this.Controls.Add(lblCustomerId);
            this.Controls.Add(txtCustomerId);
            this.Controls.Add(lblTotalAmount);
            this.Controls.Add(txtTotalAmount);
            this.Controls.Add(lblPaymentMethod);
            this.Controls.Add(cmbPaymentMethod);
            this.Controls.Add(btnCreateOrder);
            this.Controls.Add(btnProcessPayment);
            this.Controls.Add(btnPrintInvoice);
            this.Controls.Add(dgvOrders);
        }

        public TextBox txtCustomerId;
        public TextBox txtTotalAmount;
        public ComboBox cmbPaymentMethod;
        private Button btnCreateOrder;
        private Button btnProcessPayment;
        private Button btnPrintInvoice;
        private DataGridView dgvOrders;
    }
}
