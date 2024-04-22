using LINGYUN.Abp.OpenIddict.WeChat;
using LINGYUN.Abp.WeChat.Common.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeChatOpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowWeChatFlow(this OpenIddictServerBuilder builder)
    {
        return builder
            .AllowCustomFlow(WeChatTokenExtensionGrantConsts.OfficialGrantType)
            .AllowCustomFlow(WeChatTokenExtensionGrantConsts.MiniProgramGrantType);
    }

    public static OpenIddictServerBuilder RegisterWeChatScopes(this OpenIddictServerBuilder builder)
    {
        return builder.RegisterScopes(new[]
            {
                WeChatTokenExtensionGrantConsts.ProfileKey,
            });
    }

    public static OpenIddictServerBuilder RegisterWeChatClaims(this OpenIddictServerBuilder builder)
    {
        return builder.RegisterClaims(new[]
            {
                AbpWeChatClaimTypes.Country,
                AbpWeChatClaimTypes.Province,
                AbpWeChatClaimTypes.City,
                AbpWeChatClaimTypes.AvatarUrl,
                AbpWeChatClaimTypes.Sex,
                AbpWeChatClaimTypes.UnionId,
                AbpWeChatClaimTypes.OpenId,
                AbpWeChatClaimTypes.NickName,
                AbpWeChatClaimTypes.Privilege,
            });
    }
}
