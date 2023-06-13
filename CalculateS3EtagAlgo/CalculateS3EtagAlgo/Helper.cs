using System.Security.Cryptography;

namespace CalculateS3EtagAlgo
{
    public static class Helper
    {
        public static byte[] GetHash(this byte[] array, HashAlgorithm algorithm)
        {
            var hash = algorithm.ComputeHash(array);
            return hash;
        }

        public static byte[] ToByteArray(this string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ToHexString(this byte[] bytes)
        {
            var chars = new char[bytes.Length * 2 + 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                chars[2 * i + 2] = ToHexDigit(bytes[i] / 16);
                chars[2 * i + 3] = ToHexDigit(bytes[i] % 16);
            }

            return new string(chars).ToLower();
        }

        public static char ToHexDigit(int i)
        {
            if (i < 10)
            {
                return (char)(i + '0');
            }
            return (char)(i - 10 + 'A');
        }

        public static string ToHashString(this byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
