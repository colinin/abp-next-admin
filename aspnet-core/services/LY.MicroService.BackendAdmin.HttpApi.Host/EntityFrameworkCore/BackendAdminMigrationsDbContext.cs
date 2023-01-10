using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using Volo.Abp.Data;

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore;

[ConnectionStringName("BackendAdminDbMigrator")]
public class BackendAdminMigrationsDbContext : AbpDbContext<BackendAdminMigrationsDbContext>
{
    public BackendAdminMigrationsDbContext(DbContextOptions<BackendAdminMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureFeatureManagement();
        modelBuilder.ConfigureSaas();
        modelBuilder.ConfigureSettingManagement();
        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureTextTemplating();
    }
}
