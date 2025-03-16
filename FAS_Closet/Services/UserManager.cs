using System;
using Microsoft.Data.Sqlite;
using FASCloset.Models;
using System.IO;

namespace FASCloset.Services
{
    public class UserManager
    {
        private static string GetConnectionString()
        {
            string? baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDirectory == null)
            {
                throw new InvalidOperationException("Base directory is null.");
            }

            string? projectDir = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
            if (projectDir == null)
            {
                throw new InvalidOperationException("Project directory is null.");
            }

            string dbPath = Path.Combine(projectDir, "Data", "FASClosetDB.sqlite");
            return $"Data Source={dbPath};";
        }

        public static void RegisterUser(User user)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Email, @Phone)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
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
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT UserID, Username, PasswordHash, PasswordSalt FROM Users WHERE Username = @Username";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var user = new User
                                {
                                    UserID = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    PasswordHash = (byte[])reader["PasswordHash"],
                                    PasswordSalt = (byte[])reader["PasswordSalt"]
                                };
                                if (PasswordHasher.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
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
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
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
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT UserID, Username, Name, Email, Phone FROM Users WHERE UserID = @UserID";
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
                                    Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Phone = reader.IsDBNull(4) ? null : reader.GetString(4)
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
    }
}
