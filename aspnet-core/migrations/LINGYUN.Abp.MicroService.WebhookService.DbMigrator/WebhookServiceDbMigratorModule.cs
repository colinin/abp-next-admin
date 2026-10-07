using LINGYUN.Abp.MicroService.WebhookService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.WebhookService.DbMigrator;

[DependsOn(
    typeof(WebhookServiceMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class WebhookServiceDbMigratorModule : AbpModule
{
}
