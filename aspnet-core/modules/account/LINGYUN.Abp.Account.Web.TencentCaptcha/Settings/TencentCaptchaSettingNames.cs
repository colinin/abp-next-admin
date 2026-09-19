using LINGYUN.Abp.Tencent.Settings;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Settings;

public static class TencentCaptchaSettingNames
{
    private const string Prefix = TencentCloudSettingNames.Prefix + ".Captcha";

    public const string CaptchaAppId = Prefix + ".CaptchaAppId";

    public const string AppSecretKey = Prefix + ".AppSecretKey";

    public const string CaptchaAppIdEncryptedType = Prefix + ".CaptchaAppIdEncryptedType";
    public const string CaptchaAppIdEncryptedExpireTime = Prefix + ".CaptchaAppIdEncryptedExpireTime";
}
