using LY.MicroService.WebhooksManagement.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.WebhooksManagement.DbMigrator;

[DependsOn(
    typeof(WebhooksManagementMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class WebhooksManagementDbMigratorModule : AbpModule
{
}
