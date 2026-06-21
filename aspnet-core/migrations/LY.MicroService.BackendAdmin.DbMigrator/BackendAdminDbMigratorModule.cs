using LY.MicroService.BackendAdmin.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.BackendAdmin.DbMigrator;

[DependsOn(
    typeof(BackendAdminMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class BackendAdminDbMigratorModule : AbpModule
{
}
