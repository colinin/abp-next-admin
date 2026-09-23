using LINGYUN.Abp.Account;
using LINGYUN.Abp.Auditing;
using LINGYUN.Abp.BlobManagement;
using LINGYUN.Abp.CachingManagement;
using LINGYUN.Abp.DataProtectionManagement;
using LINGYUN.Abp.FeatureManagement;
using LINGYUN.Abp.Gdpr;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.LocalizationManagement;
using LINGYUN.Abp.MessageService;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.OpenIddict;
using LINGYUN.Abp.PermissionManagement;
using LINGYUN.Abp.ProjectManagement;
using LINGYUN.Abp.RulesEngineManagement;
using LINGYUN.Abp.Saas;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.TaskManagement;
using LINGYUN.Abp.TextTemplating;
using LINGYUN.Abp.WebhooksManagement;
using LINGYUN.Platform;
using LY.MicroService.BackendAdmin.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.BackendAdmin.DbMigrator;

[DependsOn(
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpAuditingApplicationContractsModule),
    typeof(AbpCachingManagementApplicationContractsModule),
    typeof(AbpDataProtectionManagementApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpGdprApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpLocalizationManagementApplicationContractsModule),
    typeof(AbpOpenIddictApplicationContractsModule),
    typeof(AbpBlobManagementApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(PlatformApplicationContractModule),
    typeof(AbpProjectManagementApplicationContractsModule),
    typeof(AbpMessageServiceApplicationContractsModule),
    typeof(AbpNotificationsApplicationContractsModule),
    typeof(RulesEngineManagementApplicationContractsModule),
    typeof(AbpSaasApplicationContractsModule),
    typeof(TaskManagementApplicationContractsModule),
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(WebhooksManagementApplicationContractsModule))]

[DependsOn(
    typeof(BackendAdminMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class BackendAdminDbMigratorModule : AbpModule
{
}
