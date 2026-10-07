using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.MicroService.MessageService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.MessageService.DbMigrator;

[DependsOn(
    typeof(MessageServiceMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAutofacModule)
    )]
public partial class MessageServiceDbMigratorModule : AbpModule
{
}
