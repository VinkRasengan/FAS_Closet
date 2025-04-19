// This file defines the CategoryManager class, which handles category-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Data;
using FASCloset.Models;
using Microsoft.Data.Sqlite;

namespace FASCloset.Services
{
    /// <summary>
    /// Manages category-related operations including CRUD functions and product relationships
    /// </summary>
    public static class CategoryManager
    {
        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        /// <returns>List of all categories</returns>
        public static List<Category> GetAllCategories()
        {
            string query = "SELECT * FROM Category ORDER BY CategoryName";
            return DataAccessHelper.ExecuteReader(query, MapCategoryFromReader);
        }
        
        /// <summary>
        /// Gets only active categories from the database
        /// </summary>
        /// <returns>List of active categories</returns>
        public static List<Category> GetActiveCategories()
        {
            string query = "SELECT * FROM Category WHERE IsActive = 1 ORDER BY CategoryName";
            return DataAccessHelper.ExecuteReader(query, MapCategoryFromReader);
        }
        
        /// <summary>
        /// Gets a specific category by ID
        /// </summary>
        /// <param name="categoryId">ID of the category to retrieve</param>
        /// <returns>Category object or null if not found</returns>
        public static Category? GetCategoryById(int categoryId)
        {
            string query = "SELECT * FROM Category WHERE CategoryID = @CategoryID";
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, MapCategoryFromReader, parameters);
        }
        
        /// <summary>
        /// Gets a category by its name
        /// </summary>
        /// <param name="categoryName">Name of the category to retrieve</param>
        /// <returns>Category object or null if not found</returns>
        public static Category? GetCategoryByName(string categoryName)
        {
            string query = "SELECT * FROM Category WHERE CategoryName = @CategoryName";
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryName", categoryName }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, MapCategoryFromReader, parameters);
        }
        
        /// <summary>
        /// Adds a new category to the database
        /// </summary>
        /// <param name="category">Category object to add</param>
        /// <returns>ID of the newly created category, or -1 on failure</returns>
        public static int AddCategory(Category category)
        {
            try
            {
                // Check if category with same name already exists
                if (CategoryExists(category.CategoryName))
                {
                    throw new ArgumentException($"Category '{category.CategoryName}' already exists");
                }
                
                string query = @"
                    INSERT INTO Category (CategoryName, Description, IsActive, CreatedDate)
                    VALUES (@CategoryName, @Description, @IsActive, @CreatedDate);
                    SELECT last_insert_rowid();";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@CategoryName", category.CategoryName },
                    { "@Description", category.Description ?? (object)DBNull.Value },
                    { "@IsActive", category.IsActive ? 1 : 0 },
                    { "@CreatedDate", category.CreatedDate == DateTime.MinValue ? DateTime.Now : category.CreatedDate }
                };
                
