using System;

namespace LINGYUN.Abp.Gdpr;
/// <summary>
/// AbpCookie同意选项
/// See: https://abp.io/docs/latest/modules/gdpr#abpcookieconsentoptions
/// </summary>
public class AbpCookieConsentOptions
{
    /// <summary>
    /// 启用或禁用Cookie Consent特征
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 定义 Cookie 策略页面 URL.<br />
    /// 设置后, "Cookie 策略"页面 URL 会自动添加到 Cookie 同意声明中.<br />
    /// 因此, 用户可以在接受 cookie 同意之前检查 cookie 政策.<br />
    /// 您可以将其设置为本地地址或完整 URL, 例如:<br />
    /// /CookiePolicy<br />
    /// https://example.com/cookie-policy
    /// </summary>
    public string CookiePolicyUrl { get; set; }
    /// <summary>
    /// 定义隐私政策页面 URL.<br />
    /// 设置后, "隐私政策"页面 URL 会自动添加到 Cookie 同意声明中.<br />
    /// 因此, 用户可以在接受 cookie 同意之前查看隐私政策.<br />
    /// 您可以将其设置为本地地址或完整 URL, 例如:<br />
    /// /PrivacyPolicy<br />
    /// https://example.com/privacy-policy
    /// </summary>
    public string PrivacyPolicyUrl { get; set; }
    /// <summary>
    /// 定义 Cookie 同意的 Cookie 过期时间.<br />
    /// 默认情况下, 当接受 Cookie 同意时, 它会将 Cookie 设置为有效期为 6 个月.
    /// </summary>
    public TimeSpan Expiration { get; set; }
    public AbpCookieConsentOptions()
    {
        IsEnabled = true;
        CookiePolicyUrl = "/CookiePolicy";
        PrivacyPolicyUrl = "/PrivacyPolicy";
        Expiration = TimeSpan.FromDays(180);
    }
}
