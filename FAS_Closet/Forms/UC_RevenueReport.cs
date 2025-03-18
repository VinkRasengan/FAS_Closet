using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using FASCloset.Services;
using System.ComponentModel; // Add this line

namespace FASCloset.Forms
{
    public partial class UcRevenueReport : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required DateTimePicker DateTimePickerStartDate { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required DateTimePicker DateTimePickerEndDate { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public required DataGridView DataGridViewReport { get; set; }

        private BackgroundWorker backgroundWorker; // Add this line

        public UcRevenueReport()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            DateTimePickerStartDate = new DateTimePicker();
            DateTimePickerEndDate = new DateTimePicker();
            DataGridViewReport = new DataGridView();
            backgroundWorker = new BackgroundWorker(); // Add this line
            backgroundWorker.DoWork += BackgroundWorker_DoWork; // Add this line
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted; // Add this line
            // Initialize other components and set properties
            this.Controls.Add(DateTimePickerStartDate);
            this.Controls.Add(DateTimePickerEndDate);
            this.Controls.Add(DataGridViewReport);
        }

        private void btnGenerateSalesReport_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync("GenerateSalesReport"); // Modify this line
            }
        }

        private void btnExportDetailedReport_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync("ExportDetailedReport"); // Modify this line
            }
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e) // Update nullability
        {
            string task = e.Argument as string;
            if (task == "GenerateSalesReport")
            {
                DateTime startDate = DateTimePickerStartDate.Value;
                DateTime endDate = DateTimePickerEndDate.Value;
                e.Result = ReportManager.GenerateSalesReport(startDate, endDate);
            }
            else if (task == "ExportDetailedReport")
            {
                DateTime startDate = DateTimePickerStartDate.Value;
                DateTime endDate = DateTimePickerEndDate.Value;
                var reportData = ReportManager.GenerateDetailedSalesReport(startDate, endDate);
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = "DetailedSalesReport.csv"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (DataColumn column in reportData.Columns)
                        {
                            writer.Write(column.ColumnName + ",");
                        }
                        writer.WriteLine();
                        foreach (DataRow row in reportData.Rows)
                        {
                            foreach (var item in row.ItemArray)
                            {
                                writer.Write(item + ",");
                            }
                            writer.WriteLine();
                        }
                    }
                }
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e) // Update nullability
        {
            if (e.Error != null)
            {
                MessageBox.Show("An error occurred: " + e.Error.Message);
            }
            else if (e.Result is DataTable reportData)
            {
                DataGridViewReport.DataSource = reportData;
            }
        }
    }
}
