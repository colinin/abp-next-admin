using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.AuthServer;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AuthServerMigrationsEntityFrameworkCoreModule))]
public class AuthServerDbMigratorModule : AbpModule
{
}
