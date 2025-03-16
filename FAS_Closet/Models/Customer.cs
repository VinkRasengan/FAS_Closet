namespace FASCloset.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
