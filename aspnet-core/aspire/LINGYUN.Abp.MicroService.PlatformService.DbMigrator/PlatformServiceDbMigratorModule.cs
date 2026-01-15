using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.PlatformService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(PlatformServiceMigrationsEntityFrameworkCoreModule)
    )]
public class PlatformServiceDbMigratorModule : AbpModule
{
}
