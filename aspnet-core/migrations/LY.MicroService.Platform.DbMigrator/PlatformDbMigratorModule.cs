using LY.MicroService.Platform.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.Platform.DbMigrator;

[DependsOn(
    typeof(PlatformMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class PlatformDbMigratorModule : AbpModule
{
}
