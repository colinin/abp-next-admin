using LINGYUN.Abp.WeChat.Official;
using LINGYUN.Abp.WeChat.Official.Features;
using LINGYUN.Abp.WeChat.OpenId;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace LINGYUN.Abp.OpenIddict.WeChat.Controllers;
public class WeChatOfficialTokenController : WeChatTokenController
{
    public override string Name => WeChatTokenExtensionGrantConsts.OfficialGrantType;

    public override string LoginProvider => AbpWeChatOfficialConsts.ProviderName;

    public override string AuthenticationMethod => AbpWeChatOfficialConsts.AuthenticationMethod;

    protected async override Task CheckFeatureAsync(ExtensionGrantContext context)
    {
        if (!await FeatureChecker.IsEnabledAsync(WeChatOfficialFeatures.EnableAuthorization))
        {
            throw new AbpException(L["OfficialAuthorizationDisabledMessage"]);
        }
    }

    protected async override Task<WeChatOpenId> FindOpenIdAsync(string code)
    {
        var optionsFactory = LazyServiceProvider.LazyGetRequiredService<AbpWeChatOfficialOptionsFactory>();
        var options = await optionsFactory.CreateAsync();

        return await WeChatOpenIdFinder.FindAsync(code, options.AppId, options.AppSecret);
    }
}
