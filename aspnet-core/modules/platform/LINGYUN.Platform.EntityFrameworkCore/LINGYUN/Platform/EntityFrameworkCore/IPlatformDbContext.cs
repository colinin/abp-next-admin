using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Versions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    [ConnectionStringName(PlatformDbProperties.ConnectionStringName)]
    public interface IPlatformDbContext : IEfCoreDbContext
    {
        DbSet<Menu> Menus { get; set; }
        DbSet<Layout> Layouts { get; set; }
        DbSet<Data> Datas { get; set; }
        DbSet<AppVersion> AppVersions { get; set; }
    }
}
