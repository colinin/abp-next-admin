using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.Encryption.SM4;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(StringEncryptionService), typeof(IStringEncryptionService))]
public class SM4StringEncryptionService : StringEncryptionService
{
    public SM4StringEncryptionService(
        IOptions<AbpStringEncryptionOptions> options) 
        : base(options)
    {
    }

    public override string Decrypt(string cipherText, string passPhrase = null, byte[] salt = null)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return null;
        }

        passPhrase ??= Options.DefaultPassPhrase;
        salt ??= Options.DefaultSalt;

        var cipherTextBytes = Convert.FromBase64String(cipherText);

        using var password = new Rfc2898DeriveBytes(passPhrase, salt);
        // 128-bit key
        var keyBytes = password.GetBytes(16);
        var ivBytes = password.GetBytes(16);

        var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new SM4Engine()), new Pkcs7Padding());
        cipher.Init(false, new ParametersWithIV(new KeyParameter(keyBytes), ivBytes));

        var decryptTextBytes = cipher.DoFinal(cipherTextBytes);

        return Encoding.UTF8.GetString(decryptTextBytes);
    }

    public override string Encrypt(string plainText, string passPhrase = null, byte[] salt = null)
    {
        if (plainText == null)
        {
            return null;
        }

        passPhrase ??= Options.DefaultPassPhrase;
        salt ??= Options.DefaultSalt;

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        using var password = new Rfc2898DeriveBytes(passPhrase, salt);
        // 128-bit key
        var keyBytes = password.GetBytes(16);
        var ivBytes = password.GetBytes(16);

        var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new SM4Engine()), new Pkcs7Padding());
        cipher.Init(true, new ParametersWithIV(new KeyParameter(keyBytes), ivBytes));

        var decryptTextBytes = cipher.DoFinal(plainTextBytes);

        return Convert.ToBase64String(decryptTextBytes);
    }
}
