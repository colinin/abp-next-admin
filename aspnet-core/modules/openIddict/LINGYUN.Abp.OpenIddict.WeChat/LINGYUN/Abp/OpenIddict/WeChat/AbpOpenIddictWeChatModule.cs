using LINGYUN.Abp.Identity.WeChat;
using LINGYUN.Abp.OpenIddict.WeChat.Controllers;
using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.Official;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OpenIddict.WeChat;

[DependsOn(
    typeof(AbpWeChatOfficialModule),
    typeof(AbpWeChatMiniProgramModule),
    typeof(AbpIdentityWeChatModule),
    typeof(AbpOpenIddictAspNetCoreModule))]
public class AbpOpenIddictWeChatModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder
                .AllowWeChatFlow()
                .RegisterWeChatScopes()
                .RegisterWeChatClaims();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.TryAdd(
                WeChatTokenExtensionGrantConsts.OfficialGrantType,
                new WeChatOfficialTokenController());

            options.Grants.TryAdd(
                WeChatTokenExtensionGrantConsts.MiniProgramGrantType,
                new WeChatMiniProgramTokenController());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictWeChatModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddVirtualJson("/LINGYUN/Abp/OpenIddict/WeChat/Localization/Resources");
        });
    }
}