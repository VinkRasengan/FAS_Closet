// This file defines the Product class, which represents a product in the system.

namespace FASCloset.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public int CategoryID { get; set; }
        public int? ManufacturerID { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public required string Description { get; set; }
    }
}
