using System;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace src.Infrastructure.Security
{
    public class PasswordHasher
    {
        public static byte[] GenerateSalt(int length = 16)
        {
            byte[] salt = new byte[length];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            using var argon2 = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                MemorySize = 65536,
                Iterations = 4
            };

            byte[] hash = argon2.GetBytes(32);
            return Convert.ToBase64String(hash) + ":" + Convert.ToBase64String(salt);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[1]);
            var hashToVerify = HashPassword(password, salt).Split(':')[0];
            return parts[0] == hashToVerify;
        }
    }

}
