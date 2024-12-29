using LINGYUN.Abp.UI.Navigation.VueVbenAdmin;
using Microsoft.Extensions.DependencyInjection;
using PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.MySql;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName.AIO.DbMigrator;

[DependsOn(
    typeof(AbpUINavigationVueVbenAdminModule),
    // typeof(SingleMigrationsEntityFrameworkCorePostgreSqlModule),
    typeof(SingleMigrationsEntityFrameworkCoreMySqlModule),
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
