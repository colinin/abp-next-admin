using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.MiniProgram.Features;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;

namespace LINGYUN.Abp.OpenIddict.WeChat;
public class WeChatMiniProgramTokenExtensionGrant : WeChatTokenExtensionGrant
{
    public override string Name => WeChatTokenExtensionGrantConsts.MiniProgramGrantType;

    public override string LoginProvider => AbpWeChatMiniProgramConsts.ProviderName;

    public override string AuthenticationMethod => AbpWeChatMiniProgramConsts.AuthenticationMethod;

    protected async override Task CheckFeatureAsync(ExtensionGrantContext context)
    {
        var featureChecker = GetRequiredService<IFeatureChecker>(context);

        if (!await featureChecker.IsEnabledAsync(WeChatMiniProgramFeatures.EnableAuthorization))
        {
            var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);

            throw new AbpException(localizer["MiniProgramAuthorizationDisabledMessage"]);
        }
    }

    protected async override Task<WeChatOpenId> FindOpenIdAsync(ExtensionGrantContext context, string code)
    {
        var weChatOpenIdFinder = GetRequiredService<IWeChatOpenIdFinder>(context);
        var optionsFactory = GetRequiredService<AbpWeChatMiniProgramOptionsFactory>(context);

        var options = await optionsFactory.CreateAsync();

        return await weChatOpenIdFinder.FindAsync(code, options.AppId, options.AppSecret);
    }
}
