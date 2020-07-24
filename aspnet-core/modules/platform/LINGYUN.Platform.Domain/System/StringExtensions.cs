using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static string GetHash(this string str)
        {
            using (var sha = new SHA1Managed())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }
    }
}
