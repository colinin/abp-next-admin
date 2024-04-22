using LINGYUN.Abp.Authentication.WeChat;
using LINGYUN.Abp.WeChat.Common.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace Microsoft.AspNetCore.Authentication.WeChat.Official
{
    public class WeChatOfficialOAuthOptions : OAuthOptions
    {
        public WeChatOfficialOAuthOptions()
        {
            // 用于防止初始化错误,会在OAuthHandler.InitializeHandlerAsync中进行重写
            ClientId = "WeChatOfficial";
            ClientSecret = "WeChatOfficial";

            ClaimsIssuer = AbpAuthenticationWeChatConsts.ProviderKey;
            CallbackPath = new PathString(AbpAuthenticationWeChatConsts.CallbackPath);

            AuthorizationEndpoint = AbpAuthenticationWeChatConsts.AuthorizationEndpoint;
            TokenEndpoint = AbpAuthenticationWeChatConsts.TokenEndpoint;
            UserInformationEndpoint = AbpAuthenticationWeChatConsts.UserInformationEndpoint;

            Scope.Add(AbpAuthenticationWeChatConsts.LoginScope);
            Scope.Add(AbpAuthenticationWeChatConsts.UserInfoScope);

            // 这个原始的属性一定要写进去,框架与UserLogin.ProviderKey进行关联判断是否绑定微信
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");

            // 把自定义的身份标识写进令牌
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.OpenId, "openid");
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.UnionId, "unionid");// 公众号如果与小程序关联,这个可以用上
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.NickName, "nickname");
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.Sex, "sex", ClaimValueTypes.Integer);
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.Province, "province");
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.City, "city");
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.AvatarUrl, "headimgurl");
            ClaimActions.MapCustomJson(AbpWeChatClaimTypes.Privilege, user =>
            {
                return string.Join(",", user.GetStrings("privilege"));
            });
        }
    }
}
