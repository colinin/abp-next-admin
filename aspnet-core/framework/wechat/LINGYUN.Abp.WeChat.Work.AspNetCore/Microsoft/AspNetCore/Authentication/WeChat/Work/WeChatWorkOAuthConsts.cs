using LINGYUN.Abp.WeChat.Work;

namespace Microsoft.AspNetCore.Authentication.WeChat.Work;

public static class WeChatWorkOAuthConsts
{
    /// <summary>
    /// 微信个人信息标识
    /// </summary>
    public static string ProfileKey => AbpWeChatWorkGlobalConsts.ProfileKey;
    /// <summary>
    /// 微信提供者标识
    /// </summary>
    public static string ProviderKey => AbpWeChatWorkGlobalConsts.ProviderName;
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
    public const string AuthorizationEndpoint = "https://login.work.weixin.qq.com/wwlogin/sso/login";
    /// <summary>
    /// 用户允许授权后通过返回的code换取access_token地址
    /// </summary>
    public const string TokenEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

    /// <summary>
    /// 使用access_token获取用户个人信息地址
    /// </summary>
    public const string UserInformationEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/auth/getuserinfo";

    public const string UserInfoScope = "snsapi_privateinfo";

    public const string LoginScope = "snsapi_base";
}
