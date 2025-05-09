using System;
using System.Security.Cryptography;
using System.Text;

namespace FASCloset.Services
{
    public class PasswordCredentials
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }

    public static class PasswordHasher
    {
        /// <summary>
        /// Creates a hash and salt from a password
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="passwordHash">Output parameter for the generated hash</param>
        /// <param name="passwordSalt">Output parameter for the generated salt</param>
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies a password against a hash and salt
        /// </summary>
        /// <param name="password">The password to verify</param>
        /// <param name="storedHash">The stored hash</param>
        /// <param name="storedSalt">The stored salt</param>
        /// <returns>True if the password matches the hash, false otherwise</returns>
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (storedHash.Length != 64) return false;
            if (storedSalt.Length != 128) return false;

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Verifies a password against a stored hash and salt
        /// </summary>
        /// <param name="password">The password to verify</param>
        /// <param name="storedHash">The stored hash (Base64 encoded)</param>
        /// <param name="storedSalt">The stored salt (Base64 encoded)</param>
        /// <returns>True if the password matches the hash</returns>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            
            using (var hmac = new HMACSHA512(saltBytes))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                
                // Compare the computed hash with the stored hash
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hashBytes[i]) return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Creates random password credentials for demo users
        /// </summary>
        /// <param name="password">Optional password to use, otherwise a default is used</param>
        /// <returns>PasswordCredentials containing hash and salt</returns>
        public static PasswordCredentials CreateRandomPasswordCredentials(string password = "admin123")
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            
            return new PasswordCredentials
            {
                Hash = Convert.ToBase64String(passwordHash),
                Salt = Convert.ToBase64String(passwordSalt)
            };
        }
    }
}
