using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.RealtimeMessage.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpDataDbMigratorModule)
    )]
public class RealtimeMessageMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<RealtimeMessageMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL(
                mysql =>
                {
                    // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
                    mysql.TranslateParameterizedCollectionsToConstants();
                });
        });
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.Databases.Configure("Platform", database =>
            {
                database.MapConnection("AbpSaas");
                database.MapConnection("Workflow");
                database.MapConnection("AppPlatform");
                database.MapConnection("TaskManagement");
                database.MapConnection("AbpAuditLogging");
                database.MapConnection("AbpTextTemplating");
                database.MapConnection("AbpSettingManagement");
                database.MapConnection("AbpFeatureManagement");
                database.MapConnection("AbpPermissionManagement");
                database.MapConnection("AbpLocalizationManagement");
                database.MapConnection("AbpDataProtectionManagement");
            });
            options.Databases.Configure("Identity", database =>
            {
                database.MapConnection("AbpGdpr");
                database.MapConnection("AbpIdentity");
                database.MapConnection("AbpOpenIddict");
                database.MapConnection("AbpIdentityServer");
            });
            options.Databases.Configure("Realtime", database =>
            {
                database.MapConnection("Notifications");
                database.MapConnection("MessageService");
            });
        });
    }
}
