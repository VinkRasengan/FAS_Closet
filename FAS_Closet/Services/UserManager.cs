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
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Email, @Phone)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(UsernameParameter, user.Username);
                        command.Parameters.AddWithValue("@PasswordHash", Convert.FromBase64String(user.PasswordHash));
                        command.Parameters.AddWithValue("@PasswordSalt", Convert.FromBase64String(user.PasswordSalt));
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(user.Email) ? DBNull.Value : user.Email);
                        command.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(user.Phone) ? DBNull.Value : user.Phone);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while registering user.", ex);
            }
        }

        public User? Login(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT UserID, Username, PasswordHash, PasswordSalt, Name, Email, Phone FROM Users WHERE Username = @Username";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(UsernameParameter, username);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var user = new User
                                {
                                    UserID = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    PasswordHash = Convert.ToBase64String((byte[])reader["PasswordHash"]),
                                    PasswordSalt = Convert.ToBase64String((byte[])reader["PasswordSalt"]),
                                    Name = reader.GetString(4),
                                    Email = reader.IsDBNull(5) ? string.Empty : reader.GetString(5), // Fix nullability
                                    Phone = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)  // Fix nullability
                                };
                                if (PasswordHasher.VerifyPasswordHash(password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
                                {
                                    return user;
                                }
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while logging in.", ex);
            }
            return null;
        }

        public static bool IsUsernameTaken(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(UsernameParameter, username);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while checking username.", ex);
            }
        }

        public User? GetUserById(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT UserID, Username, Name, Email, Phone, PasswordHash, PasswordSalt FROM Users WHERE UserID = @UserID";
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
                                    Name = reader.GetString(2),
                                    Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3), // Fix nullability
                                    Phone = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),  // Fix nullability
                                    PasswordHash = Convert.ToBase64String((byte[])reader["PasswordHash"]),
                                    PasswordSalt = Convert.ToBase64String((byte[])reader["PasswordSalt"])
                                };
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving user.", ex);
            }
            return null;
        }

        public User? GetUserByUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT UserID, Username, Name, Email, Phone, PasswordHash, PasswordSalt FROM Users WHERE Username = @Username";
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
                                    Name = reader.GetString(2),
                                    Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    Phone = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    PasswordHash = Convert.ToBase64String((byte[])reader["PasswordHash"]),
                                    PasswordSalt = Convert.ToBase64String((byte[])reader["PasswordSalt"])
                                };
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving user by username.", ex);
            }
            return null;
        }
    }
}
