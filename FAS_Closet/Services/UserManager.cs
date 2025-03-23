// This file defines the UserManager class, which handles user-related operations.

using System;
using Microsoft.Data.Sqlite;
using FASCloset.Models;
using System.IO;

namespace FASCloset.Services
{
    public class UserManager
    {
        private const string UsernameParameter = "@Username";

        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        public static void RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            string query = @"
                INSERT INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone)
                VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Email, @Phone)
            ";

            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(UsernameParameter, user.Username);
                        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Phone", user.Phone);
                        
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqliteException ex)
                {
                    throw new InvalidOperationException($"Error registering user: {ex.Message}", ex);
                }
            }
        }

        public User? Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            string query = @"
                SELECT UserID, Username, PasswordHash, PasswordSalt, Name, Email, Phone 
                FROM User 
                WHERE Username = @Username
            ";

            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(UsernameParameter, username);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader.GetString(2); // PasswordHash
                                string storedSalt = reader.GetString(3); // PasswordSalt
                                
                                byte[] hashBytes = Convert.FromBase64String(storedHash);
                                byte[] saltBytes = Convert.FromBase64String(storedSalt);
                                
                                if (PasswordHasher.VerifyPasswordHash(password, hashBytes, saltBytes))
                                {
                                    return new User
                                    {
                                        UserID = reader.GetInt32(0),
                                        Username = reader.GetString(1),
                                        PasswordHash = storedHash,
                                        PasswordSalt = storedSalt,
                                        Name = reader.GetString(4),
                                        Email = reader.GetString(5),
                                        Phone = reader.GetString(6)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Login error: {ex.Message}");
                    // We don't throw here to avoid exposing database errors to the login screen
                }
            }
            
            return null;
        }

        public static bool IsUsernameTaken(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            string query = "SELECT COUNT(*) FROM User WHERE Username = @Username";

            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(UsernameParameter, username);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error checking if username is taken: {ex.Message}", ex);
                }
            }
        }

        public User? GetUserById(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException("User ID cannot be zero", nameof(userId));
            }

            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT UserID, Username, PasswordHash, PasswordSalt, Name, Email, Phone FROM User WHERE UserID = @UserID";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                PasswordSalt = reader.GetString(3),
                                Name = reader.GetString(4),
                                Email = reader.GetString(5),
                                Phone = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public User? GetUserByUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT UserID, Username, PasswordHash, PasswordSalt, Name, Email, Phone FROM User WHERE Username = @Username";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue(UsernameParameter, username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                PasswordSalt = reader.GetString(3),
                                Name = reader.GetString(4),
                                Email = reader.GetString(5),
                                Phone = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
