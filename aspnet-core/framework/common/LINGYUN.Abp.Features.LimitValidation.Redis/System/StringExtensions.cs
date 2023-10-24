using System.Security.Cryptography;
using System.Text;

namespace System
{
    internal static class StringExtensions
    {
        public static byte[] Sha1(this string str)
        {
            using (var sha = SHA1.Create())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
                return hashBytes;
            }
        }
    }
}
