using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.LocalizationManagement.DbMigrator.EntityFrameworkCore;

[ConnectionStringName("LocalizationManagementDbMigrator")]
public class LocalizationManagementMigrationsDbContext : AbpDbContext<LocalizationManagementMigrationsDbContext>
{
    public LocalizationManagementMigrationsDbContext(DbContextOptions<LocalizationManagementMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureLocalization();
    }
}
