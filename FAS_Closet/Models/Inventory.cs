// This file defines the Inventory class, which represents the inventory of a product.

using System;

namespace FASCloset.Models
{
    public class Inventory
    {
        /// <summary>
        /// Unique identifier for the inventory record
        /// </summary>
        public int InventoryID { get; set; }
        
        /// <summary>
        /// The product ID that this inventory record is associated with
        /// </summary>
        public int ProductID { get; set; }
        
        /// <summary>
        /// The warehouse ID where this inventory is located
        /// </summary>
        public int WarehouseID { get; set; }
        
        /// <summary>
        /// The current quantity in stock at this warehouse
        /// </summary>
        public int StockQuantity { get; set; }
        
        /// <summary>
        /// The minimum threshold for triggering low stock warnings at this warehouse
        /// </summary>
        public int MinimumStockThreshold { get; set; }
        
        /// <summary>
        /// Navigation property to the associated Product (not stored in database)
        /// </summary>
        public Product? Product { get; set; }
        
        /// <summary>
        /// Navigation property to the associated Warehouse (not stored in database)
        /// </summary>
        public Warehouse? Warehouse { get; set; }
        
        /// <summary>
        /// Calculated property to determine if the stock is low
        /// </summary>
        public bool IsLowStock => StockQuantity <= MinimumStockThreshold;
    }
}
