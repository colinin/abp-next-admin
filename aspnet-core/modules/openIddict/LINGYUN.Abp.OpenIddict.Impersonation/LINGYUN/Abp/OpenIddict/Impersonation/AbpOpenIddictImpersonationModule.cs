using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OpenIddict.Impersonation;

[DependsOn(
    typeof(AbpOpenIddictAspNetCoreModule))]
public class AbpOpenIddictImpersonationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AllowImpersonationFlow();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.TryAdd(
                ImpersonationTokenExtensionGrantConsts.GrantType,
                new ImpersonationTokenExtensionGrant());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictImpersonationModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddVirtualJson("/LINGYUN/Abp/OpenIddict/Impersonation/Localization/Resources");
        });
    }
}
