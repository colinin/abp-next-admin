using LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;
using LINGYUN.Abp.Demo.EntityFrameworkCore;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;
using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore;

[ConnectionStringName("SingleDbMigrator")]
public class SingleMigrationsDbContext : AbpDbContext<SingleMigrationsDbContext>
{
    public SingleMigrationsDbContext(DbContextOptions<SingleMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigureIdentity();
        modelBuilder.ConfigureIdentityServer();
        modelBuilder.ConfigureOpenIddict();
        modelBuilder.ConfigureSaas();
        modelBuilder.ConfigureFeatureManagement();
        modelBuilder.ConfigureSettingManagement();
        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureTextTemplating();
        modelBuilder.ConfigureTaskManagement();
        modelBuilder.ConfigureWebhooksManagement();
        modelBuilder.ConfigurePlatform();
        modelBuilder.ConfigureLocalization();
        modelBuilder.ConfigureNotifications();
        modelBuilder.ConfigureNotificationsDefinition();
        modelBuilder.ConfigureMessageService();
        modelBuilder.ConfigureDataProtectionManagement();
        modelBuilder.ConfigureWebhooksManagement();

        modelBuilder.ConfigureDemo();
    }
}
