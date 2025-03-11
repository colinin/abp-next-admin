using LINGYUN.Abp.WeChat.Work;

namespace Microsoft.AspNetCore.Authentication.WeChat.Work;

public static class WeChatWorkOAuthConsts
{
    /// <summary>
    /// 企业微信授权名称
    /// </summary>
    public static string AuthenticationScheme => AbpWeChatWorkGlobalConsts.ProviderName;
    /// <summary>
    /// 微信提供者显示名称
    /// </summary>
    public static string DisplayName => AbpWeChatWorkGlobalConsts.DisplayName;
    /// <summary>
    /// 回调地址
    /// </summary>
    public static string CallbackPath { get; set; } = "/signin-wxwork";
    /// <summary>
    /// 微信客户端内的网页登录
    /// </summary>
    public const string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
    /// <summary>
    /// Web网页登录
    /// </summary>
    public const string AuthorizationSsoEndpoint = "https://login.work.weixin.qq.com/wwlogin/sso/login";
    /// <summary>
    /// 用户允许授权后通过返回的code换取access_token地址
    /// </summary>
    public const string TokenEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

    /// <summary>
    /// 使用access_token获取用户个人信息地址
    /// </summary>
    public const string UserInfoEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/auth/getuserinfo";
    /// <summary>
    /// 使用access_token获取用户敏感信息地址
    /// </summary>
    public const string UserDetailEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/auth/getuserdetail";

    public const string UserInfoScope = "snsapi_privateinfo";

    public const string LoginScope = "snsapi_base";
}
