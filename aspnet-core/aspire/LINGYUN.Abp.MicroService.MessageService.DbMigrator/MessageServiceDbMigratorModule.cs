using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.MessageService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MessageServiceMigrationsEntityFrameworkCoreModule)
    )]
public class MessageServiceDbMigratorModule : AbpModule
{
}
