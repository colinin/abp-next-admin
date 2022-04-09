using System.Security.Cryptography;
using System.Text;

namespace System;

internal static class AbpStringCryptographyExtensions
{
    public static string Sha256(this string planText, byte[] salt)
    {
        var data = planText.GetBytes();
        using var hmacsha256 = new HMACSHA256(salt);
        var retVal = hmacsha256.ComputeHash(data);
        var sb = new StringBuilder();
        for (var i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString();
    }
}
