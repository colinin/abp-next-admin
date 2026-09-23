using System;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Aliyun.Captcha.Security;

public class SceneIdEncryptor : ISceneIdEncryptor, ISingletonDependency
{
    // IV 固定为 16 字节 (AES block size)
    private const int IvLengthBytes = 16;

    /// <summary>
    /// 对 sceneId 进行加密，生成 EncryptedSceneId
    /// </summary>
    /// <param name="sceneId">验证码业务标识（原 CaptchaAppid）</param>
    /// <param name="ekeyStr">控制台获取的 ekey（作为密钥基础，Base64 字符串）</param>
    /// <param name="expireTimeSec">密文过期时间，单位秒，范围 1~86400</param>
    /// <returns>Base64 编码的字符串：EncryptedSceneId（IV + 加密数据）</returns>
    public virtual string Encrypt(string sceneId, string ekeyStr, int expireTimeSec)
    {
        if (expireTimeSec <= 0 || expireTimeSec > 86400)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expireTimeSec),
                "expireTimeSec must be between 1 and 86400 seconds.");
        }

        // 获取当前时间戳（秒级）
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // ==================== 步骤 1: 构造密钥 Key（32字节） ====================
        var keyBytes = Convert.FromBase64String(ekeyStr);

        if (keyBytes.Length != 32)
        {
            throw new ArgumentException("ekey must decode to 32 bytes for AES-256.", nameof(ekeyStr));
        }

        // ==================== 步骤 2: 构造明文并执行 AES-256-CBC 加密 ====================
        // 明文格式：sceneId&timestamp&expireTime
        var plaintext = $"{sceneId}&{timestamp}&{expireTimeSec}";
        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

        // 随机生成 16 字节 IV
        var iv = new byte[IvLengthBytes];
        RandomNumberGenerator.Fill(iv);

        byte[] encryptedBytes;

        using (var aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes;
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor();
            encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
        }

        // ==================== 步骤 3: 拼接 IV + 密文，并进行 Base64 编码 ====================
        var result = new byte[IvLengthBytes + encryptedBytes.Length];
        Buffer.BlockCopy(iv, 0, result, 0, IvLengthBytes);
        Buffer.BlockCopy(encryptedBytes, 0, result, IvLengthBytes, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }
}
