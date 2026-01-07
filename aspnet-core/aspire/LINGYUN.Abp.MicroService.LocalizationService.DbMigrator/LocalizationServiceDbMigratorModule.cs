using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.LocalizationService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(LocalizationServiceMigrationsEntityFrameworkCoreModule)
    )]
public class LocalizationServiceDbMigratorModule : AbpModule
{
}
