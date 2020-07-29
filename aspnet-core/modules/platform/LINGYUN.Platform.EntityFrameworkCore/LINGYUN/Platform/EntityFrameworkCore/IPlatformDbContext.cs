using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    [ConnectionStringName(AppPlatformDbProperties.ConnectionStringName)]
    public interface IPlatformDbContext : IEfCoreDbContext
    {
        DbSet<Route> Routes { get; set; }
        DbSet<AppVersion> AppVersions { get; set; }
    }
}
