// This file defines the PasswordHasher class, which handles password hashing and verification.

using System;
using System.Linq;
using System.Security.Cryptography;

namespace FASCloset.Services
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000; // Increased for better security

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                passwordSalt = new byte[SaltSize];
                rng.GetBytes(passwordSalt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, Iterations, HashAlgorithmName.SHA256))
            {
                passwordHash = pbkdf2.GetBytes(HashSize);
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, Iterations, HashAlgorithmName.SHA256))
            {
                var computedHash = pbkdf2.GetBytes(HashSize);
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
