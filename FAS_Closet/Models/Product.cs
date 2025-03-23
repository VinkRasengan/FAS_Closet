// This file defines the Product class, which represents a product in the system.

namespace FASCloset.Models
{
    public class Product
    {
        // Initialize all properties with default values to avoid null reference exceptions
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public int? ManufacturerID { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties - not stored in database but useful for displaying related data
        public string CategoryName { get; set; } = string.Empty;
        public string ManufacturerName { get; set; } = string.Empty;
        
        // Calculated property for low stock status
        public bool IsLowStock { get; set; }
        
        // Properties for sales reporting
        public int SalesCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Revenue { get; set; }
    }
}
