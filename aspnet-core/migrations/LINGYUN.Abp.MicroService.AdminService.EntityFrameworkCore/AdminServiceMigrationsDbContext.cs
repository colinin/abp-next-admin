using LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.AdminService.EntityFrameworkCore;

[ConnectionStringName("BackendAdminDbMigrator")]
public class AdminServiceMigrationsDbContext : AbpDbContext<AdminServiceMigrationsDbContext>
{
    public AdminServiceMigrationsDbContext(DbContextOptions<AdminServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureSaas();
        modelBuilder.ConfigureTextTemplating();
        modelBuilder.ConfigureFeatureManagement();
        modelBuilder.ConfigureSettingManagement();
        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureDataProtectionManagement();

        modelBuilder.Entity<ResourcePermissionGrant>(b =>
        {
            var maxResourceNameLength = modelBuilder.IsUsingMySQL() ? 128 : PermissionGrantConsts.MaxResourceNameLength;
            var maxResourceKeyLength = modelBuilder.IsUsingMySQL() ? 128 : PermissionGrantConsts.MaxResourceKeyLength;

            b.Property(x => x.ResourceName).HasMaxLength(maxResourceNameLength).IsRequired();
            b.Property(x => x.ResourceKey).HasMaxLength(maxResourceKeyLength).IsRequired();
        });
    }
}
