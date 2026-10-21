using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using System.Threading.Tasks;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace LINGYUN.Abp.OpenIddict.WeChat.Work;

public class WeChatWorkMiniProgramTokenExtensionGrant : WeChatWorkTokenExtensionGrant
{
    public override string Name => AbpWeChatWorkGlobalConsts.MiniProgramGrantType;

    protected async override Task<WeChatWorkUserInfo> FindUserInfoAsync(ExtensionGrantContext context, string code)
    {
        var userFinder = GetRequiredService<IWeChatWorkUserFinder>(context);

        var code2Session = await userFinder.GetCode2SessionAsync(code);

        return new WeChatWorkUserInfo
        {
            UserId = code2Session.UserId,
            UserTicket = code2Session.SessionKey,
        };
    }
}
