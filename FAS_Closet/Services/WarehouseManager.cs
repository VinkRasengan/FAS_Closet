using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Models;
using FASCloset.Data;
using Microsoft.Data.Sqlite;

namespace FASCloset.Services
{
    public static class WarehouseManager
    {
        // Define constants for column names to address warning S1192
        private const string COL_ADDRESS = "Address";
        private const string COL_DESCRIPTION = "Description";
        private const string COL_MANAGER_NAME = "ManagerName";
        private const string COL_WAREHOUSE_ID = "WarehouseID";
        private const string COL_NAME = "Name";
        private const string COL_IS_ACTIVE = "IsActive";
        
        /// <summary>
        /// Get all warehouses in the system
        /// </summary>
        public static List<Warehouse> GetWarehouses(bool includeInactive = false)
        {
            string query = @"
                SELECT w.*, u.Name as ManagerName 
                FROM Warehouse w
                LEFT JOIN User u ON w.ManagerUserID = u.UserID
                WHERE w.IsActive = 1 OR @IncludeInactive = 1
                ORDER BY w.Name";
                
            var parameters = new Dictionary<string, object>
            {
                { "@IncludeInactive", includeInactive ? 1 : 0 }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader => new Warehouse
            {
                WarehouseID = reader.GetInt32(reader.GetOrdinal(COL_WAREHOUSE_ID)),
                Name = reader.GetString(reader.GetOrdinal(COL_NAME)),
                Address = reader.IsDBNull(reader.GetOrdinal(COL_ADDRESS)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_ADDRESS)),
                ManagerUserID = reader.GetInt32(reader.GetOrdinal("ManagerUserID")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                IsActive = reader.GetBoolean(reader.GetOrdinal(COL_IS_ACTIVE)),
                Description = reader.IsDBNull(reader.GetOrdinal(COL_DESCRIPTION)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_DESCRIPTION)),
                ManagerName = reader.IsDBNull(reader.GetOrdinal(COL_MANAGER_NAME)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_MANAGER_NAME))
            }, parameters);
        }
        
        /// <summary>
        /// Get warehouses managed by a specific user
        /// </summary>
        public static List<Warehouse> GetWarehousesByUser(int userId)
        {
            string query = @"
                SELECT w.*, u.Name as ManagerName 
                FROM Warehouse w
                LEFT JOIN User u ON w.ManagerUserID = u.UserID
                WHERE w.ManagerUserID = @UserID AND w.IsActive = 1
                ORDER BY w.Name";
                
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader => new Warehouse
            {
                WarehouseID = reader.GetInt32(reader.GetOrdinal(COL_WAREHOUSE_ID)),
                Name = reader.GetString(reader.GetOrdinal(COL_NAME)),
                Address = reader.IsDBNull(reader.GetOrdinal(COL_ADDRESS)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_ADDRESS)),
                ManagerUserID = reader.GetInt32(reader.GetOrdinal("ManagerUserID")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                IsActive = reader.GetBoolean(reader.GetOrdinal(COL_IS_ACTIVE)),
                Description = reader.IsDBNull(reader.GetOrdinal(COL_DESCRIPTION)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_DESCRIPTION)),
                ManagerName = reader.IsDBNull(reader.GetOrdinal(COL_MANAGER_NAME)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_MANAGER_NAME))
            }, parameters);
        }
        
        /// <summary>
        /// Get a warehouse by ID
        /// </summary>
        public static Warehouse? GetWarehouseById(int warehouseId)
        {
            string query = @"
                SELECT w.*, u.Name as ManagerName 
                FROM Warehouse w
                LEFT JOIN User u ON w.ManagerUserID = u.UserID
                WHERE w.WarehouseID = @WarehouseID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@WarehouseID", warehouseId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, reader => new Warehouse
            {
                WarehouseID = reader.GetInt32(reader.GetOrdinal(COL_WAREHOUSE_ID)),
                Name = reader.GetString(reader.GetOrdinal(COL_NAME)),
                Address = reader.IsDBNull(reader.GetOrdinal(COL_ADDRESS)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_ADDRESS)),
                ManagerUserID = reader.GetInt32(reader.GetOrdinal("ManagerUserID")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                IsActive = reader.GetBoolean(reader.GetOrdinal(COL_IS_ACTIVE)),
                Description = reader.IsDBNull(reader.GetOrdinal(COL_DESCRIPTION)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_DESCRIPTION)),
                ManagerName = reader.IsDBNull(reader.GetOrdinal(COL_MANAGER_NAME)) ? string.Empty : reader.GetString(reader.GetOrdinal(COL_MANAGER_NAME))
            }, parameters);
        }
        
        /// <summary>
        /// Add a new warehouse
        /// </summary>
        public static int AddWarehouse(Warehouse warehouse)
        {
            string query = @"
                INSERT INTO Warehouse (Name, Address, ManagerUserID, Description, IsActive)
                VALUES (@Name, @Address, @ManagerUserID, @Description, @IsActive);
                SELECT last_insert_rowid();";
                
            var parameters = new Dictionary<string, object>
            {
                { "@Name", warehouse.Name },
                { "@Address", warehouse.Address ?? string.Empty },
                { "@ManagerUserID", warehouse.ManagerUserID },
                { "@Description", warehouse.Description ?? string.Empty },
                { "@IsActive", warehouse.IsActive ? 1 : 0 }
            };
            
            return DataAccessHelper.ExecuteScalar<int>(query, parameters);
        }
        
        /// <summary>
        /// Update an existing warehouse
        /// </summary>
        public static void UpdateWarehouse(Warehouse warehouse)
        {
            string query = @"
                UPDATE Warehouse 
                SET Name = @Name,
                    Address = @Address,
                    ManagerUserID = @ManagerUserID,
                    Description = @Description,
                    IsActive = @IsActive
                WHERE WarehouseID = @WarehouseID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@WarehouseID", warehouse.WarehouseID },
                { "@Name", warehouse.Name },
                { "@Address", warehouse.Address ?? string.Empty },
                { "@ManagerUserID", warehouse.ManagerUserID },
                { "@Description", warehouse.Description ?? string.Empty },
                { "@IsActive", warehouse.IsActive ? 1 : 0 }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
        
        /// <summary>
        /// Delete a warehouse (mark as inactive)
        /// </summary>
        public static void DeleteWarehouse(int warehouseId)
        {
            // We don't actually delete warehouses, just mark them as inactive
            string query = "UPDATE Warehouse SET IsActive = 0 WHERE WarehouseID = @WarehouseID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@WarehouseID", warehouseId }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
    }
}
