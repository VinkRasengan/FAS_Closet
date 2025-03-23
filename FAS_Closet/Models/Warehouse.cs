using System;

namespace FASCloset.Models
{
    /// <summary>
    /// Represents a warehouse or inventory location in the system
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// Unique identifier for the warehouse
        /// </summary>
        public int WarehouseID { get; set; }
        
        /// <summary>
        /// Name of the warehouse
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Physical address of the warehouse
        /// </summary>
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// User ID of the manager/owner of this warehouse
        /// </summary>
        public int ManagerUserID { get; set; }
        
        /// <summary>
        /// When the warehouse was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Whether this warehouse is active
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Description or notes about this warehouse
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// User/manager name for display (not stored in database)
        /// </summary>
        public string ManagerName { get; set; } = string.Empty;
    }
}
