using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using VoloAbpAspNetCoreMvcUIMultiTenancyModule = Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.AbpAspNetCoreMvcUiMultiTenancyModule;

namespace LINGYUN.Abp.AspNetCore.Mvc.UI.MultiTenancy;

[DependsOn(typeof(VoloAbpAspNetCoreMvcUIMultiTenancyModule))]
public class AbpAspNetCoreMvcUiMultiTenancyModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AbpUiMultiTenancyResource),
                typeof(AbpAspNetCoreMvcUiMultiTenancyModule).Assembly
            );
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiMultiTenancyModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpUiMultiTenancyResource>()
                .AddVirtualJson("/LINGYUN/Abp/AspNetCore/Mvc/UI/MultiTenancy/Localization/Resources");
        });
    }
}
