using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.Platform.EntityFrameworkCore;

[ConnectionStringName("PlatformDbMigrator")]
public class PlatformMigrationsDbContext : AbpDbContext<PlatformMigrationsDbContext>
{
    public PlatformMigrationsDbContext(DbContextOptions<PlatformMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePlatform();
    }
}
