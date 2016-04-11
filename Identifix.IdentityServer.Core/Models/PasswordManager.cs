using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Identifix.IdentityServer.Infrastructure;

namespace Identifix.IdentityServer.Models
{
    public class PasswordManager : IPasswordManager
    {
        public const int DefaultIterations = 1000;
        public const int SaltSize = 24;
        public const int HashSize = 60;
        private const char Delimiter = ':';
        

        public PasswordManager(ISecuritySettings settings)
        {
            Guard.IsNotNull(settings, "settings");
            Settings = settings;
        }

        protected ISecuritySettings Settings { get; private set; }

        protected internal int Iterations => (Settings.PasswordHashIterations < 1) ? DefaultIterations : Settings.PasswordHashIterations;

        public string HashPassword(string password)
        {
            Guard.IsNotNullOrWhiteSpace(password, "password");
            return HashPassword(password, GenerateSalt());
        }

        internal string HashPassword(string password, byte[] salt)
        {
            Guard.IsNotNullOrWhiteSpace(password, "password");
            Guard.IsNotNull(salt, "salt");
            Guard.IsRequiredThat(ValidatePassword(password), "Password must be valid according to system password criteria.");

            byte[] hash = GetPbkdf2Bytes(password, salt, Iterations, HashSize);

            return $"{Iterations}{Delimiter}{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string expectedHash)
        {
            Guard.IsNotNullOrWhiteSpace(password, "password");
            Guard.IsNotNullOrEmpty(expectedHash, "expectedHash");
            char[] delimiterArray = new[] {Delimiter};
            string[] data = expectedHash.Split(delimiterArray);
            int iterations = int.Parse(data[0]);
            byte[] salt = Convert.FromBase64String(data[1]);
            byte[] hash = Convert.FromBase64String(data[2]);
            byte[] testHash = GetPbkdf2Bytes(password, salt, iterations, HashSize);
            return Compare(hash, testHash);
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations
            };
            return pbkdf2.GetBytes(outputBytes); 
        }

        private static byte[] GenerateSalt()
        {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltSize];
            cryptoProvider.GetBytes(salt);
            return salt;
        }

        private static bool Compare(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        public static bool ValidatePassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) &&
            password.Length >= 8 &&
            password.Any(c => char.IsLower(c)) &&
            password.Any(c => char.IsUpper(c)) &&
            password.Any(c => char.IsDigit(c));
        }
    }
}
