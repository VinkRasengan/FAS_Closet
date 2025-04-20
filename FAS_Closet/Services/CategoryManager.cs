// This file defines the CategoryManager class, which handles category-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class CategoryManager
    {
        // Define constant for frequently used parameter
        private const string ParamCategoryId = "@CategoryID";

        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
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

        public static void UpdateCategory(Category category)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE Category SET CategoryName = @CategoryName, Description = @Description, IsActive = @IsActive WHERE CategoryID = @CategoryID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                        command.Parameters.AddWithValue("@Description", category.Description);
                        command.Parameters.AddWithValue("@IsActive", category.IsActive);
                        command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while updating category.", ex);
            }
        }

        public static void DeleteCategory(int categoryId)
        {
            try
            {
                // First check if any products are using this category
                if (IsCategoryUsed(categoryId))
                {
                    // Instead of deleting, mark the category as inactive to maintain database integrity
                    using (var connection = new SqliteConnection(GetConnectionString()))
                    {
                        connection.Open();
                        string query = "UPDATE Category SET IsActive = 0 WHERE CategoryID = @CategoryID";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue(ParamCategoryId, categoryId);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    // If no products are using this category, delete it completely
                    using (var connection = new SqliteConnection(GetConnectionString()))
                    {
                        connection.Open();
                        string query = "DELETE FROM Category WHERE CategoryID = @CategoryID";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue(ParamCategoryId, categoryId);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while deleting category.", ex);
            }
        }

        public static List<Category> GetCategories()
        {
            var categories = new List<Category>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
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

        public static Category? GetCategoryById(int categoryId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = $"SELECT CategoryID, CategoryName, Description, IsActive, CreatedDate FROM Category WHERE CategoryID = {ParamCategoryId}";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(ParamCategoryId, categoryId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Category
                                {
                                    CategoryID = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    IsActive = reader.GetBoolean(3),
                                    CreatedDate = reader.GetDateTime(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving category.", ex);
            }
            return null;
        }

        public static bool IsCategoryUsed(int categoryId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Product WHERE CategoryID = @CategoryID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(ParamCategoryId, categoryId);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while checking category usage.", ex);
            }
        }

        // Added method to maintain compatibility with code that expects GetAllCategories
        public static List<Category> GetAllCategories()
        {
            // Simply calls the existing GetCategories method
            return GetCategories();
        }
    }
}
