using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.Official;

namespace LINGYUN.Abp.IdentityServer.WeChat.Official
{
    /// <summary>
    /// 与微信公众号认证相关的静态(可变)常量
    /// </summary>
    public static class WeChatOfficialOAuthConsts
    {
        /// <summary>
        /// 微信个人信息标识
        /// </summary>
        public static string ProfileKey { get; set; } = "wechat.profile";
        /// <summary>
        /// 微信提供者标识
        /// </summary>
        public static string ProviderKey => AbpWeChatOfficialConsts.ProviderName;
        /// <summary>
        /// 微信提供者显示名称
        /// </summary>
        public static string DisplayName => AbpWeChatGlobalConsts.DisplayName;
        /// <summary>
        /// 回调地址
        /// </summary>
        public static string CallbackPath { get; set; } = "/signin-wechat";

        /// <summary>
        /// 微信客户端外的网页登录
        /// </summary>
        public const string QrConnectEndpoint = "https://open.weixin.qq.com/connect/qrconnect";

        /// <summary>
        /// 微信客户端内的网页登录
        /// </summary>
        public const string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";

        /// <summary>
        /// 用户允许授权后通过返回的code换取access_token地址
        /// </summary>
        public const string TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";

        /// <summary>
        /// 使用access_token获取用户个人信息地址
        /// </summary>
        public const string UserInformationEndpoint = "https://api.weixin.qq.com/sns/userinfo";
        /// <summary>
        /// 弹出授权页面，可通过openid拿到昵称、性别、所在地。
        /// 并且， 即使在未关注的情况下，只要用户授权，也能获取其信息
        /// <br />
        /// <br />
        /// 详询: <see cref="https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/Wechat_webpage_authorization.html"/>
        /// </summary>
        /// <remarks>
        /// 以snsapi_userinfo为scope发起的网页授权，是用来获取用户的基本信息的。
        /// 但这种授权需要用户手动同意，并且由于用户同意过，所以无须关注，就可在授权后获取该用户的基本信息
        /// </remarks>
        public const string UserInfoScope = "snsapi_userinfo";
        /// <summary>
        /// 不弹出授权页面，直接跳转，只能获取用户openid
        /// <br />
        /// <br />
        /// 详询: <see cref="https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/Wechat_webpage_authorization.html"/>
        /// </summary>
        /// <remarks>
        /// 以snsapi_base为scope发起的网页授权，是用来获取进入页面的用户的openid的，并且是静默授权并自动跳转到回调页的。
        /// 用户感知的就是直接进入了回调页（往往是业务页面）
        /// </remarks>
        public const string LoginScope = "snsapi_login";
    }
}