                return DataAccessHelper.ExecuteScalar<int>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Updates an existing category in the database
        /// </summary>
        /// <param name="category">Category with updated information</param>
        /// <returns>True if update was successful</returns>
        public static bool UpdateCategory(Category category)
        {
            try
            {
                // Check if another category with the same name exists
                if (CategoryExistsWithDifferentId(category.CategoryName, category.CategoryID))
                {
                    throw new ArgumentException($"Another category with name '{category.CategoryName}' already exists");
                }
                
                string query = @"
                    UPDATE Category
                    SET CategoryName = @CategoryName,
                        Description = @Description,
                        IsActive = @IsActive
                    WHERE CategoryID = @CategoryID";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@CategoryID", category.CategoryID },
                    { "@CategoryName", category.CategoryName },
                    { "@Description", category.Description ?? (object)DBNull.Value },
                    { "@IsActive", category.IsActive ? 1 : 0 }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a category from the database if it has no associated products
        /// </summary>
        /// <param name="categoryId">ID of the category to delete</param>
        /// <returns>True if deletion was successful</returns>
        public static bool DeleteCategory(int categoryId)
        {
            try
            {
                // Check if there are products using this category
                int productCount = GetProductCountInCategory(categoryId);
                if (productCount > 0)
                {
                    throw new InvalidOperationException($"Cannot delete category with {productCount} associated products. Remove products first or deactivate the category instead.");
                }
                
                string query = "DELETE FROM Category WHERE CategoryID = @CategoryID";
                var parameters = new Dictionary<string, object>
                {
                    { "@CategoryID", categoryId }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Marks a category as inactive instead of deleting it
        /// </summary>
        /// <param name="categoryId">ID of the category to deactivate</param>
        /// <returns>True if deactivation was successful</returns>
        public static bool DeactivateCategory(int categoryId)
        {
            try
            {
                string query = "UPDATE Category SET IsActive = 0 WHERE CategoryID = @CategoryID";
                var parameters = new Dictionary<string, object>
                {
                    { "@CategoryID", categoryId }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deactivating category: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Reactivates a previously deactivated category
        /// </summary>
        /// <param name="categoryId">ID of the category to activate</param>
        /// <returns>True if activation was successful</returns>
        public static bool ActivateCategory(int categoryId)
        {
            try
            {
                string query = "UPDATE Category SET IsActive = 1 WHERE CategoryID = @CategoryID";
                var parameters = new Dictionary<string, object>
                {
                    { "@CategoryID", categoryId }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error activating category: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Checks if a category with the given name exists
        /// </summary>
        /// <param name="categoryName">Name to check</param>
        /// <returns>True if a category with the name exists</returns>
        public static bool CategoryExists(string categoryName)
        {
            string query = "SELECT COUNT(*) FROM Category WHERE CategoryName = @CategoryName";
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryName", categoryName }
            };
            
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }
        
        /// <summary>
        /// Checks if a category with the given name exists with a different ID
        /// </summary>
        /// <param name="categoryName">Name to check</param>
        /// <param name="categoryId">ID to exclude from check</param>
        /// <returns>True if a category with the name exists with a different ID</returns>
        private static bool CategoryExistsWithDifferentId(string categoryName, int categoryId)
        {
            string query = "SELECT COUNT(*) FROM Category WHERE CategoryName = @CategoryName AND CategoryID != @CategoryID";
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryName", categoryName },
                { "@CategoryID", categoryId }
            };
            
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }
        
        /// <summary>
        /// Gets the count of products in a specified category
        /// </summary>
        /// <param name="categoryId">ID of the category to check</param>
        /// <returns>Number of products in the category</returns>
        public static int GetProductCountInCategory(int categoryId)
        {
            string query = "SELECT COUNT(*) FROM Product WHERE CategoryID = @CategoryID";
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryId }
            };
            
            return DataAccessHelper.ExecuteScalar<int>(query, parameters);
        }
        
        /// <summary>
        /// Gets products in a specified category
        /// </summary>
        /// <param name="categoryId">ID of the category</param>
        /// <returns>List of products in the category</returns>
        public static List<Product> GetProductsInCategory(int categoryId)
        {
            // Use the ProductManager to get products by category
            return ProductManager.GetProductsByCategory(categoryId);
        }
        
        /// <summary>
        /// Gets all categories from the database (alias for GetAllCategories)
        /// </summary>
        /// <returns>List of all categories</returns>
        public static List<Category> GetCategories()
        {
            return GetAllCategories();
        }
        
        /// <summary>
        /// Checks if a category is used in any products
        /// </summary>
        /// <param name="categoryId">ID of the category to check</param>
        /// <returns>True if category is used in any products</returns>
        public static bool IsCategoryUsed(int categoryId)
        {
            return GetProductCountInCategory(categoryId) > 0;
        }
        
        /// <summary>
        /// Maps data from a reader to a Category object
        /// </summary>
        /// <param name="reader">Data reader containing category data</param>
        /// <returns>Populated Category object</returns>
        private static Category MapCategoryFromReader(IDataReader reader)
        {
            return new Category
            {
                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }
        
        /// <summary>
        /// Ensures the Category table exists in the database
        /// </summary>
        internal static void EnsureCategoryTableExists()
        {
            DatabaseConnection.ExecuteDbOperation(connection =>
            {
                string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Category';";
                using (var command = new SqliteCommand(checkTableQuery, connection))
                {
                    if (command.ExecuteScalar() == null)
                    {
                        // Category table doesn't exist, create it
                        using (var createCommand = new SqliteCommand(@"
                            CREATE TABLE Category (
                                CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                                CategoryName TEXT NOT NULL UNIQUE,
                                Description TEXT,
                                IsActive BOOLEAN NOT NULL DEFAULT 1,
                                CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                            );", connection))
                        {
                            createCommand.ExecuteNonQuery();
                            Console.WriteLine("Category table created successfully.");
                        }
                    }
                }
            });
        }
    }
}
