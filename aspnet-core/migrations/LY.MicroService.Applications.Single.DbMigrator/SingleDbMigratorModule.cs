using LINGYUN.Abp.UI.Navigation.VueVbenAdmin;
using Microsoft.Extensions.DependencyInjection;
using LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql;
// using LY.MicroService.Applications.Single.EntityFrameworkCore.MySql;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.DbMigrator;

[DependsOn(
    typeof(AbpUINavigationVueVbenAdminModule),
    typeof(SingleMigrationsEntityFrameworkCorePostgreSqlModule),
    // typeof(SingleMigrationsEntityFrameworkCoreMySqlModule),
    typeof(AbpAutofacModule)
    )]
public partial class SingleDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ConfigureTiming(configuration);
    }
}
