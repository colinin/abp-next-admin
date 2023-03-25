using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Localization;
using Volo.Abp.VirtualFileSystem;
using VoloAbpPermissionManagementApplicationContractsModule = Volo.Abp.PermissionManagement.AbpPermissionManagementApplicationContractsModule;

namespace LINGYUN.Abp.PermissionManagement;

[DependsOn(
    typeof(VoloAbpPermissionManagementApplicationContractsModule))]
public class AbpPermissionManagementApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPermissionManagementApplicationContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpPermissionManagementResource>()
                .AddVirtualJson("/LINGYUN/Abp/PermissionManagement/Localization/Application/Contracts");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(PermissionManagementErrorCodes.Namespace, typeof(AbpPermissionManagementResource));
        });
    }
}
