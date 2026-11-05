using LINGYUN.Abp.OperationRateLimiting.Management;
using LINGYUN.Abp.OperationRateLimiting.Management.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(typeof(AbpLocalizationModule))]
public class AbpOperationRateLimitingDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOperationRateLimitingDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<OperationRateLimitingManagementResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/OperationRateLimiting/Management/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(
                AbpOperationRateLimitingManagementErrorCodes.Namespace,
                typeof(OperationRateLimitingManagementResource));
        });
    }
}
