using LY.MicroService.LocalizationManagement.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.LocalizationManagement.DbMigrator;

[DependsOn(
    typeof(LocalizationManagementMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class LocalizationManagementDbMigratorModule : AbpModule
{
}
