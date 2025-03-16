namespace FASCloset.Forms
{
    partial class ThresholdEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.TextBox txtThreshold;
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
            this.labelThreshold = new System.Windows.Forms.Label();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(12, 15);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(54, 13);
            this.labelThreshold.TabIndex = 0;
            this.labelThreshold.Text = "Threshold";
            // 
            // txtThreshold
            // 
            this.txtThreshold.Location = new System.Drawing.Point(72, 12);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.Size = new System.Drawing.Size(200, 20);
            this.txtThreshold.TabIndex = 1;
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
            // ThresholdEditorForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 71);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.labelThreshold);
            this.Name = "ThresholdEditorForm";
            this.Text = "Threshold Editor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
