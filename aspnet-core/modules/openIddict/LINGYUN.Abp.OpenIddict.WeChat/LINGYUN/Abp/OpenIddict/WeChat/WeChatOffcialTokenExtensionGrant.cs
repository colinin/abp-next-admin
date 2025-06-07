using LINGYUN.Abp.WeChat.Official;
using LINGYUN.Abp.WeChat.OpenId;
using System.Threading.Tasks;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace LINGYUN.Abp.OpenIddict.WeChat;
public class WeChatOffcialTokenExtensionGrant : WeChatTokenExtensionGrant
{
    public override string Name => WeChatTokenExtensionGrantConsts.OfficialGrantType;

    public override string LoginProvider => AbpWeChatOfficialConsts.ProviderName;

    public override string AuthenticationMethod => AbpWeChatOfficialConsts.AuthenticationMethod;

    //protected async override Task CheckFeatureAsync(ExtensionGrantContext context)
    //{
    //    var featureChecker = GetRequiredService<IFeatureChecker>(context);

    //    if (!await featureChecker.IsEnabledAsync(WeChatOfficialFeatures.EnableAuthorization))
    //    {
    //        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);

    //        throw new AbpException(localizer["OfficialAuthorizationDisabledMessage"]);
    //    }
    //}

    protected async override Task<WeChatOpenId> FindOpenIdAsync(ExtensionGrantContext context, string code)
    {
        var weChatOpenIdFinder = GetRequiredService<IWeChatOpenIdFinder>(context);
        var optionsFactory = GetRequiredService<AbpWeChatOfficialOptionsFactory>(context);

        var options = await optionsFactory.CreateAsync();

        return await weChatOpenIdFinder.FindAsync(code, options.AppId, options.AppSecret);
    }
}
