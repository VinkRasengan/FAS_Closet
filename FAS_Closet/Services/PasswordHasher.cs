using System;
using System.Security.Cryptography;
using System.Text;

namespace FASCloset.Services
{
    public static class PasswordHasher
    {
        // Size of salt in bytes
        private const int SaltSize = 128;
        
        // Number of iterations for PBKDF2
        private const int Iterations = 10000;
        
        // Size of hash in bytes
        private const int HashSize = 64;

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
    }
}
