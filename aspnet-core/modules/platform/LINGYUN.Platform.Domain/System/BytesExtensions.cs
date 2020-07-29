using System.Security.Cryptography;

namespace System
{
    public static class BytesExtensions
    {
        public static string Md5(this byte[] data, bool lowercase = false)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(data);
                var md5Str = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return lowercase ? md5Str.ToLower() : md5Str;
            }
        }

        public static string Sha1(this byte[] data, bool lowercase = false)
        {
            using (var sha = SHA1.Create())
            {
                var hashBytes = sha.ComputeHash(data);
                var sha1 = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return lowercase ? sha1.ToLower() : sha1;
            }
        }

        public static string Sha256(this byte[] data, bool lowercase = false)
        {
            using (var sha = SHA256.Create())
            {
                var hashBytes = sha.ComputeHash(data);
                var sha256 = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return lowercase ? sha256.ToLower() : sha256;
            }
        }

        public static string Sha512(this byte[] data, bool lowercase = false)
        {
            using (var sha = SHA512.Create())
            {
                var hashBytes = sha.ComputeHash(data);
                var sha512 = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return lowercase ? sha512.ToLower() : sha512;
            }
        }
    }
}
