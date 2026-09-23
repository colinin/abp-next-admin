using LINGYUN.Abp.Tencent.Settings;

namespace LINGYUN.Abp.Tencent.Captcha.Settings;

public static class TencentCaptchaSettingNames
{
    private const string Prefix = TencentCloudSettingNames.Prefix + ".Captcha";
    /// <summary>
    /// 验证码业务ID
    /// </summary>
    public const string CaptchaAppId = Prefix + ".CaptchaAppId";
    /// <summary>
    /// 原始密钥
    /// </summary>
    public const string AppSecretKey = Prefix + ".AppSecretKey";
    /// <summary>
    /// 加密方式
    /// </summary>
    public const string CaptchaAppIdEncryptedType = Prefix + ".CaptchaAppIdEncryptedType";
    /// <summary>
    /// 过期时间（秒），最大值 86400 秒
    /// </summary>
    public const string CaptchaAppIdEncryptedExpireTime = Prefix + ".CaptchaAppIdEncryptedExpireTime";
}
