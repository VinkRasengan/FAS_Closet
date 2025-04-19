using System;
using System.Drawing;
using System.Windows.Forms;

namespace FASCloset.Extensions
{
    /// <summary>
    /// Helper class for applying consistent styles to DataGridViews across the application
    /// </summary>
    public static class DataGridViewStyleHelper
    {
        /// <summary>
        /// Apply basic styling to a DataGridView (colors, fonts, borders)
        /// </summary>
        /// <param name="dataGridView">The DataGridView to style</param>
        public static void ApplyBasicStyle(DataGridView dataGridView)
        {
            if (dataGridView == null)
                return;
                
            // Styling
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.GridColor = Color.FromArgb(230, 230, 230);
            
            // Alternative row coloring
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView.DefaultCellStyle.BackColor = Color.White;
            
            // Font settings
            dataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            
            // Selection styling
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 215, 229);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            
            // Row styling
            dataGridView.RowTemplate.Height = 35;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            
            // Selection behavior
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        
        /// <summary>
        /// Apply full styling including header styling to a DataGridView
        /// </summary>
        /// <param name="dataGridView">The DataGridView to style</param>
        /// <param name="headerBackColor">Optional background color for headers</param>
        public static void ApplyFullStyle(DataGridView dataGridView, Color? headerBackColor = null)
        {
            if (dataGridView == null)
                return;
                
            // Apply basic styling first
            ApplyBasicStyle(dataGridView);
            
            // Disable default header styling
            dataGridView.EnableHeadersVisualStyles = false;
            
            // Header styling
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = headerBackColor ?? Color.FromArgb(37, 150, 190);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersHeight = 40;
            
            // Hide row headers
            dataGridView.RowHeadersVisible = false;
        }
        
        /// <summary>
        /// Apply specific column formatting based on column content type
        /// </summary>
        /// <param name="dataGridView">The DataGridView to format</param>
        public static void ApplyColumnFormatting(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.Columns.Count == 0)
                return;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                string headerText = column.HeaderText.ToLower();
                
                // Currency columns
                if (headerText.Contains("tiền") || headerText.Contains("giá") || 
                    headerText.Contains("thu") || headerText.Contains("chi") ||
                    headerText.Contains("total") || headerText.Contains("price") || 
                    headerText.Contains("amount"))
                {
                    column.DefaultCellStyle.Format = "N0";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.DefaultCellStyle.ForeColor = Color.FromArgb(31, 111, 139);
                    column.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                }
                
                // Date columns
                else if (headerText.Contains("ngày") || headerText.Contains("date"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                
                // ID columns
                else if (headerText.Contains("mã") || headerText.Contains("id"))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                
                // Text columns that might need word wrap
                else if (headerText.Contains("tên") || headerText.Contains("name") || 
                         headerText.Contains("mô tả") || headerText.Contains("description"))
                {
                    column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    if (column.Width > 250)
                        column.Width = 250;
                }
                
                // Payment method/status columns
                else if (headerText.Contains("thanh toán") || headerText.Contains("payment") || 
                         headerText.Contains("status") || headerText.Contains("trạng thái"))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }
        
        /// <summary>
        /// Apply sorted row style highlighting based on a condition
        /// </summary>
        /// <param name="dataGridView">The DataGridView to apply conditional formatting to</param>
        /// <param name="rowFormatFunc">Function that returns a cell style for each row</param>
        public static void ApplyConditionalFormatting(DataGridView dataGridView, Func<DataGridViewRow, DataGridViewCellStyle?> rowFormatFunc)
        {
            if (dataGridView == null || rowFormatFunc == null)
                return;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var style = rowFormatFunc(row);
                if (style != null)
                {
                    row.DefaultCellStyle = style;
                }
            }
        }
        
        /// <summary>
        /// Create a highlight style for important rows
        /// </summary>
        /// <param name="backColor">Background color</param>
        /// <param name="foreColor">Foreground color</param>
        /// <returns>A DataGridViewCellStyle for highlighting</returns>
        public static DataGridViewCellStyle CreateHighlightStyle(Color backColor, Color foreColor)
        {
            var style = new DataGridViewCellStyle
            {
                BackColor = backColor,
                ForeColor = foreColor,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold)
            };
            
            return style;
        }
    }
}