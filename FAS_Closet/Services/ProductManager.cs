// This file defines the ProductManager class, which handles product-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class ProductManager
    {
        private const string DescriptionParameter = "@Description";
        
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        public static void AddProduct(Product product)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Product (ProductName, CategoryID, Price, Stock, Description) VALUES (@ProductName, @CategoryID, @Price, @Stock, @Description)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Stock", product.Stock); // Include Stock
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while adding product.", ex);
            }
        }

        public static void UpdateProduct(Product product)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE Product SET ProductName = @ProductName, CategoryID = @CategoryID, Price = @Price, Description = @Description WHERE ProductID = @ProductID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while updating product.", ex);
            }
        }

        public static void DeleteProduct(int productId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "DELETE FROM Product WHERE ProductID = @ProductID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while deleting product.", ex);
            }
        }

        public static List<Product> GetProducts()
        {
            var products = new List<Product>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT ProductID, ProductName, CategoryID, ManufacturerID, Price, Stock, Description FROM Product"; // Adjust column names
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    CategoryID = reader.GetInt32(2),
                                    ManufacturerID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    Price = reader.GetDecimal(4),
                                    Stock = reader.GetInt32(5),
                                    Description = reader.GetString(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving products.", ex);
            }
            return products;
        }

        public static List<Product> GetProductsByCategory(int categoryId)
        {
            var products = new List<Product>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT ProductID, ProductName, CategoryID, ManufacturerID, Price, Stock, Description FROM Product WHERE CategoryID = @CategoryID"; // Adjust column names
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryID", categoryId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    CategoryID = reader.GetInt32(2),
                                    ManufacturerID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    Price = reader.GetDecimal(4),
                                    Stock = reader.GetInt32(5),
                                    Description = reader.GetString(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving products by category.", ex);
            }
            return products;
        }

        public static Product? GetProductById(int id) // Allow null return
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT ProductID, ProductName, CategoryID, ManufacturerID, Price, Stock, Description FROM Product WHERE ProductID = @ProductID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    ProductID = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    CategoryID = reader.GetInt32(2),
                                    ManufacturerID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    Price = reader.GetDecimal(4),
                                    Stock = reader.GetInt32(5),
                                    Description = reader.GetString(6)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving product by ID.", ex);
            }
            return null;
        }

        public static Product? GetProductByName(string productName)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT ProductID, ProductName, CategoryID, ManufacturerID, Price, Stock, Description FROM Product WHERE ProductName = @ProductName";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    ProductID = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    CategoryID = reader.GetInt32(2),
                                    ManufacturerID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    Price = reader.GetDecimal(4),
                                    Stock = reader.GetInt32(5),
                                    Description = reader.GetString(6)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving product by name.", ex);
            }
            return null;
        }

        public static List<Category> GetCategories()
        {
            var categories = new List<Category>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    EnsureCategoriesTableExists(connection); // Ensure table exists
                    string query = "SELECT CategoryID, CategoryName, Description, IsActive, CreatedDate FROM Category";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new Category
                                {
                                    CategoryID = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    IsActive = reader.GetBoolean(3),
                                    CreatedDate = reader.GetDateTime(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving categories.", ex);
            }
            return categories;
        }

        public static void AddCategory(Category category)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Category (CategoryName, Description, IsActive, CreatedDate) VALUES (@CategoryName, @Description, @IsActive, @CreatedDate)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                        command.Parameters.AddWithValue("@Description", category.Description);
                        command.Parameters.AddWithValue("@IsActive", category.IsActive);
                        command.Parameters.AddWithValue("@CreatedDate", category.CreatedDate);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while adding category.", ex);
            }
        }

        public static void AddManufacturer(Manufacturer manufacturer)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Manufacturer (ManufacturerName, Description) VALUES (@ManufacturerName, @Description)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ManufacturerName", manufacturer.ManufacturerName);
                        command.Parameters.AddWithValue("@Description", manufacturer.Description);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while adding manufacturer.", ex);
            }
        }

        public static List<Manufacturer> GetManufacturers()
        {
            var manufacturers = new List<Manufacturer>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT ManufacturerID, ManufacturerName, Description FROM Manufacturer";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                manufacturers.Add(new Manufacturer
                                {
                                    ManufacturerID = reader.GetInt32(0),
                                    ManufacturerName = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving manufacturers.", ex);
            }
            return manufacturers;
        }

        private static void EnsureCategoriesTableExists(SqliteConnection connection)
        {
            string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Categories';";
            using (var command = new SqliteCommand(checkTableQuery, connection))
            {
                if (command.ExecuteScalar() == null)
                {
                    using (var createCommand = new SqliteCommand(@"
                        CREATE TABLE Categories (
                            CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                            CategoryName TEXT NOT NULL,
                            Description TEXT,
                            IsActive INTEGER NOT NULL,
                            CreatedDate DATETIME NOT NULL
                        );", connection))
                    {
                        createCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
