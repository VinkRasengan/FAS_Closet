using System;

// This file defines the Category class, which represents a category in the system.

namespace FASCloset.Models
{
    public class Category
    {
        /// <summary>
        /// The unique identifier for the category
        /// </summary>
        public int CategoryID { get; set; }
        
        /// <summary>
        /// The name of the category
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        
        /// <summary>
        /// Optional description of the category
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Indicates if the category is active or archived
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// The date when the category was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
