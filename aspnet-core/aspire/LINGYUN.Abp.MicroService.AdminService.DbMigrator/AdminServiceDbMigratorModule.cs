using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.AdminService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AdminServiceMigrationsEntityFrameworkCoreModule)
    )]
public class AdminServiceDbMigratorModule : AbpModule
{

}
