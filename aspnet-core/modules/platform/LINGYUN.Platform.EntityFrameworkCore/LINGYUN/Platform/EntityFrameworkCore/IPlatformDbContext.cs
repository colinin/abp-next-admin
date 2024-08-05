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
public interface IPlatformDbContext : IEfCoreDbContext
{
    DbSet<Menu> Menus { get; }
    DbSet<Layout> Layouts { get; }
    DbSet<RoleMenu> RoleMenus { get; }
    DbSet<UserMenu> UserMenus { get; }
    DbSet<UserFavoriteMenu> UserFavoriteMenus { get; }
    DbSet<Data> Datas { get; }
    DbSet<DataItem> DataItems { get; }
    DbSet<Package> Packages { get; }
    DbSet<PackageBlob> PackageBlobs { get; }
    DbSet<Enterprise> Enterprises { get; }
}
