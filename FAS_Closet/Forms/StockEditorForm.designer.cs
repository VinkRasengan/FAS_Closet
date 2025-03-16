namespace FASCloset.Forms
{
    partial class StockEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelStockQuantity;
        private System.Windows.Forms.TextBox txtStockQuantity;
        private System.Windows.Forms.Button btnSave;

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
            this.labelStockQuantity = new System.Windows.Forms.Label();
            this.txtStockQuantity = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelStockQuantity
            // 
            this.labelStockQuantity.AutoSize = true;
            this.labelStockQuantity.Location = new System.Drawing.Point(12, 15);
            this.labelStockQuantity.Name = "labelStockQuantity";
            this.labelStockQuantity.Size = new System.Drawing.Size(79, 13);
            this.labelStockQuantity.TabIndex = 0;
            this.labelStockQuantity.Text = "Stock Quantity";
            // 
            // txtStockQuantity
            // 
            this.txtStockQuantity.Location = new System.Drawing.Point(97, 12);
            this.txtStockQuantity.Name = "txtStockQuantity";
            this.txtStockQuantity.Size = new System.Drawing.Size(175, 20);
            this.txtStockQuantity.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(197, 38);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // StockEditorForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 71);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtStockQuantity);
            this.Controls.Add(this.labelStockQuantity);
            this.Name = "StockEditorForm";
            this.Text = "Stock Editor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
