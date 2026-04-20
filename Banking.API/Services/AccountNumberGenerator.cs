using System.Security.Cryptography;
using System.Text;

namespace Banking.Services
{
    public class AccountNumberGenerator
    {
        /// <summary>
        /// Generates a cryptographically secure random account number.
        /// </summary>
        /// <param name="length">Length of the account number (default: 10)</param>
        /// <returns>A numeric string of specified length with random digits 0-9</returns>
        public static string Generate(int length = 10)
        {
            if (length < 10 || length > 12)
                throw new ArgumentException("Account number length must be between 10 and 12 digits.", nameof(length));

            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            var sb = new StringBuilder(length);
            foreach (var b in bytes)
            {
                sb.Append(b % 10); // Ensures digit 0-9
            }

            return sb.ToString();
        }
    }
}
