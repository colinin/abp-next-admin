using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.MySql;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.PostgreSql;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.AllInOne.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SingleMigrationsEntityFrameworkCorePostgreSqlModule),
    typeof(SingleMigrationsEntityFrameworkCoreSqlServerModule),
    typeof(SingleMigrationsEntityFrameworkCoreMySqlModule)
    )]
public partial class SingleDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ConfigureTiming(configuration);
    }
}
