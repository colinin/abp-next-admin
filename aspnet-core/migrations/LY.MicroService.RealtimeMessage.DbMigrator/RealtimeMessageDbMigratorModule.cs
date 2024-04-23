using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Notifications;
using LY.MicroService.RealtimeMessage.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace LY.MicroService.RealtimeMessage.DbMigrator;

[DependsOn(
    typeof(RealtimeMessageMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAutofacModule)
    )]
public partial class RealtimeMessageDbMigratorModule : AbpModule
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
        Configure<AbpNotificationsManagementOptions>(options =>
        {
            options.IsDynamicNotificationsStoreEnabled = true;
            options.SaveStaticNotificationsToDatabase = true;
        });
    }
}
