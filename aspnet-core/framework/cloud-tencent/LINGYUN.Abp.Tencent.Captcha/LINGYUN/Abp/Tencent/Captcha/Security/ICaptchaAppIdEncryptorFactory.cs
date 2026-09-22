namespace LINGYUN.Abp.Tencent.Captcha.Security;

public interface ICaptchaAppIdEncryptorFactory
{
    ICaptchaAppIdEncryptor Create(string aidEncryptedType);
}
