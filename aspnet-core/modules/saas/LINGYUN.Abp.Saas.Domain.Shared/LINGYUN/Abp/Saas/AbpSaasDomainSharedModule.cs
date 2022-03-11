using LINGYUN.Abp.Saas.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Saas;

[DependsOn(typeof(AbpValidationModule))]
public class AbpSaasDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSaasDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpSaasResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/Saas/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(AbpSaasErrorCodes.Namespace, typeof(AbpSaasResource));
            // 见租户管理模块
            options.MapCodeNamespace("Volo.AbpIo.MultiTenancy", typeof(AbpSaasResource));
        });
    }
}
