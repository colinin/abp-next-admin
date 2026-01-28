using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.AIService.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AIServiceMigrationsEntityFrameworkCoreModule)
    )]
public class AIServiceDbMigratorModule : AbpModule
{

}
