using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeChatWorkOpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowWeChatWorkFlow(this OpenIddictServerBuilder builder)
    {
        return builder
            .AllowCustomFlow(AbpWeChatWorkGlobalConsts.GrantType);
    }

    public static OpenIddictServerBuilder RegisterWeChatWorkScopes(this OpenIddictServerBuilder builder)
    {
        return builder.RegisterScopes(new[]
        {
            AbpWeChatWorkGlobalConsts.ProfileKey,
        });
    }

    public static OpenIddictServerBuilder RegisterWeChatWorkClaims(this OpenIddictServerBuilder builder)
    {
        return builder.RegisterClaims(new[]
        {
            AbpWeChatWorkClaimTypes.UserId,
        });
    }
}
