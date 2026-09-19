using System;
using System.Security.Cryptography;
using System.Text;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Utils;

public static class GcmEncryptHelper
{
    private const int GcmTagLengthBits = 128; // 16 字节 = 128 bit
    private const int GcmTagLengthBytes = GcmTagLengthBits / 8; // 16
    private const int IvLengthBytes = 12;     // GCM 推荐 12 字节 IV

    /// <summary>
    /// AES GCM 加密
    /// </summary>
    /// <param name="plaintext">明文</param>
    /// <param name="key">密钥（32 字节）</param>
    /// <param name="iv">初始化向量（12 字节）</param>
    /// <param name="aad">附加认证数据（可为 null）</param>
    /// <returns>Base64(IV + 密文 + tag)</returns>
    public static string Encrypt(string plaintext, byte[] key, byte[] iv, byte[]? aad)
    {
        ArgumentNullException.ThrowIfNull(plaintext);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(iv);

        if (key.Length != 32)
            throw new ArgumentException("Key must be 32 bytes for AES-256.", nameof(key));
        if (iv.Length != IvLengthBytes)
            throw new ArgumentException($"IV must be {IvLengthBytes} bytes for GCM.", nameof(iv));

        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

        // 输出缓冲区：密文 + tag
        var ciphertextWithTag = new byte[plaintextBytes.Length + GcmTagLengthBytes];
        var tag = ciphertextWithTag.AsSpan(plaintextBytes.Length, GcmTagLengthBytes);
        var ciphertext = ciphertextWithTag.AsSpan(0, plaintextBytes.Length);

        // .NET 10 中 AesGcm 构造函数需要显式指定 tag 长度
        using var aesGcm = new AesGcm(key, GcmTagLengthBytes);

        aesGcm.Encrypt(
            nonce: iv,
            plaintext: plaintextBytes,
            ciphertext: ciphertext,
            tag: tag,
            associatedData: aad);

        // 拼接 IV + 密文 + tag
        var ivAndCiphertext = new byte[iv.Length + ciphertextWithTag.Length];
        Buffer.BlockCopy(iv, 0, ivAndCiphertext, 0, iv.Length);
        Buffer.BlockCopy(ciphertextWithTag, 0, ivAndCiphertext, iv.Length, ciphertextWithTag.Length);

        return Convert.ToBase64String(ivAndCiphertext);
    }

    /// <summary>
    /// 循环填充密钥至 32 字节
    /// </summary>
    /// <param name="appSecretKey">原始密钥</param>
    /// <returns>32 字节密钥</returns>
    public static byte[] PadKey(byte[] appSecretKey)
    {
        ArgumentNullException.ThrowIfNull(appSecretKey);
        if (appSecretKey.Length == 0)
            throw new ArgumentException("appSecretKey must not be empty.", nameof(appSecretKey));

        if (appSecretKey.Length >= 32)
            return appSecretKey;

        var key = new byte[32];
        var srcPos = 0;
        for (var i = 0; i < 32; i++)
        {
            key[i] = appSecretKey[srcPos];
            srcPos++;
            if (srcPos >= appSecretKey.Length)
                srcPos = 0;
        }
        return key;
    }
}
