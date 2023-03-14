using LY.MicroService.Applications.Single.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.DbMigrator;

[DependsOn(
    typeof(SingleMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class SingleDbMigratorModule : AbpModule
{

}
