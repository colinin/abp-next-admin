using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Packages;
using LINGYUN.Platform.Portal;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.EntityFrameworkCore;

[ConnectionStringName(PlatformDbProperties.ConnectionStringName)]
public class PlatformDbContext : AbpDbContext<PlatformDbContext>, IPlatformDbContext
{
    public DbSet<RoleMenu> RoleMenus { get; set; }
    public DbSet<UserMenu> UserMenus { get; set; }
    public DbSet<UserFavoriteMenu> UserFavoriteMenus { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Layout> Layouts { get; set; }
    public DbSet<Data> Datas { get; set; }
    public DbSet<DataItem> DataItems { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<PackageBlob> PackageBlobs { get; set; }
    public DbSet<Enterprise> Enterprises { get; set; }
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
