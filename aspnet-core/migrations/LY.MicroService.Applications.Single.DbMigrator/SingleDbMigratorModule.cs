using LINGYUN.Abp.UI.Navigation.VueVbenAdmin5;
using LY.MicroService.Applications.Single.EntityFrameworkCore.MySql;
using LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql;
using LY.MicroService.Applications.Single.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpUINavigationVueVbenAdmin5Module),
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
