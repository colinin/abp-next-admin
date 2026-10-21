namespace LINGYUN.Abp.Tencent.Captcha.Security;

public interface ICaptchaAppIdEncryptor
{
    string Encrypt(string plaintext, byte[] key, byte[] iv, byte[]? aad = null);

    byte[] PadKey(byte[] appSecretKey);
}
