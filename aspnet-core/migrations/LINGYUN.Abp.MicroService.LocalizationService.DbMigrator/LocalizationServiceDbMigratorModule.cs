using LINGYUN.Abp.MicroService.LocalizationService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.LocalizationService.DbMigrator;

[DependsOn(
    typeof(LocalizationServiceMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class LocalizationServiceDbMigratorModule : AbpModule
{
}
