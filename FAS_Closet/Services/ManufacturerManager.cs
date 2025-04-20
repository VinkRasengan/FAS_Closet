using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Data;
using FASCloset.Models;
using Microsoft.Data.Sqlite;

namespace FASCloset.Services
{
    /// <summary>
    /// Class to manage manufacturer data
    /// </summary>
    public static class ManufacturerManager
    {
        /// <summary>
        /// Get a list of all manufacturers
        /// </summary>
        public static List<Manufacturer> GetManufacturers()
        {
            string query = "SELECT * FROM Manufacturer ORDER BY ManufacturerName";
            
            return DataAccessHelper.ExecuteReader(query, reader => new Manufacturer
            {
                ManufacturerID = Convert.ToInt32(reader["ManufacturerID"]),
                ManufacturerName = reader["ManufacturerName"].ToString() ?? string.Empty,
                Description = reader["Description"]?.ToString() ?? string.Empty
            });
        }
        
        /// <summary>
        /// Get a manufacturer by ID
        /// </summary>
        public static Manufacturer? GetManufacturerById(int manufacturerId)
        {
            string query = "SELECT * FROM Manufacturer WHERE ManufacturerID = @ManufacturerId";
            var parameters = new Dictionary<string, object>
            {
                { "@ManufacturerId", manufacturerId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, reader => new Manufacturer
            {
                ManufacturerID = Convert.ToInt32(reader["ManufacturerID"]),
                ManufacturerName = reader["ManufacturerName"].ToString() ?? string.Empty,
                Description = reader["Description"]?.ToString() ?? string.Empty
            }, parameters);
        }
        
        /// <summary>
        /// Add a new manufacturer
        /// </summary>
        public static void AddManufacturer(Manufacturer manufacturer)
        {
            string query = "INSERT INTO Manufacturer (ManufacturerName, Description) VALUES (@ManufacturerName, @Description)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@ManufacturerName", manufacturer.ManufacturerName },
                { "@Description", manufacturer.Description }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
            
            // Get the ID of the just-added manufacturer
            manufacturer.ManufacturerID = DataAccessHelper.ExecuteScalar<int>("SELECT last_insert_rowid()");
        }
        
        /// <summary>
        /// Update an existing manufacturer
        /// </summary>
        public static void UpdateManufacturer(Manufacturer manufacturer)
        {
            string query = @"
                UPDATE Manufacturer 
                SET ManufacturerName = @ManufacturerName, 
                    Description = @Description
                WHERE ManufacturerID = @ManufacturerID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@ManufacturerID", manufacturer.ManufacturerID },
                { "@ManufacturerName", manufacturer.ManufacturerName },
                { "@Description", manufacturer.Description }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
        
        /// <summary>
        /// Delete a manufacturer
        /// </summary>
        public static void DeleteManufacturer(int manufacturerId)
        {
            string query = "DELETE FROM Manufacturer WHERE ManufacturerID = @ManufacturerID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@ManufacturerID", manufacturerId }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
    }
}