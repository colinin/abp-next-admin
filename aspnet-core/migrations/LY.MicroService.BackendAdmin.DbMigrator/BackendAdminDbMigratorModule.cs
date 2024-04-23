using LINGYUN.Abp.TextTemplating;
using LY.MicroService.BackendAdmin.EntityFrameworkCore;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace LY.MicroService.BackendAdmin.DbMigrator;

[DependsOn(
    typeof(BackendAdminMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class BackendAdminDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SettingManagementOptions>(options =>
        {
            options.IsDynamicSettingStoreEnabled = true;
            options.SaveStaticSettingsToDatabase = true;
        });
        Configure<FeatureManagementOptions>(options =>
        {
            options.IsDynamicFeatureStoreEnabled = true;
            options.SaveStaticFeaturesToDatabase = true;
        });
        Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = true;
            options.SaveStaticPermissionsToDatabase = true;
        });
        Configure<AbpTextTemplatingCachingOptions>(options =>
        {
            options.IsDynamicTemplateDefinitionStoreEnabled = true;
            options.SaveStaticTemplateDefinitionToDatabase = true;
        });
    }
}
