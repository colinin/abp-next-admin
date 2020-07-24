using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    [ConnectionStringName(PlatformDbProperties.ConnectionStringName)]
    public class PlatformDbContext : AbpDbContext<PlatformDbContext>, IPlatformDbContext
    {
        public DbSet<Route> Routes { get; set; }

        public DbSet<AppVersion> AppVersions { get; set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePlatform();
        }
    }
}
