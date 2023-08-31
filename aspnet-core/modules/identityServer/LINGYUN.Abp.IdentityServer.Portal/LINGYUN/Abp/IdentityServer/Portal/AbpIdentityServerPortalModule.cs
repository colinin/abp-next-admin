using LINGYUN.Platform;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.IdentityServer.Portal;

[DependsOn(
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(PlatformDomainModule))]
public class AbpIdentityServerPortalModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<IIdentityServerBuilder>(builder =>
        {
            builder.AddExtensionGrantValidator<PortalGrantValidator>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityServerPortalModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpIdentityServerResource>()
                .AddVirtualJson("/LINGYUN/Abp/IdentityServer/Portal/Localization");
        });
    }
}
