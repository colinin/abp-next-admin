namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Security;

public interface ICaptchaAppIdEncryptor
{
    string Encrypt(string plaintext, byte[] key, byte[] iv, byte[]? aad = null);

    byte[] PadKey(byte[] appSecretKey);
}
