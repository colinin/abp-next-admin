namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Security;

public interface ICaptchaAppIdEncryptorFactory
{
    ICaptchaAppIdEncryptor Create(string aidEncryptedType);
}
