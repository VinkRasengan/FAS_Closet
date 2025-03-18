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
                    ExecuteAddCategoryCommand(connection, category);
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while adding category.", ex);
            }
        }

        private static void ExecuteAddCategoryCommand(SqliteConnection connection, Category category)
        {
            string query = "INSERT INTO Categories (CategoryName, Description, IsActive, CreatedDate) VALUES (@CategoryName, @Description, @IsActive, @CreatedDate)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("@Description", category.Description);
                command.Parameters.AddWithValue("@IsActive", category.IsActive);
                command.Parameters.AddWithValue("@CreatedDate", category.CreatedDate);
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateCategory(Category category)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description, IsActive = @IsActive WHERE CategoryID = @CategoryID";
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
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryID", categoryId);
                        command.ExecuteNonQuery();
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
                    EnsureCategoriesTableExists(connection); // Ensure table exists
                    string query = "SELECT CategoryID, CategoryName, Description, IsActive, CreatedDate FROM Categories";
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
                Console.WriteLine($"Database error: {ex.Message}");
                // Optionally, show a user-friendly message
                return categories; // Return empty list instead of crashing
            }
            return categories;
        }

        private static void EnsureCategoriesTableExists(SqliteConnection connection)
        {
            string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Categories';";
            using (var command = new SqliteCommand(checkTableQuery, connection))
            {
                if (command.ExecuteScalar() == null)
                {
                    string createTableQuery = @"
                        CREATE TABLE Categories (
                            CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                            CategoryName TEXT NOT NULL,
                            Description TEXT,
                            IsActive INTEGER NOT NULL,
                            CreatedDate DATETIME NOT NULL
                        );";
                    using (var createCommand = new SqliteCommand(createTableQuery, connection))
                    {
                        createCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
