using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Security;

public class CaptchaAppIdEncryptorFactory : ICaptchaAppIdEncryptorFactory, ISingletonDependency
{
    public ICaptchaAppIdEncryptor Create(string aidEncryptedType)
    {
        return aidEncryptedType.ToLower() switch
        {
            "gcm" => new GcmCaptchaAppIdEncryptor(),
            _ => new CbcCaptchaAppIdEncryptor(),
        };
    }
}
