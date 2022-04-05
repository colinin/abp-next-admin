using System.Security.Cryptography;

namespace System;

internal static class AbpStringCryptographyExtensions
{
    public static string Sha256(this string planText, byte[] salt)
    {
        var data = planText.GetBytes();
        using var hmacsha256 = new HMACSHA256(salt);
        return BitConverter.ToString(hmacsha256.ComputeHash(data));
    }
}
