using LINGYUN.Abp.Aliyun.Settings;

namespace LINGYUN.Abp.Aliyun.Captcha.Settings;

public static class AliyunCaptchaSettingNames
{
    public const string CaptchaPrefix = AliyunSettingNames.Prefix + ".Captcha";
    /// <summary>
    /// 验证码场景的唯一标识
    /// </summary>
    public const string SceneId = CaptchaPrefix + ".SceneId";
    /// <summary>
    /// 控制台获取的 身份标
    /// </summary>
    public const string Prefix = CaptchaPrefix + ".Prefix";
    /// <summary>
    /// 控制台获取的 ekey
    /// </summary>
    public const string EKey = CaptchaPrefix + ".EKey";
    /// <summary>
    /// 使用加密验证码场景标识
    /// </summary>
    public const string UseEncryptedSceneId = CaptchaPrefix + ".UseEncryptedSceneId";
    /// <summary>
    /// 密文过期时间，单位秒，范围 1~86400
    /// </summary>
    public const string EncryptedExpireTimeSec = CaptchaPrefix + ".EncryptedExpireTimeSec";
}
