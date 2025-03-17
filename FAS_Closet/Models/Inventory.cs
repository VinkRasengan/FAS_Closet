// This file defines the Inventory class, which represents the inventory of a product.

namespace FASCloset.Models
{
    public class Inventory
    {
        public int ProductID { get; set; }
        public int StockQuantity { get; set; }
        public int MinimumStockThreshold { get; set; }
    }
}
