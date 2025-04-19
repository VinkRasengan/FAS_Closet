using System;
using System.Security.Cryptography;
using System.Text;

namespace FASCloset.Services
{
    /// <summary>
    /// Provides secure password hashing and verification functionality
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Creates a hash and salt from a plain text password
        /// </summary>
        /// <param name="password">Plain text password to hash</param>
        /// <param name="passwordHash">Output parameter that receives the password hash</param>
        /// <param name="passwordSalt">Output parameter that receives the salt</param>
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
                
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies a plain text password against a stored hash and salt
        /// </summary>
        /// <param name="password">Plain text password to verify</param>
        /// <param name="storedHash">Previously stored password hash</param>
        /// <param name="storedSalt">Previously stored salt</param>
        /// <returns>True if the password matches the hash, otherwise false</returns>
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
                
            if (storedHash.Length != 64)
                return false; // SHA512 produces 512 bits / 64 bytes
                
            if (storedSalt.Length != 128)
                return false; // HMACSHA512 key is 1024 bits / 128 bytes
                
            using (var hmac = new HMACSHA512(storedSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                
                // Compare each byte of the computed hash with the stored hash
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Generates credentials with a random password for demo purposes
        /// </summary>
        /// <returns>Object containing hash and salt as Base64 strings</returns>
        public static DemoCredentials CreateRandomPasswordCredentials()
        {
            // Generate a random password
            string randomPassword = GenerateRandomPassword(10);
            
            // Create hash and salt
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(randomPassword, out passwordHash, out passwordSalt);
            
            return new DemoCredentials
            {
                Password = randomPassword,
                Hash = Convert.ToBase64String(passwordHash),
                Salt = Convert.ToBase64String(passwordSalt)
            };
        }
        
        /// <summary>
        /// Generates a random password with the specified length
        /// </summary>
        /// <param name="length">Length of the password to generate</param>
        /// <returns>Random password string</returns>
        private static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[length];
                rng.GetBytes(bytes);
                
                var chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[bytes[i] % validChars.Length];
                }
                
                return new string(chars);
            }
        }
    }
    
    /// <summary>
    /// Holds password credentials for demo purposes
    /// </summary>
    public class DemoCredentials
    {
        /// <summary>
        /// Clear text password (for demo use only)
        /// </summary>
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// Base64-encoded password hash
        /// </summary>
        public string Hash { get; set; } = string.Empty;
        
        /// <summary>
        /// Base64-encoded salt
        /// </summary>
        public string Salt { get; set; } = string.Empty;
    }
}
