using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    public class PlatformHttpApiHostMigrationsDbContext : AbpDbContext<PlatformHttpApiHostMigrationsDbContext>
    {
        public PlatformHttpApiHostMigrationsDbContext(DbContextOptions<PlatformHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePlatform();
        }
    }
}
