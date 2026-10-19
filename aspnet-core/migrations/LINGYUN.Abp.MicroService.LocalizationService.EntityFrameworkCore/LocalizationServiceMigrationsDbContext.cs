using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.LocalizationService.EntityFrameworkCore;

[ConnectionStringName("LocalizationManagementDbMigrator")]
public class LocalizationServiceMigrationsDbContext : AbpDbContext<LocalizationServiceMigrationsDbContext>
{
    public LocalizationServiceMigrationsDbContext(DbContextOptions<LocalizationServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureLocalization();
    }
}
