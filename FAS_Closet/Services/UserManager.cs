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

            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "INSERT INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Email, @Phone)";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Phone", user.Phone);
                    command.ExecuteNonQuery();
                }
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
                            var user = new User
                            {
                                UserID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                PasswordSalt = reader.GetString(3),
                                Name = reader.GetString(4),
                                Email = reader.GetString(5),
                                Phone = reader.GetString(6)
                            };

                            if (PasswordHasher.VerifyPasswordHash(password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
                            {
                                return user;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static bool IsUsernameTaken(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM User WHERE Username = @Username";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue(UsernameParameter, username);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        public User? GetUserById(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId));
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
