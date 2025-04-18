using System;
using System.Drawing;
using System.Windows.Forms;

namespace FASCloset.Extensions
{
    /// <summary>
    /// Lớp tiện ích để áp dụng style đồng nhất cho tất cả các DataGridView trong ứng dụng
    /// </summary>
    public static class DataGridViewStyleHelper
    {
        /// <summary>
        /// Áp dụng style cơ bản cho DataGridView để đồng bộ giao diện toàn ứng dụng
        /// </summary>
        /// <param name="dataGridView">DataGridView cần áp dụng style</param>
        public static void ApplyBasicStyle(DataGridView dataGridView)
        {
            if (dataGridView == null)
                return;

            // Thiết lập thuộc tính cơ bản
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.GridColor = Color.FromArgb(230, 230, 230);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Định dạng tiêu đề cột
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 150, 190);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersHeight = 40;

            // Định dạng dòng
            dataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 215, 229);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView.RowTemplate.Height = 35;
        }

        /// <summary>
        /// Áp dụng định dạng cho các cột trong DataGridView theo kiểu dữ liệu hoặc tên cột
        /// </summary>
        /// <param name="dataGridView">DataGridView cần định dạng cột</param>
        public static void FormatColumns(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.Columns.Count == 0)
                return;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Định dạng cột tiền tệ
                if (column.HeaderText.Contains("tiền") || column.HeaderText.Contains("thu") || 
                    column.HeaderText.Contains("giá") || column.HeaderText.Contains("chi") ||
                    column.HeaderText.Contains("phí"))
                {
                    column.DefaultCellStyle.Format = "N0";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.DefaultCellStyle.ForeColor = Color.FromArgb(31, 111, 139);
                    column.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
                }
                
                // Định dạng cột ngày tháng
                if (column.HeaderText.Contains("Ngày") || column.HeaderText.Contains("ngày") ||
                    column.HeaderText.Contains("Date") || column.HeaderText.Contains("date"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                
                // Định dạng cột mã (ID)
                if (column.HeaderText.Contains("Mã") || column.HeaderText.Contains("ID"))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                
                // Giới hạn chiều rộng cột và thêm tooltip khi nội dung bị cắt
                if (column.HeaderText.Contains("Tên") || column.HeaderText.Contains("Name") || 
                    column.HeaderText.Contains("Email") || column.HeaderText.Contains("Địa chỉ") || 
                    column.HeaderText.Contains("Mô tả") || column.HeaderText.Contains("Description"))
                {
                    column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    column.MinimumWidth = 150;
                }
            }

            // Auto resize các cột theo nội dung tốt nhất
            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            
            // Giới hạn chiều rộng tối đa của cột
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Width > 250)
                    column.Width = 250;
            }
        }

        /// <summary>
        /// Áp dụng tất cả các style cho DataGridView (kết hợp ApplyBasicStyle và FormatColumns)
        /// </summary>
        /// <param name="dataGridView">DataGridView cần áp dụng toàn bộ style</param>
        public static void ApplyFullStyle(DataGridView dataGridView)
        {
            ApplyBasicStyle(dataGridView);
            FormatColumns(dataGridView);
        }
    }
}