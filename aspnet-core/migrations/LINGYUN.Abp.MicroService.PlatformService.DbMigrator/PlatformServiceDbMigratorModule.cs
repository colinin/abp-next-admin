using LINGYUN.Abp.MicroService.PlatformService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.PlatformService.DbMigrator;

[DependsOn(
    typeof(PlatformServiceMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class PlatformServiceDbMigratorModule : AbpModule
{
}
