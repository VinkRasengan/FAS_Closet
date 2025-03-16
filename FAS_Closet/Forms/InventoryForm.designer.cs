namespace FASCloset.Forms
{
    partial class InventoryForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewInventory;
        private System.Windows.Forms.Button btnUpdateStock;
        private System.Windows.Forms.Button btnSetThreshold;
        private System.Windows.Forms.Button btnCheckLowStock;

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
            this.components = new System.ComponentModel.Container();
            this.dataGridViewInventory = new System.Windows.Forms.DataGridView();
            this.btnUpdateStock = new System.Windows.Forms.Button();
            this.btnSetThreshold = new System.Windows.Forms.Button();
            this.btnCheckLowStock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewInventory
            // 
            this.dataGridViewInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInventory.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewInventory.Name = "dataGridViewInventory";
            this.dataGridViewInventory.Size = new System.Drawing.Size(560, 300);
            this.dataGridViewInventory.TabIndex = 0;
            // 
            // btnUpdateStock
            // 
            this.btnUpdateStock.Location = new System.Drawing.Point(12, 318);
            this.btnUpdateStock.Name = "btnUpdateStock";
            this.btnUpdateStock.Size = new System.Drawing.Size(100, 23);
            this.btnUpdateStock.TabIndex = 1;
            this.btnUpdateStock.Text = "Update Stock";
            this.btnUpdateStock.UseVisualStyleBackColor = true;
            this.btnUpdateStock.Click += new System.EventHandler(this.btnUpdateStock_Click);
            // 
            // btnSetThreshold
            // 
            this.btnSetThreshold.Location = new System.Drawing.Point(118, 318);
            this.btnSetThreshold.Name = "btnSetThreshold";
            this.btnSetThreshold.Size = new System.Drawing.Size(100, 23);
            this.btnSetThreshold.TabIndex = 2;
            this.btnSetThreshold.Text = "Set Threshold";
            this.btnSetThreshold.UseVisualStyleBackColor = true;
            this.btnSetThreshold.Click += new System.EventHandler(this.btnSetThreshold_Click);
            // 
            // btnCheckLowStock
            // 
            this.btnCheckLowStock.Location = new System.Drawing.Point(224, 318);
            this.btnCheckLowStock.Name = "btnCheckLowStock";
            this.btnCheckLowStock.Size = new System.Drawing.Size(100, 23);
            this.btnCheckLowStock.TabIndex = 3;
            this.btnCheckLowStock.Text = "Check Low Stock";
            this.btnCheckLowStock.UseVisualStyleBackColor = true;
            this.btnCheckLowStock.Click += new System.EventHandler(this.btnCheckLowStock_Click);
            // 
            // InventoryForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btnCheckLowStock);
            this.Controls.Add(this.btnSetThreshold);
            this.Controls.Add(this.btnUpdateStock);
            this.Controls.Add(this.dataGridViewInventory);
            this.Name = "InventoryForm";
            this.Text = "Inventory Management";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
