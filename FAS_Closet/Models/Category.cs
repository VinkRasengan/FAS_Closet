// This file defines the Category class, which represents a category in the system.

namespace FASCloset.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public required string CategoryName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
