using LINGYUN.Abp.AuditLogging.EntityFrameworkCore;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.IdentityServer.EntityFrameworkCore;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;
using LINGYUN.Abp.WeChat;
using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(WebhooksManagementEntityFrameworkCoreModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
    typeof(AbpWeChatModule),
    typeof(AbpDataDbMigratorModule)
    )]
public class SingleMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SingleMigrationsDbContext>();
    }
}
