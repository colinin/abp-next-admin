using LINGYUN.Abp.Auditing;
using LINGYUN.Abp.CachingManagement;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.IdentityServer;
using LINGYUN.Abp.LocalizationManagement;
using LINGYUN.Abp.MessageService;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.OpenIddict;
using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.TaskManagement;
using LINGYUN.Abp.TextTemplating;
using LINGYUN.Abp.WebhooksManagement;
using LINGYUN.Platform;
using LY.MicroService.BackendAdmin.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace LY.MicroService.BackendAdmin.DbMigrator;

[DependsOn(
    typeof(BackendAdminMigrationsEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpLocalizationManagementApplicationContractsModule),
    typeof(AbpCachingManagementApplicationContractsModule),
    typeof(AbpAuditingApplicationContractsModule),
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpIdentityServerApplicationContractsModule),
    typeof(AbpOpenIddictApplicationContractsModule),
    typeof(PlatformApplicationContractModule),
    typeof(AbpOssManagementApplicationContractsModule),
    typeof(AbpNotificationsApplicationContractsModule),
    typeof(AbpMessageServiceApplicationContractsModule),
    typeof(TaskManagementApplicationContractsModule),
    typeof(WebhooksManagementApplicationContractsModule),
    typeof(AbpAutofacModule)
    )]
public partial class BackendAdminDbMigratorModule : AbpModule
{
}
