using LINGYUN.Abp.WeChat.Work.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Volo.Abp.Security.Claims;

namespace Microsoft.AspNetCore.Authentication.WeChat.Work;

public class WeChatWorkOAuthOptions : OAuthOptions
{
    /// <summary>
    /// 企业Id
    /// </summary>
    public string CorpId { get; set; }
    public WeChatWorkOAuthOptions()
    {
        // 用于防止初始化错误,会在OAuthHandler.InitializeHandlerAsync中进行重写
        CorpId = "CorpId";
        ClientId = "ClientId";
        ClientSecret = "ClientSecret";

        CallbackPath = new PathString(WeChatWorkOAuthConsts.CallbackPath);

        AuthorizationEndpoint = WeChatWorkOAuthConsts.AuthorizationEndpoint;
        TokenEndpoint = WeChatWorkOAuthConsts.TokenEndpoint;
        UserInformationEndpoint = WeChatWorkOAuthConsts.UserInformationEndpoint;

        Scope.Add(WeChatWorkOAuthConsts.LoginScope);
        Scope.Add(WeChatWorkOAuthConsts.UserInfoScope);

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userid");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "userid");
        ClaimActions.MapJsonKey("sub", "userid");

        // 把自定义的身份标识写进令牌
        ClaimActions.MapJsonKey(AbpWeChatWorkClaimTypes.UserId, "userid");
        ClaimActions.MapJsonKey(AbpWeChatWorkClaimTypes.QrCode, "qr_code");
        ClaimActions.MapJsonKey(AbpWeChatWorkClaimTypes.BizMail, "biz_mail");
        ClaimActions.MapJsonKey(AbpWeChatWorkClaimTypes.Address, "address");
        ClaimActions.MapJsonKey(AbpClaimTypes.PhoneNumber, "mobile");
        ClaimActions.MapJsonKey(AbpClaimTypes.Email, "email");
    }
}
