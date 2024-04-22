using LINGYUN.Abp.Authentication.QQ;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authentication.QQ
{
    public class QQConnectOAuthOptions : OAuthOptions
    {
        /// <summary>
        /// 是否移动端样式
        /// </summary>
        public bool IsMobile { get; set; }
        /// <summary>
        /// 获取用户OpenID_OAuth2.0
        /// </summary>
        public string OpenIdEndpoint { get; set; }

        public QQConnectOAuthOptions()
        {
            // 用于防止初始化错误,会在OAuthHandler.InitializeHandlerAsync中进行重写
            ClientId = "QQConnect";
            ClientSecret = "QQConnect";

            ClaimsIssuer = "connect.qq.com";
            CallbackPath = new PathString(AbpAuthenticationQQConsts.CallbackPath);

            AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";
            TokenEndpoint = "https://graph.qq.com/oauth2.0/token";
            OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";
            UserInformationEndpoint = "https://graph.qq.com/user/get_user_info";

            Scope.Add("get_user_info");

            // 这个原始的属性一定要写进去,框架关联判断是否绑定QQ
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");

            // 把自定义的身份标识写进令牌
            ClaimActions.MapJsonKey(AbpQQClaimTypes.OpenId, "openid");
            ClaimActions.MapJsonKey(AbpQQClaimTypes.NickName, "nickname");
            ClaimActions.MapJsonKey(AbpQQClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(AbpQQClaimTypes.AvatarUrl, "figureurl_qq_1");
        }
    }
}
