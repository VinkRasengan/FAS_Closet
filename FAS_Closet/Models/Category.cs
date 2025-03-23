// This file defines the Category class, which represents a category in the system.

namespace FASCloset.Models
{
    public class Category
    {
        // Initialize all properties with default values to avoid any need for `required` keyword
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
