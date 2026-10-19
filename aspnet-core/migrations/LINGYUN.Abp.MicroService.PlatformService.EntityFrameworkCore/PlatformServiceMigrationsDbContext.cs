using LINGYUN.Abp.BlobManagement.EntityFrameworkCore;
using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.PlatformService.EntityFrameworkCore;

[ConnectionStringName("PlatformDbMigrator")]
public class PlatformServiceMigrationsDbContext : AbpDbContext<PlatformServiceMigrationsDbContext>
{
    public PlatformServiceMigrationsDbContext(DbContextOptions<PlatformServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePlatform();
        modelBuilder.ConfigureBlobManagement();
    }
}
