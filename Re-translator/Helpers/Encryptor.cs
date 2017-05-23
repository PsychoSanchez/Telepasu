using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Proxy.Helpers
{
    internal static class Encryptor
    {
        private static readonly Random Random = new Random();
        const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefgjklmznxlwqe0123456789";
        public static string GenerateChallenge()
        {
            return new string(Enumerable.Repeat(Chars, 10)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
