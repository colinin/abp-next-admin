using LINGYUN.Abp.Account;
using LINGYUN.Abp.Auditing;
using LINGYUN.Abp.CachingManagement;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.DataProtectionManagement;
using LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;
using LINGYUN.Abp.FeatureManagement;
using LINGYUN.Abp.Gdpr;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.IdentityServer;
using LINGYUN.Abp.LocalizationManagement;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MessageService;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.OpenIddict;
using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.ProjectManagement;
using LINGYUN.Abp.RulesEngineManagement;
using LINGYUN.Abp.Saas;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.TaskManagement;
using LINGYUN.Abp.TextTemplating;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using LINGYUN.Abp.WebhooksManagement;
using LINGYUN.Platform;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.AdminService;

[DependsOn(
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpAuditingApplicationContractsModule),
    typeof(AbpCachingManagementApplicationContractsModule),
    typeof(AbpDataProtectionManagementApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpGdprApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpIdentityServerApplicationContractsModule),
    typeof(AbpLocalizationManagementApplicationContractsModule),
    typeof(AbpOpenIddictApplicationContractsModule),
    typeof(AbpOssManagementApplicationContractsModule),
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
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpDataProtectionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),// 用户角色权限需要引用包
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpDataDbMigratorModule)
    )]
public class AdminServiceMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AdminServiceMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });
    }
}
