using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.MiniProgram.Features;
using LINGYUN.Abp.WeChat.OpenId;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace LINGYUN.Abp.OpenIddict.WeChat.Controllers;

public class WeChatMiniProgramTokenController : WeChatTokenController
{
    public override string Name => WeChatTokenExtensionGrantConsts.MiniProgramGrantType;

    public override string LoginProvider => AbpWeChatMiniProgramConsts.ProviderName;

    public override string AuthenticationMethod => AbpWeChatMiniProgramConsts.AuthenticationMethod;

    protected async override Task CheckFeatureAsync(ExtensionGrantContext context)
    {
        if (!await FeatureChecker.IsEnabledAsync(WeChatMiniProgramFeatures.EnableAuthorization))
        {
            throw new AbpException(L["MiniProgramAuthorizationDisabledMessage"]);
        }
    }

    protected async override Task<WeChatOpenId> FindOpenIdAsync(string code)
    {
        var optionsFactory = LazyServiceProvider.LazyGetRequiredService<AbpWeChatMiniProgramOptionsFactory>();
        var options = await optionsFactory.CreateAsync();

        return await WeChatOpenIdFinder.FindAsync(code, options.AppId, options.AppSecret);
    }
}
