using LINGYUN.Abp.WeChat.Work;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OpenIddict.WeChat.Work;

[DependsOn(
    typeof(AbpWeChatWorkModule),
    typeof(AbpOpenIddictAspNetCoreModule))]
public class AbpOpenIddictWeChatWorkModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder
                .AllowWeChatWorkFlow()
                .RegisterWeChatWorkScopes()
                .RegisterWeChatWorkClaims();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.TryAdd(
                AbpWeChatWorkGlobalConsts.GrantType,
                new WeChatWorkTokenExtensionGrant());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictWeChatWorkModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddVirtualJson("/LINGYUN/Abp/OpenIddict/WeChat/Work/Localization/Resources");
        });
    }
}
