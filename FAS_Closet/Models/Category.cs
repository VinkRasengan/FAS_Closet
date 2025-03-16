namespace FASCloset.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public required string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
