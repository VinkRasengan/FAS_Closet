// This file defines the UserManager class, which handles user-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;
using System.Security.Cryptography;
using System.Text;
using FASCloset.Data;

namespace FASCloset.Services
{
    /// <summary>
    /// Manages user-related operations including authentication, registration, and user account management
    /// </summary>
    public class UserManager
    {
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        /// <summary>
        /// Authenticates a user and provides user information if successful
        /// </summary>
        /// <param name="username">Username to authenticate</param>
        /// <param name="password">Plain text password to validate</param>
        /// <returns>User object if authentication successful, null otherwise</returns>
        public User? Login(string username, string password)
        {
            try
            {
                User? user = GetUserByUsername(username);
                
                if (user == null)
                    return null;
                
                // Verify password
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;
                
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Login failed.", ex);
            }
        }

        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <param name="user">User object with registration information</param>
        public static void RegisterUser(User user)
        {
            try
            {
                string query = @"
                    INSERT INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) 
                    VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Email, @Phone)";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@Username", user.Username },
                    { "@PasswordHash", user.PasswordHash },
                    { "@PasswordSalt", user.PasswordSalt },
                    { "@Name", user.Name },
                    { "@Email", user.Email },
                    { "@Phone", user.Phone }
                };
                
                DataAccessHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error registering user.", ex);
            }
        }

        /// <summary>
        /// Checks if a username is already taken
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <returns>True if username exists in the database</returns>
        public static bool IsUsernameTaken(string username)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM User WHERE Username = @Username";
                var parameters = new Dictionary<string, object>
                {
                    { "@Username", username }
                };
                
                int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking username: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if an email is already taken by an existing user
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if email exists in the database</returns>
        public static bool IsEmailTaken(string email)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM User WHERE Email = @Email";
                var parameters = new Dictionary<string, object>
                {
                    { "@Email", email }
                };
                
                int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking email: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a phone number is already taken by an existing user
        /// </summary>
        /// <param name="phone">Phone number to check</param>
        /// <returns>True if phone exists in the database</returns>
        public static bool IsPhoneTaken(string phone)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM User WHERE Phone = @Phone";
                var parameters = new Dictionary<string, object>
                {
                    { "@Phone", phone }
                };
                
                int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking phone number: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves a user by username
        /// </summary>
        /// <param name="username">Username to search for</param>
        /// <returns>User object if found, null otherwise</returns>
        public static User? GetUserByUsername(string username)
        {
            try
            {
                string query = "SELECT UserID, Username, PasswordHash, PasswordSalt, Name, Email, Phone FROM User WHERE Username = @Username";
                var parameters = new Dictionary<string, object>
                {
                    { "@Username", username }
                };
                
                return DataAccessHelper.ExecuteReaderSingle<User>(query, reader => new User
                {
                    UserID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    PasswordSalt = reader.GetString(3),
                    Name = reader.GetString(4),
                    Email = reader.GetString(5),
                    Phone = reader.GetString(6)
                }, parameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving user by username: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifies a password against stored hash and salt
        /// </summary>
        /// <param name="password">Plain text password to verify</param>
        /// <param name="storedHashStr">Stored password hash as Base64 string</param>
        /// <param name="storedSaltStr">Stored password salt as Base64 string</param>
        /// <returns>True if password matches the stored hash and salt</returns>
        private static bool VerifyPasswordHash(string password, string storedHashStr, string storedSaltStr)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (string.IsNullOrWhiteSpace(storedHashStr) || string.IsNullOrWhiteSpace(storedSaltStr)) return false;
            
            try
            {
                byte[] storedHash = Convert.FromBase64String(storedHashStr);
                byte[] storedSalt = Convert.FromBase64String(storedSaltStr);
                
                using (var hmac = new HMACSHA512(storedSalt))
                {
                    byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i]) return false;
                    }
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>List of all users</returns>
        public static List<User> GetUsers()
        {
            string query = "SELECT * FROM User ORDER BY Name";
            
            return DataAccessHelper.ExecuteReader(query, reader => new User
            {
                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.GetString(reader.GetOrdinal("Phone"))
                // We don't include password information for security
            });
        }
    }
}
