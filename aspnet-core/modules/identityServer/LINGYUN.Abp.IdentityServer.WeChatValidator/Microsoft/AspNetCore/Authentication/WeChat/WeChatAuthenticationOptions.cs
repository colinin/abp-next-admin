using LINGYUN.Abp.WeChat.Authorization;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace Microsoft.AspNetCore.Authentication.WeChat
{
    public class WeChatAuthenticationOptions : OAuthOptions
    {
        public string AppId
        {
            get => ClientId;
            set => ClientId = value;
        }

        public string AppSecret
        {
            get => ClientSecret;
            set => ClientSecret = value;
        }

        public WeChatAuthenticationOptions()
        {
            ClaimsIssuer = AbpWeChatAuthorizationConsts.ProviderKey;
            CallbackPath = new PathString(AbpWeChatAuthorizationConsts.CallbackPath);

            AuthorizationEndpoint = AbpWeChatAuthorizationConsts.AuthorizationEndpoint;
            TokenEndpoint = AbpWeChatAuthorizationConsts.TokenEndpoint;
            UserInformationEndpoint = AbpWeChatAuthorizationConsts.UserInformationEndpoint;

            Scope.Add(AbpWeChatAuthorizationConsts.LoginScope);
            Scope.Add(AbpWeChatAuthorizationConsts.UserInfoScope);

            // 这个原始的属性一定要写进去,框架与UserLogin.ProviderKey进行关联判断是否绑定微信
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");

            // 把自定义的身份标识写进令牌
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.OpenId, "openid");
            ClaimActions.MapJsonKey(AbpWeChatClaimTypes.UnionId, "unionid"); // TODO: 可用作tenant对比?
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
