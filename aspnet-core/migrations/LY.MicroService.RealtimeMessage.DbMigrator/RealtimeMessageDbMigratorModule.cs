using LINGYUN.Abp.Data.DbMigrator;
using LY.MicroService.RealtimeMessage.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.RealtimeMessage.DbMigrator;

[DependsOn(
    typeof(RealtimeMessageMigrationsEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAutofacModule)
    )]
public partial class RealtimeMessageDbMigratorModule : AbpModule
{
}
