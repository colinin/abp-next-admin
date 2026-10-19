using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using System.Threading.Tasks;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace LINGYUN.Abp.OpenIddict.WeChat.Work;

public class WeChatWorkOAuthTokenExtensionGrant : WeChatWorkTokenExtensionGrant
{
    public override string Name => AbpWeChatWorkGlobalConsts.OAuthGrantType;

    protected async override Task<WeChatWorkUserInfo> FindUserInfoAsync(ExtensionGrantContext context, string code)
    {
        var userFinder = GetRequiredService<IWeChatWorkUserFinder>(context);

        return await userFinder.GetUserInfoAsync(code);
    }
}
