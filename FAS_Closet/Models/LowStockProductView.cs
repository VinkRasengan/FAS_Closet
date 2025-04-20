// This file defines the LowStockProductView class, which represents a product with low stock in the system.

namespace FASCloset.Models
{
    public class LowStockProductView
    {
        // Basic product properties
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public int? ManufacturerID { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public string CategoryName { get; set; } = string.Empty;
        public string ManufacturerName { get; set; } = string.Empty;
        
        // Inventory specific properties
        public int StockQuantity { get; set; }
        public int MinimumStockThreshold { get; set; }
        
        // Helper properties for display
        public string DisplayName => $"{ProductName} - {Price:C}";
        
        // Calculated property to determine stock status
        public bool IsOutOfStock => StockQuantity <= 0;
        
        // Calculated property to determine how critical the stock level is (as percentage)
        public double StockPercentage => MinimumStockThreshold > 0 ? 
            Math.Min(100, Math.Max(0, (double)StockQuantity / MinimumStockThreshold * 100)) : 0;
            
        // Text representation of stock status
        public string StockStatus 
        {
            get
            {
                if (IsOutOfStock) return "Hết hàng";
                if (StockQuantity <= MinimumStockThreshold * 0.25) return "Cực kỳ thấp";
                if (StockQuantity <= MinimumStockThreshold * 0.5) return "Rất thấp";
                if (StockQuantity <= MinimumStockThreshold) return "Thấp";
                return "Đủ";
            }
        }
    }
}