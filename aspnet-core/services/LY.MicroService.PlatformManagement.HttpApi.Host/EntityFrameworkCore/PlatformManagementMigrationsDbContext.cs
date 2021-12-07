using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.PlatformManagement.EntityFrameworkCore;

public class PlatformManagementMigrationsDbContext : AbpDbContext<PlatformManagementMigrationsDbContext>
{
    public PlatformManagementMigrationsDbContext(DbContextOptions<PlatformManagementMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePlatform();
    }
}
