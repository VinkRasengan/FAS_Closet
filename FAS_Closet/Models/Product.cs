namespace FASCloset.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
    }
}
