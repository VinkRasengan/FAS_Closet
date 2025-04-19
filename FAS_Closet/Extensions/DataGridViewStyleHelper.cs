using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FASCloset.Extensions
{
    /// <summary>
    /// Helper class for DataGridView styling and visibility optimization
    /// </summary>
    public static class DataGridViewStyleHelper
    {
        /// <summary>
        /// Applies a complete style and optimizes visibility of all columns
        /// </summary>
        public static void ApplyCompleteVisibilityOptimization(DataGridView dgv, Color headerBackColor)
        {
            // 1. Basic style
            ApplyBasicStyle(dgv, headerBackColor);
            
            // 2. Optimize column widths to show all content
            OptimizeColumnWidths(dgv);
            
            // 3. Apply enhanced cell styles for better visibility
            ApplyEnhancedCellStyles(dgv);
            
            // 4. Ensure all columns are visible
            EnsureColumnsVisible(dgv);
            
            // 5. Configure events to maintain visibility when size changes
            AttachResizeEvent(dgv);
        }
        
        /// <summary>
        /// Applies basic styling to all tables in the application
        /// </summary>
        public static void ApplyBasicStyle(DataGridView dgv, Color headerBackColor)
        {
            // General DataGridView style
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // Alternate row color
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.Fixed3D;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            
            // Header styles
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = headerBackColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerBackColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersHeight = 35;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            
            // Row styles
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 28;
            dgv.RowTemplate.DefaultCellStyle.Padding = new Padding(2);
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 100, 200); // Dark blue for selection
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            
            // Selection style
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Disable column reordering to maintain consistency
            dgv.AllowUserToOrderColumns = false;
            
            // Make the grid look modern
            dgv.EnableHeadersVisualStyles = false;
            
            // Scroll bars always visible
            dgv.ScrollBars = ScrollBars.Both;
        }
        
        /// <summary>
        /// Applies basic styling to all tables in the application using the default color
        /// </summary>
        public static void ApplyBasicStyle(DataGridView dgv)
        {
            Color defaultBlueHeader = Color.FromArgb(0, 123, 255); // Bootstrap primary blue
            ApplyBasicStyle(dgv, defaultBlueHeader);
        }
        
        /// <summary>
        /// Applies complete styling to a DataGridView
        /// </summary>
        public static void ApplyFullStyle(DataGridView dgv)
        {
            Color defaultBlueHeader = Color.FromArgb(0, 123, 255); // Bootstrap primary blue
            ApplyCompleteVisibilityOptimization(dgv, defaultBlueHeader);
        }

        /// <summary>
        /// Applies complete styling to a DataGridView with a custom color
        /// </summary>
        public static void ApplyFullStyle(DataGridView dgv, Color headerColor)
        {
            ApplyCompleteVisibilityOptimization(dgv, headerColor);
        }
        
        /// <summary>
        /// Optimizes column widths to show all content
        /// </summary>
        public static void OptimizeColumnWidths(DataGridView dgv)
        {
            try
            {
                // First try adjusting columns to their content
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                
                // If there are few columns, we can use Fill to utilize available space
                if (dgv.Columns.Count <= 5)
                {
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    return;
                }
                
                // Set minimum and maximum widths for each column to prevent
                // some from being too narrow or too wide
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    // Calculate optimal width based on content of the first 10 rows
                    int optimalWidth = CalculateOptimalColumnWidth(dgv, column.Index);
                    
                    // Set reasonable minimum and maximum limits
                    int minWidth = 70;  // Minimum width for readability
                    int maxWidth = 250; // Maximum width to avoid taking too much space
                    
                    // Use a width that's within limits
                    column.Width = Math.Max(minWidth, Math.Min(optimalWidth, maxWidth));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error optimizing column widths: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Optimizes columns so all are visible and content is readable
        /// </summary>
        public static void OptimizeVisibility(DataGridView dgv)
        {
            OptimizeColumnWidths(dgv);
            EnsureColumnsVisible(dgv);
        }

        /// <summary>
        /// Calculates the optimal width for a column based on its content
        /// </summary>
        private static int CalculateOptimalColumnWidth(DataGridView dgv, int columnIndex)
        {
            // If there's no data, use a default value
            if (dgv.Rows.Count == 0)
                return 100;
            
            try
            {
                // Calculate space for the header
                string headerText = dgv.Columns[columnIndex].HeaderText;
                int headerWidth = TextRenderer.MeasureText(headerText, dgv.ColumnHeadersDefaultCellStyle.Font).Width + 15;
                
                // Find the widest cell in the first 10 rows (or fewer if there aren't that many)
                int maxCellWidth = 0;
                int rowsToCheck = Math.Min(10, dgv.Rows.Count);
                
                for (int i = 0; i < rowsToCheck; i++)
                {
                    var cell = dgv.Rows[i].Cells[columnIndex];
                    if (cell.Value != null)
                    {
                        string cellText = cell.Value?.ToString() ?? string.Empty;
                        int cellWidth = TextRenderer.MeasureText(cellText, dgv.DefaultCellStyle.Font).Width + 10;
                        maxCellWidth = Math.Max(maxCellWidth, cellWidth);
                    }
                }
                
                // Use the maximum between header width and cell content width
                return Math.Max(headerWidth, maxCellWidth);
            }
            catch
            {
                // In case of error, return a default value
                return 100;
            }
        }
        
        /// <summary>
        /// Applies enhanced cell styles for better visibility
        /// </summary>
        private static void ApplyEnhancedCellStyles(DataGridView dgv)
        {
            if (dgv.Columns.Count == 0) return;
            
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                // Check data type to apply specific styles
                string propertyName = column.DataPropertyName;
                
                // Do nothing if there's no associated property
                if (string.IsNullOrEmpty(propertyName)) continue;
                
                ApplyNumericStyles(dgv, column, propertyName);
                ApplyDateStyles(column, propertyName);
                ApplyStatusStyles(dgv, column, propertyName);
                ApplyIdStyles(dgv, column, propertyName);
            }
        }
        
        private static void ApplyNumericStyles(DataGridView dgv, DataGridViewColumn column, string propertyName)
        {
            // Style for numeric columns (right alignment)
            if (propertyName.Contains("Amount") || 
                propertyName.Contains("Price") || 
                propertyName.Contains("Total") ||
                propertyName.Contains("Quantity") ||
                propertyName.Contains("Stock"))
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                // Give it a slightly different background color
                column.DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 240);
                
                // If it's a monetary column, apply currency format
                if (propertyName.Contains("Amount") || propertyName.Contains("Price") || propertyName.Contains("Total"))
                {
                    column.DefaultCellStyle.Format = "N0";
                    column.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font, FontStyle.Bold);
                }
            }
        }
        
        private static void ApplyDateStyles(DataGridViewColumn column, string propertyName)
        {
            // Style for dates (localized date format)
            if (propertyName.Contains("Date") || propertyName.Contains("Time"))
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Format = "dd/MM/yyyy";
                // Soft background color for dates
                column.DefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            }
        }
        
        private static void ApplyStatusStyles(DataGridView dgv, DataGridViewColumn column, string propertyName)
        {
            // Style for status fields or booleans
            if (propertyName.Contains("Status") || propertyName.Contains("Active") || propertyName.Contains("Enabled"))
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // Use the CellFormatting event to change colors based on values
                dgv.CellFormatting += (sender, e) => 
                {
                    if (e.ColumnIndex == column.Index && e.Value != null && e.CellStyle != null)
                    {
                        string value = e.Value.ToString()?.ToLower() ?? string.Empty;
                        if (value == "true" || value == "active" || value == "yes" || value == "1")
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new Font(dgv.DefaultCellStyle.Font, FontStyle.Bold);
                        }
                        else if (value == "false" || value == "inactive" || value == "no" || value == "0")
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                    }
                };
            }
        }
        
        private static void ApplyIdStyles(DataGridView dgv, DataGridViewColumn column, string propertyName) 
        {
            // Style for IDs (smaller and less prominent)
            if (propertyName.EndsWith("ID"))
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 
                                                       dgv.DefaultCellStyle.Font.Size - 0.5f);
                column.DefaultCellStyle.ForeColor = Color.DarkGray;
            }
        }

        /// <summary>
        /// Formats DataGridView columns based on data types
        /// </summary>
        public static void ApplyColumnFormatting(DataGridView dgv)
        {
            if (dgv == null || dgv.Columns.Count == 0) return;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                // Check the column's data type if it has a DataPropertyName
                if (!string.IsNullOrEmpty(column.DataPropertyName))
                {
                    ApplyFormatBasedOnColumnName(column);
                }
            }
        }

        /// <summary>
        /// Applies formatting based on column name
        /// </summary>
        private static void ApplyFormatBasedOnColumnName(DataGridViewColumn column)
        {
            string columnName = column.DataPropertyName.ToLower();

            // Format for numeric and monetary columns
            if (columnName.Contains("price") || columnName.Contains("cost") || 
                columnName.Contains("amount") || columnName.Contains("total") ||
                columnName.Contains("quantity") || columnName.Contains("stock") || 
                columnName.Contains("count") || columnName.Contains("number"))
            {
                column.DefaultCellStyle.Format = "N0"; // Number format without decimals
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            // Format for date columns
            else if (columnName.Contains("date") || columnName.Contains("time"))
            {
                column.DefaultCellStyle.Format = "dd/MM/yyyy";
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // Format for status columns
            else if (columnName.Contains("status") || columnName.Contains("state"))
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Applies conditional formatting to DataGridView rows based on an evaluation function
        /// </summary>
        /// <param name="dgv">DataGridView to format</param>
        /// <param name="evaluateRow">Function that evaluates each row and returns a style or null</param>
        public static void ApplyConditionalFormatting(DataGridView dgv, Func<DataGridViewRow, DataGridViewCellStyle?> evaluateRow)
        {
            if (dgv == null || evaluateRow == null) return;

            // Configure event to apply conditional formatting
            dgv.CellFormatting -= ConditionalFormatting_Event;
            dgv.RowPrePaint -= ConditionalRowFormatting_Event;

            // Save the evaluation function to use it in the event
            dgv.Tag = evaluateRow;
            
            // Attach events
            dgv.RowPrePaint += ConditionalRowFormatting_Event;
        }

        /// <summary>
        /// Creates a highlight style for a cell or row
        /// </summary>
        /// <param name="backColor">Background color</param>
        /// <param name="foreColor">Text color</param>
        /// <param name="isBold">Whether text should be bold</param>
        /// <returns>A style to apply to cells or rows</returns>
        public static DataGridViewCellStyle CreateHighlightStyle(Color backColor, Color foreColor, bool isBold = false)
        {
            var style = new DataGridViewCellStyle
            {
                BackColor = backColor,
                ForeColor = foreColor
            };

            if (isBold)
            {
                style.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Bold);
            }

            return style;
        }

        /// <summary>
        /// Default font for styles
        /// </summary>
        private static Font DefaultFont => new Font("Segoe UI", 9F);

        /// <summary>
        /// Event to apply conditional formatting to rows
        /// </summary>
        private static void ConditionalRowFormatting_Event(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (sender is DataGridView dgv && dgv.Tag is Func<DataGridViewRow, DataGridViewCellStyle?> evaluateRow && e.RowIndex >= 0 && e.RowIndex < dgv.Rows.Count)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                var style = evaluateRow(row);

                if (style != null)
                {
                    row.DefaultCellStyle = style;
                }
            }
        }

        /// <summary>
        /// Event to apply conditional formatting to cells
        /// </summary>
        private static void ConditionalFormatting_Event(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            // This method is used for conditional formatting at cell level if needed
        }
        
        /// <summary>
        /// Ensures all columns are visible
        /// </summary>
        private static void EnsureColumnsVisible(DataGridView dgv)
        {
            // Check that all columns are properly visible
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible && column.Width < 60) 
                {
                    // If a column is visible but too narrow, increase its width
                    column.Width = 60;
                }
            }
            
            AdjustLastColumnWidth(dgv);
        }
        
        private static void AdjustLastColumnWidth(DataGridView dgv)
        {
            // Ensure that the last column isn't partially visible
            if (dgv.Columns.Count > 0)
            {
                var lastVisibleColumn = GetLastVisibleColumn(dgv);
                if (lastVisibleColumn != null)
                {
                    // If the last column is partially visible, adjust its width
                    int availableWidth = dgv.Width - dgv.RowHeadersWidth;
                    int usedWidth = 0;
                    
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible && column != lastVisibleColumn)
                        {
                            usedWidth += column.Width;
                        }
                    }
                    
                    int remainingWidth = availableWidth - usedWidth;
                    if (remainingWidth > 50) // If there's enough space
                    {
                        lastVisibleColumn.Width = Math.Max(lastVisibleColumn.Width, remainingWidth - 5);
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets the last visible column in the DataGridView
        /// </summary>
        private static DataGridViewColumn? GetLastVisibleColumn(DataGridView dgv)
        {
            for (int i = dgv.Columns.Count - 1; i >= 0; i--)
            {
                if (dgv.Columns[i].Visible)
                {
                    return dgv.Columns[i];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Attaches a resize event to maintain visibility
        /// </summary>
        private static void AttachResizeEvent(DataGridView dgv)
        {
            // Remove any previous handler
            dgv.ClientSizeChanged -= DataGridView_ClientSizeChanged;
            
            // Add handler
            dgv.ClientSizeChanged += DataGridView_ClientSizeChanged;
        }
        
        /// <summary>
        /// Handler for resize event
        /// </summary>
        private static void DataGridView_ClientSizeChanged(object? sender, EventArgs e)
        {
            if (sender is DataGridView dgv)
            {
                // Re-optimize columns when size changes
                OptimizeColumnWidths(dgv);
                EnsureColumnsVisible(dgv);
            }
        }
        
        /// <summary>
        /// Adds row numbers for better reference
        /// </summary>
        public static void AddRowNumbers(DataGridView dgv)
        {
            // Ensure row headers are visible
            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidth = 50;
            
            // Configure event to display row numbers
            dgv.RowPostPaint -= RowPostPaint_Event;
            dgv.RowPostPaint += RowPostPaint_Event;
        }
        
        /// <summary>
        /// Event to draw row numbers
        /// </summary>
        private static void RowPostPaint_Event(object? sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            
            // Row number format
            string rowNumber = (e.RowIndex + 1).ToString();
            
            // Style for row number
            var centerFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            
            // Area to draw
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, 
                                          grid.RowHeadersWidth, e.RowBounds.Height);
            
            // Draw the number with soft shadow for readability
            e.Graphics.DrawString(rowNumber, grid.Font, new SolidBrush(Color.FromArgb(80, 0, 0, 0)), 
                                headerBounds, centerFormat);
            e.Graphics.DrawString(rowNumber, grid.Font, Brushes.Black, headerBounds, centerFormat);
        }
        
        /// <summary>
        /// Shows a message when no data is available
        /// </summary>
        public static void ShowNoDataMessage(DataGridView dgv, string message = "No data available")
        {
            if (dgv.Rows.Count == 0)
            {
                // Clear any existing columns
                dgv.Columns.Clear();
                
                // Add a column to show the message
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "NoDataColumn",
                    HeaderText = "",
                    Width = dgv.Width - 5
                });
                
                // Add a row with the message
                dgv.Rows.Add(message);
                
                // Center the text
                dgv.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Rows[0].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Italic);
                dgv.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                
                // Don't allow selecting this row
                dgv.Rows[0].Selected = false;
            }
        }

        /// <summary>
        /// Configures the DataGridView with basic style and standard cell format
        /// </summary>
        public static void ConfigureGridForStandardView(DataGridView dgv)
        {
            ApplyFullStyle(dgv);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
        }

        /// <summary>
        /// Configures the DataGridView with basic style and format for data entry
        /// </summary>
        public static void ConfigureGridForDataEntry(DataGridView dgv)
        {
            ApplyFullStyle(dgv);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = true;
            dgv.AllowUserToDeleteRows = true;
            dgv.ReadOnly = false;
        }
    }
}