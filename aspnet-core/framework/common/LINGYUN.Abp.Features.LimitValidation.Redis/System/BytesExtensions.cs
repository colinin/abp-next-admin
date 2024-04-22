using System.Security.Cryptography;

namespace System
{
    internal static class BytesExtensions
    {
        public static byte[] Sha1(this byte[] data)
        {
            using (var sha = SHA1.Create())
            {
                var hashBytes = sha.ComputeHash(data);
                return hashBytes;
            }
        }
    }
}
