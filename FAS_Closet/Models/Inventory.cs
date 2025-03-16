namespace FASCloset.Models
{
    public class Inventory
    {
        public int ProductID { get; set; }
        public int StockQuantity { get; set; }
        public int MinimumStockThreshold { get; set; }
    }
}
