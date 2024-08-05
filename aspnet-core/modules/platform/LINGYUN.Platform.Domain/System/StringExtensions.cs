using System.Security.Cryptography;
using System.Text;

namespace System;

public static class StringExtensions
{
    public static string Md5(this string str, bool lowercase = false)
    {
        using (var md5 = MD5.Create())
        {
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var md5Str = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return lowercase ? md5Str.ToLower() : md5Str;
        }
    }

    public static string Sha1(this string str, bool lowercase = false)
    {
        using (var sha = SHA1.Create())
        {
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sha1 = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return lowercase ? sha1.ToLower() : sha1;
        }
    }

    public static string Sha256(this string str, bool lowercase = false)
    {
        using (var sha = SHA256.Create())
        {
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sha256 = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return lowercase ? sha256.ToLower() : sha256;
        }
    }

    public static string Sha512(this string str, bool lowercase = false)
    {
        using (var sha = SHA512.Create())
        {
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sha512 = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return lowercase ? sha512.ToLower() : sha512;
        }
    }
}
