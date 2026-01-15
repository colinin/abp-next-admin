using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.WebhookService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WebhookServiceMigrationsEntityFrameworkCoreModule)
    )]
public class WebhookServiceDbMigratorModule : AbpModule
{
}
